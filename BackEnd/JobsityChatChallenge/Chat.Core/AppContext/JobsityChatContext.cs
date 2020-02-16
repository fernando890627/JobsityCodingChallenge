using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Chat.Core.Entity;

namespace Chat.Core.AppContext
{
    public class JobsityChatContext : IdentityDbContext<ApplicationUser, ApplicationRole, long>
    {
        public JobsityChatContext(DbContextOptions<JobsityChatContext> options)
       : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }
        public DbSet<Message> Message { get; set; }
        public DbSet<ChatRoom> Chatrooms { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
