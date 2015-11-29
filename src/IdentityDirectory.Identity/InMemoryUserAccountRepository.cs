namespace Klaims.Framework.IdentityMangement
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Linq.Expressions;

	using Klaims.Framework.IdentityMangement.Models;

	public class InMemoryUserAccountRepository : IQueryableUserAccountRepository<UserAccount>
	{
		private static readonly List<UserAccount> Users = new List<UserAccount>
			                                           {
				                                           new UserAccount
					                                           {
						                                           Id = Guid.Parse("2819c223-7f76-453a-919d-413861904646"),
						                                           Username = "bjensen@example.com",
						                                           GivenName = "Barbara",
						                                           FamilyName = "Jensen"
					                                           }
			                                           };

		public void Create(UserAccount item)
		{
			Users.Add(item);
		}

		public void Remove(UserAccount item)
		{
			Users.Remove(item);
		}

		public void Update(UserAccount item)
		{
			Users[Users.IndexOf(item)] = item;
		}

		public UserAccount FindById(Guid id)
		{
			return Users.FirstOrDefault(user => user.Id == id);
		}

		public UserAccount FindByUsername(string username)
		{
			return Users.FirstOrDefault(user => user.Username == username);
		}

		public UserAccount FindByEmail(string email)
		{
			return Users.FirstOrDefault(user => user.Emails.Any(e => e.Value == email));
		}

		public IEnumerable<UserAccount> Search(Expression<Func<UserAccount, bool>> predicate, int? skip, int? count)
		{
			var query = Users.AsQueryable().Where(predicate);
			if (skip.HasValue)
			{
				query = query.Skip(skip.Value);
			}
			if (count.HasValue)
			{
				query = query.Take(count.Value);
			}
			return query.ToList();
		}

		
	}
}