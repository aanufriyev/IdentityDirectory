namespace Klaims.Scim.Resources
{
	using System.Collections.Generic;

	public class ScimGroup : ScimResource
	{
		public ScimGroup()
		{
		}

		public ScimGroup(string name)
		{
			this.DisplayName = name;
		}

		public ScimGroup(string id, string name)
			: base(id)
		{
			this.DisplayName = name;
		}

		public override string[] Schemas => new[] { "urn:ietf:params:scim:schemas:core:2.0:Group" };

		public string DisplayName { get; set; }

		public List<ScimGroupMember> Members { get; set; }

		public override string ToString()
		{
			// Until string interpolations works
			return string.Format(
				"(Group id: {0}, name: {1}, created: {2}, modified: {3}, version: {4}, members: {5})",
				this.Id,
				this.DisplayName,
				this.Meta.Created,
				this.Meta.LastModified,
				this.Meta.Version,
				this.Members);
		}
	}
}