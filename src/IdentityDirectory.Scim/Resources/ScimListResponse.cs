namespace Klaims.Scim.Resources
{
	using System.Collections.Generic;

	public class ScimListResponse<TResource> : ScimResource
		where TResource : ScimResource
	{
		public ScimListResponse(IEnumerable<TResource> resources, int total, int? startIndex = null, int? itemsPerPage = null)
		{
			this.Resources = new List<TResource>(resources);
			this.TotalResults = total;
			this.StartIndex = startIndex;
			this.ItemsPerPage = itemsPerPage;
		}

		public override string[] Schemas => new[] { "urn:ietf:params:scim:schemas:core:2.0:ListResponse" };

		//	The total number of results returned by the list or query operation.This may not be equal to the number of elements
		//  in the resources attribute of the list response if pagination (Section 3.2.2.4) is requested.REQUIRED.
		public int TotalResults { get; set; }

		// A multi-valued list of complex objects containing the requested resources.This may be a subset of the full set of
		// resources if pagination(Section 3.2.2.4) is requested. REQUIRED if "totalResults" is non-zero.
		public List<TResource> Resources { get; set; }

		//The 1-based index of the first result in the current set of list results. REQUIRED when partial results returned due to pagination.
		public int? StartIndex { get; set; }

		// The number of resources returned in a list response page. REQUIRED when partial results returned due to pagination.
		public int? ItemsPerPage { get; set; }
	}
}