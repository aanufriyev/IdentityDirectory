namespace Klaims.Scim.Resources
{
	using System.Net;

	using Klaims.Scim.Exceptions;

	public class ScimError
	{
		public ScimError(HttpStatusCode code, string detail)
			: this(code, null, detail)
		{
		}

		public ScimError(HttpStatusCode code, ScimType scimType, string detail)
		{
			this.Status = (int)code;
			this.ScimType = scimType;
			this.Detail = detail;
		}

		public string[] Schemas => new[] { "urn:ietf:params:scim:api:messages:2.0:Error" };

		public string Detail { get; private set; }

		public ScimType ScimType { get; private set; }

		public int Status { get; private set; }
	}
}