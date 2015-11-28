namespace Klaims.Scim.Services
{
	using Klaims.Scim.Resources;
	using Klaims.Scim.Rest;

	public interface IScimUserManager : IQueryableResourceManager<ScimUser>
	{
		ScimUser CreateUser(ScimUser user, string password);

		void ChangePassword(string id, string oldPassword, string newPassword);

		ScimUser VerifyUser(string id, int version);
	}
}