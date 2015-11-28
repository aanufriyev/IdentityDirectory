namespace Klaims.Scim.Services
{
	public class DefaultAttributeNameMapper :IAttributeNameMapper
	{
		// Simple uppercase for now.
		public string MapToInternal(string attr)
		{
			return char.ToUpper(attr[0]) + attr.Substring(1);
		}

		public string[] MapToInternal(string[] attr)
		{
			throw new System.NotImplementedException();
		}

		public string MapFromInternal(string attr)
		{
			throw new System.NotImplementedException();
		}

		public string[] MapFromInternal(string[] attr)
		{
			throw new System.NotImplementedException();
		}
	}
}