﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.UI;
using Newtonsoft.Json.Linq;
using NuGet.Frameworks;
using NuGet.Packaging;
using NuGet.Versioning;
using NuGetGallery.Auditing;
using NuGetGallery.Auditing.AuditedEntities;
using NuGetGallery.Authentication;
using NuGetGallery.Configuration;
using NuGetGallery.Filters;
using NuGetGallery.Infrastructure.Authentication;
using NuGetGallery.Packaging;
using PackageIdValidator = NuGetGallery.Packaging.PackageIdValidator;

namespace NuGetGallery
{
    public partial class ApiController
        : AppController
    {
        public IEntitiesContext EntitiesContext { get; set; }
        public INuGetExeDownloaderService NugetExeDownloaderService { get; set; }
        public IPackageFileService PackageFileService { get; set; }
        public IPackageService PackageService { get; set; }
        public IUserService UserService { get; set; }
        public IStatisticsService StatisticsService { get; set; }
        public IContentService ContentService { get; set; }
        public ISearchService SearchService { get; set; }
        public IIndexingService IndexingService { get; set; }
        public IAutomaticallyCuratePackageCommand AutoCuratePackage { get; set; }
        public IStatusService StatusService { get; set; }
        public IMessageService MessageService { get; set; }
        public IAuditingService AuditingService { get; set; }
        public IGalleryConfigurationService ConfigurationService { get; set; }
        public ITelemetryService TelemetryService { get; set; }
        public ICredentialBuilder CredentialBuilder { get; set; }

        protected ApiController()
        {
            AuditingService = NuGetGallery.Auditing.AuditingService.None;
        }

        public ApiController(
            IEntitiesContext entitiesContext,
            IPackageService packageService,
            IPackageFileService packageFileService,
            IUserService userService,
            INuGetExeDownloaderService nugetExeDownloaderService,
            IContentService contentService,
            IIndexingService indexingService,
            ISearchService searchService,
            IAutomaticallyCuratePackageCommand autoCuratePackage,
            IStatusService statusService,
            IMessageService messageService,
            IAuditingService auditingService,
            IGalleryConfigurationService configurationService,
            ITelemetryService telemetryService,
            ICredentialBuilder credentialBuilder)
        {
            EntitiesContext = entitiesContext;
            PackageService = packageService;
            PackageFileService = packageFileService;
            UserService = userService;
            NugetExeDownloaderService = nugetExeDownloaderService;
            ContentService = contentService;
            IndexingService = indexingService;
            SearchService = searchService;
            AutoCuratePackage = autoCuratePackage;
            StatusService = statusService;
            MessageService = messageService;
            AuditingService = auditingService;
            ConfigurationService = configurationService;
            TelemetryService = telemetryService;
            StatisticsService = null;
            CredentialBuilder = credentialBuilder;
        }

        public ApiController(
            IEntitiesContext entitiesContext,
            IPackageService packageService,
            IPackageFileService packageFileService,
            IUserService userService,
            INuGetExeDownloaderService nugetExeDownloaderService,
            IContentService contentService,
            IIndexingService indexingService,
            ISearchService searchService,
            IAutomaticallyCuratePackageCommand autoCuratePackage,
            IStatusService statusService,
            IStatisticsService statisticsService,
            IMessageService messageService,
            IAuditingService auditingService,
            IGalleryConfigurationService configurationService,
            ITelemetryService telemetryService,
            ICredentialBuilder credentialBuilder)
            : this(entitiesContext, packageService, packageFileService, userService, nugetExeDownloaderService, contentService, indexingService, searchService, autoCuratePackage, statusService, messageService, auditingService, configurationService, telemetryService, credentialBuilder)
        {
            StatisticsService = statisticsService;
        }

        [HttpGet]
        [ActionName("GetPackageApi")]
        public virtual async Task<ActionResult> GetPackage(string id, string version)
        {
            // some security paranoia about URL hacking somehow creating e.g. open redirects
            // validate user input: explicit calls to the same validators used during Package Registrations
            // Ideally shouldn't be necessary?
            if (!PackageIdValidator.IsValidPackageId(id ?? ""))
            {
                return new HttpStatusCodeWithBodyResult(HttpStatusCode.BadRequest, "The format of the package id is invalid");
            }

            // if version is non-null, check if it's semantically correct and normalize it.
            if (!String.IsNullOrEmpty(version))
            {
                NuGetVersion dummy;
                if (!NuGetVersion.TryParse(version, out dummy))
                {
                    return new HttpStatusCodeWithBodyResult(HttpStatusCode.BadRequest, "The package version is not a valid semantic version");
                }

                // Normalize the version
                version = NuGetVersionNormalizer.Normalize(version);
            }
            else
            {
                // if version is null, get the latest version from the database.
                // This ensures that on package restore scenario where version will be non null, we don't hit the database.
                try
                {
                    var package = PackageService.FindPackageByIdAndVersion(id, version, allowPrerelease: false);
                    if (package == null)
                    {
                       return new HttpStatusCodeWithBodyResult(HttpStatusCode.NotFound, String.Format(CultureInfo.CurrentCulture, Strings.PackageWithIdAndVersionNotFound, id, version));
                    }
                    version = package.NormalizedVersion;

                }
                catch (SqlException e)
                {
                    QuietLog.LogHandledException(e);

                    // Database was unavailable and we don't have a version, return a 503
                    return new HttpStatusCodeWithBodyResult(HttpStatusCode.ServiceUnavailable, Strings.DatabaseUnavailable_TrySpecificVersion);
                }
                catch (DataException e)
                {
                    QuietLog.LogHandledException(e);

			        // Database was unavailable and we don't have a version, return a 503
                    return new HttpStatusCodeWithBodyResult(HttpStatusCode.ServiceUnavailable, Strings.DatabaseUnavailable_TrySpecificVersion);
                }
            }

            if (ConfigurationService.Features.TrackPackageDownloadCountInLocalDatabase)
            {
                await PackageService.IncrementDownloadCountAsync(id, version);
            }

            return await PackageFileService.CreateDownloadPackageActionResultAsync(
                HttpContext.Request.Url,
                id, version);
        }

        [HttpGet]
        [ActionName("GetNuGetExeApi")]
        [RequireHttps]
        [OutputCache(VaryByParam = "none", Location = OutputCacheLocation.ServerAndClient, Duration = 600)]
        public virtual Task<ActionResult> GetNuGetExe()
        {
            return NugetExeDownloaderService.CreateNuGetExeDownloadActionResultAsync(HttpContext.Request.Url);
        }

        [HttpGet]
        [ActionName("StatusApi")]
        public async virtual Task<ActionResult> Status()
        {
            if (StatusService == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.ServiceUnavailable, "Status service is unavailable");
            }
            return await StatusService.GetStatus();
        }

