# Seeding Users & Roles

Our goal is to restrict movie management operations to MovieManagers. To begin, we need to create this role in our application. More importantly, when we deploy our application, we should have a user assigned to this role. This way the store manager can start entering movies and then delegate other tasks to the staff.

First we are going to register a guest user. Back in visual studio we can go to **~/Controllers/AccountController.cs**. From the list of methods we will look at the **Register()** method. We're going to modify the logic of this action and assign any new users to the store manager role. We will do this only to populate our database with a StoreManager, and then we will change the method back. This is what the code looks like before we edit it:

```cs
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

                    return RedirectToAction("Index", "Home");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }
```

To work with ASP.NET Identity we use Manager classes like: UserManager, RoleManager, and SignInManager. All these managers use a store under the hood like: UserStore and RoleStore.

First we will create a role called **roleStore** that will be a generic class where we will specify the type of the role in our application. In this application we don't have any specific role class, so we use the one that comes with ASP.NET Identity called: IdentityRole. We will pass through a new ApplicationDbContext object. 

```cs
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
                    // temp code
                    var roleStore = new RoleStore<IdentityRole>(new ApplicationDbContext());
                    var roleManager = new RoleManager<IdentityRole>(roleStore);
                    await roleManager.CreateAsync(new IdentityRole("CanManageMovies"));
                    await UserManager.AddToRoleAsync(user.Id, "CanManageMovies");

                    await SignInManager.SignInAsync(user, isPersistent:false, rememberBrowser:false);

                    return RedirectToAction("Index", "Home");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }
```

Now that we have our store, we should create a RoleManager. We call **roleManager.CreateAsync(new IdentityRole("CanManageMovies"))**. Since we set the generic parameter to identityRole, we need to pass an instance of identityRole. In the constructor we set the name of the role. Since it is an Async method, we need to await it.

Now we can assign this new user to our new role. So we use the **UserManager.AddToRoleAsync(user.Id, "CanManageMovies")**. We pass to this method the id of this user, and the name of the role.

Now we can create a new user by launching the application and registering a new user

***

Now that we have our admin created we can go back and delete the four lines under the temp code comment in the **AccountController.cs**

Now we have a guest and an admin user and we want these users to be deployed with our application. To do this we will use a code-first migration. This will be an empty migration because we have not modified our domain model. In our blank migration we will use SQL to add our two users and the corresponding role in our database. We will get the SQL for these user by going into the AspNetUsers table, converting it into a script, and copying the SQL script. Then we will copy the SQL script from the AspNetRoles table as well.

Finally, we need the record that associates our admin user with the new role from the AspNetUserRoles table. After pasting all of this code into the migration, you should have this:

```cs
namespace Vidly.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SeedUsers : DbMigration
    {
        public override void Up()
        {
            Sql(@"
                INSERT INTO [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) VALUES (N'0ace796b-919b-4393-8ce5-ef05aedeaa85', N'Guest1!@vidly.com', 0, N'ABA8suh43m/+YBTOqPCNk2BtcGixKCW8XMc7J6R+olhulPV0UfrrJMqBMDxM7OUlyA==', N'4987df83-a8bc-4854-8095-ad82af0d8a51', NULL, 0, 0, NULL, 1, 0, N'Guest1!@vidly.com')
                INSERT INTO [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) VALUES (N'792bcf07-824b-47a4-91db-0f9815481e27', N'Admin1!@vidly.com', 0, N'AP8Kj1j+CS0it3l+n+LyKT7G+Fhb6VfAS9P8Q1RqbjYA1sFKmhmR+DR7b97P358+1w==', N'994e613e-7a50-4756-92b8-cb14b9aec834', NULL, 0, 0, NULL, 1, 0, N'Admin1!@vidly.com')
                INSERT INTO [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'5bcd746a-93aa-4274-b14d-ddbb7c491437', N'CanManageMovies')
                INSERT INTO [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'792bcf07-824b-47a4-91db-0f9815481e27', N'5bcd746a-93aa-4274-b14d-ddbb7c491437')
            ");
        }
        
        public override void Down()
        {
        }
    }
}

```

Now we need to remove the records from the AspNetRoles table and the AspNetUsers table and then run this migration using the **Update-Database** command in package manager. This will populate our database with the users and their correct roles.

The beauty of this approach is that, if you run these migrations on another database, you will have the exact same setup. This is the proper way to seed our database with users and roles.
