using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityDirectory.Scim.Expressions
{
    using Query;

    public class ScimCallExpression : ScimExpression
    {
        public ScimCallExpression(string opName, params ScimExpression[] operands)
        {
            if (opName == null)
            {
                throw new ArgumentNullException("opName");
            }
            if (operands == null)
            {
                throw new ArgumentNullException("operands");
            }
            OperatorName = opName;
            Operands = operands;
        }

        public ScimExpression[] Operands { get; }

        public string OperatorName { get; }
   
        public override string ToString()
        {
            return OperatorName + "(" + string.Join(",", Operands.Select(o => o.ToString())) + ")";
        }
    }
}
