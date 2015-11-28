namespace IdentityDirectory.Identity.EntityFramework
{
    #region

    using Klaims.Framework.IdentityMangement.Models;
    using Microsoft.Data.Entity;

    #endregion

    public class IdentityDbContext : DbContext
    {
        public DbSet<UserAccount> UserAccounts { get; set; }

        public DbSet<UserEmail> UserEmails { get; set; }

        public DbSet<UserPhone> UserPhones { get; set; }

        public DbSet<UserClaim> UserClaims { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<UserAccount>(
                b =>
                {
                    b.HasKey(u => u.Id);
                    b.ToTable("UserAccounts");
                    b.Property(u => u.Version).IsConcurrencyToken();
                });

            builder.Entity<UserClaim>(
                b =>
                {
                    b.HasKey(uc => uc.Id);
                    b.HasOne<UserAccount>().WithMany().ForeignKey(uc => uc.UserId);
                    b.ToTable("UserClaims");
                });

            builder.Entity<UserEmail>(
                b =>
                {
                    b.HasKey(rc => rc.Id);
                    b.HasOne<UserAccount>().WithMany().ForeignKey(rc => rc.UserId);
                    b.ToTable("UserEmails");
                });

            builder.Entity<UserPhone>(
                b =>
                {
                    b.HasKey(rc => rc.Id);
                    b.HasOne<UserPhone>().WithMany().ForeignKey(rc => rc.UserId);
                    b.ToTable("UserPhones");
                });
        }
    }
}