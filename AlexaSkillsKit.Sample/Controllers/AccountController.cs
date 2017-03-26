// --------------------------------------------------------------------------------------------------------------------
// <copyright company="" file="AccountController.cs">
//
// </copyright>
// <summary>
//   The account controller.
// </summary>
//
// --------------------------------------------------------------------------------------------------------------------
namespace Sample.Controllers
{
    /// <summary>
    ///     The account controller.
    /// </summary>
    [System.Web.Http.Authorize]
    [System.Web.Http.RoutePrefix("api/Account")]
    public class AccountController : System.Web.Http.ApiController
    {
        /// <summary>
        ///     The local login provider.
        /// </summary>
        private const string LocalLoginProvider = "Local";

        /// <summary>
        ///     The _user manager.
        /// </summary>
        private ApplicationUserManager _userManager;

        /// <summary>
        ///     Initializes a new instance of the <see cref="AccountController" /> class.
        /// </summary>
        public AccountController()
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountController"/> class.
        /// </summary>
        /// <param name="userManager">
        /// The user manager.
        /// </param>
        /// <param name="accessTokenFormat">
        /// The access token format.
        /// </param>
        public AccountController(
            ApplicationUserManager userManager, Microsoft.Owin.Security.ISecureDataFormat<Microsoft.Owin.Security.AuthenticationTicket> accessTokenFormat)
        {
            this.UserManager = userManager;
            this.AccessTokenFormat = accessTokenFormat;
        }

        /// <summary>
        ///     Gets the access token format.
        /// </summary>
        public Microsoft.Owin.Security.ISecureDataFormat<Microsoft.Owin.Security.AuthenticationTicket> AccessTokenFormat { get; private set; }

        /// <summary>
        ///     Gets the user manager.
        /// </summary>
        public ApplicationUserManager UserManager
        {
            get
            {
                return this._userManager
                       ?? Microsoft.AspNet.Identity.Owin.OwinContextExtensions
                           .GetUserManager<ApplicationUserManager>(
                               System.Net.Http.OwinHttpRequestMessageExtensions.GetOwinContext(
                                   this.Request));
            }

            private set
            {
                this._userManager = value;
            }
        }

        /// <summary>
        ///     Gets the authentication.
        /// </summary>
        private Microsoft.Owin.Security.IAuthenticationManager Authentication
        {
            get
            {
                return
                    System.Net.Http.OwinHttpRequestMessageExtensions.GetOwinContext(
                        this.Request).Authentication;
            }
        }

        // POST api/Account/AddExternalLogin
        /// <summary>
        /// The add external login.
        /// </summary>
        /// <param name="model">
        /// The model.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [System.Web.Http.Route("AddExternalLogin")]
        public async System.Threading.Tasks.Task<System.Web.Http.IHttpActionResult> AddExternalLogin
            (Models.AddExternalLoginBindingModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            this.Authentication.SignOut(
                Microsoft.AspNet.Identity.DefaultAuthenticationTypes.ExternalCookie);

            Microsoft.Owin.Security.AuthenticationTicket ticket =
                this.AccessTokenFormat.Unprotect(model.ExternalAccessToken);

            if (ticket == null || ticket.Identity == null
                || (ticket.Properties != null && ticket.Properties.ExpiresUtc.HasValue
                     && ticket.Properties.ExpiresUtc.Value < System.DateTimeOffset.UtcNow))
            {
                return this.BadRequest("External login failure.");
            }

            ExternalLoginData externalData = ExternalLoginData.FromIdentity(ticket.Identity);

            if (externalData == null)
            {
                return this.BadRequest("The external login is already associated with an account.");
            }

            Microsoft.AspNet.Identity.IdentityResult result =
                await
                    this.UserManager.AddLoginAsync(
                        Microsoft.AspNet.Identity.IdentityExtensions.GetUserId(
                            this.User.Identity),
                        new Microsoft.AspNet.Identity.UserLoginInfo(
                            externalData.LoginProvider,
                            externalData.ProviderKey));

            if (!result.Succeeded)
            {
                return this.GetErrorResult(result);
            }

            return this.Ok();
        }

