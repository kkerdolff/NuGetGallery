﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Security.Principal;
using NuGet.Versioning;

namespace NuGetGallery
{
    public interface ITelemetryService
    {
        void TrackODataQueryFilterEvent(string callContext, bool isEnabled, bool isAllowed, string queryPattern);

        void TrackPackagePushEvent(Package package, User user, IIdentity identity);

        void TrackPackagePushFailureEvent(string id, NuGetVersion version);

        void TrackPackageUnlisted(Package package);

        void TrackPackageListed(Package package);

        void TrackPackageDelete(Package package, bool isHardDelete);

        void TrackPackageReupload(Package package);

        void TrackPackageReflow(Package package);

        void TrackPackageHardDeleteReflow(string packageId, string packageVersion);

        void TrackPackageRevalidate(Package package);

        void TrackPackageReadMeChangeEvent(Package package, string readMeSourceType, PackageEditReadMeState readMeState);

        void TrackCreatePackageVerificationKeyEvent(string packageId, string packageVersion, User user, IIdentity identity);

        void TrackPackagePushNamespaceConflictEvent(string packageId, string packageVersion, User user, IIdentity identity);

        void TrackVerifyPackageKeyEvent(string packageId, string packageVersion, User user, IIdentity identity, int statusCode);

        void TrackNewUserRegistrationEvent(User user, Credential identity);

        void TrackUserChangedMultiFactorAuthentication(User user, bool enabledMultiFactorAuth);

        void TrackNewCredentialCreated(User user, Credential credential);

        void TrackUserLogin(User user, Credential credential, bool wasMultiFactorAuthenticated);

        /// <summary>
        /// A telemetry event emitted when the service checks whether a user package delete is allowed.
        /// </summary>
        void TrackUserPackageDeleteChecked(UserPackageDeleteEvent details, UserPackageDeleteOutcome outcome);

        void TrackPackageMetadataComplianceError(string packageId, string packageVersion, IEnumerable<string> complianceFailures);

        void TrackPackageMetadataComplianceWarning(string packageId, string packageVersion, IEnumerable<string> complianceWarnings);

        /// <summary>
        /// A telemetry event emitted when a user package delete is executed.
        /// </summary>
        void TrackUserPackageDeleteExecuted(int packageKey, string packageId, string packageVersion, ReportPackageReason reason, bool success);

        /// <summary>
        /// A telemetry event emitted when a certificate is added to the database.
        /// </summary>
        /// <param name="thumbprint">The certificate thumbprint.</param>
        /// <exception cref="ArgumentException">Thrown if <paramref name="thumbprint" /> is <c>null</c>
        /// or empty.</exception>
        void TrackCertificateAdded(string thumbprint);

        /// <summary>
        /// A telemetry event emitted when a package owner was automatically added to a package registration.
        /// </summary>
        /// <param name="packageId">The package registration id.</param>
        /// <param name="packageVersion">The normalized package version.</param>
        void TrackPackageOwnershipAutomaticallyAdded(string packageId, string packageVersion);

        /// <summary>
        /// A telemetry event emitted when a certificate is activated for an account.
        /// </summary>
        /// <param name="thumbprint">The certificate thumbprint.</param>
        /// <exception cref="ArgumentException">Thrown if <paramref name="thumbprint" /> is <c>null</c>
        /// or empty.</exception>
        void TrackCertificateActivated(string thumbprint);

        /// <summary>
        /// A telemetry event emitted when a certificate is deactivated for an account.
        /// </summary>
        /// <param name="thumbprint">The certificate thumbprint.</param>
        /// <exception cref="ArgumentException">Thrown if <paramref name="thumbprint" /> is <c>null</c>
        /// or empty.</exception>
        void TrackCertificateDeactivated(string thumbprint);

        /// <summary>
        /// A telemetry event emitted when the required signer is set on a package registration.
        /// </summary>
        /// <param name="packageId">The package ID.</param>
        /// <exception cref="ArgumentException">Thrown if <paramref name="thumbprint" /> is <c>null</c>
        /// or empty.</exception>
        void TrackRequiredSignerSet(string packageId);

        /// <summary>
        /// A telemetry event emitted when a user requests transformation of their account into an organization.
        /// </summary>
        void TrackOrganizationTransformInitiated(User user);

