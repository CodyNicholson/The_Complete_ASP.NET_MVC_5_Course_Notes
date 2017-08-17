# Code-First Migrations

In the Code-First workflow we start with the code every time we modify our domain model by adding a class, or modifying one of the existing ones

We do this by creating a migration and then running it on the database

To do this we will use the **package manager console** a lot which you can find in Visual Studio under: Tools -> NuGet Package Manager -> Package Manager Console

-

We need to **enable migrations** the first time we want to use them by running the "enable-migrations" command in the package manager console. If it worked you should see a "Migrations" folder in your solution explorer. All of the migrations you add will be stored in this folder.

To **create our first migration** we will execute the "add-migration {MigrationName}" command in the package manager console with a name that describes the change we made, in this case we will call it *InitialModel*.

In the Migrations folder you will see that a C-Sharp class got created for our migration. Inside that class you will see all of the tables that are automatically created by Entity Framework.

In the Models folder you will find a C-Sharp class called **IdentityModels.cs**. You can see the contents of this file below, I added the DbSet<Customer> Customers to the ApplicationDbContext class so that our DbContext is aware of the Customer class.

```cs
namespace Vidly.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        //DbContext is now aware of our Customer class since I added the below line
        public DbSet<Customer> Customers { get; set; }

        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}
```

Now that we have added this DbSet we can recreate our migration by going to the package manager console and running "add-migration InitialModel -force". Since we already have a migration called InitialModel and we want to overwrite it, we use the "-force" flag.

Now if you go back to the class that was generated for the migration in the Migrations folder you will see:

```cs
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Super = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
```

Entity framework has created a table for the Customer class that we added to the DbContext in the migration class, and has added the Fields of the Customer class as columns in the new Customers table.

However, we still need to run this migration to generate our database. To do this, all we need to do is run "update-database" in the package manager console. Then if we go to the solution explore, click the "Show all files" icon at the top, we will see a file listed under our App_Data folder. This is our database file where all the tables are stored.
