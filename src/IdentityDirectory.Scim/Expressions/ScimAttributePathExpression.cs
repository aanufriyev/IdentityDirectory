namespace IdentityDirectory.Scim.Expressions
{
    #region

    using System;
    using System.Linq;
    using Query;

    #endregion

    public class ScimAttributePathExpression : ScimExpression
    {
        public string AttributePath { get; set; }

        public ScimAttributePathExpression(string attrPath)
        {
            if (attrPath == null)
            {
                throw new ArgumentNullException("attrPath");
            }

            AttributePath = attrPath;

        }

        public override string ToString()
        {
            return AttributePath;
        }
    }
}