        /// <summary>
        /// A telemetry event emitted when a user completes transformation of their account into an organization.
        /// </summary>
        void TrackOrganizationTransformCompleted(User user);

        /// <summary>
        /// A telemetry event emitted when a user's request to transform their account into an organization is declined.
        /// </summary>
        void TrackOrganizationTransformDeclined(User user);

        /// <summary>
        /// A telemetry event emitted when a user cancels their request to transform their account into an organization.
        /// </summary>
        void TrackOrganizationTransformCancelled(User user);

        /// <summary>
        /// A telemetry event emitted when a user adds a new organization to their account.
        /// </summary>
        void TrackOrganizationAdded(Organization organization);

        /// <summary>
        /// Create a trace for an exception. These are informational for support requests.
        /// </summary>
        void TraceException(Exception exception);

        /// <summary>
        /// Create a log for an exception. These are warnings for live site.
        /// </summary>
        void TrackException(Exception exception, Action<Dictionary<string, string>> addProperties);

        /// <summary>
        /// A telemetry event emitted when an account is deleted.
        /// </summary>
        /// <param name="deletedUser">The <see cref="User"/> that was deleted.</param>
        /// <param name="deletedBy">The <see cref="User"/> that performed the delete.</param>
        /// <param name="success">The success of the operation.</param>
        void TrackAccountDeletionCompleted(User deletedUser, User deletedBy, bool success);

        /// <summary>
        /// A telemetry event emitted when an account deletion is requested.
        /// </summary>
        /// <param name="user">The <see cref="User"/> requesting the delete.</param>
        void TrackRequestForAccountDeletion(User user);

        /// <summary>
        /// A telemetry event emitted when an email is sent.
        /// </summary>
        /// <param name="smtpUri">URI to the SMTP server</param>
        /// <param name="startTime">The start time of when sending the email is attempted.</param>
        /// <param name="duration">The duration of how long the send took.</param>
        /// <param name="success">Whether sending the email was successful.</param>
        /// <param name="attemptNumber">The number of attempts the message has tried to be sent.</param>
        void TrackSendEmail(string smtpUri, DateTimeOffset startTime, TimeSpan duration, bool success, int attemptNumber);

        /// <summary>
        /// A telemetry event emitted when a symbol package is pushed.
        /// </summary>
        /// <param name="packageId">The id of the package that has the symbols uploaded.</param>
        /// <param name="packageVersion">The version of the package that has the symbols uploaded.</param>
        void TrackSymbolPackagePushEvent(string packageId, string packageVersion);

        /// <summary>
        /// A telemetry event emitted when a symbol package fails to be pushed.
        /// </summary>
        /// <param name="packageId">The id of the package that has the symbols uploaded.</param>
        /// <param name="packageVersion">The version of the package that has the symbols uploaded.</param>
        void TrackSymbolPackagePushFailureEvent(string packageId, string packageVersion);

        /// <summary>
        /// A telemetry event emitted when a symbol package fails Gallery validation.
        /// </summary>
        /// <param name="packageId">The id of the package that has the symbols uploaded.</param>
        /// <param name="packageVersion">The version of the package that has the symbols uploaded.</param>
        void TrackSymbolPackageFailedGalleryValidationEvent(string packageId, string packageVersion);

        /// <summary>
        /// The typosquatting check result for the uploaded package.
        /// </summary>
        /// <param name="packageId">The id of the uploaded package.</param>
        /// <param name="checklistRetrievalTime">The time used to retrieval the checklist from the database.</param>
        /// <param name="algorithmProcessingTime">The time used to finish the typosquatting check algorithm.</param>
        /// <param name ="ownersCheckTime">The time used to double check the owners of collision Ids</param>
        /// <param name="wasUploadBlocked">Whether the uploaded package is blocked because of typosquatting check.</param>
        /// <param name="collisionPackageIds">The list of already existing collision package Ids for this uploaded package.</param>
        /// <param name="checklistLength">The length of checklist for typosquatting check</param>
        void TrackMetricForTyposquattingCheck(
            string packageId,
            TimeSpan checklistRetrievalTime,
            TimeSpan algorithmProcessingTime,
            TimeSpan ownersCheckTime,
            bool wasUploadBlocked,
            List<string> collisionPackageIds,
            int checklistLength);
    }
}