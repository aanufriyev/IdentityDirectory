namespace IdentityDirectory.Scim.Query
{
    #region

    using System;

    #endregion

    public sealed class ExpressionOperator
    {
        public static readonly ExpressionOperator And = new ExpressionOperator("and", "logical and");

        public static readonly ExpressionOperator Co = new ExpressionOperator("co", "contains");

        public static readonly ExpressionOperator Eq = new ExpressionOperator("eq", "equal");

        public static readonly ExpressionOperator Ge = new ExpressionOperator("ge", "greater than or equal");

        public static readonly ExpressionOperator Gt = new ExpressionOperator("gt", "greater than");

        public static readonly ExpressionOperator Le = new ExpressionOperator("le", "less than or equal");

        public static readonly ExpressionOperator Lt = new ExpressionOperator("lt", "less than");

        public static readonly ExpressionOperator Or = new ExpressionOperator("or", "logical or");

        public static readonly ExpressionOperator Pr = new ExpressionOperator("pr", "present (has value)");

        public static readonly ExpressionOperator Sw = new ExpressionOperator("sw", "starts with");

        public static readonly ExpressionOperator Unknown = new ExpressionOperator("", "unknown operator");

        private readonly string _description;

        private readonly string _name;

        private ExpressionOperator(string val, string desc)
        {
            _name = val;
            _description = desc;
        }

        public override bool Equals(object type)
        {
            var identityType = type as ExpressionOperator;
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

        public static ExpressionOperator GetByName(string name)
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