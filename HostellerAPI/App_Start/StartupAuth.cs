//using Microsoft.AspNet.Identity;
//using Microsoft.Owin;
//using Microsoft.Owin.Security.Cookies;
//using Microsoft.Owin.Security.OAuth;
//using Owin;
//using System;

//namespace HostellerAPI.App_Start
//{
//    public partial class StartupAuth
//    {
//        static StartupAuth()
//        {
//            PublicClientId = "web";

//            OAuthOptions = new OAuthAuthorizationServerOptions
//            {
//                TokenEndpointPath = new PathString("/Token"),
//                AuthorizeEndpointPath = new PathString("/Account/Authorize"),
//                Provider = new AppOAuthProvider(PublicClientId),
//                AccessTokenExpireTimeSpan = TimeSpan.FromDays(14),
//                AllowInsecureHttp = true
//            };
//        }
//        public static OAuthAuthorizationServerOptions OAuthOptions { get; private set; }

//        /// <summary>   
//        /// Public client ID property.  
//        /// </summary>  
//        public static string PublicClientId { get; private set; }

//        public void ConfigureAuth(IAppBuilder app)
//        {

//            // Enable the application to use a cookie to store information for the signed in user  
//            // and to use a cookie to temporarily store information about a user logging in with a third party login provider  
//            // Configure the sign in cookie  
//            app.UseOAuthAuthorizationServer(new OAuthAuthorizationServerOptions
//            {
//                TokenEndpointPath = new PathString("/oauth/access_token"),
//                Provider = new AppOAuthProvider(PublicClientId),
//                AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(30),
//                AllowInsecureHttp = true,

//            });
//            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());

//            // Configure the application for OAuth based flow  
//            PublicClientId = "self";
//            OAuthOptions = new OAuthAuthorizationServerOptions
//            {
//                TokenEndpointPath = new PathString("/Token"),
//                Provider = new AppOAuthProvider(PublicClientId),
//                AuthorizeEndpointPath = new PathString("/Account/ExternalLogin"),
//                AccessTokenExpireTimeSpan = TimeSpan.FromHours(4),
//                AllowInsecureHttp = true //Don't do this in production ONLY FOR DEVELOPING: ALLOW INSECURE HTTP!  
//            };

//            // Enable the application to use bearer tokens to authenticate users  
//            app.UseOAuthBearerTokens(OAuthOptions);

//            // Enables the application to temporarily store user information when they are verifying the second factor in the two-factor authentication process.  
//            app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));

//            // Enables the application to remember the second login verification factor such as phone or email.  
//            // Once you check this option, your second step of verification during the login process will be remembered on the device where you logged in from.  
//            // This is similar to the RememberMe option when you log in.  
//            app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);

//            // Uncomment the following lines to enable logging in with third party login providers  
//            //app.UseMicrosoftAccountAuthentication(  
//            //    clientId: "",  
//            //    clientSecret: "");  

//            //app.UseTwitterAuthentication(  
//            //   consumerKey: "",  
//            //   consumerSecret: "");  

//            //app.UseFacebookAuthentication(  
//            //   appId: "",  
//            //   appSecret: "");  

//            //app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()  
//            //{  
//            //    ClientId = "",  
//            //    ClientSecret = ""  
//            //});  
//        }
//    }

//}