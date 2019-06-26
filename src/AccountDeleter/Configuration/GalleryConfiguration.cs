﻿using NuGetGallery.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace NuGetGallery.AccountDeleter
{
    public class GalleryConfiguration : IAppConfiguration
    {
        public LuceneIndexLocation LuceneIndexLocation { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string Environment { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string WarningBanner { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool RequireSSL { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int SSLPort { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string[] ForceSslExclusion { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string AzureStorage_Auditing_ConnectionString { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string AzureStorage_UserCertificates_ConnectionString { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string AzureStorage_Content_ConnectionString { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string AzureStorage_Errors_ConnectionString { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string AzureStorage_Packages_ConnectionString { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string AzureStorage_FlatContainer_ConnectionString { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string AzureStorage_Statistics_ConnectionString { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string AzureStorage_Uploads_ConnectionString { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string AzureStorage_Revalidation_ConnectionString { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool AzureStorageReadAccessGeoRedundant { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public TimeSpan FeatureFlagsRefreshInterval { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool AdminPanelDatabaseAccessEnabled { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool AsynchronousPackageValidationEnabled { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool BlockingAsynchronousPackageValidationEnabled { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public TimeSpan AsynchronousPackageValidationDelay { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public TimeSpan ValidationExpectedTime { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool DeprecateNuGetPasswordLogins { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool ConfirmEmailAddresses { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool ReadOnlyMode { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool FeedOnlyMode { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string FileStorageDirectory { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string Brand { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public StorageType StorageType { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Uri SmtpUri { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string SqlConnectionString { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string SqlConnectionStringSupportRequest { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string SqlConnectionStringValidation { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string AzureCdnHost { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string FacebookAppId { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string AppInsightsInstrumentationKey { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public double AppInsightsSamplingPercentage { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string SiteRoot { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string ReCaptchaPrivateKey { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string ReCaptchaPublicKey { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string GoogleAnalyticsPropertyId { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool CollectPerfLogs { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool AutoUpdateSearchIndex { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string EnforcedAuthProviderForAdmin { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string UserPasswordRegex { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string UserPasswordHint { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int ExpirationInDaysForApiKeyV1 { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int WarnAboutExpirationInDaysForApiKeyV1 { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string AlternateSiteRootList { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool EnableMachineKeyConfiguration { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string MachineKeyDecryption { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string MachineKeyDecryptionKey { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string MachineKeyValidationAlgorithm { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string MachineKeyValidationKey { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool IsODataFilterEnabled { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string ExternalStatusUrl { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string ExternalAboutUrl { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string ExternalPrivacyPolicyUrl { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string ExternalTermsOfUseUrl { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string ExternalBrandingUrl { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string ExternalBrandingMessage { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string TrademarksUrl { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool EnforceDefaultSecurityPolicies { get => true; set => throw new NotImplementedException(); }
        public bool IsHosted { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool RejectSignedPackagesWithNoRegisteredCertificate { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool RejectPackagesWithTooManyPackageEntries { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool BlockSearchEngineIndexing { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string[] RedirectedCuratedFeeds { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool AsynchronousEmailServiceEnabled { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool RejectPackagesWithLicense { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool BlockLegacyLicenseUrl { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool AllowLicenselessPackages { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Uri SearchServiceUriPrimary { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Uri SearchServiceUriSecondary { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Uri PreviewSearchServiceUriPrimary { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Uri PreviewSearchServiceUriSecondary { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int SearchCircuitBreakerDelayInSeconds { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int SearchCircuitBreakerWaitAndRetryIntervalInMilliseconds { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int SearchCircuitBreakerWaitAndRetryCount { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int SearchCircuitBreakerBreakAfterCount { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int SearchHttpClientTimeoutInMilliseconds { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public MailAddress GalleryOwner { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public MailAddress GalleryNoReplyAddress { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string SqlReadOnlyReplicaConnectionString { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool IgnoreIconUrl { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string EmbeddedIconUrlTemplate { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
