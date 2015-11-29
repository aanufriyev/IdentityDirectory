namespace IdentityDirectory.Scim.Query
{
    #region

    using System;

    #endregion

    public sealed class ExpresssionOperator
    {
        public static readonly ExpresssionOperator And = new ExpresssionOperator("and", "logical and");

        public static readonly ExpresssionOperator Co = new ExpresssionOperator("co", "contains");

        public static readonly ExpresssionOperator Eq = new ExpresssionOperator("eq", "equal");

        public static readonly ExpresssionOperator Ge = new ExpresssionOperator("ge", "greater than or equal");

        public static readonly ExpresssionOperator Gt = new ExpresssionOperator("gt", "greater than");

        public static readonly ExpresssionOperator Le = new ExpresssionOperator("le", "less than or equal");

        public static readonly ExpresssionOperator Lt = new ExpresssionOperator("lt", "less than");

        public static readonly ExpresssionOperator Or = new ExpresssionOperator("or", "logical or");

        public static readonly ExpresssionOperator Pr = new ExpresssionOperator("pr", "present (has value)");

        public static readonly ExpresssionOperator Sw = new ExpresssionOperator("sw", "starts with");

        public static readonly ExpresssionOperator Unknown = new ExpresssionOperator("", "unknown operator");

        private readonly string _description;

        private readonly string _name;

        private ExpresssionOperator(string val, string desc)
        {
            _name = val;
            _description = desc;
        }

        public override bool Equals(object type)
        {
            var identityType = type as ExpresssionOperator;
            return identityType != null && identityType._name.Equals(_name, StringComparison.OrdinalIgnoreCase);
        }

        public override int GetHashCode()
        {
            return _name.GetHashCode();
        }

        public override string ToString()
        {
            return _name;
        }

        public static ExpresssionOperator GetByName(string name)
        {
            name = name.ToLowerInvariant();

            if (name.Equals(Eq._name))
            {
                return Eq;
            }
            if (name.Equals(Co._name))
            {
                return Co;
            }
            if (name.Equals(Sw._name))
            {
                return Sw;
            }
            if (name.Equals(Pr._name))
            {
                return Pr;
            }
            if (name.Equals(Gt._name))
            {
                return Gt;
            }
            if (name.Equals(Ge._name))
            {
                return Ge;
            }
            if (name.Equals(Lt._name))
            {
                return Lt;
            }
            if (name.Equals(Le._name))
            {
                return Le;
            }
            if (name.Equals(And._name))
            {
                return And;
            }
            if (name.Equals(Or._name))
            {
                return Or;
            }

            return Unknown;
        }
    }
}