namespace Klaims.Scim.Exceptions
{
	using System;

	public class ScimResourceConflictException :ScimException
	{
		public ScimResourceConflictException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		public ScimResourceConflictException(string message)
			: base(message)
		{
		}
	}
}