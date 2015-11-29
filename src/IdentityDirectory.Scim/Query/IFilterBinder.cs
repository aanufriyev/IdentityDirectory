namespace IdentityDirectory.Scim.Query
{
    using System;
    using System.Linq.Expressions;
    using Services;

    public interface IFilterBinder
	{
		Expression<Func<TResource, bool>> Bind<TResource>(FilterExpresstion filter, string sortBy, bool @ascending, IAttributeNameMapper mapper = null);
	}
}