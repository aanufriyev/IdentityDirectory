namespace Klaims.Scim.Resources
{
	#region

	using System;
	using System.Collections.Generic;
	using System.Linq;

	using Framework.Utility;
	using Framework.Utility.Extensions;

	using Newtonsoft.Json;

	#endregion

	public class ScimUser : ScimResource
	{
		private HashSet<Group> groups = new HashSet<Group>();

		private List<PhoneNumber> phoneNumbers = new List<PhoneNumber>();

		public ScimUser()
		{
		}

		public ScimUser(string id, string userName, string givenName, string familyName)
			: base(id)
		{
			this.UserName = userName;
			this.Name = new CommonName(givenName, familyName);
		}

		public string UserName { get; set; }

		public CommonName Name { get; set; }

		public List<Email> Emails { get; set; }

		public HashSet<Group> Groups
		{
			get
			{
				return new HashSet<Group>(this.groups);
			}
			set
			{
				this.groups = new HashSet<Group>(value);
			}
		}

		public List<PhoneNumber> PhoneNumbers
		{
			get
			{
				return this.phoneNumbers;
			}
			set
			{
				if (value != null && value.Any())
				{
					this.phoneNumbers = value.Where(pn => !string.IsNullOrEmpty(pn?.Value)).ToList();
				}
				else
				{
					this.phoneNumbers = value;
				}
			}
		}

		public List<Address> Addresses { get; set; }

		public List<InstantMessagner> Ims { get; set; }

		public string DisplayName { get; set; }

		public string NickName { get; set; }

		public string ProfileUrl { get; set; }

		public string Title { get; set; }

		public string UserType { get; set; }

		public string PreferredLanguage { get; set; }

		public string Locale { get; set; }

		public string Timezone { get; set; }

		public bool Active { get; set; } = true;

		public bool Verified { get; set; } = false;

		public string Origin { get; set; } = "";

		public string Password { get; set; }

		[JsonIgnore]
		public string PrimaryEmail
		{
			get
			{
				if (this.Emails == null || !this.Emails.Any())
				{
					return null;
				}

				var primaryEmail = this.Emails.FirstOrDefault(m => m.Primary) ?? this.Emails[0];
				return primaryEmail.Value;
			}
			set
			{
				var newPrimaryEmail = new Email { Primary = true, Value = value };
				if (this.Emails == null)
				{
					this.Emails = new List<Email>(1);
				}

				var currentPrimaryEmail = this.Emails.FirstOrDefault(m => m.Primary);
				if (currentPrimaryEmail != null)
				{
					this.Emails.Remove(currentPrimaryEmail);
				}

				this.Emails.Insert(0, newPrimaryEmail);
			}
		}

		[JsonIgnore]
		public string GivenName => this.Name?.GivenName;

		[JsonIgnore]
		public string FamilyName => this.Name?.FamilyName;

		public override string[] Schemas => new[] { "urn:ietf:params:scim:schemas:core:2.0:User" };

		public void AddEmail(string newEmail)
		{
			Check.Argument.IsNotNullOrEmpty(newEmail, "newEmail");
			Ensure.Collection.IsNotNull(this.Emails);

			if (this.Emails.Any(m => m.Value.Equals(newEmail)))
			{
				throw new ArgumentException("Already contains email " + newEmail);
			}
			var e = new Email { Value = newEmail };
			this.Emails.Add(e);
		}

		public void AddPhoneNumber(string newPhoneNumber)
		{
			if (!newPhoneNumber.HasText())
			{
				return;
			}

			Ensure.Collection.IsNotNull(this.PhoneNumbers);

			if (this.PhoneNumbers.Any(m => m.Value.Equals(newPhoneNumber)))
			{
				throw new ArgumentException("Already contains phoneNumber " + newPhoneNumber);
			}

			var number = new PhoneNumber { Value = newPhoneNumber };
			this.PhoneNumbers.Add(number);
		}



		public sealed class Group
		{
			public Group()
				: this(null, null)
			{
			}

			public Group(string value, string display)
				: this(value, display, MembershipType.Direct)
			{
			}

			public Group(string value, string display, MembershipType type)
			{
				this.Value = value;
				this.Display = display;
				this.Type = type;
			}

			public string Value { get; set; }

			public string Display { get; set; }

			public MembershipType Type { get; set; }

			public override int GetHashCode()
			{
				//TODO: Need imutable hascode
				const int Prime = 31;
				var result = 1;
				result = Prime * result + (this.Display?.GetHashCode() ?? 0);
				result = Prime * result + (this.Value?.GetHashCode() ?? 0);
				result = Prime * result + (this.Type?.GetHashCode() ?? 0);
				return result;
			}

			public override bool Equals(object obj)
			{
				if (this == obj)
				{
					return true;
				}
				if (obj == null)
				{
					return false;
				}
				if (this.GetType() != obj.GetType())
				{
					return false;
				}
				var other = (Group)obj;
				if (this.Display == null)
				{
					if (other.Display != null)
					{
						return false;
					}
				}
				else if (!this.Display.Equals(other.Display))
				{
					return false;
				}
				if (this.Value == null)
				{
					if (other.Value != null)
					{
						return false;
					}
				}
				else if (!this.Value.Equals(other.Value))
				{
					return false;
				}
				return this.Type == other.Type;
			}

			public override string ToString()
			{
				return string.Format("(id: {0}, name: {1}, type: {2})", this.Value, this.Display, this.Type);
			}

			public class MembershipType
			{
				public static readonly MembershipType Direct = new MembershipType("direct");

				public static readonly MembershipType Indirect = new MembershipType("indirect");

				private readonly string name;

				private MembershipType(string name)
				{
					this.name = name;
				}

				public override string ToString()
				{
					return this.name;
				}
			}
		}

		public sealed class CommonName
		{
			public CommonName()
			{
			}

			public CommonName(string givenName, string familyName)
			{
				this.GivenName = givenName;
				this.FamilyName = familyName;
				this.Formatted = givenName + " " + familyName;
			}

			public string FamilyName { get; set; }

			public string Formatted { get; set; }

			public string GivenName { get; set; }

			public string HonorificPrefix { get; set; }

			public string HonorificSuffix { get; set; }

			public string MiddleName { get; set; }
		}

		public sealed class Email
		{
			public bool Primary { get; set; }

			public EmailType Type { get; set; }

			public string Value { get; set; }

			public override bool Equals(object obj)
			{
				if (this == obj)
				{
					return true;
				}
				if (obj == null || this.GetType() != obj.GetType())
				{
					return false;
				}

				var email = (Email)obj;

				if (this.Primary != email.Primary)
				{
					return false;
				}
				if (!this.Type?.Equals(email.Type) ?? email.Type != null)
				{
					return false;
				}
				if (!this.Value?.Equals(email.Value) ?? email.Value != null)
				{
					return false;
				}

				return true;
			}

			public override int GetHashCode()
			{
				//TODO: Need imutable hascode
				var result = this.Value?.GetHashCode() ?? 0;
				result = 31 * result + (this.Type?.GetHashCode() ?? 0);
				result = 31 * result + (this.Primary ? 1 : 0);
				return result;
			}

			public class EmailType
			{
				public static readonly EmailType Work = new EmailType("work");

				public static readonly EmailType Home = new EmailType("home");

				private readonly string name;

				private EmailType(string name)
				{
					this.name = name;
				}

				public override bool Equals(object obj)
				{
					var other = obj as EmailType;
					return other != null && other.name.Equals(this.name);
				}

				public override int GetHashCode()
				{
					return this.name.GetHashCode();
				}

				public override string ToString()
				{
					return this.name;
				}
			}
		}

		public sealed class Address
		{
			public AddressType Type { get; set; }
			public string StreetAddress { get; set; }
			public string Locality { get; set; }
			public string Region { get; set; }
			public string PostalCode { get; set; }
			public string Country { get; set; }
			public string Formatted { get; set; }
			public bool? Primary { get; set; }


			public override bool Equals(object obj)
			{
				if (this == obj)
				{
					return true;
				}
				if (obj == null || this.GetType() != obj.GetType())
				{
					return false;
				}

				var target = (Address)obj;

				return this.GetHashCode().Equals(target.GetHashCode());
			}

			public override int GetHashCode()
			{
				//TODO: Need imutable hascode
				var result = this.Formatted?.GetHashCode() ?? 0;
				result = 31 * result + (this.Type?.GetHashCode() ?? 0);
				result = 31 * result + (this.Primary.GetValueOrDefault() ? 1 : 0);
				return result;
			}

			public class AddressType
			{
				public static readonly AddressType Work = new AddressType("work");
				public static readonly AddressType Home = new AddressType("home");
				public static readonly AddressType Other = new AddressType("other");

				private readonly string name;

				private AddressType(string name)
				{
					this.name = name;
				}

				public override bool Equals(object obj)
				{
					var other = obj as AddressType;
					return other != null && other.name.Equals(this.name);
				}

				public override int GetHashCode()
				{
					return this.name.GetHashCode();
				}

				public override string ToString()
				{
					return this.name;
				}
			}
		}

		public sealed class InstantMessagner
		{
			public InstantMessagnerType Type { get; set; }

			public string Value { get; set; }

			public class InstantMessagnerType :TypedEnum
			{
				public static readonly InstantMessagnerType Aim = new InstantMessagnerType("aim");
				public static readonly InstantMessagnerType Gtalk = new InstantMessagnerType("gtalk");
				public static readonly InstantMessagnerType Icq = new InstantMessagnerType("icq");
				public static readonly InstantMessagnerType Xmpp = new InstantMessagnerType("xmpp");
				public static readonly InstantMessagnerType Msn = new InstantMessagnerType("msn");
				public static readonly InstantMessagnerType Skype = new InstantMessagnerType("skype");
				public static readonly InstantMessagnerType Qq = new InstantMessagnerType("qq");
				public static readonly InstantMessagnerType Yahoo = new InstantMessagnerType("yahoo");
				public static readonly InstantMessagnerType Other = new InstantMessagnerType("other");

				protected InstantMessagnerType(string name)
					: base(name)
				{
				}
			}
		}

		public sealed class PhoneNumber
		{
			public PhoneNumberType Type { get; set; }

			public string Value { get; set; }

			public class PhoneNumberType : TypedEnum
			{
				public static readonly PhoneNumberType Work = new PhoneNumberType("work");
				public static readonly PhoneNumberType Home = new PhoneNumberType("home");

				protected PhoneNumberType(string name)
					: base(name)
				{
				}
			}
		}
	}
}