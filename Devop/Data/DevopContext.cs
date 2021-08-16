using Devop.Models;
using Devop.Models.Content;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Devop.Models.ViewModel.UserViewModels;

namespace Devop.Data
{
    public class DevopContext : IdentityDbContext<ApplicationUser>
    {
        public DevopContext(DbContextOptions<DevopContext> options) : base(options)
        {

        }
        public virtual DbSet<City> tblCity { get; set; }
        public virtual DbSet<Suburb> tblSuburb { get; set; }
        public virtual DbSet<MedicalAid> tblMedicalAid { get; set; }
        public virtual DbSet<MedicalPlan> tblMedicalPlans { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<City>(entity =>
            {
                entity.HasKey(e => e.CityId);
                entity.Property(e => e.CityId).HasMaxLength(50);
                

                entity.ToTable("tblCity");
                entity.Property(e => e.CityId).ValueGeneratedOnAdd();

                entity.Property(e => e.CityName)
                                      .IsRequired()
                                      .HasMaxLength(50);

            });

            modelBuilder.Entity<Suburb>(entity =>
            {
                entity.HasKey(e => e.SuburbId);
                entity.Property(e => e.SuburbId).HasMaxLength(50);

                entity.ToTable("tblSuburb");
                entity.Property(e => e.SuburbId).ValueGeneratedOnAdd();

                entity.Property(e => e.SuburbName)
                                      .IsRequired()
                                      .HasMaxLength(50);
                

            });

            modelBuilder.Entity<MedicalAid>(entity =>
            {
                entity.HasKey(e => e.MedId);
                entity.Property(e => e.MedId).HasMaxLength(50);

                entity.ToTable("tblMedicalAid");
                entity.Property(e => e.MedId).ValueGeneratedOnAdd();

                entity.Property(e => e.MedicalName)
                                      .IsRequired()
                                      .HasMaxLength(50);


            });
            modelBuilder.Entity<MedicalPlan>(entity =>
            {
                entity.HasKey(e => e.PlanId);
                entity.Property(e => e.PlanId).HasMaxLength(50);

                entity.ToTable("tblMedicalPlans");
                entity.Property(e => e.PlanId).ValueGeneratedOnAdd();

                entity.Property(e => e.PlanName)
                                      .IsRequired()
                                      .HasMaxLength(50);


            });


            base.OnModelCreating(modelBuilder);
        }

        public static async Task CreateAdminAccount(IServiceProvider serviceProvider,IConfiguration configuration)
        {
            UserManager<ApplicationUser> userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            RoleManager<IdentityRole> roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            string username = configuration["Data:AdminUser:Name"];
            string email = configuration["Data:AdminUser:Email"];
            string password = configuration["Data:AdminUser:Password"];
            string role = configuration["Data:AdminUser:Role"];

            if (await userManager.FindByNameAsync(username) == null)
            {
                if (await roleManager.FindByNameAsync(role) == null)
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
                ApplicationUser user = new ApplicationUser
                {
                    UserName = username,
                    Email = email
                };
                IdentityResult result = await userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, role);
                }
            }
        }

        public DbSet<Devop.Models.ViewModel.UserViewModels.EditUserModel> EditUserModel { get; set; }

    }
}
