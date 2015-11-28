namespace Klaims.Scim.Resources
{
	public abstract class ScimResource
	{
		protected ScimResource(string id)
		{
			this.Id = id;
		}

		protected ScimResource()
		{
		}

		public ScimMetadata Meta { get; protected set; } = new ScimMetadata();

		public abstract string[] Schemas { get; }

		public string Id { get; }

		public string ExternalId { get; set; }

		public override int GetHashCode()
		{
			return this.Id?.GetHashCode() ?? base.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			var other = obj as ScimResource;
			if (other != null)
			{
				return this.Id?.Equals(other.Id) ?? false;
			}
			var otherId = obj as string;
			return otherId != null && this.Id.Equals(otherId);
		}
	}
}