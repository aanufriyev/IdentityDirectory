namespace Klaims.Scim.Resources
{
	#region

	using System;

	#endregion

	public class ScimMetadata
	{
		public ScimMetadata()
		{
		}

		public ScimMetadata(string resourceType, string location, DateTime created, DateTime lastModified, string version)
		{
			this.ResourceType = resourceType;
			this.Location = location;
			this.Created = created;
			this.LastModified = lastModified;
			this.Version = version;
		}

		public string ResourceType { get; set; }

		public string Location { get; set; }

		public string Version { get; set; }

		public DateTime? Created { get; set; } = new DateTime();

		public DateTime? LastModified { get; set; }
	}
}