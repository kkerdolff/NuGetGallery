﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Globalization;
using System.Net.Mail;

namespace NuGetGallery.Infrastructure.Mail.Messages
{
    public class OrganizationTransformRejectedMessage : EmailBuilder
    {
        private readonly IMessageServiceConfiguration _configuration;

        public OrganizationTransformRejectedMessage(
            IMessageServiceConfiguration configuration,
            User accountToTransform,
            User adminUser,
            bool isCanceledByAdmin)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

            if (adminUser == null)
            {
                throw new ArgumentNullException(nameof(adminUser));
            }

            AccountToTransform = accountToTransform ?? throw new ArgumentNullException(nameof(accountToTransform));
            AccountToSendTo = isCanceledByAdmin ? accountToTransform : adminUser;
            AccountToReplyTo = isCanceledByAdmin ? adminUser : accountToTransform;
            IsCancelledByAdmin = isCanceledByAdmin;
        }
        public override MailAddress Sender => _configuration.GalleryNoReplyAddress;

        public bool IsCancelledByAdmin { get; }

        public User AccountToSendTo { get; }

        public User AccountToReplyTo { get; }

        public User AccountToTransform { get; }

        public override IEmailRecipients GetRecipients()
        {
            if (!AccountToSendTo.EmailAllowed)
            {
                return EmailRecipients.None;
            }

            return new EmailRecipients(
                to: new[] { AccountToSendTo.ToMailAddress() },
                replyTo: new[] { AccountToReplyTo.ToMailAddress() });
        }

        public override string GetSubject()
            => $"[{_configuration.GalleryOwner.DisplayName}] Transformation of account '{AccountToTransform.Username}' has been cancelled";

        protected override string GetMarkdownBody()
        {
            return GetPlainTextBody();
        }

        protected override string GetPlainTextBody()
        {
            return string.Format(
                CultureInfo.CurrentCulture,
                $@"Transformation of account '{AccountToTransform.Username}' has been cancelled by user '{AccountToReplyTo.Username}'.

Thanks,
The {_configuration.GalleryOwner.DisplayName} Team");
        }
    }
}