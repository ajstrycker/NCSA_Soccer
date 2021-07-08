namespace NCSA.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class seeduserandrole : DbMigration
    {
        public override void Up()
        {
            Sql(@"
                INSERT [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'4c1eaf40-00ae-4e68-88da-a8247e117d42', N'Admin')
                INSERT [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'8a95c865-ca5a-4da2-a278-e5989070704b', N'CommunityLeader')
                INSERT [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'fd3a74c7-8c3c-4729-8987-bd425a870dc0', N'Referee')
                INSERT [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) VALUES (N'063a3ef7-8c4b-4048-b79a-bdf97c337da0', N'walkertonncsasoccer@gmail.com', 0, N'ADwlHAgs6NEsABwtWgjWsj1YfW5rzwEMXj1kUGlQdVVwBUsoiavdvrp/OqDxo676Cg==', N'b32bd43f-6d28-4725-b2a9-b24755e1f9f5', NULL, 0, 0, NULL, 1, 0, N'walkertonncsasoccer@gmail.com')
                INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'063a3ef7-8c4b-4048-b79a-bdf97c337da0', N'4c1eaf40-00ae-4e68-88da-a8247e117d42')
            ");
        }
        
        public override void Down()
        {
        }
    }
}
