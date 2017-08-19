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
