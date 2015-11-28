namespace Klaims.Scim.Rest
{
	public interface IResourceManager<TResource>
		where TResource : class
	{
		TResource FindById(string id);

		TResource Create(TResource resource);

		TResource Update(string id, TResource resource);

		TResource Remove(string id, int version);
	}
}