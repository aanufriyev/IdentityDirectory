namespace Klaims.Scim.Query
{
	using System;
	using System.Linq.Expressions;

	using Klaims.Scim.Query.Filter;
	using Klaims.Scim.Services;

	public interface IFilterBinder
	{
		Expression<Func<TResource, bool>> Bind<TResource>(FilterNode filter, string sortBy, bool @ascending, IAttributeNameMapper mapper = null);
	}
}