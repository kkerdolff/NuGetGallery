﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace NuGetGallery
{
    public static class RouteName
    {
        public const string Account = "Account";
        public const string OrganizationAccount = "ManageOrganization";
        public const string AddOrganization = "AddOrganization";
        public const string OrganizationMemberAddAjax = "AddMember";
        public const string OrganizationMemberAdd = "AddMemberJson";
        public const string OrganizationMemberConfirmRedirect = "ConfirmMemberRequestRedirect";
        public const string OrganizationMemberConfirm = "ConfirmMemberRequest";
        public const string OrganizationMemberRejectRedirect = "RejectMemberRequestRedirect";
        public const string OrganizationMemberReject = "RejectMemberRequest";
        public const string OrganizationMemberCancelAjax = "CancelMemberRequest";
        public const string OrganizationMemberCancel = "CancelMemberRequestJson";
        public const string OrganizationMemberUpdateAjax = "UpdateMember";
        public const string OrganizationMemberUpdate = "UpdateMemberJson";
        public const string OrganizationMemberDeleteAjax = "DeleteMember";
        public const string OrganizationMemberDelete = "DeleteMemberJson";
        public const string ChangeOrganizationEmailSubscription = "ChangeOrganizationEmailSubscription";
        public const string TransformToOrganization = "TransformToOrganization";
        public const string TransformToOrganizationConfirmationRedirect = "ConfirmTransformRedirect";
        public const string TransformToOrganizationConfirmation = "ConfirmTransform";
        public const string TransformToOrganizationRejectionRedirect = "RejectTransformRedirect";
        public const string TransformToOrganizationRejection = "RejectTransform";
        public const string TransformToOrganizationCancellationRedirect = "CancelTransformRedirect";
        public const string TransformToOrganizationCancellation = "CancelTransform";
        public const string ApiKeys = "ApiKeys";
        public const string Profile = "Profile";
        public const string DisplayPackage = "package-route";
        public const string DisplayReleasePackage = "package-release-route";
        public const string DisplayPrereleasePackage = "package-prerelease-route";
        public const string DisplayPackageFeed = "package-route-feed";
        public const string DownloadPackage = "DownloadPackage";
        public const string DownloadSymbolsPackage = "DownloadSymbolsPackage";
        public const string DownloadNuGetExe = "DownloadNuGetExe";
        public const string Home = "Home";
        public const string Stats = "Stats";
        public const string Policies = "Policies";
        public const string ListPackages = "ListPackages";
        public const string Authentication = "SignIn";
        public const string UploadPackage = "UploadPackage";
        public const string UploadPackageProgress = "UploadPackageProgress";
        public const string PackageVersionAction = "PackageVersionAction";
        public const string PackageOwnerConfirmationRedirect = "PackageOwnerConfirmationRedirect";
        public const string PackageOwnerConfirmation = "PackageOwnerConfirmation";
        public const string PackageOwnerRejectionRedirect = "PackageOwnerRejectionRedirect";
        public const string PackageOwnerRejection = "PackageOwnerRejection";
        public const string PackageOwnerCancellation = "PackageOwnerCancellation";
        public const string PackageAction = "PackageAction";
        public const string PackageDeleteAction = "PackageDeleteAction";
        public const string PushPackageApi = "PushPackageApi";
        public const string PublishPackageApi = "PublishPackageApi";
        public const string DeprecatePackageApi = "DeprecatePackageApi";
        public const string DeletePackageApi = "DeletePackageApi";
        public const string PushSymbolPackageApi = "PushSymbolPackageApi";
        public const string PasswordReset = "PasswordReset";
        public const string PasswordSet = "PasswordSet";
        public const string NewSubmission = "NewSubmission";
        public const string VerifyPackage = "VerifyPackage";
        public const string PreviewReadMe = "PreviewReadMe";
        public const string GetReadMeMd = "GetReadMeMd";
        public const string CreatePackageVerificationKey = "CreatePackageVerificationKey";
        public const string VerifyPackageKey = "VerifyPackageKey";
        public const string CancelUpload = "CancelUpload";
        public const string StatisticsHome = "StatisticsHome";
        public const string StatisticsPackages = "StatisticsPackages";
        public const string StatisticsPackageVersions = "StatisticsPackageVersions";
        public const string StatisticsPackageDownloadsByVersion = "StatisticsPackageDownloadsByVersion";
        public const string StatisticsPackageDownloadsByVersionReport = "StatisticsPackageDownloadsByVersionReport";
        public const string StatisticsPackageDownloadsDetail = "StatisticsPackageDownloadsDetail";
        public const string StatisticsPackageDownloadsDetailReport = "StatisticsPackageDownloadsDetailReport";
        public const string StatisticsDownloadsApi = "StatisticsDownloadsApi";
        public const string LegacyRegister = "LegacyRegister";
        public const string LegacyRegister2 = "LegacyRegister2";
        public const string PackageEnableLicenseReport = "EnableLicenseReport";
        public const string PackageDisableLicenseReport = "DisableLicenseReport";
        public const string Pages = "Pages";
        public const string ExternalAuthentication = "ExternalAuthentication";
        public const string ExternalAuthenticationCallback = "ExternalAuthenticationCallback";
        public const string RemoveCredential = "RemoveCredential";
        public const string RemovePassword = "RemovePassword";
        public const string ConfirmAccount = "ConfirmAccount";
        public const string SigninAssistance = "SigninAssistance";
        public const string ChangeEmailSubscription = "ChangeEmailSubscription";
        public const string ChangeMultiFactorAuthentication = "ChangeMultiFactorAuthentication";
        public const string ErrorReadOnly = "ErrorReadOnly";
        public const string Error500 = "Error500";
        public const string Error404 = "Error404";
        public const string Error400 = "Error400";
        public const string Status = "Status";
        public const string HealthProbe = "HealthProbe";
        public const string Contributors = "Contributors";
        public const string Team = "Team";
        public const string JsonApi = "JsonApi";
        public const string ManageDeprecationJsonApi = "ManageDeprecationJsonApi";
        public const string Downloads = "Downloads";
        public const string AdminDeleteAccount = "AdminDeleteAccount";
        public const string UserDeleteAccount = "DeleteAccount";
        public const string AddUserCertificate = "AddUserCertificate";
        public const string DeleteUserCertificate = "DeleteUserCertificate";
        public const string GetUserCertificate = "GetUserCertificate";
        public const string GetUserCertificates = "GetUserCertificates";
        public const string GetAccountAvatar = "GetUserAvatar";
        public const string AddOrganizationCertificate = "AddOrganizationCertificate";
        public const string DeleteOrganizationCertificate = "DeleteOrganizationCertificate";
        public const string GetOrganizationCertificate = "GetOrganizationCertificate";
        public const string GetOrganizationCertificates = "GetOrganizationCertificates";
        public const string SetRequiredSigner = "SetRequiredSigner";
        public const string License = "License";
        public const string PagesSimulateError = "PagesSimulateError";
        public const string ApiSimulateError = "ApiSimulateError";
        public const string ExperimentsSearchSideBySide = "ExperimentsSearchSideBySide";
        public const string PackageReflowAction = "PackageReflowAction";
        public const string PackageRevalidateAction = "PackageRevalidateAction";
        public const string PackageRevalidateSymbolsAction = "PackageRevalidateSymbolsAction";
        public const string Send2FAFeedback = "Send2FAFeedback";
    }
}