        [HttpPut]
        [RequireSsl]
        [ApiAuthorize]
        [ApiScopeRequired(NuGetScopes.PackagePush, NuGetScopes.PackagePushVersion)]
        [ActionName("CreatePackageVerificationKey")]
        public virtual Task<ActionResult> CreatePackageVerificationKeyPutAsync(string id, string version)
        {
            return CreatePackageVerificationKeyInternalAsync(id, version);
        }

        [HttpPost]
        [RequireSsl]
        [ApiAuthorize]
        [ApiScopeRequired(NuGetScopes.PackagePush, NuGetScopes.PackagePushVersion)]
        [ActionName("CreatePackageVerificationKey")]
        public virtual Task<ActionResult> CreatePackageVerificationKeyPostAsync(string id, string version)
        {
            return CreatePackageVerificationKeyInternalAsync(id, version);
        }
        
        private async Task<ActionResult> CreatePackageVerificationKeyInternalAsync(string id, string version)
        {
            var package = PackageService.FindPackageByIdAndVersion(id, version);
            if (package == null)
            {
                return new HttpStatusCodeWithBodyResult(
                    HttpStatusCode.NotFound, String.Format(CultureInfo.CurrentCulture, Strings.PackageWithIdAndVersionNotFound, id, version));
            }
            var user = GetCurrentUser();
            var credential = CredentialBuilder.CreatePackageVerificationApiKey(id);
            user.Credentials.Add(credential);

            await EntitiesContext.SaveChangesAsync();

            return Json(new
            {
                Key = credential.Value,
                Expires = credential.Expires,
                Scope = credential.Scopes.FirstOrDefault()?.Subject
            });
        }

        private async Task<bool> DeletePackageVerificationKeyAsync(Credential credential)
        {
            var scopesToRemove = credential.Scopes.ToArray();
            foreach (var scope in scopesToRemove)
            {
                EntitiesContext.Scopes.Remove(scope);
            }
            EntitiesContext.Credentials.Remove(credential);

            var rowsUpdated = await EntitiesContext.SaveChangesAsync();

            // Verify that credential and scope have been removed, and return a failure if the
            // verification key was created incorrectly without a scope.
            return rowsUpdated > 1;
        }

