namespace Klaims.Scim.Resources
{
	using System.Collections.Generic;

	public class ScimSchema :ScimResource
	{
		public ScimSchema(string id) :base(id)
		{
			this.Meta.ResourceType = "Schema";
			this.Meta.Version = "3694e05e9dff596";
		}

		public string Name { get; set; }
		public string Description { get; set; }
		public override string[] Schemas => null;
		public List<SchemaAttribute> Attributes { get; set; }  = new List<SchemaAttribute>();


		public class SchemaAttribute
		{
			public string Name { get; set; }
			public string Type { get; set; }
			public bool MultiValued { get; set; }
			public bool Required { get; set; }
			public bool CaseExact { get; set; }
			public string Description { get; set; }
			public string Mutability { get; set; }
			public string Returned { get; set; }
			public string Uniqueness { get; set; }
			public List<SchemaAttribute> SubAttributes { get; set; }
		}
	}
}