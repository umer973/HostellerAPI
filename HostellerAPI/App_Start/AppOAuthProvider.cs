

namespace HostellerAPI.App_Start
{
    using Microsoft.Owin.Security;
    using Microsoft.Owin.Security.Cookies;
    using Microsoft.Owin.Security.OAuth;
    using System;
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Modals;
    using BusinessLogic;
    using System.Data;
    using Common;
    using CommonLib.Encryption;

    public class AppOAuthProvider : OAuthAuthorizationServerProvider
    {

        /// <returns>Returns validation of client authentication</returns>  
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            // Resource owner password credentials does not provide a client ID.  

            context.Validated();
        }
        private readonly string _publicClientId;

        /// <summary>  
        /// Database Store property.  
        /// </summary>  
        private LoginBL _loginBL = new LoginBL();

        private static DataTable LoginDetails = null;

        //public AppOAuthProvider(string publicClientId)
        //{
        //    //TODO: Pull from configuration  
        //    if (publicClientId == null)
        //    {
        //        throw new ArgumentNullException(nameof(publicClientId));
        //    }

        //    // Settings.  
        //    _publicClientId = publicClientId;
        //}

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {


            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            var user = new User
            {
                username = context.UserName,
                password = Encryption.encrypt(context.Password),
            };

            LoginDetails = _loginBL.Login(user);

            if (LoginDetails != null && LoginDetails.Rows.Count > 0)
            {
                //identity.AddClaim(new Claim("username", user.username));
                //identity.AddClaim(new Claim(ClaimTypes.Name, user.username));
                //context.Validated(identity);

                ClaimsIdentity oAuthIdentity = new ClaimsIdentity(context.Options.AuthenticationType);
                ClaimsIdentity cookiesIdentity = new ClaimsIdentity(context.Options.AuthenticationType);
                AuthenticationProperties properties = CreateProperties(context.UserName, LoginDetails);
                AuthenticationTicket ticket = new AuthenticationTicket(oAuthIdentity, properties);
                context.Validated(ticket);
                context.Request.Context.Authentication.SignIn(cookiesIdentity);
            }
            else
            {
                context.SetError("Invalid credentials");
                return;
            }
        }
        public static AuthenticationProperties CreateProperties(string userName, DataTable loginData)
        {
            IDictionary<string, string> data = new Dictionary<string, string>();
            data.Add("Username", userName);
            if (loginData != null && loginData.Rows.Count > 0)
            {
                data.Add("UserId", loginData.Rows[0]["UserId"].ToString());
                data.Add("UserType", loginData.Rows[0]["UserType"].ToString());
                data.Add("Password", Encryption.Decrypt(loginData.Rows[0]["Password"].ToString()));
                data.Add("Email", loginData.Rows[0]["Email"].ToString());
            }
            return new AuthenticationProperties(data);
        }
        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                // Adding.  
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            // Return info.  
            return Task.FromResult<object>(LoginDetails);

        }

        /// <summary>  
        /// Validate Client authntication override method  
        /// </summary>  
        /// <param name="context">Contect parameter</param>  



        #region Validate client redirect URI override method  

        /// <summary>  
        /// Validate client redirect URI override method  
        /// </summary>  
        /// <param name="context">Context parmeter</param>  
        /// <returns>Returns validation of client redirect URI</returns>  
        public override Task ValidateClientRedirectUri(OAuthValidateClientRedirectUriContext context)
        {
            // Verification.  
            if (context.ClientId == _publicClientId)
            {
                // Initialization.  
                Uri expectedRootUri = new Uri(context.Request.Uri, "/");

                // Verification.  
                if (expectedRootUri.AbsoluteUri == context.RedirectUri)
                {
                    // Validating.  
                    context.Validated();
                }
            }

            // Return info.  
            return Task.FromResult<object>(null);
        }

        #endregion

        #region Create Authentication properties method.  

        /// <summary>  
        /// Create Authentication properties method.  
        /// </summary>  
        /// <param name="userName">User name parameter</param>  
        /// <returns>Returns authenticated properties.</returns>  
        public static AuthenticationProperties CreateProperties(string userName)
        {
            // Settings.  
            IDictionary<string, string> data = new Dictionary<string, string>
                                               {
                                                   { "userName", userName }
                                               };

            // Return info.  
            return new AuthenticationProperties(data);
        }

        #endregion



    }
}