namespace IdentityDirectory.Scim.Services
{
    using System;
    using Resources;

    public interface IScimErrorHandler
	{
		ScimError Handle(Exception exception);
	}
}