namespace Klaims.Scim.Endpoints
{
	#region

	using System;

	using Klaims.Scim.Exceptions;
	using Klaims.Scim.Resources;
	using Klaims.Scim.Rest;
	using Klaims.Scim.Services;

	using Microsoft.AspNet.Mvc;

	#endregion

	[Route(ScimConstants.Routes.Templates.Users)]
	public class UsersEndpoint : ScimEndpoint
	{
		private readonly IScimUserManager resourceManager;

		public UsersEndpoint(IScimUserManager resourceManager)
		{
			this.resourceManager = resourceManager;
		}

		[HttpGet]
		public ScimListResponse<ScimUser> GetAll()
		{
			var queryResults = this.resourceManager.Query("id pr");
			return new ScimListResponse<ScimUser>(queryResults, 1);
		}

		[HttpGet("{userId}", Name = "GetScimUserRoute")]
		public ScimUser GetUser(string userId)
		{
			var result = this.resourceManager.FindById(userId);
			if (result == null)
			{
				throw new ScimResourceNotFoundException("Resource does not exist.");
			}

			return result;
		}

		[HttpPost]
		public void Create([FromBody] ScimUser item)
		{
			if (!this.ModelState.IsValid)
			{
				this.HttpContext.Response.StatusCode = 400;
			}
			else
			{
				this.resourceManager.Create(item);
				var url = this.Url.RouteUrl("GetByIdRoute", new { id = item.Id }, this.Request.Scheme, this.Request.Host.ToUriComponent());
				this.HttpContext.Response.StatusCode = 201;
				this.HttpContext.Response.Headers["Location"] = url;
			}
		}

		[HttpDelete("{id}")]
		public ScimUser DeleteItem(string id)
		{
			var removed = this.resourceManager.Remove(id, -1);
			if (removed == null)
			{
				throw new Exception("User not found");
			}
			return removed; // 201 No Content
		}
	}
}