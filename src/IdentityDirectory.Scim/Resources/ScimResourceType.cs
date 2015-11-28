namespace Klaims.Scim.Resources
{
	using System.Collections.Generic;

	public class ScimResourceType : ScimResource
	{
		public ScimResourceType(string id)
			: base(id)
		{
			this.Meta.ResourceType = "ResourceType";
		}

		public string Name { get; set; }

		public string Endpoint { get; set; }

		public string Description { get; set; }

		public string Schema { get; set; }

		public List<SchemaExtension> SchemaExtensions { get; set; }

		public override string[] Schemas => new[] { "urn:ietf:params:scim:schemas:core:2.0:ResourceType" };

		public class SchemaExtension
		{
			public string Schema { get; set; }

			public string Requires { get; set; }
		}
	}
}