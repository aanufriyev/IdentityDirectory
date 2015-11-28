namespace Klaims.Scim.Endpoints
{
	using Klaims.Scim.Endpoints.Filters;

	using Microsoft.AspNet.Mvc;

	[Produces(ScimConstants.ScimMediaType)]
	[ScimExceptionFilter]
	public class ScimEndpoint : Controller
	{
	}
}