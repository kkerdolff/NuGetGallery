﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Globalization;
using System.Net.Mail;

namespace NuGetGallery.Infrastructure.Mail.Messages
{
    public class PackageValidationTakingTooLongMessage : EmailBuilder
    {
        private readonly IMessageServiceConfiguration _configuration;
        private readonly Package _package;
        private readonly string _packageUrl;

        public PackageValidationTakingTooLongMessage(
            IMessageServiceConfiguration configuration,
            Package package,
            string packageUrl)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _package = package ?? throw new ArgumentNullException(nameof(package));
            _packageUrl = packageUrl ?? throw new ArgumentNullException(nameof(packageUrl));
        }

        public override MailAddress Sender => _configuration.GalleryNoReplyAddress;

        public override IEmailRecipients GetRecipients()
        {
            var to = EmailRecipients.GetOwnersSubscribedToPackagePushedNotification(_package.PackageRegistration);
            return new EmailRecipients(to);
        }

        public override string GetSubject()
            => string.Format(
                CultureInfo.CurrentCulture,
                "[{0}] Package validation taking longer than expected - {1} {2}",
                _configuration.GalleryOwner.DisplayName,
                _package.PackageRegistration.Id,
                _package.Version);

        protected override string GetMarkdownBody()
        {
            string body = "It is taking longer than expected for your package [{1} {2}]({3}) to get published.\n\n" +
                   "We are looking into it and there is no action on you at this time. We’ll send you an email notification when your package has been published.\n\n" +
                   "Thank you for your patience.";

            return string.Format(
                CultureInfo.CurrentCulture,
                body,
                _configuration.GalleryOwner.DisplayName,
                _package.PackageRegistration.Id,
                _package.Version,
                _packageUrl);
        }

        protected override string GetPlainTextBody()
        {
            string body = "It is taking longer than expected for your package {1} {2} ({3}) to get published.\n\n" +
                   "We are looking into it and there is no action on you at this time. We’ll send you an email notification when your package has been published.\n\n" +
                   "Thank you for your patience.";

            return string.Format(
                CultureInfo.CurrentCulture,
                body,
                _configuration.GalleryOwner.DisplayName,
                _package.PackageRegistration.Id,
                _package.Version,
                _packageUrl);
        }
    }
}
