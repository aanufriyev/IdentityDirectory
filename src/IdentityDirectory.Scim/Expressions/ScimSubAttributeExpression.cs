namespace IdentityDirectory.Scim.Expressions
{
    using System;
    using Query;

    public class ScimSubAttributeExpression : ScimExpression
    {
        public string AttributePath { get; }

        public ScimExpression Parent { get; }

        public ScimSubAttributeExpression(string attrPath, ScimExpression parent)
        {
            if (attrPath == null)
                throw new ArgumentNullException("attrPath");

            this.AttributePath = attrPath;
            this.Parent = parent;
        }

        public override string ToString()
        {
            return this.Parent + "." + this.AttributePath;
        }
    }
}