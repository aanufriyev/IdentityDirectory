namespace IdentityDirectory.Scim.Services
{
    using System.Collections.Generic;

    public interface IQueryableResourceManager<TResource> : IResourceManager<TResource> where TResource : class
	{
		IEnumerable<TResource> Query(string filter);

		IEnumerable<TResource> Query(string filter, int skip, int count);
	}
}