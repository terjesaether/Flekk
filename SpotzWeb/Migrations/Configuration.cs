using System.Collections.Generic;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SpotzWeb.Models;

namespace SpotzWeb.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "SpotzWeb.Models.ApplicationDbContext";
        }

        protected override void Seed(ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            const string admin = "Administrator";
            const string user = "User";

            // Create Roles
            if (!context.Roles.Any())
            {
                var roleStore = new RoleStore<IdentityRole>(context);
                var roleManager = new RoleManager<IdentityRole>(roleStore);
                var roleAdmin = new IdentityRole { Name = admin };
                var roleUser = new IdentityRole { Name = user };

                roleManager.Create(roleAdmin);
                roleManager.Create(roleUser);
            }

            // Create Users
            if (context.Users.Any()) return;
            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);
            var passwordHash = new PasswordHasher();

            var adminUser = new ApplicationUser
            {
                UserName = "admin@terje.com",
                Email = "admin@terje.com",
                PasswordHash = passwordHash.HashPassword("SpotZ123.")
            };

            var user1 = new ApplicationUser
            {
                UserName = "petter@petter.com",
                Email = "petter@petter.com",
                PasswordHash = passwordHash.HashPassword("SpotZ456.")
            };

            var user2 = new ApplicationUser
            {
                UserName = "kari@kari.com",
                Email = "kari@kari.com",
                PasswordHash = passwordHash.HashPassword("SpotZ789.")
            };

            userManager.Create(adminUser);
            userManager.Create(user1);
            userManager.Create(user2);
            userManager.AddToRole(adminUser.Id, admin);
            userManager.AddToRole(user1.Id, user);
            userManager.AddToRole(user2.Id, user);

            //var spotzId1 = Guid.NewGuid();
            //var spotzId2 = Guid.NewGuid();
            //var spotzId3 = Guid.NewGuid();

            // Comments
            var comment1 = new Comment
            {
                CommentId = Guid.NewGuid(),
                Text = "Good Spotz!",
                Timestamp = DateTime.Now,
                User = user1,

            };
            var comment2 = new Comment
            {
                CommentId = Guid.NewGuid(),
                Text = "Nice Spotz!",
                Timestamp = DateTime.Now,
                User = user2
            };

            context.Comments.AddOrUpdate(
                c => c.Text,
                comment1,
                comment2
                );

            var listOfComments = new List<Comment> { comment1, comment2 };

            // Tags
            var tag1 = new Tag
            {
                TagId = Guid.NewGuid(),
                TagName = "City"
            };
            var tag2 = new Tag
            {
                TagId = Guid.NewGuid(),
                TagName = "Tourist"
            };
            context.Tags.AddOrUpdate(
                c => c.TagName,
                tag1,
                tag2
                );

            var listOfTags = new List<Tag> { tag1, tag2 };

            // Spotz
            context.Spotzes.AddOrUpdate(
                s => s.Title,
                new Models.Spotz { SpotzId = Guid.NewGuid(), Title = "Vigelandsparken", Description = "Nice place with lots of naked people", Timestamp = DateTime.Now, User = user1, Tags = listOfTags, Comments = listOfComments },
                new Models.Spotz { SpotzId = Guid.NewGuid(), Title = "Holmenkollen", Description = "Nice place with lots of ski people", Timestamp = DateTime.Now, User = user2, Tags = listOfTags },
                new Models.Spotz { Title = "Barcode", Description = "Nice place with lots of tall houses", Timestamp = DateTime.Now, User = adminUser, Tags = listOfTags }
                );

        }
    }
}
