# Social Logins

To enable SSL, click a project in solution explorer and press f4. In the menu that appears, set SSL Enabled to true. A URL should populate in the **SSL URL** field. This is the address we use in order to establish a secure connection with Vidly. Copy this URL, go back to solution explorer, right click on the project, go to properties. Click the **Web** left menu option and replace the **Project URL** with the **SSL URL** you copied from the previous menu. Upon running the application you will have to trust some program and install a certificate - do both. If you look at the address bar for our project when it loads, you will see that our project is HTTPS now. The HTTPS will be red because we don't have a proper certificate. If we deploy our application to a web server, we need to get a certificate from the web hosting company. We can still access our website from the old port number if we want.

We should now go to our **~/App_Start/FilterConfig.cs** file and add a new filter:

```cs
using System.Web;
using System.Web.Mvc;

namespace Vidly
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new AuthorizeAttribute());
            filters.Add(new RequireHttpsAttribute());
        }
    }
}

```

Now we will no be able to get to our application through the old port number, so it is more safe

***

### Registering Application With Facebook

To start we need to go to developers.facebook.com to create a facebook app. Once here, we create a new app, call it Vidly, enter in our project localhost URL, save changes, and go to the dashboard for the app. Here we want to grab the **App Id** and the **App Secret**. We will paste these values into our application in the **~/App_Start/Startup.Auth.cs** file:

```cs
namespace Vidly
{
    public partial class Startup
    {
        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            // Configure the db context, user manager and signin manager to use a single instance per request
            app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);

            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            // Configure the sign in cookie
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                Provider = new CookieAuthenticationProvider
                {
                    // Enables the application to validate the security stamp when the user logs in.
                    // This is a security feature which is used when you change a password or add an external login to your account.  
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser>(
                        validateInterval: TimeSpan.FromMinutes(30),
                        regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
                }
            });            
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Enables the application to temporarily store user information when they are verifying the second factor in the two-factor authentication process.
            app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));

            // Enables the application to remember the second login verification factor such as phone or email.
            // Once you check this option, your second step of verification during the login process will be remembered on the device where you logged in from.
            // This is similar to the RememberMe option when you log in.
            app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);

            // Uncomment the following lines to enable logging in with third party login providers
            //app.UseMicrosoftAccountAuthentication(
            //    clientId: "",
            //    clientSecret: "");

            //app.UseTwitterAuthentication(
            //   consumerKey: "",
            //   consumerSecret: "");

            app.UseFacebookAuthentication(
               appId: "462677844104908",
               appSecret: "SeCrEtVaLuE");

            //app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
            //{
            //    ClientId = "",
            //    ClientSecret = ""
            //});
        }
    }
}
```

That's it! If we go to the Register page we will see a Facebook login button that will take us to Facebook. If we go to Facebook, login, and allow Vidly to use our basic information then when we come back to Vidly and input our email address we will get a validation error. This is because Facebook doesn't have all of the fields needed by Vidly, it is missing the Drivers License information.

To fix this we go to **~/Views/Account/ExternalLoginComfirmation.cshtml**. This is the registration page when we use a social login provider. When we add the Driving License field, it will say it is out of scope. We go to the **AccountViewModels.cs** and add the DrivingLicense field to the **ExternalLoginConfirmationViewModel** class:

```cs
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Driving License")]
        public string DrivingLicense { get; set; }
    }
```

Now when this form is posted, we need to get this property from our view model and put it in our domain object. To do this, we will go back to the **ExternalLoginComfirmation.cshtml** and scroll to the top. The action that this from will be posted to is the **ExternalLoginConfirmation**. If we go to the declaration of this method, it will take us to the **AccountController.cs**.

```cs
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }
```

Near the middle of the above code block, when we create an ApplicationUser, we want to initialize our driving license. 

```cs
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { 
                    UserName = model.Email,
                    Email = model.Email,
                    DrivingLicense = model.DrivingLicense
                };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }
```

***

One last thing to be aware of is, if we go to the database and look at the AspNetUser table, you can see the accounts with the email from Facebook. We also have another record for this user in the AspNetUserLogin table. This is because we logged in by Facebook, so my login provider is Facebook in this table. The ProviderKey is the App Id, and the UserId is the secret.