        // POST api/Account/ChangePassword
        /// <summary>
        /// The change password.
        /// </summary>
        /// <param name="model">
        /// The model.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [System.Web.Http.Route("ChangePassword")]
        public async System.Threading.Tasks.Task<System.Web.Http.IHttpActionResult> ChangePassword(
            Models.ChangePasswordBindingModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            Microsoft.AspNet.Identity.IdentityResult result =
                await
                    this.UserManager.ChangePasswordAsync(
                        Microsoft.AspNet.Identity.IdentityExtensions.GetUserId(
                            this.User.Identity),
                        model.OldPassword,
                        model.NewPassword);

            if (!result.Succeeded)
            {
                return this.GetErrorResult(result);
            }

            return this.Ok();
        }

        // GET api/Account/ExternalLogin
        /// <summary>
        /// The get external login.
        /// </summary>
        /// <param name="provider">
        /// The provider.
        /// </param>
        /// <param name="error">
        /// The error.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [System.Web.Http.OverrideAuthentication]
        [System.Web.Http.HostAuthentication(
             Microsoft.AspNet.Identity.DefaultAuthenticationTypes.ExternalCookie)]
        [System.Web.Http.AllowAnonymous]
        [System.Web.Http.Route("ExternalLogin", Name = "ExternalLogin")]
        public async System.Threading.Tasks.Task<System.Web.Http.IHttpActionResult> GetExternalLogin
            (string provider, string error = null)
        {
            if (error != null)
            {
                return
                    this.Redirect(
                        this.Url.Content("~/") + "#error=" + System.Uri.EscapeDataString(error));
            }

            if (!this.User.Identity.IsAuthenticated)
            {
                return new Results.ChallengeResult(provider, this);
            }

            ExternalLoginData externalLogin =
                ExternalLoginData.FromIdentity(
                    this.User.Identity as System.Security.Claims.ClaimsIdentity);

            if (externalLogin == null)
            {
                return this.InternalServerError();
            }

            if (externalLogin.LoginProvider != provider)
            {
                this.Authentication.SignOut(
                    Microsoft.AspNet.Identity.DefaultAuthenticationTypes.ExternalCookie);
                return new Results.ChallengeResult(provider, this);
            }

            Models.ApplicationUser user =
                await
                    this.UserManager.FindAsync(
                        new Microsoft.AspNet.Identity.UserLoginInfo(
                            externalLogin.LoginProvider,
                            externalLogin.ProviderKey));

            bool hasRegistered = user != null;

            if (hasRegistered)
            {
                this.Authentication.SignOut(
                    Microsoft.AspNet.Identity.DefaultAuthenticationTypes.ExternalCookie);

                System.Security.Claims.ClaimsIdentity oAuthIdentity =
                    await
                        user.GenerateUserIdentityAsync(
                            this.UserManager,
                            Microsoft.Owin.Security.OAuth.OAuthDefaults.AuthenticationType);
                System.Security.Claims.ClaimsIdentity cookieIdentity =
                    await
                        user.GenerateUserIdentityAsync(
                            this.UserManager,
                            Microsoft.Owin.Security.Cookies.CookieAuthenticationDefaults
                                .AuthenticationType);

                Microsoft.Owin.Security.AuthenticationProperties properties =
                    Providers.ApplicationOAuthProvider.CreateProperties(user.UserName);
                this.Authentication.SignIn(properties, oAuthIdentity, cookieIdentity);
            }
            else
            {
                System.Collections.Generic.IEnumerable<System.Security.Claims.Claim> claims =
                    externalLogin.GetClaims();
                System.Security.Claims.ClaimsIdentity identity =
                    new System.Security.Claims.ClaimsIdentity(
                        claims,
                        Microsoft.Owin.Security.OAuth.OAuthDefaults.AuthenticationType);
                this.Authentication.SignIn(identity);
            }

            return this.Ok();
        }

