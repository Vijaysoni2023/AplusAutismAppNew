using aplusautism.Data.Models;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aplusautism.Data
{
    public class AplusautismDbContext : IdentityDbContext<AppUser> 
    {
        #region Constructor and Configuration
        public AplusautismDbContext(DbContextOptions<AplusautismDbContext> options) : base(options)
        {
            
    }

        public DbSet<SubscriptionSetup> SubscriptionSetup { get; set; }
        public DbSet<AB_Address> aB_Address { get; set; }
        public DbSet<AB_User> aB_User { get; set; }
        public IEnumerable<object> AB_User { get; set; }
        public DbSet<LessonSetupLanguage> lessonSetupLanguage { get; set; }
        public DbSet<AB_Main> Ab_main { get; set; }
        public DbSet<GlobalCodeCategory> globalCodeCategories { get; set; }
        public DbSet<GlobalCodes> globalCodes { get; set; }

        public DbSet<LessonSetup> LessonSetup { get; set; }

        public DbSet<Payments> Payments { get; set; }

        public DbSet<Cities> Cities { get; set; }
        public DbSet<Countries> Countries { get; set; }
        public DbSet<States> States { get; set; }
        public DbSet<ExceptionMessageTable> ExceptionMessageTable { get; set; }
        #endregion

        #region OnModelCreating

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

        }
        #endregion
    }
}