        private Credential GetCurrentCredential(User user)
        {
            var identity = User.Identity as ClaimsIdentity;
            var apiKey = identity.GetClaimOrDefault(NuGetClaims.ApiKey);
            
            return user.Credentials.FirstOrDefault(c => c.Value == apiKey);
        }

        [HttpGet]
        [RequireSsl]
        [ApiAuthorize]
        [ApiScopeRequired(NuGetScopes.PackageVerify)]
        [ActionName("VerifyPackageKey")]
        public async virtual Task<ActionResult> VerifyPackageKeyAsync(string id, string version)
        {
            var user = GetCurrentUser();
            var credential = GetCurrentCredential(user);

            var isPackageVerificationKey = CredentialTypes.IsPackageVerificationApiKey(credential.Type);
            // Todo: Add telemetry based on whether isPackageVerificationKey

            if (string.IsNullOrWhiteSpace(id))
            {
                return new HttpStatusCodeWithBodyResult(HttpStatusCode.Forbidden, Strings.ApiKeyNotAuthorized);
            }

            // Verify that the user has permission to push for the specific Id \ version combination.
            var package = PackageService.FindPackageByIdAndVersion(id, version);
            if (package == null)
            {
                return new HttpStatusCodeWithBodyResult(
                    HttpStatusCode.NotFound, String.Format(CultureInfo.CurrentCulture, Strings.PackageWithIdAndVersionNotFound, id, version));
            }

            if (!package.IsOwner(user))
            {
                return new HttpStatusCodeWithBodyResult(HttpStatusCode.Forbidden, Strings.ApiKeyNotAuthorized);
            }

            if (isPackageVerificationKey)
            {
                // Verify that verification key matches requested package scope.
                if (!User.Identity.HasScopeThatAllowsActionForSubject(id, new[] { NuGetScopes.PackageVerify }))
                {
                    return new HttpStatusCodeWithBodyResult(HttpStatusCode.Forbidden, Strings.ApiKeyNotAuthorized);
                }

                // Expire package verification API key on first use. We delete these keys to minimize the
                // size of the Credentials table. Return failure if delete detected an invalidly scoped key.
                if (!await DeletePackageVerificationKeyAsync(credential))
                {
                    return new HttpStatusCodeWithBodyResult(HttpStatusCode.Forbidden, Strings.ApiKeyNotAuthorized);
                }
            }

            return new EmptyResult();
        }

        [HttpPut]
        [RequireSsl]
        [ApiAuthorize]
        [ApiScopeRequired(NuGetScopes.PackagePush, NuGetScopes.PackagePushVersion)]
        [ActionName("PushPackageApi")]
        public virtual Task<ActionResult> CreatePackagePut()
        {
            return CreatePackageInternal();
        }

        [HttpPost]
        [RequireSsl]
        [ApiAuthorize]
        [ApiScopeRequired(NuGetScopes.PackagePush, NuGetScopes.PackagePushVersion)]
        [ActionName("PushPackageApi")]
        public virtual Task<ActionResult> CreatePackagePost()
        {
            return CreatePackageInternal();
        }

