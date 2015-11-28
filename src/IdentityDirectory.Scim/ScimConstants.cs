namespace Klaims.Scim
{
    public class ScimConstants
    {
	    public const string ScimMediaType = "application/json+scim";

        public class Routes
        {
            public class Templates
            {
                public const string Users = "scim/v2/Users";
                public const string Groups = "scim/v2/Groups";
                public const string Search = "scim/v2/Search";
                public const string Bulk = "scim/v2/Bulk";
                public const string Schemas = "scim/v2/Schemas";
                public const string ResourceTypes = "scim/v2/ResourceTypes";
                public const string Self = "scim/v2/Self";
                public const string ServiceProviderConfig = "scim/v2/ServiceProviderConfig";
            }

        } 
    }
}