        // GET api/Account/ExternalLogins?returnUrl=%2F&generateState=true
        /// <summary>
        /// The get external logins.
        /// </summary>
        /// <param name="returnUrl">
        /// The return url.
        /// </param>
        /// <param name="generateState">
        /// The generate state.
        /// </param>
        /// <returns>
        /// The <see cref="IEnumerable"/>.
        /// </returns>
        [System.Web.Http.AllowAnonymous]
        [System.Web.Http.Route("ExternalLogins")]
        public System.Collections.Generic.IEnumerable<Models.ExternalLoginViewModel> GetExternalLogins(
            string returnUrl,
            bool generateState = false)
        {
            System.Collections.Generic.IEnumerable<Microsoft.Owin.Security.AuthenticationDescription> descriptions =
                    Microsoft.Owin.Security.AuthenticationManagerExtensions
                        .GetExternalAuthenticationTypes(this.Authentication);
            System.Collections.Generic.List<Models.ExternalLoginViewModel> logins =
                new System.Collections.Generic.List<Models.ExternalLoginViewModel>();

            string state;

            if (generateState)
            {
                const int strengthInBits = 256;
                state = RandomOAuthStateGenerator.Generate(strengthInBits);
            }
            else
            {
                state = null;
            }

            foreach (Microsoft.Owin.Security.AuthenticationDescription description in descriptions)
            {
                Models.ExternalLoginViewModel login = new Models.ExternalLoginViewModel
                {
                    Name = description.Caption,
                    Url =
                                                                    this.Url.Route(
                                                                        "ExternalLogin",
                                                                        new
                                                                        {
                                                                            provider =
                                                                                description.AuthenticationType,
                                                                            response_type = "token",
                                                                            client_id = Startup.PublicClientId,
                                                                            redirect_uri =
                                                                                new System.Uri(
                                                                                    this.Request.RequestUri,
                                                                                    returnUrl).AbsoluteUri,
                                                                            state = state
                                                                        }),
                    State = state
                };
                logins.Add(login);
            }

            return logins;
        }

        // GET api/Account/ManageInfo?returnUrl=%2F&generateState=true
        /// <summary>
        /// The get manage info.
        /// </summary>
        /// <param name="returnUrl">
        /// The return url.
        /// </param>
        /// <param name="generateState">
        /// The generate state.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [System.Web.Http.Route("ManageInfo")]
        public async System.Threading.Tasks.Task<Models.ManageInfoViewModel> GetManageInfo(
            string returnUrl,
            bool generateState = false)
        {
            Microsoft.AspNet.Identity.EntityFramework.IdentityUser user =
                await
                    this.UserManager.FindByIdAsync(
                        Microsoft.AspNet.Identity.IdentityExtensions.GetUserId(
                            this.User.Identity));

            if (user == null)
            {
                return null;
            }

            System.Collections.Generic.List<Models.UserLoginInfoViewModel> logins =
                new System.Collections.Generic.List<Models.UserLoginInfoViewModel>();

            foreach (Microsoft.AspNet.Identity.EntityFramework.IdentityUserLogin linkedAccount in
                user.Logins)
            {
                logins.Add(
                    new Models.UserLoginInfoViewModel
                    {
                        LoginProvider = linkedAccount.LoginProvider,
                        ProviderKey = linkedAccount.ProviderKey
                    });
            }

            if (user.PasswordHash != null)
            {
                logins.Add(
                    new Models.UserLoginInfoViewModel
                    {
                        LoginProvider = LocalLoginProvider,
                        ProviderKey = user.UserName,
                    });
            }

            return new Models.ManageInfoViewModel
            {
                LocalLoginProvider = LocalLoginProvider,
                Email = user.UserName,
                Logins = logins,
                ExternalLoginProviders = this.GetExternalLogins(returnUrl, generateState)
            };
        }

        // GET api/Account/UserInfo
        /// <summary>
        ///     The get user info.
        /// </summary>
        /// <returns>
        ///     The <see cref="UserInfoViewModel" />.
        /// </returns>
        [System.Web.Http.HostAuthentication(
             Microsoft.AspNet.Identity.DefaultAuthenticationTypes.ExternalBearer)]
        [System.Web.Http.Route("UserInfo")]
        public Models.UserInfoViewModel GetUserInfo()
        {
            ExternalLoginData externalLogin =
                ExternalLoginData.FromIdentity(
                    this.User.Identity as System.Security.Claims.ClaimsIdentity);

            return new Models.UserInfoViewModel
            {
                Email =
                               Microsoft.AspNet.Identity.IdentityExtensions.GetUserName(
                                   this.User.Identity),
                HasRegistered = externalLogin == null,
                LoginProvider = externalLogin != null ? externalLogin.LoginProvider : null
            };
        }

