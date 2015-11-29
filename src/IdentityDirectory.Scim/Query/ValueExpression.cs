namespace IdentityDirectory.Scim.Query
{
    public class ValueExpression : FilterExpresstion
    {
        public ValueExpression(ExpresssionOperator filterOperation)
            : base(filterOperation)
        {
        }

        public string Attribute { get; set; }

        public string Value { get; set; }

        public override string ToString()
        {
            return $"({Attribute} {Operator} {Value})";
        }
    }
}