namespace IdentityDirectory.Scim.Endpoints
{
    using Filters;
    using Microsoft.AspNet.Mvc;

    [Produces(ScimConstants.ScimMediaType)]
	[ScimExceptionFilter]
	public class ScimEndpoint : Controller
	{
	}
}