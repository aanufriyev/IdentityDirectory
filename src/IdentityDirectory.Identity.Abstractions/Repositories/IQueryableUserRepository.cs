namespace Klaims.Framework.IdentityMangement
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Linq.Expressions;

	public interface IQueryableUserAccountRepository<TUser> : IUserAccountRepository<TUser>
		where TUser : class
	{
		IEnumerable<TUser> Search(Expression<Func<TUser, bool>> predicate, int? skip, int? count);
	}
}