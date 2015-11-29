namespace IdentityDirectory.Scim.Services
{
    using System.Linq;

    public class DefaultAttributeNameMapper :IAttributeNameMapper
	{
		// Simple uppercase for now.
		public string MapToInternal(string attr)
		{
		    var namePathParts = attr.Split('.');
		    var mappedPathParts = namePathParts.Select(part => char.ToUpper(part[0]) + part.Substring(1));
            return string.Join(".", mappedPathParts);
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