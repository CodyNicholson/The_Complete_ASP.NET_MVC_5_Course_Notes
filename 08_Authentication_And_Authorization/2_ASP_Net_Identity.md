# ASP.NET Identity

The framework: **ASP.NET Identity**, is what we will use for authentication and authorization. If you look in the **~/References** directory, you will see three assembly files representing the identity framework.

In terms of architecture, identity has a number of domain classes like: IdentityUser, and Role. Identity provides a simple API to work with these classes. The API consists of classes like: UserManager, RoleManager, SignInManager, and so on. These classes internally talk to another group of classes like: UserStore and RoleStore, which represent the persistent store for ASP.NET identity.

ASP.NET identity provides an implementation of these persistent stores using entity framework and a relational sequel database. You can also plug in your own implementation of a persistent store, like a NoSQL data store.

***

When we made database migrations we got all these tables in our migration like ASP.NET roles, ASP.NET UserRoles, ASP.NET Users, and so on. These tables are generated based on the domain model of ASP.NET Identity framework. If we go to our **~/Models/IdentityModels.cs** file, you will see we have ApplicationUser, which derives from IdentityUser. We also have ApplicationDbContext which derives from IdentityDbContext. Both of these classes (IdentityUser & IdentityDbContext) are part of ASP.NET Identity framework. That is why when we first created a migration, we got these tables as part of our migration.

The **~/Controllers/AccountController.cs** exposes a number of actions like register, login, log off, and so on. We have to **Register()** actions. The one without a parameter is used for loading the registration form. The other register action is called when we post a form.

```cs
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await SignInManager.SignInAsync(user, isPersistent:false, rememberBrowser:false);
                    
                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    return RedirectToAction("Index", "Home");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }
```
