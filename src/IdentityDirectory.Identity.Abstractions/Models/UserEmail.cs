namespace Klaims.Framework.IdentityMangement.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;

	public class UserEmail
	{
		public virtual int Id { get; protected internal set; }

		public virtual Guid UserId { get; protected internal set; }

		public virtual bool Primary { get; protected internal set; }

		public virtual string Type { get; protected internal set; }

		[EmailAddress]
		[StringLength(254)]
		public virtual string Value { get; protected internal set; }
	}
}