[CmdletBinding()]
param (
    [Parameter(Mandatory=$true)][string]$Instance,
    [Parameter(Mandatory=$true)][string]$Project,
    [Parameter(Mandatory=$true)][string]$PersonalAccessToken,
    [Parameter(Mandatory=$true)][string]$Repository,
    [Parameter(Mandatory=$true)][string]$Branch = "master",
    [Parameter(Mandatory=$true)][ValidateSet("production", "staging")][string]$Slot = "production",
    [Parameter(Mandatory=$true)][ValidateSet("Dev", "Int", "Prod")][string]$Environment,
    [Parameter(Mandatory=$true)][ValidateSet("USNC", "USSC")][string]$Region
)

Write-Host "Importing configuration for Gallery Functional tests on $Environment $Region"

Add-Type -AssemblyName System.IO.Compression.FileSystem

$basicAuth = [Convert]::ToBase64String([Text.Encoding]::ASCII.GetBytes(("{0}:{1}" -f 'PAT', $PersonalAccessToken)))
$headers = @{ Authorization = ("Basic {0}" -f $basicAuth) }

Function Merge-Objects {
    <#
        .SYNOPSIS
            Merges a PSObject with another PSObject.

        .DESCRIPTION
            Iterates through every NoteProperty in the source object.
            Properties of the source object are added to the output object.
            If these properties already exist in the output object, they are overwritten.
            Properties that are PSObjects are merged recursively.

        .PARAMETER Source
            The object to merge the output with.

        .PARAMETER Output
            The object that the properties of the source are copied into.
            This object is mutated by the function.

        .OUTPUTS
            None

        .EXAMPLE
            $source = New-Object PSObject -Property @{ A = "A", B = New-Object PSObject -Property @{ C = "C" }, D = "D"}
            $output = New-Object PSObject -Property @{ D = "E", E = "E" }

            Get-MergedObject -Source $source -Output $output

            $output is now @{ A = "A", B = New-Object PSObject -Property @{ C = "C" }, D = "D", E = "E" }
    #>
    param(
        [PSObject]$Source,
        [PSObject]$Output
    )

    # For each property of the source object, add the property to the output object
    $Source | `
        Get-Member -MemberType NoteProperty | `
        ForEach-Object {
            $name = $_.Name
            $value = $Source."$name"
            $existingValue = $Output."$name"
            if ($_.Definition.StartsWith("System.Management.Automation.PSCustomObject") -and $existingValue -is [PSObject]) {
                # If the property is a nested object in both the source and output, merge the nested object in the source with the output object
                Get-MergedObject -Source $value -output $existingValue
            } else {
                # Add the property to the output object
                # If the property already exists on the output object, overwrite it
                $Output | Add-Member -MemberType NoteProperty -Name $name -Value $value -Force
            }
        }
}

# Download the config files--common, per environment, and per region--and merge them into a single file
$configObject = New-Object PSObject
"Common", $Environment, "$Environment-$Region" | `
    ForEach-Object {
        $filename = "$_.json"
        $file = "$PSScriptRoot\temp-$filename"
        Write-Host "Downloading temporary configuration file $filename"
        $requestUri = "https://$Instance.visualstudio.com/DefaultCollection/$Project/_apis/git/repositories/$Repository/items?api-version=1.0&versionDescriptor.version=$Branch&scopePath=GalleryFunctionalConfig\$filename"
        $response = Invoke-WebRequest -UseBasicParsing -Uri $requestUri -Headers $headers -OutFile $file
        $configData = Get-Content -Path $file | ConvertFrom-Json
        Remove-Item -Path $file
        # Merge the current file with the last files
        Get-MergedObject -Source $configData -Output $configObject
    }

# Add a field to the file determining which slot should be tested
$configObject | Add-Member -MemberType NoteProperty -Name "Slot" -Value $Slot

# Save the file and set an environment variable to be used by the functional tests
$configurationFilePath = "$PSScriptRoot\Config-$Environment-$Region.json"
[Environment]::SetEnvironmentVariable("ConfigurationFilePath", $configurationFilePath)
Write-Host "##vso[task.setvariable variable=ConfigurationFilePath;]$configurationFilePath"
ConvertTo-Json $configObject | Out-File $configurationFilePath