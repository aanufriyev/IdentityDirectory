namespace IdentityDirectory.Scim.Expressions
{
    #region

    using System;
    using Query;

    #endregion

    public class ScimStringExpression : ScimExpression
    {
        public ScimStringExpression(string value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            Value = value;
        }

        public string Value { get; }

        public override string ToString()
        {
            return "\"" + Value.Replace("\\", "\\\\").Replace("\"", "\\\"") + "\"";
        }
    }
}