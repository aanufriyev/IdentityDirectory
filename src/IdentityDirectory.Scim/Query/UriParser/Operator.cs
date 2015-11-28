namespace Klaims.Scim.Query.Filter
{
	using System;

	public sealed class Operator
	{
		private Operator(string val, string desc)
		{
			this.name = val;
			this.description = desc;
		}

		public static readonly Operator And = new Operator("and", "logical and");

		public static readonly Operator Co = new Operator("co", "contains");

		public static readonly Operator Eq = new Operator("eq", "equal");

		public static readonly Operator Ge = new Operator("ge", "greater than or equal");

		public static readonly Operator Gt = new Operator("gt", "greater than");

		public static readonly Operator Le = new Operator("le", "less than or equal");

		public static readonly Operator Lt = new Operator("lt", "less than");

		public static readonly Operator Or = new Operator("or", "logical or");

		public static readonly Operator Pr = new Operator("pr", "present (has value)");

		public static readonly Operator Sw = new Operator("sw", "starts with");

		public static readonly Operator Unknown = new Operator("", "unknown operator");

		private readonly string description;

		private readonly string name;

		public override bool Equals(object type)
		{
			var identityType = type as Operator;
			return identityType != null && identityType.name.Equals(this.name, StringComparison.OrdinalIgnoreCase);
		}

		public override int GetHashCode()
		{
			return this.name.GetHashCode();
		}

		public override string ToString()
		{
			return this.name;
		}

		public static Operator GetByName(string name)
		{
			name = name.ToLowerInvariant();

			if (name.Equals(Eq.name))
			{
				return Eq;
			}
			if (name.Equals(Co.name))
			{
				return Co;
			}
			if (name.Equals(Sw.name))
			{
				return Sw;
			}
			if (name.Equals(Pr.name))
			{
				return Pr;
			}
			if (name.Equals(Gt.name))
			{
				return Gt;
			}
			if (name.Equals(Ge.name))
			{
				return Ge;
			}
			if (name.Equals(Lt.name))
			{
				return Lt;
			}
			if (name.Equals(Le.name))
			{
				return Le;
			}
			if (name.Equals(And.name))
			{
				return And;
			}
			if (name.Equals(Or.name))
			{
				return Or;
			}

			return Unknown;
		}
	}
}