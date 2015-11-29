namespace Klaims.Framework.IdentityMangement
{
	using System;

	using Klaims.Framework.IdentityMangement.Models;

	public class DefaultUserAccountManager : IUserAccountManager<UserAccount>
	{
		private readonly IUserAccountRepository<UserAccount> userAccountRepository;

		public DefaultUserAccountManager(IUserAccountRepository<UserAccount> userAccountRepository)
		{
			this.userAccountRepository = userAccountRepository;
		}

		public IQueryableUserAccountRepository<UserAccount> Queryable => this.userAccountRepository as IQueryableUserAccountRepository<UserAccount>;

		public void Create(UserAccount user)
		{
			throw new NotImplementedException();
		}

		public void Update(UserAccount user)
		{
			throw new NotImplementedException();
		}

		public void Delete(UserAccount user)
		{
			throw new NotImplementedException();
		}

		public UserAccount GetByUsername(string username)
		{
			throw new NotImplementedException();
		}

		public UserAccount GetByEmail(string email)
		{
			throw new NotImplementedException();
		}

		public UserAccount GetById(Guid id)
		{
			return this.userAccountRepository.FindById(id);
		}
	}
}