        // POST api/Account/Logout
        /// <summary>
        ///     The logout.
        /// </summary>
        /// <returns>
        ///     The <see cref="IHttpActionResult" />.
        /// </returns>
        [System.Web.Http.Route("Logout")]
        public System.Web.Http.IHttpActionResult Logout()
        {
            this.Authentication.SignOut(
                Microsoft.Owin.Security.Cookies.CookieAuthenticationDefaults.AuthenticationType);
            return this.Ok();
        }

        // POST api/Account/Register
        /// <summary>
        /// The register.
        /// </summary>
        /// <param name="model">
        /// The model.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [System.Web.Http.AllowAnonymous]
        [System.Web.Http.Route("Register")]
        public async System.Threading.Tasks.Task<System.Web.Http.IHttpActionResult> Register(
            Models.RegisterBindingModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var user = new Models.ApplicationUser() { UserName = model.Email, Email = model.Email };

            Microsoft.AspNet.Identity.IdentityResult result =
                await this.UserManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                return this.GetErrorResult(result);
            }

            return this.Ok();
        }

        // POST api/Account/RegisterExternal
        /// <summary>
        /// The register external.
        /// </summary>
        /// <param name="model">
        /// The model.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [System.Web.Http.OverrideAuthentication]
        [System.Web.Http.HostAuthentication(
             Microsoft.AspNet.Identity.DefaultAuthenticationTypes.ExternalBearer)]
        [System.Web.Http.Route("RegisterExternal")]
        public async System.Threading.Tasks.Task<System.Web.Http.IHttpActionResult> RegisterExternal
            (Models.RegisterExternalBindingModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var info =
                await
                    Microsoft.Owin.Security.AuthenticationManagerExtensions
                        .GetExternalLoginInfoAsync(this.Authentication);
            if (info == null)
            {
                return this.InternalServerError();
            }

            var user = new Models.ApplicationUser() { UserName = model.Email, Email = model.Email };

            Microsoft.AspNet.Identity.IdentityResult result =
                await this.UserManager.CreateAsync(user);
            if (!result.Succeeded)
            {
                return this.GetErrorResult(result);
            }

            result = await this.UserManager.AddLoginAsync(user.Id, info.Login);
            if (!result.Succeeded)
            {
                return this.GetErrorResult(result);
            }

            return this.Ok();
        }

        // POST api/Account/RemoveLogin
        /// <summary>
        /// The remove login.
        /// </summary>
        /// <param name="model">
        /// The model.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [System.Web.Http.Route("RemoveLogin")]
        public async System.Threading.Tasks.Task<System.Web.Http.IHttpActionResult> RemoveLogin(
            Models.RemoveLoginBindingModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            Microsoft.AspNet.Identity.IdentityResult result;

            if (model.LoginProvider == LocalLoginProvider)
            {
                result =
                    await
                        this.UserManager.RemovePasswordAsync(
                            Microsoft.AspNet.Identity.IdentityExtensions.GetUserId(
                                this.User.Identity));
            }
            else
            {
                result =
                    await
                        this.UserManager.RemoveLoginAsync(
                            Microsoft.AspNet.Identity.IdentityExtensions.GetUserId(
                                this.User.Identity),
                            new Microsoft.AspNet.Identity.UserLoginInfo(
                                model.LoginProvider,
                                model.ProviderKey));
            }

            if (!result.Succeeded)
            {
                return this.GetErrorResult(result);
            }

            return this.Ok();
        }

        // POST api/Account/SetPassword
        /// <summary>
        /// The set password.
        /// </summary>
        /// <param name="model">
        /// The model.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [System.Web.Http.Route("SetPassword")]
        public async System.Threading.Tasks.Task<System.Web.Http.IHttpActionResult> SetPassword(
            Models.SetPasswordBindingModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            Microsoft.AspNet.Identity.IdentityResult result =
                await
                    this.UserManager.AddPasswordAsync(
                        Microsoft.AspNet.Identity.IdentityExtensions.GetUserId(
                            this.User.Identity),
                        model.NewPassword);

            if (!result.Succeeded)
            {
                return this.GetErrorResult(result);
            }

            return this.Ok();
        }

