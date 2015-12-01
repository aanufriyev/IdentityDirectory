namespace IdentityDirectory.Scim.Query
{
    using System;
    using System.Linq.Expressions;
    using Services;

    public interface IFilterBinder
	{
		Expression<Func<TResource, bool>> Bind<TResource>(ScimExpression filter, string sortBy, bool @ascending, IAttributeNameMapper mapper = null);
	}
}