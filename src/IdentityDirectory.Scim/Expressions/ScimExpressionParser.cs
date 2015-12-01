namespace IdentityDirectory.Scim.Query
{
    using System;
    using Sprache;
    using System.Linq;

    public class ScimExpressionParser
    {
        private static readonly Parser<string> And = Operator("and", "And");
        private static readonly Parser<string> Eq = Operator("eq", "Equal");
        private static readonly Parser<string> Ne = Operator("ne", "NotEqual");
        private static readonly Parser<string> Gt = Operator("gt", "GreaterThan");
        private static readonly Parser<string> Ge = Operator("ge", "GreaterThanOrEqual");
        private static readonly Parser<string> Or = Operator("or", "Or");
        private static readonly Parser<string> Lt = Operator("lt", "LessThan");
        private static readonly Parser<string> Le = Operator("le", "LessThanOrEqual");
        private static readonly Parser<string> Co = Operator("co", "Contains");
        private static readonly Parser<string> Sw = Operator("sw", "StartsWith");
        private static readonly Parser<string> Ew = Operator("ew", "EndsWith");
        private static readonly Parser<string> Pr = Operator("pr", "Present");

        private static readonly Parser<string> IdentifierName;
        private static readonly Parser<ScimExpression> AttrName;
        private static readonly Parser<ScimExpression> AttrPath;
        private static readonly Parser<Func<ScimExpression, ScimExpression>> SubAttr;
        private static readonly Parser<Func<ScimExpression, ScimExpression>> ValuePath;

        private static readonly Parser<ScimExpression> CaseInsensitiveString;
        private static readonly Parser<ScimExpression> Operand;
        private static readonly Parser<ScimExpression> Literal;
        private static readonly Parser<ScimExpression> Filter;
        private static readonly Parser<ScimExpression> ExpressionInParentheses = from lparen in Parse.Char('(')
                                                                                 from expr in Parse.Ref(() => Filter)
                                                                                 from rparen in Parse.Char(')')
                                                                                 select expr;



        private static readonly Parser<ScimExpression> LogicalExpression;


        private static readonly Parser<ScimExpression> AttributeExpression;

        private static readonly Parser<ScimExpression> Comparison;
        private static readonly Parser<ScimExpression> Presence;


        private static readonly Parser<char> StringContentChar = Parse.CharExcept("\\\"").Or(Parse.String("\\\\").Return('\\')).Or(Parse.String("\\\"").Return('\"'));
        private static readonly Parser<string> QuotedString = from open in Parse.Char('\"')
                                                              from content in StringContentChar.Many().Text()
                                                              from close in Parse.Char('\"')
                                                              select content;
        


        static ScimExpressionParser()
        {
            CaseInsensitiveString = from content in QuotedString
                                    select ScimExpression.String(content);


            IdentifierName = Parse.Identifier(Parse.Letter, Parse.LetterOrDigit);

            //compValue = false / null / true / number / string 
            //; rules from JSON(RFC 7159)
            Literal = Parse.String("true").Return(ScimExpression.Constant(true))
                .XOr(Parse.String("false").Return(ScimExpression.Constant(false)))
                .XOr(Parse.String("null").Return(ScimExpression.Constant(null)));

            //ATTRNAME = ALPHA * (nameChar)
            //nameChar = "-" / "_" / DIGIT / ALPHA
            // TODO : check - and _
            AttrName = IdentifierName.Select(ScimExpression.Attribute);

            //valuePath = attrPath "[" valFilter "]"
            //    ; FILTER uses sub - attributes of a parent attrPath
            ValuePath = from open in Parse.Char('[')
                        from expr in Parse.Ref(() => Filter)
                        from close in Parse.Char(']')
                        select new Func<ScimExpression, ScimExpression>(r => ScimExpression.Binary("Where", r, expr));

            //subAttr = "." ATTRNAME
            //; a sub-attribute of a complex attribute
            SubAttr = Parse.Char('.')
                .Then(_ => IdentifierName)
                .Then(n => Parse.Return(new Func<ScimExpression, ScimExpression>(r => ScimExpression.SubAttribute(n, r))));

            //attrPath = [URI ":"] ATTRNAME * 1subAttr
            //     ; SCIM attribute name
            //     ; URI is SCIM "schema" URI
            AttrPath = AttrName
                .SelectMany(root => SubAttr.XOr(ValuePath).XMany(), (name, path) => path.Aggregate(name, (o, f) => f(o)));

            Operand = (ExpressionInParentheses
                .XOr(Literal.Or(AttrPath.Token()))
                .XOr(CaseInsensitiveString)).Token();

            // compareOp = "eq" / "ne" / "co" /
            //        "sw" / "ew" /
            //        "gt" / "lt" /
            //        "ge" / "le"
            Comparison = Parse.XChainOperator(Le.Or(Lt).XOr(Ge.Or(Gt)).XOr(Eq.Or(Ne)).XOr(Sw.Or(Ew)).XOr(Co).XOr(Pr), Operand, ScimExpression.Binary);

            // attrPath SP "pr"
            Presence = Operand.SelectMany(operand => Pr, (operand, pr) => ScimExpression.Unary(pr, operand));

            // attrExp = (attrPath SP "pr") /
            //   (attrPath SP compareOp SP compValue)
            AttributeExpression = Presence.Or(Comparison);

            // logExp    = FILTER SP ("and" / "or") SP FILTER
            LogicalExpression = Parse.XChainOperator(Or.Or(And), AttributeExpression, ScimExpression.Binary);
            Filter = LogicalExpression;
        }

        public static ScimExpression ParseExpression(string expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }
            return Filter.End().Parse(expression);
        }

        private static Parser<string> Operator(string op, string opName)
        {
            return Parse.String(op).Token().Return(opName);
        }



    }
}