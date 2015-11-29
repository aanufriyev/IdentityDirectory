namespace Klaims.Framework.IdentityMangement.Models
{
	using System;

	public class UserClaim
	{
		public virtual int Id { get; protected internal set; }

		public virtual Guid UserId { get; protected internal set; }

		public virtual string ClaimType { get; protected internal set; }

		public virtual string ClaimValue { get; protected internal set; }
	}
}