        private async Task<ActionResult> CreatePackageInternal()
        {
            // Get the user
            var user = GetCurrentUser();

            using (var packageStream = ReadPackageFromRequest())
            {
                try
                {
                    using (var archive = new ZipArchive(packageStream, ZipArchiveMode.Read, leaveOpen: true))
                    {
                        var reference = DateTime.UtcNow.AddDays(1); // allow "some" clock skew

                        var entryInTheFuture = archive.Entries.FirstOrDefault(
                            e => e.LastWriteTime.UtcDateTime > reference);

                        if (entryInTheFuture != null)
                        {
                            return new HttpStatusCodeWithBodyResult(HttpStatusCode.BadRequest, string.Format(
                               CultureInfo.CurrentCulture,
                               Strings.PackageEntryFromTheFuture,
                               entryInTheFuture.Name));
                        }
                    }

                    using (var packageToPush = new PackageArchiveReader(packageStream, leaveStreamOpen: false))
                    {
                        try
                        {
                            PackageService.EnsureValid(packageToPush);
                        }
                        catch (Exception ex)
                        {
                            ex.Log();

                            var message = Strings.FailedToReadUploadFile;
                            if (ex is InvalidPackageException || ex is InvalidDataException || ex is EntityException)
                            {
                                message = ex.Message;
                            }
                            
                            return new HttpStatusCodeWithBodyResult(HttpStatusCode.BadRequest, message);
                        }

                        NuspecReader nuspec;
                        var errors = ManifestValidator.Validate(packageToPush.GetNuspec(), out nuspec).ToArray();
                        if (errors.Length > 0)
                        {
                            var errorsString = string.Join("', '", errors.Select(error => error.ErrorMessage));
                            return new HttpStatusCodeWithBodyResult(HttpStatusCode.BadRequest, string.Format(
                                CultureInfo.CurrentCulture,
                                errors.Length > 1 ? Strings.UploadPackage_InvalidNuspecMultiple : Strings.UploadPackage_InvalidNuspec,
                                errorsString));
                        }

                        if (nuspec.GetMinClientVersion() > Constants.MaxSupportedMinClientVersion)
                        {
                            return new HttpStatusCodeWithBodyResult(HttpStatusCode.BadRequest, string.Format(
                                CultureInfo.CurrentCulture,
                                Strings.UploadPackage_MinClientVersionOutOfRange,
                                nuspec.GetMinClientVersion()));
                        }

                        // Ensure that the user can push packages for this partialId.
                        var packageRegistration = PackageService.FindPackageRegistrationById(nuspec.GetId());
                        if (packageRegistration == null)
                        {
                            // Check if API key allows pushing a new package id
                            if (!ApiKeyScopeAllows(
                                subject: nuspec.GetId(), 
                                requestedActions: NuGetScopes.PackagePush))
                            {
                                // User cannot push a new package ID as the API key scope does not allow it
                                return new HttpStatusCodeWithBodyResult(HttpStatusCode.Unauthorized, Strings.ApiKeyNotAuthorized);
                            }
                        }
                        else
                        {
                            // Is the user allowed to push this Id?
                            if (!packageRegistration.IsOwner(user))
                            {
                                // Audit that a non-owner tried to push the package
                                await AuditingService.SaveAuditRecordAsync(
                                    new FailedAuthenticatedOperationAuditRecord(
                                        user.Username, 
                                        AuditedAuthenticatedOperationAction.PackagePushAttemptByNonOwner, 
                                        attemptedPackage: new AuditedPackageIdentifier(
                                            nuspec.GetId(), nuspec.GetVersion().ToNormalizedStringSafe())));

                                // User cannot push a package to an ID owned by another user.
                                return new HttpStatusCodeWithBodyResult(HttpStatusCode.Conflict,
                                    string.Format(CultureInfo.CurrentCulture, Strings.PackageIdNotAvailable,
                                        nuspec.GetId()));
                            }

                            // Check if API key allows pushing the current package id
                            if (!ApiKeyScopeAllows(
                                packageRegistration.Id, 
                                NuGetScopes.PackagePushVersion, NuGetScopes.PackagePush))
                            {
                                // User cannot push a package as the API key scope does not allow it
                                return new HttpStatusCodeWithBodyResult(HttpStatusCode.Unauthorized, Strings.ApiKeyNotAuthorized);
                            }

                            // Check if a particular Id-Version combination already exists. We eventually need to remove this check.
                            string normalizedVersion = nuspec.GetVersion().ToNormalizedString();
                            bool packageExists =
                                packageRegistration.Packages.Any(
                                    p => string.Equals(
                                        p.NormalizedVersion,
                                        normalizedVersion,
                                        StringComparison.OrdinalIgnoreCase));

                            if (packageExists)
                            {
                                return new HttpStatusCodeWithBodyResult(
                                    HttpStatusCode.Conflict,
                                    string.Format(CultureInfo.CurrentCulture, Strings.PackageExistsAndCannotBeModified,
                                        nuspec.GetId(), nuspec.GetVersion().ToNormalizedStringSafe()));
                            }
                        }

                        var packageStreamMetadata = new PackageStreamMetadata
                        {
                            HashAlgorithm = Constants.Sha512HashAlgorithmId,
                            Hash = CryptographyService.GenerateHash(packageStream.AsSeekableStream()),
                            Size = packageStream.Length
                        };

                        var package = await PackageService.CreatePackageAsync(
                            packageToPush, 
                            packageStreamMetadata,
                            user,
                            commitChanges: false);

                        await AutoCuratePackage.ExecuteAsync(package, packageToPush, commitChanges: false);

                        using (Stream uploadStream = packageStream)
                        {
                            uploadStream.Position = 0;

                            try
                            {
                                await PackageFileService.SavePackageFileAsync(package, uploadStream.AsSeekableStream());
                            }
                            catch (InvalidOperationException ex)
                            {
                                ex.Log();

                                return new HttpStatusCodeWithBodyResult(HttpStatusCode.Conflict, Strings.UploadPackage_IdVersionConflict);
                            }
                        }

                        try
                        {
                            await EntitiesContext.SaveChangesAsync();
                        }
                        catch
                        {
                            // If saving to the DB fails for any reason, we need to delete the package we just saved.
                            await PackageFileService.DeletePackageFileAsync(nuspec.GetId(), nuspec.GetVersion().ToNormalizedString());
                            throw;
                        }

                        // Handle in separate transaction because of concurrency check with retry
                        await PackageService.UpdateIsLatestAsync(package.PackageRegistration);

                        IndexingService.UpdatePackage(package);
                        
                        // Write an audit record
                        await AuditingService.SaveAuditRecordAsync(
                            new PackageAuditRecord(package, AuditedPackageAction.Create, PackageCreatedVia.Api));

                        // Notify user of push
                        MessageService.SendPackageAddedNotice(package,
                            Url.Action("DisplayPackage", "Packages", routeValues: new { id = package.PackageRegistration.Id, version = package.Version }, protocol: Request.Url.Scheme),
                            Url.Action("ReportMyPackage", "Packages", routeValues: new { id = package.PackageRegistration.Id, version = package.Version }, protocol: Request.Url.Scheme),
                            Url.Action("Account", "Users", routeValues: null, protocol: Request.Url.Scheme));

                        TelemetryService.TrackPackagePushEvent(user, User.Identity);

                        return new HttpStatusCodeResult(HttpStatusCode.Created);
                    }
                }
                catch (InvalidPackageException ex)
                {
                    return BadRequestForExceptionMessage(ex);
                }
                catch (InvalidDataException ex)
                {
                    return BadRequestForExceptionMessage(ex);
                }
                catch (EntityException ex)
                {
                    return BadRequestForExceptionMessage(ex);
                }
                catch (FrameworkException ex)
                {
                    return BadRequestForExceptionMessage(ex);
                }
            }
        }

