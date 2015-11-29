namespace Klaims.Framework.IdentityMangement
{
	using System;

	public interface IUserAccountRepository<TUser> where TUser :class
	{
		void Create(TUser item);
		void Remove(TUser item);
		void Update(TUser item);

		TUser FindById(Guid id);
		TUser FindByUsername(string username);
		TUser FindByEmail(string email);
	}
}