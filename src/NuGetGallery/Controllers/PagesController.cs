﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using NuGetGallery.Areas.Admin;
using NuGetGallery.Filters;
using NuGetGallery.Infrastructure.Mail;
using NuGetGallery.Infrastructure.Mail.Messages;
using NuGetGallery.Infrastructure.Mail.Requests;
using NuGetGallery.ViewModels;

namespace NuGetGallery
{
    public partial class PagesController
        : AppController
    {
        private readonly IContentService _contentService;
        private readonly IContentObjectService _contentObjectService;
        private readonly IMessageService _messageService;
        private readonly ISupportRequestService _supportRequestService;
        private readonly IMessageServiceConfiguration _messageServiceConfiguration;

        protected PagesController() { }
        public PagesController(
            IContentService contentService,
            IContentObjectService contentObjectService,
            IMessageService messageService,
            ISupportRequestService supportRequestService,
            IMessageServiceConfiguration messageServiceConfiguration)
        {
            _contentService = contentService ?? throw new ArgumentNullException(nameof(contentService));
            _contentObjectService = contentObjectService ?? throw new ArgumentNullException(nameof(contentObjectService));
            _messageService = messageService ?? throw new ArgumentNullException(nameof(messageService));
            _supportRequestService = supportRequestService ?? throw new ArgumentNullException(nameof(supportRequestService));
            _messageServiceConfiguration = messageServiceConfiguration ?? throw new ArgumentNullException(nameof(messageServiceConfiguration));
        }

        // This will let you add 'static' cshtml pages to the site under View/Pages or Branding/Views/Pages
        [HttpGet]
        public virtual ActionResult Page(string pageName)
        {
            // Prevent traversal attacks and serving non-pages by disallowing ., /, %, and more!
            if (pageName == null || pageName.Any(c => !Char.IsLetterOrDigit(c)))
            {
                return HttpNotFound();
            }

            return View(pageName);
        }

        [HttpGet]
        public virtual ActionResult About()
        {
            return View();
        }

        [HttpGet]
        public virtual ActionResult Contact()
        {
            return View(new ContactSupportViewModel());
        }

        [HttpGet]
        public virtual ActionResult Downloads()
        {
            return View();
        }

        [HttpPost]
        [UIAuthorize]
        [ValidateAntiForgeryToken]
        public virtual async Task<ActionResult> Contact(ContactSupportViewModel contactForm)
        {
            if (!ModelState.IsValid)
            {
                return View(contactForm);
            }

            // since HTML is allowed in these fields, encode it to avoid malicious HTML
            contactForm.Message = HttpUtility.HtmlEncode(contactForm.Message);
            contactForm.SubjectLine = HttpUtility.HtmlEncode(contactForm.SubjectLine);

            var user = GetCurrentUser();
            var request = new ContactSupportRequest
            {
                CopySender = contactForm.CopySender,
                Message = contactForm.Message,
                SubjectLine = contactForm.SubjectLine,
                FromAddress = user.ToMailAddress(),
                RequestingUser = user
            };

            var subject = $"Support Request for user '{user.Username}'";
            await _supportRequestService.AddNewSupportRequestAsync(subject, contactForm.Message, user.EmailAddress, "Other", user);

            var emailMessage = new ContactSupportMessage(_messageServiceConfiguration, request);
            await _messageService.SendMessageAsync(emailMessage);

            ModelState.Clear();

            TempData["Message"] = "Your message has been sent to support. We'll be in contact with you shortly.";

            return View();
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Head)]
        public virtual ActionResult Home()
        {
            var identity = OwinContext.Authentication?.User?.Identity as ClaimsIdentity;
            var showTransformModal = ClaimsExtensions.HasDiscontinuedLoginClaims(identity);
            var user = GetCurrentUser();
            var transformIntoOrganization = _contentObjectService
                .LoginDiscontinuationConfiguration
                .ShouldUserTransformIntoOrganization(user);
            var externalIdentityList = ClaimsExtensions.GetExternalCredentialIdentityList(identity);
            return View(new GalleryHomeViewModel(showTransformModal, transformIntoOrganization, externalIdentityList));
        }

        [HttpGet]
        public virtual ActionResult EmptyHome()
        {
            return new HttpStatusCodeResult(HttpStatusCode.OK, "Empty Home");
        }

        [HttpGet]
        public virtual async Task<ActionResult> Terms()
        {
            if (_contentService != null)
            {
                ViewBag.Content = await _contentService.GetContentItemAsync(
                    Constants.ContentNames.TermsOfUse,
                    TimeSpan.FromDays(1));
            }
            return View();
        }

        [HttpGet]
        public virtual async Task<ActionResult> Privacy()
        {
            if (_contentService != null)
            {
                ViewBag.Content = await _contentService.GetContentItemAsync(
                    Constants.ContentNames.PrivacyPolicy,
                    TimeSpan.FromDays(1));
            }
            return View();
        }
    }
}