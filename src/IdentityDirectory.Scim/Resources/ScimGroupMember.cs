namespace Klaims.Scim.Resources
{
	using System;
	using System.Collections.Generic;

	using Klaims.Framework;

	using Newtonsoft.Json;

	public class ScimGroupMember
	{
		public enum MemberType
		{
			User,

			Group
		}

		public enum Role
		{
			Member,
			Reader,
			Writer
		}

		public static readonly List<Role> GroupMember = new List<Role> { Role.Member };

		public static readonly List<Role> GroupAdmin = new List<Role> { Role.Reader, Role.Writer };

		private string origin = Constants.Origin.Klaims;

		public ScimGroupMember()
		{
		}

		public ScimGroupMember(string memberId)
			: this(memberId, MemberType.User, GroupMember)
		{
		}

		public ScimGroupMember(string memberId, MemberType type, List<Role> roles)
		{
			MemberId = memberId;
			Type = type;
			Roles = roles;
		}

		[JsonProperty("value")]
		public string MemberId { get; }

		public MemberType Type { get; }

		[JsonIgnore]
		public List<Role> Roles { get; set; }

		public string Origin
		{
			get
			{
				return this.origin;
			}
			set
			{
				if (value == null)
				{
					throw new NullReferenceException();
				}
				this.origin = value;
			}
		}

		public override string ToString()
		{
			return $"(memberId: {this.MemberId}, type: {this.Type}, roles: {this.Roles})";
		}

		public override bool Equals(object o)
		{
			if (this == o)
			{
				return true;
			}
			if (o == null || this.GetType() != o.GetType())
			{
				return false;
			}

			var that = (ScimGroupMember)o;

			if (!this.MemberId.Equals(that.MemberId))
			{
				return false;
			}
			if (!this.origin.Equals(that.Origin))
			{
				return false;
			}
			if (this.Type != that.Type)
			{
				return false;
			}

			return true;
		}

		public override int GetHashCode()
		{
			var result = MemberId.GetHashCode();
			result = 31 * result + Origin.GetHashCode();
			result = 31 * result + Type.GetHashCode();
			return result;
		}
	}
}