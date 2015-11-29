namespace IdentityDirectory.Scim.Endpoints
{
    using System;
    using Microsoft.AspNet.Mvc;
    using Resources;
    using Services;

    [Route(ScimConstants.Routes.Templates.Self)]
	public class SelfEndpoint : ScimEndpoint
	{
		private readonly IScimUserManager resourceManager;

		public SelfEndpoint(IScimUserManager resourceManager)
		{
			this.resourceManager = resourceManager;
		}

		// Just a stub for now. Assume this claim is required.
		private string UserId => this.User.FindFirst("urn:claims.userId").Value;

		public ScimUser Get()
		{
			var result = this.resourceManager.FindById(this.UserId);

			if (result == null)
			{
				// this will be mappped later in filter
				throw new Exception("User not found");
			}

			return result;
		}

		[HttpPost]
		public IActionResult Create([FromBody] ScimUser item)
		{
			if (!this.ModelState.IsValid)
			{
			    return HttpBadRequest();
            }
			else
			{
				this.resourceManager.Create(item);
                return CreatedAtRoute("GetByIdRoute", new { id = item.Id });
			}
		}

		[HttpDelete]
		public ScimUser DeleteItem()
		{
			var removed = this.resourceManager.Remove(this.UserId, -1);
			if (removed == null)
			{
				throw new Exception("User not found");
			}
			return removed; // 201 No Content
		}
	}
}