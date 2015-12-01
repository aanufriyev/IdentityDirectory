namespace IdentityDirectory.Scim.Query
{
    using System;
    using Expressions;

    public class ScimExpression
    {
        public static ScimExpression String(string value)
        {
            return new ScimStringExpression(value);
        }

        public static ScimExpression Binary(string opName, ScimExpression leftOperand, ScimExpression rightOperand)
        {
            return new ScimCallExpression(opName, leftOperand, rightOperand);
        }

        public static ScimExpression Unary(string opName, ScimExpression operand)
        {
            return new ScimCallExpression(opName, operand);
        }

        public static ScimExpression Attribute(string name)
        {
            return new ScimAttributePathExpression(name);
        }

        public static ScimExpression SubAttribute(string name, ScimExpression parent)
        {
            return new ScimSubAttributeExpression(name, parent);
        }

        public static ScimExpression Constant(object constantValue)
        {
            return new ScimConstantExpression(constantValue);
        }
    }
}