        /// <summary>
        /// The dispose.
        /// </summary>
        /// <param name="disposing">
        /// The disposing.
        /// </param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && this._userManager != null)
            {
                this._userManager.Dispose();
                this._userManager = null;
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// The get error result.
        /// </summary>
        /// <param name="result">
        /// The result.
        /// </param>
        /// <returns>
        /// The <see cref="IHttpActionResult"/>.
        /// </returns>
        private System.Web.Http.IHttpActionResult GetErrorResult(
            Microsoft.AspNet.Identity.IdentityResult result)
        {
            if (result == null)
            {
                return this.InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        this.ModelState.AddModelError(string.Empty, error);
                    }
                }

                if (this.ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return this.BadRequest();
                }

                return this.BadRequest(this.ModelState);
            }

            return null;
        }

        /// <summary>
        ///     The random o auth state generator.
        /// </summary>
        private static class RandomOAuthStateGenerator
        {
            /// <summary>
            ///     The _random.
            /// </summary>
            private static System.Security.Cryptography.RandomNumberGenerator _random =
                new System.Security.Cryptography.RNGCryptoServiceProvider();

            /// <summary>
            /// The generate.
            /// </summary>
            /// <param name="strengthInBits">
            /// The strength in bits.
            /// </param>
            /// <returns>
            /// The <see cref="string"/>.
            /// </returns>
            /// <exception cref="ArgumentException">
            /// </exception>
            public static string Generate(int strengthInBits)
            {
                const int bitsPerByte = 8;

                if (strengthInBits % bitsPerByte != 0)
                {
                    throw new System.ArgumentException(
                              "strengthInBits must be evenly divisible by 8.",
                              "strengthInBits");
                }

                int strengthInBytes = strengthInBits / bitsPerByte;

                byte[] data = new byte[strengthInBytes];
                _random.GetBytes(data);
                return System.Web.HttpServerUtility.UrlTokenEncode(data);
            }
        }

        /// <summary>
        ///     The external login data.
        /// </summary>
        private class ExternalLoginData
        {
            /// <summary>
            ///     Gets or sets the login provider.
            /// </summary>
            public string LoginProvider { get; set; }

            /// <summary>
            ///     Gets or sets the provider key.
            /// </summary>
            public string ProviderKey { get; set; }

            /// <summary>
            ///     Gets or sets the user name.
            /// </summary>
            public string UserName { get; set; }

            /// <summary>
            /// The from identity.
            /// </summary>
            /// <param name="identity">
            /// The identity.
            /// </param>
            /// <returns>
            /// The <see cref="ExternalLoginData"/>.
            /// </returns>
            public static ExternalLoginData FromIdentity(
                System.Security.Claims.ClaimsIdentity identity)
            {
                if (identity == null)
                {
                    return null;
                }

                System.Security.Claims.Claim providerKeyClaim =
                    identity.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);

                if (providerKeyClaim == null || string.IsNullOrEmpty(providerKeyClaim.Issuer)
                    || string.IsNullOrEmpty(providerKeyClaim.Value))
                {
                    return null;
                }

                if (providerKeyClaim.Issuer
                    == System.Security.Claims.ClaimsIdentity.DefaultIssuer)
                {
                    return null;
                }

                return new ExternalLoginData
                {
                    LoginProvider = providerKeyClaim.Issuer,
                    ProviderKey = providerKeyClaim.Value,
                    UserName =
                                   Microsoft.AspNet.Identity.IdentityExtensions.FindFirstValue(
                                       identity,
                                       System.Security.Claims.ClaimTypes.Name)
                };
            }

            /// <summary>
            ///     The get claims.
            /// </summary>
            /// <returns>
            ///     The <see cref="IList" />.
            /// </returns>
            public System.Collections.Generic.IList<System.Security.Claims.Claim> GetClaims()
            {
                System.Collections.Generic.IList<System.Security.Claims.Claim> claims =
                    new System.Collections.Generic.List<System.Security.Claims.Claim>();
                claims.Add(
                    new System.Security.Claims.Claim(
                        System.Security.Claims.ClaimTypes.NameIdentifier,
                        this.ProviderKey,
                        null,
                        this.LoginProvider));

                if (this.UserName != null)
                {
                    claims.Add(
                        new System.Security.Claims.Claim(
                            System.Security.Claims.ClaimTypes.Name,
                            this.UserName,
                            null,
                            this.LoginProvider));
                }

                return claims;
            }
        }
    }
}