        private bool ApiKeyScopeAllows(string subject, params string[] requestedActions)
        {
            return User.Identity.HasScopeThatAllowsActionForSubject(
                subject: subject,
                requestedActions: requestedActions);
        }

        private static ActionResult BadRequestForExceptionMessage(Exception ex)
        {
            return new HttpStatusCodeWithBodyResult(
                HttpStatusCode.BadRequest,
                string.Format(CultureInfo.CurrentCulture, Strings.UploadPackage_InvalidPackage, ex.Message));
        }

        [HttpDelete]
        [RequireSsl]
        [ApiAuthorize]
        [ApiScopeRequired(NuGetScopes.PackageUnlist)]
        [ActionName("DeletePackageApi")]
        public virtual async Task<ActionResult> DeletePackage(string id, string version)
        {
            var package = PackageService.FindPackageByIdAndVersion(id, version);
            if (package == null)
            {
                return new HttpStatusCodeWithBodyResult(
                    HttpStatusCode.NotFound, String.Format(CultureInfo.CurrentCulture, Strings.PackageWithIdAndVersionNotFound, id, version));
            }

            var user = GetCurrentUser();
            if (!package.IsOwner(user))
            {
                return new HttpStatusCodeWithBodyResult(HttpStatusCode.Forbidden, Strings.ApiKeyNotAuthorized);
            }

            // Check if API key allows listing/unlisting the current package id
            if (!ApiKeyScopeAllows(
                subject: id, 
                requestedActions: NuGetScopes.PackageUnlist))
            {
                return new HttpStatusCodeWithBodyResult(HttpStatusCode.Forbidden, Strings.ApiKeyNotAuthorized);
            }

            await PackageService.MarkPackageUnlistedAsync(package);

            // Handle in separate transaction because of concurrency check with retry. Due to using
            // separate transactions, we must always call UpdateIsLatest on delete/unlist. This is
            // because a concurrent thread could be marking the package as latest before this thread
            // is able to commit the delete /unlist.
            await PackageService.UpdateIsLatestAsync(package.PackageRegistration);

            IndexingService.UpdatePackage(package);
            return new EmptyResult();
        }

