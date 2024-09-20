﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
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

    }

}
