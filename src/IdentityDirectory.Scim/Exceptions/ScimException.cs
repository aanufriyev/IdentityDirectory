namespace Klaims.Scim.Exceptions
{
	using System;

	public class ScimException : Exception
	{
		public ScimException(string message, Exception innerException)
			: base(message, innerException)
		{

		}

		public ScimException(string message)
			: base(message)
		{

		}

	
		
	}
}