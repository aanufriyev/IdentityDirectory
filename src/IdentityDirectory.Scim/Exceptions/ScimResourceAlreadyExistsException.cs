namespace Klaims.Scim.Exceptions
{
	using System;

	public class ScimResourceAlreadyExistsException :ScimException
	{
		public ScimResourceAlreadyExistsException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		public ScimResourceAlreadyExistsException(string message)
			: base(message)
		{
		}
	}
}