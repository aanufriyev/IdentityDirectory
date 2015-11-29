namespace Klaims.Framework.IdentityMangement
{
	using System;
	using System.Collections.Generic;

	public interface IGroupRepository<TGroup> where TGroup : class
	{
		TGroup Create();
		void Add(TGroup item);
		void Remove(TGroup item);
		void Update(TGroup item);
		
		TGroup FindById(Guid id);
		TGroup FindByName(string name);
	}
}