namespace Klaims.Scim.Exceptions
{
	using System;

	public class ScimResourceNotFoundException :ScimException
	{
		public ScimResourceNotFoundException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		public ScimResourceNotFoundException(string message)
			: base(message)
		{
		}
	}
}