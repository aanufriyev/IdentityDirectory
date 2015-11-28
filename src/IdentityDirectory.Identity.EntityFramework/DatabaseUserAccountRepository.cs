namespace IdentityDirectory.Identity.EntityFramework
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using Klaims.Framework.IdentityMangement;
    using Klaims.Framework.IdentityMangement.Models;
    using Klaims.Framework.Utility;
    using Microsoft.Data.Entity;

    public class DatabaseUserAccountRepository : IQueryableUserAccountRepository<UserAccount>
	{
		public void Create(UserAccount account)
		{
			using (var context = new IdentityDbContext())
			{
				context.ChangeTracker.TrackGraph(account, node => node.Entry.State = EntityState.Added);
				context.SaveChanges();
			}
		}

		public void Remove(UserAccount item)
		{
			using (var context = new IdentityDbContext())
			{
				context.UserAccounts.Remove(item);
				context.SaveChanges();
			}
		}

		public void Update(UserAccount item)
		{
			throw new NotImplementedException();
		}

		public UserAccount FindById(Guid id)
		{
			using (var context = new IdentityDbContext())
			{
				return context.UserAccounts.Include(ua => ua.Claims).FirstOrDefault(u => u.Id.Equals(id));
			}
		}

		public UserAccount FindByUsername(string username)
		{
			Check.Argument.IsNotNullOrEmpty(username, nameof(username));

			using (var context = new IdentityDbContext())
			{
				return context.UserAccounts.Include(ua => ua.Claims).FirstOrDefault(u => u.Username.Equals(username));
			}
		}

		public UserAccount FindByEmail(string email)
		{
			Check.Argument.IsNotNullOrEmpty(email, nameof(email));

			using (var context = new IdentityDbContext())
			{
				return context.UserAccounts.Include(ua => ua.Claims).Include(ua => ua.Emails).FirstOrDefault(u => u.Emails.Any(e => e.Value.Equals(email)));
			}
		}

		public IEnumerable<UserAccount> Search(Expression<Func<UserAccount, bool>> predicate, int? skip, int? count)
		{
			Check.Argument.IsNotNull(predicate, nameof(predicate));

			using (var context = new IdentityDbContext())
			{
				return context.UserAccounts.Include(ua => ua.Claims).Include(ua => ua.Emails).Where(predicate).ToArray();
			}
		}
	}
}