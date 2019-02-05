namespace Vidly.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SeedUsers : DbMigration
    {
        public override void Up()
        {
	        Sql(@"
INSERT INTO [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) VALUES (N'29f5be93-f0ad-4372-9882-a568900ba761', N'admin@Vidly.com', 0, N'AOo0JnGI2+prYw7Wlzdi5Ffl9KJ4N42DpkkTKYSmMKfly68pYlwv0d5QA9kzFt/ZEA==', N'98f01562-a69b-4dbc-b197-7473cb97eca1', NULL, 0, 0, NULL, 1, 0, N'admin@Vidly.com')


INSERT INTO [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'c854be09-002a-4ce8-9857-35965e18f2d4', N'CanManageMovies')

INSERT INTO [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'29f5be93-f0ad-4372-9882-a568900ba761', N'c854be09-002a-4ce8-9857-35965e18f2d4')

"
				);
        }
        
        public override void Down()
        {
        }
    }
}
