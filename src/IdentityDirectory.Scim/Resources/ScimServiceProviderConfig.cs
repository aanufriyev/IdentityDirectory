namespace Klaims.Scim.Resources
{
	using System;
	using System.Collections.Generic;

	public class ScimServiceProviderConfig : ScimResource
	{

		public ScimServiceProviderConfig()
		{
			this.Meta.ResourceType = "ServiceProviderConfig";
			// Just a const from spec for now.
			this.Meta.Version = "3694e05e9dff594";
		}

		public override string[] Schemas => new[] { "urn:ietf:params:scim:schemas:core:2.0:ServiceProviderConfig" };
		
		public string DocumentationUrl { get; set; }
		public List<AuthenticationScheme> AuthenticationSchemes { get; set; } = new List<AuthenticationScheme>();
		public OperationConfig Patch { get; set; } = new OperationConfig();
		public BulkOperationConfig Bulk { get; set; } = new BulkOperationConfig();
		public FilterOperationConfig Filter { get; set; } = new FilterOperationConfig();
		public OperationConfig ChangePassword { get; set; } = new OperationConfig();
		public OperationConfig Sort { get; set; } = new OperationConfig();
		public OperationConfig Etag { get; set; } = new OperationConfig();

		public class OperationConfig
		{
			public bool Supported { get; set; }
		}
		public class BulkOperationConfig : OperationConfig
		{
			public int MaxOperations { get; set; }
			public int MaxPayloadSize { get; set; }
		}
		public class FilterOperationConfig : OperationConfig
        {
			public int MaxResults { get; set; }
		}
		public class AuthenticationScheme
		{
			public string Name { get; set; }
			public string Description { get; set; }
			public string SpecUrl { get; set; }
			public string DocumentationUrl { get; set; }
			public string Type { get; set; }
			public bool? Primary { get; set; }
		}
	}
}