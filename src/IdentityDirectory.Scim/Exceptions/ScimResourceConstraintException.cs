namespace Klaims.Scim.Exceptions
{
	using System;

	public class ScimResourceConstraintException : ScimException
	{
		public ScimResourceConstraintException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		public ScimResourceConstraintException(string message)
			: base(message)
		{
		}
	}
}