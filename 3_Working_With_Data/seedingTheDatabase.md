# Seeding The Database

To seed the database with fake records we create we first execute "add-migration {Migration Name}" with a Migration Name of PopulateMembershipTypes. Since we don't have any model class called PopulateMembershipTypes the migration class that Entity Framework will automatically create for us in the Migrations folder will not have any fields in its' Up() and Down() methods:

```cs
namespace Vidly.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PopulateMembershipTypes : DbMigration
    {
        public override void Up()
        {
        }
        
        public override void Down()
        {
        }
    }
}
```

To populate our database we can include the below SQL statements in our Up() method, and then run the migration.

```cs
namespace Vidly.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PopulateMembershipTypes : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO MembershipTypes (Id, SignUpFee, DurationInMonths, DiscountRate) VALUES (1, 0, 0, 0)");
            Sql("INSERT INTO MembershipTypes (Id, SignUpFee, DurationInMonths, DiscountRate) VALUES (2, 30, 1, 10)");
            Sql("INSERT INTO MembershipTypes (Id, SignUpFee, DurationInMonths, DiscountRate) VALUES (3, 90, 3, 15)");
            Sql("INSERT INTO MembershipTypes (Id, SignUpFee, DurationInMonths, DiscountRate) VALUES (1, 300, 12, 20)");
        }
        
        public override void Down()
        {
        }
    }
}
```

To run this migration all we need to do is execute the "Update-Database" command after we have included the SQL statements that we want to run.
