namespace Klaims.Scim.Rest
{
	using System.Collections.Generic;

	using Klaims.Scim.Query;

	public interface IQueryableResourceManager<TResource> : IResourceManager<TResource> where TResource : class
	{
		IEnumerable<TResource> Query(string filter);

		IEnumerable<TResource> Query(string filter, int skip, int count);
	}
}