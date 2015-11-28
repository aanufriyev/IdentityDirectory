namespace Klaims.Framework.IdentityMangement.Models
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.Linq;

	using Klaims.Framework.Utility;

	public class UserAccount
	{
		public virtual Guid Id { get; set; }

		[Required]
		public virtual string Username { get; set; }

		public virtual string DisplayName { get; set; }

		public virtual string Nickname { get; set; }

		public virtual string FamilyName { get; set; }

		public virtual string Formatted { get; set; }

		public virtual string GivenName { get; set; }

		public virtual string HonorificPrefix { get; set; }

		public virtual string HonorificSuffix { get; set; }

		public virtual string MiddleName { get; set; }

		public virtual string ProfileUrl { get; set; }

		public virtual string Title { get; set; }

		public virtual string PreferredLanguage { get; set; }

		public virtual string Locale { get; set; }

		public virtual string Timezone { get; set; }

		public virtual DateTime Created { get; set; }

		public virtual DateTime LastUpdated { get; set; }

		public virtual bool IsAccountClosed { get; set; }

		public virtual DateTime? AccountClosed { get; set; }

		public virtual bool IsLoginAllowed { get; set; }

		public virtual DateTime? LastLogin { get; set; }

		public virtual DateTime? LastFailedLogin { get; set; }

		public virtual int FailedLoginCount { get; set; }

		public virtual string Password { get; set; }

		public virtual DateTime? PasswordChanged { get; set; }

		public virtual bool RequiresPasswordReset { get; set; }

		public virtual bool IsAccountVerified { get; set; }

		public virtual DateTime? LastFailedPasswordReset { get; set; }

		public virtual int FailedPasswordResetCount { get; set; }

		public virtual int Version { get; set; }

		// Emails 
		protected virtual ICollection<UserEmail> EmailCollection { get; set; }

		public IEnumerable<UserEmail> Emails => this.EmailCollection;

		public string PrimaryEmail
		{
			get
			{
				if (this.EmailCollection == null || !this.EmailCollection.Any())
				{
					return null;
				}

				var primaryEmail = this.EmailCollection.FirstOrDefault(m => m.Primary) ?? this.EmailCollection.First();
				return primaryEmail.Value;
			}
			set
			{
				Ensure.Collection.IsNotNull(this.EmailCollection);

				var newPrimaryEmail = this.EmailCollection.FirstOrDefault(m => m.Value == value);
				if (newPrimaryEmail == null)
				{
					// Just exit for now.
					return;
				}

				var currentPrimaryEmail = this.EmailCollection.FirstOrDefault(m => m.Primary);
				if (currentPrimaryEmail != null)
				{
					currentPrimaryEmail.Primary = false;
				}
				newPrimaryEmail.Primary = true;
			}
		}

		// Claims 
		protected virtual ICollection<UserClaim> ClaimCollection { get; set; }

		public IEnumerable<UserClaim> Claims => this.ClaimCollection;

		// Phones (work and mobile)
		protected virtual ICollection<UserPhone> PhoneCollection { get; set; }

		public IEnumerable<UserPhone> Phones => this.PhoneCollection;

		protected internal void AddEmail(UserEmail item) => this.EmailCollection.Add(item);

		protected internal void RemoveEmail(UserEmail item) => this.EmailCollection.Remove(item);

		protected internal void AddClaim(UserClaim item) => this.ClaimCollection.Add(item);

		protected internal void RemoveClaim(UserClaim item) => this.ClaimCollection.Remove(item);

		protected internal void AddUserPhone(UserPhone item) => this.PhoneCollection.Add(item);

		protected internal void RemoveUserPhone(UserPhone item) => this.PhoneCollection.Remove(item);
	}
}