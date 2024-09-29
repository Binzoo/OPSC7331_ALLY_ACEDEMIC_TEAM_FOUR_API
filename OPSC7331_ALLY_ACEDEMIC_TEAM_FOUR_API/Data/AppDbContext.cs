using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using OPSC7331_ALLY_ACEDEMIC_TEAM_FOUR_API.Models;

namespace OPSC7331_ALLY_ACEDEMIC_TEAM_FOUR_API.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<PasswordReset> PasswordResetsCodes { get; set; }
        public DbSet<NoticeBoard> NoticeBoards { get; set; }
        public DbSet<Degree> Degrees { get; set; }
        public DbSet<Module> Modules { get; set; }
        public DbSet<HomeWork> HomeWorks { get; set; }
        public DbSet<SchoolNavigation> SchoolNavigations { get; set; }
        public DbSet<MultiMedia> MultiMedias { get; set; }
        public DbSet<ModuleClass> ModuleClasses { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
    }
}