        [HttpPost]
        [RequireSsl]
        [ApiAuthorize]
        [ApiScopeRequired(NuGetScopes.PackageUnlist)]
        [ActionName("PublishPackageApi")]
        public virtual async Task<ActionResult> PublishPackage(string id, string version)
        {
            var package = PackageService.FindPackageByIdAndVersion(id, version);
            if (package == null)
            {
                return new HttpStatusCodeWithBodyResult(
                    HttpStatusCode.NotFound, String.Format(CultureInfo.CurrentCulture, Strings.PackageWithIdAndVersionNotFound, id, version));
            }

            User user = GetCurrentUser();
            if (!package.IsOwner(user))
            {
                return new HttpStatusCodeWithBodyResult(HttpStatusCode.Forbidden, String.Format(CultureInfo.CurrentCulture, Strings.ApiKeyNotAuthorized, "publish"));
            }

            // Check if API key allows listing/unlisting the current package id
            if (!ApiKeyScopeAllows(
                subject: id, 
                requestedActions: NuGetScopes.PackageUnlist))
            {
                return new HttpStatusCodeWithBodyResult(HttpStatusCode.Forbidden, Strings.ApiKeyNotAuthorized);
            }

            await PackageService.MarkPackageListedAsync(package);
            
            // handle in separate transaction because of concurrency check with retry
            await PackageService.UpdateIsLatestAsync(package.PackageRegistration);

            IndexingService.UpdatePackage(package);
            return new EmptyResult();
        }

        public virtual async Task<ActionResult> Team()
        {
            var team = await ContentService.GetContentItemAsync(Constants.ContentNames.Team, TimeSpan.FromHours(1));
            return Content(team.ToString(), "application/json");
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            var exception = filterContext.Exception;
            if (exception is ReadOnlyModeException)
            {
                filterContext.ExceptionHandled = true;
                filterContext.Result = new HttpStatusCodeWithBodyResult(
                    HttpStatusCode.ServiceUnavailable, exception.Message);
            }
            else
            {
                var request = filterContext.HttpContext.Request;
                filterContext.ExceptionHandled = true;
                filterContext.Result = new HttpStatusCodeWithBodyResult(
                    HttpStatusCode.InternalServerError, exception.Message, request.IsLocal ? exception.StackTrace : exception.Message);
            }
        }

        protected internal virtual Stream ReadPackageFromRequest()
        {
            Stream stream;
            if (Request.Files.Count > 0)
            {
                // If we're using the newer API, the package stream is sent as a file.
                // ReSharper disable once PossibleNullReferenceException
                stream = Request.Files[0].InputStream;
            }
            else
            {
                stream = Request.InputStream;
            }

            return stream;
        }

        [HttpGet]
        [ActionName("PackageIDs")]
        public virtual async Task<ActionResult> GetPackageIds(string partialId, bool? includePrerelease)
        {
            var query = GetService<IAutoCompletePackageIdsQuery>();
            return new JsonResult
            {
                Data = (await query.Execute(partialId, includePrerelease)).ToArray(),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [HttpGet]
        [ActionName("PackageVersions")]
        public virtual async Task<ActionResult> GetPackageVersions(string id, bool? includePrerelease)
        {
            var query = GetService<IAutoCompletePackageVersionsQuery>();
            return new JsonResult
            {
                Data = (await query.Execute(id, includePrerelease)).ToArray(),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [HttpGet]
        [ActionName("StatisticsDownloadsApi")]
        public virtual async Task<ActionResult> GetStatsDownloads(int? count)
        {
            var result = await StatisticsService.LoadDownloadPackageVersions();

            if (result.Loaded)
            {
                int i = 0;

                JArray content = new JArray();
                foreach (StatisticsPackagesItemViewModel row in StatisticsService.DownloadPackageVersionsAll)
                {
                    JObject item = new JObject();

                    item.Add("PackageId", row.PackageId);
                    item.Add("PackageVersion", row.PackageVersion);
                    item.Add("Gallery", Url.PackageGallery(row.PackageId, row.PackageVersion));
                    item.Add("PackageTitle", row.PackageTitle ?? row.PackageId);
                    item.Add("PackageDescription", row.PackageDescription);
                    item.Add("PackageIconUrl", row.PackageIconUrl ?? Url.PackageDefaultIcon());
                    item.Add("Downloads", row.Downloads);

                    content.Add(item);

                    i++;

                    if (count.HasValue && count.Value == i)
                    {
                        break;
                    }
                }

                return new ContentResult
                {
                    Content = content.ToString(),
                    ContentType = "application/json"
                };
            }

            return new HttpStatusCodeResult(HttpStatusCode.NotFound);
        }
    }
}
