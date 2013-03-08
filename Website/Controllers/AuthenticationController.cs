﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;
using NuGetGallery.Infrastructure;
using WorldDomination.Web.Authentication;
using WorldDomination.Web.Authentication.Mvc;

namespace NuGetGallery
{
    public partial class AuthenticationController : Controller
    {
        public IFormsAuthenticationService FormsAuth { get; protected set; }
        public IUserService Users { get; protected set; }
        public ICryptographyService Crypto { get; protected set; }

        // For sub-classes to initialize services themselves
        protected AuthenticationController()
        {
        }

        public AuthenticationController(
            IFormsAuthenticationService formsAuthService,
            IUserService userService,
            ICryptographyService cryptoService)
        {
            FormsAuth = formsAuthService;
            Users = userService;
            Crypto = cryptoService;
        }

        [RequireRemoteHttps(OnlyWhenAuthenticated = false)]
        public virtual ActionResult LogOn(string returnUrl)
        {
            // I think it should be obvious why we don't want the current URL to be the return URL here ;)
            ViewData[Constants.ReturnUrlViewDataKey] = returnUrl;
            return View(new SignInRequest() { ReturnUrl = returnUrl });
        }

        [HttpPost]
        [RequireRemoteHttps(OnlyWhenAuthenticated = false)]
        public virtual ActionResult LogOn(SignInRequest request, string returnUrl)
        {
            // I think it should be obvious why we don't want the current URL to be the return URL here ;)
            ViewData[Constants.ReturnUrlViewDataKey] = returnUrl;

            // TODO: improve the styling of the validation summary
            // TODO: modify the Object.cshtml partial to make the first text box autofocus, or use additional metadata

            if (!ModelState.IsValid)
            {
                return View();
            }

            var user = Users.FindByUsernameOrEmailAddressAndPassword(
                request.UserNameOrEmail,
                request.Password);

            if (user == null)
            {
                ModelState.AddModelError(
                    String.Empty,
                    Strings.UserNotFound);

                return View();
            }

            if (!user.Confirmed)
            {
                ViewBag.ConfirmationRequired = true;
                return View();
            }

            IEnumerable<string> roles = null;
            if (user.Roles.AnySafe())
            {
                roles = user.Roles.Select(r => r.Name);
            }

            FormsAuth.SetAuthCookie(
                user.Username,
                true,
                roles);

            return SafeRedirect(returnUrl);
        }

        [HttpGet]
        public virtual ActionResult LinkOrCreateUser(string token, string returnUrl)
        {
            // Set the returnURL for the login link.
            ViewData[Constants.ReturnUrlViewDataKey] = returnUrl;

            // Deserialize the token
            OAuthLinkToken linkToken = DecodeToken(token);

            // Send down the view model
            return View(new LinkOrCreateViewModel()
            {
                CreateModel = new LinkOrCreateViewModel.CreateViewModel()
                {
                    Username = Regex.IsMatch(linkToken.UserName, Constants.UserNameRegex) ? linkToken.UserName : null,
                    EmailAddress = linkToken.EmailAddress
                },
                LinkModel = new LinkOrCreateViewModel.LinkViewModel()
                {
                    UserNameOrEmail = linkToken.EmailAddress
                }
            });
        }

        [HttpPost]
        public virtual ActionResult LinkOrCreateUser(LinkOrCreateViewModel model, string token, string returnUrl)
        {
            // Decode the token
            OAuthLinkToken linkToken = DecodeToken(token);

            return Json(model);
        }

        public virtual ActionResult LogOff(string returnUrl)
        {
            // TODO: this should really be a POST

            FormsAuth.SignOut();

            return SafeRedirect(returnUrl);
        }

        [NonAction]
        public virtual ActionResult SafeRedirect(string returnUrl)
        {
            if (!String.IsNullOrWhiteSpace(returnUrl)
                && Url.IsLocalUrl(returnUrl)
                && returnUrl.Length > 1
                && returnUrl.StartsWith("/", StringComparison.Ordinal)
                && !returnUrl.StartsWith("//", StringComparison.Ordinal)
                && !returnUrl.StartsWith("/\\", StringComparison.Ordinal))
            {
                return Redirect(returnUrl);
            }

            return Redirect(Url.Home());
        }

        private OAuthLinkToken DecodeToken(string token)
        {
            return OAuthLinkToken.FromToken(
                            Crypto.DecryptString(token, OAuthLinkToken.CryptoPurpose));
        }
    }
}
