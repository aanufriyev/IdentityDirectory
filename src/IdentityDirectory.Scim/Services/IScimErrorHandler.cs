using System;

namespace Klaims.Scim.Services
{
	using Klaims.Scim.Resources;

	public interface IScimErrorHandler
	{
		ScimError Handle(Exception exception);
	}
}