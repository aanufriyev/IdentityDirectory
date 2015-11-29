namespace IdentityDirectory.Scim.Endpoints
{
    #region

    using System;
    using Exceptions;
    using Microsoft.AspNet.Mvc;
    using Resources;
    using Services;

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
            var queryResults = resourceManager.Query("id pr");
            return new ScimListResponse<ScimUser>(queryResults, 1);
        }

        [HttpGet("{userId}", Name = "GetScimUserRoute")]
        public ScimUser GetUser(string userId)
        {
            var result = resourceManager.FindById(userId);
            if (result == null)
            {
                throw new ScimResourceNotFoundException("Resource does not exist.");
            }

            return result;
        }

        [HttpPost]
        public ActionResult Create([FromBody] ScimUser item)
        {
            if (!ModelState.IsValid)
            {
                return HttpBadRequest();
            }
            resourceManager.Create(item);
            return CreatedAtRoute("GetByIdRoute", new {id = item.Id});
        }

        [HttpDelete("{id}")]
        public ScimUser DeleteItem(string id)
        {
            var removed = resourceManager.Remove(id, -1);
            if (removed == null)
            {
                throw new Exception("User not found");
            }
            return removed; // 201 No Content
        }
    }
}