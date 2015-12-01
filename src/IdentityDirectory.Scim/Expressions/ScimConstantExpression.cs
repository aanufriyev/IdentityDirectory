namespace IdentityDirectory.Scim.Expressions
{
    #region

    using Query;

    #endregion

    public class ScimConstantExpression : ScimExpression
    {
        public ScimConstantExpression(object constantValue)
        {
            ConstantValue = constantValue;
        }


        public object ConstantValue { get; }

        public override string ToString()
        {
            return (ConstantValue ?? "null").ToString();
        }
    }
}