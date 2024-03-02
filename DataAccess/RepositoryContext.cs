using Entities.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class RepositoryContext : IdentityDbContext<CustomUser>
    {
        public RepositoryContext(DbContextOptions<RepositoryContext> options) : base(options)
        {
        }
        public DbSet<Messages> Messages
        {
            get; set;
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            // Messages ve AspNetUsers arasındaki ilişkiyi yapılandır
            builder.Entity<Messages>()
                .HasOne<CustomUser>(m => m.Sender)
                .WithMany()
                .HasForeignKey(m => m.SenderID)
                .OnDelete(DeleteBehavior.NoAction); // CASCADE DELETE devre dışı bırakıldı

            builder.Entity<Messages>()
                .HasOne<CustomUser>(m => m.Receiver)
                .WithMany()
                .HasForeignKey(m => m.ReceiverID)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
