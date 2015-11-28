namespace Klaims.Scim.Tests.Models
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Reflection;
	using System.Text;



	public class TestExpressionVisitor : ExpressionVisitor
	{
		private int? _skip = null;

		private StringBuilder sb;

		public TestExpressionVisitor()
		{
		}

		public int? Skip => this._skip;

		public int? Take { get; private set; } = null;

		public string OrderBy { get; private set; } = string.Empty;

		public string WhereClause { get; private set; } = string.Empty;

		public string Translate(Expression expression)
		{
			this.sb = new StringBuilder();
			this.Visit(expression);
			this.WhereClause = this.sb.ToString();
			return this.WhereClause;
		}

		private static Expression StripQuotes(Expression e)
		{
			while (e.NodeType == ExpressionType.Quote)
			{
				e = ((UnaryExpression)e).Operand;
			}
			return e;
		}

		protected override Expression VisitMethodCall(MethodCallExpression m)
		{
			if (m.Method.DeclaringType == typeof(Queryable) && m.Method.Name == "Where")
			{
				this.Visit(m.Arguments[0]);
				var lambda = (LambdaExpression)StripQuotes(m.Arguments[1]);
				this.Visit(lambda.Body);
				return m;
			}
			else if (m.Method.Name == "Contains")
			{
				//var newMatchExpression = this.Visit(m.Object);
				//var newPatternExpression = this.Visit(m.Arguments[0]);
				//this.sb.AppendFormat("{0} LIKE %{1}%", newMatchExpression, newPatternExpression);
				var memberExpression = (MemberExpression)m.Object;
				var targetExpression = (ConstantExpression)m.Arguments[0];
				this.sb.AppendFormat("{0} LIKE '%{1}%'", memberExpression.Member.Name, targetExpression.Value);
				return m;
				//return this.Visit(m.Arguments[0]);
			}
			else if (m.Method.Name == "Take")
			{
				if (this.ParseTakeExpression(m))
				{
					var nextExpression = m.Arguments[0];
					return this.Visit(nextExpression);
				}
			}
			else if (m.Method.Name == "Skip")
			{
				if (this.ParseSkipExpression(m))
				{
					var nextExpression = m.Arguments[0];
					return this.Visit(nextExpression);
				}
			}
		
			else if (m.Method.Name == "OrderBy")
			{
				if (this.ParseOrderByExpression(m, "ASC"))
				{
					var nextExpression = m.Arguments[0];
					return this.Visit(nextExpression);
				}
			}
			else if (m.Method.Name == "OrderByDescending")
			{
				if (this.ParseOrderByExpression(m, "DESC"))
				{
					var nextExpression = m.Arguments[0];
					return this.Visit(nextExpression);
				}
			}

			throw new NotSupportedException(string.Format("The method '{0}' is not supported", m.Method.Name));
		}

		protected override Expression VisitUnary(UnaryExpression u)
		{
			switch (u.NodeType)
			{
				case ExpressionType.Not:
					sb.Append(" NOT ");
					this.Visit(u.Operand);
					break;
				case ExpressionType.Convert:
					this.Visit(u.Operand);
					break;
				default:
					throw new NotSupportedException(string.Format("The unary operator '{0}' is not supported", u.NodeType));
			}
			return u;
		}

		/// <summary>
		/// </summary>
		/// <param name="b"></param>
		/// <returns></returns>
		protected override Expression VisitBinary(BinaryExpression b)
		{
			sb.Append("(");
			this.Visit(b.Left);

			switch (b.NodeType)
			{
				case ExpressionType.And:
					sb.Append(" AND ");
					break;

				case ExpressionType.AndAlso:
					sb.Append(" AND ");
					break;

				case ExpressionType.Or:
					sb.Append(" OR ");
					break;

				case ExpressionType.OrElse:
					sb.Append(" OR ");
					break;

				case ExpressionType.Equal:
					if (IsNullConstant(b.Right))
					{
						sb.Append(" IS ");
					}
					else
					{
						sb.Append(" = ");
					}
					break;

				case ExpressionType.NotEqual:
					if (IsNullConstant(b.Right))
					{
						sb.Append(" IS NOT ");
					}
					else
					{
						sb.Append(" <> ");
					}
					break;

				case ExpressionType.LessThan:
					sb.Append(" < ");
					break;

				case ExpressionType.LessThanOrEqual:
					sb.Append(" <= ");
					break;

				case ExpressionType.GreaterThan:
					sb.Append(" > ");
					break;

				case ExpressionType.GreaterThanOrEqual:
					sb.Append(" >= ");
					break;

				default:
					throw new NotSupportedException(string.Format("The binary operator '{0}' is not supported", b.NodeType));
			}

			this.Visit(b.Right);
			sb.Append(")");
			return b;
		}

		protected override Expression VisitConstant(ConstantExpression c)
		{
			var q = c.Value as IQueryable;

			if (q == null && c.Value == null)
			{
				sb.Append("NULL");
			}
			else if (q == null)
			{
				switch (Type.GetTypeCode(c.Value.GetType()))
				{
					case TypeCode.Boolean:
						sb.Append(((bool)c.Value) ? 1 : 0);
						break;

					case TypeCode.String:
						sb.Append("'");
						sb.Append(c.Value);
						sb.Append("'");
						break;

					case TypeCode.DateTime:
						sb.Append("'");
						sb.Append(c.Value);
						sb.Append("'");
						break;

					case TypeCode.Object:
						throw new NotSupportedException(string.Format("The constant for '{0}' is not supported", c.Value));

					default:
						sb.Append(c.Value);
						break;
				}
			}

			return c;
		}

		protected override Expression VisitMember(MemberExpression m)
		{
			if (m.Expression != null && m.Expression.NodeType == ExpressionType.Parameter)
			{
				sb.Append(m.Member.Name);
				return m;
			}

			throw new NotSupportedException(string.Format("The member '{0}' is not supported", m.Member.Name));
		}

		protected bool IsNullConstant(Expression exp)
		{
			return (exp.NodeType == ExpressionType.Constant && ((ConstantExpression)exp).Value == null);
		}

		private bool ParseOrderByExpression(MethodCallExpression expression, string order)
		{
			var unary = (UnaryExpression)expression.Arguments[1];
			var lambdaExpression = (LambdaExpression)unary.Operand;

			lambdaExpression = (LambdaExpression)Evaluator.PartialEval(lambdaExpression);

			var body = lambdaExpression.Body as MemberExpression;
			if (body != null)
			{
				if (string.IsNullOrEmpty(this.OrderBy))
				{
					this.OrderBy = string.Format("{0} {1}", body.Member.Name, order);
				}
				else
				{
					this.OrderBy = string.Format("{0}, {1} {2}", this.OrderBy, body.Member.Name, order);
				}

				return true;
			}

			return false;
		}

		private bool ParseTakeExpression(MethodCallExpression expression)
		{
			var sizeExpression = (ConstantExpression)expression.Arguments[1];

			int size;
			if (int.TryParse(sizeExpression.Value.ToString(), out size))
			{
				this.Take = size;
				return true;
			}

			return false;
		}

		private bool ParseContainsExpression(MethodCallExpression expression)
		{
			var sizeExpression = (ConstantExpression)expression.Arguments[1];

			int size;
			if (int.TryParse(sizeExpression.Value.ToString(), out size))
			{
				this.Take = size;
				return true;
			}

			return false;
		}

		

		private bool ParseSkipExpression(MethodCallExpression expression)
		{
			var sizeExpression = (ConstantExpression)expression.Arguments[1];

			int size;
			if (int.TryParse(sizeExpression.Value.ToString(), out size))
			{
				_skip = size;
				return true;
			}

			return false;
		}
	}

	public static class Evaluator
	{
		/// <summary>
		///     Performs evaluation & replacement of independent sub-trees
		/// </summary>
		/// <param name="expression">The root of the expression tree.</param>
		/// <param name="fnCanBeEvaluated">
		///     A function that decides whether a given expression node can be part of the local
		///     function.
		/// </param>
		/// <returns>A new tree with sub-trees evaluated and replaced.</returns>
		public static Expression PartialEval(Expression expression, Func<Expression, bool> fnCanBeEvaluated)
		{
			return new SubtreeEvaluator(new Nominator(fnCanBeEvaluated).Nominate(expression)).Eval(expression);
		}

		/// <summary>
		///     Performs evaluation & replacement of independent sub-trees
		/// </summary>
		/// <param name="expression">The root of the expression tree.</param>
		/// <returns>A new tree with sub-trees evaluated and replaced.</returns>
		public static Expression PartialEval(Expression expression)
		{
			return PartialEval(expression, CanBeEvaluatedLocally);
		}

		private static bool CanBeEvaluatedLocally(Expression expression)
		{
			return expression.NodeType != ExpressionType.Parameter;
		}

		/// <summary>
		///     Evaluates & replaces sub-trees when first candidate is reached (top-down)
		/// </summary>
		private class SubtreeEvaluator : ExpressionVisitor
		{
			private HashSet<Expression> candidates;

			internal SubtreeEvaluator(HashSet<Expression> candidates)
			{
				this.candidates = candidates;
			}

			internal Expression Eval(Expression exp)
			{
				return this.Visit(exp);
			}

			public override Expression Visit(Expression exp)
			{
				if (exp == null)
				{
					return null;
				}
				if (this.candidates.Contains(exp))
				{
					return this.Evaluate(exp);
				}
				return base.Visit(exp);
			}

			private Expression Evaluate(Expression e)
			{
				if (e.NodeType == ExpressionType.Constant)
				{
					return e;
				}
				var lambda = Expression.Lambda(e);
				var fn = lambda.Compile();
				return Expression.Constant(fn.DynamicInvoke(null), e.Type);
			}
		}

		/// <summary>
		///     Performs bottom-up analysis to determine which nodes can possibly
		///     be part of an evaluated sub-tree.
		/// </summary>
		public class Nominator : ExpressionVisitor
		{
			private HashSet<Expression> candidates;

			private bool cannotBeEvaluated;

			private Func<Expression, bool> fnCanBeEvaluated;

			internal Nominator(Func<Expression, bool> fnCanBeEvaluated)
			{
				this.fnCanBeEvaluated = fnCanBeEvaluated;
			}

			internal HashSet<Expression> Nominate(Expression expression)
			{
				this.candidates = new HashSet<Expression>();
				this.Visit(expression);
				return this.candidates;
			}

			public override Expression Visit(Expression expression)
			{
				if (expression != null)
				{
					var saveCannotBeEvaluated = this.cannotBeEvaluated;
					this.cannotBeEvaluated = false;
					base.Visit(expression);
					if (!this.cannotBeEvaluated)
					{
						if (this.fnCanBeEvaluated(expression))
						{
							this.candidates.Add(expression);
						}
						else
						{
							this.cannotBeEvaluated = true;
						}
					}
					this.cannotBeEvaluated |= saveCannotBeEvaluated;
				}
				return expression;
			}
		}
	}
}