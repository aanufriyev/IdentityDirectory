// Ported from eSCIMo under ASL 2.0 

namespace Klaims.Scim.Query.Filter
{
	using System;
	using System.Collections.Generic;
	using System.Text;

	public class UriFilterExpressionParser
	{
		public static FilterNode Parse(string filter)
		{
			if (filter == null)
			{
				return null;
			}

			var position = new Position();
			FilterNode node = null;
			while (position.Value < filter.Length)
			{
				var @char = filter[position.Value];
				FilterNode next;
				switch (@char)
				{
					case ' ':
						position.Increment();
						continue;

					case '(':
						var group = GetWithinParenthesis(position, filter);
						next = Parse(group);
						break;

					default:
						next = ParseNode(position, filter);
						break;
				}
				node = AddChildNode(node, next);
			}

			return node;
		}

		private static FilterNode AddChildNode(FilterNode parent, FilterNode child)
		{
			if (parent == null)
			{
				return child;
			}

			var branchNode = parent as BranchNode;
			if (branchNode != null)
			{
				if (!branchNode.HasBothChildren)
				{
					branchNode.AddNode(child);
					return parent;
				}
				var childNode = child as BranchNode;
				if (childNode != null)
				{
					childNode.AddNode(branchNode);
					return child;
				}
			}

			var node = child as BranchNode;
			if (node == null)
			{
				return null;
			}
			node.AddNode(parent);
			return child;
		}

		private static FilterNode ParseNode(Position position, string filter)
		{
			var attribute = ParseToken(position, filter);

			var branchOperator = Operator.GetByName(attribute);

			if (!branchOperator.Equals(Operator.Unknown))
			{
				if (!branchOperator.Equals(Operator.And) && !branchOperator.Equals(Operator.Or))
				{
					throw new InvalidOperationException("Invalid predicate in filter, expected an attribute or an operator token but found " + attribute);
				}
				return new BranchNode(branchOperator);
			}

			var filterOperator = Operator.GetByName(ParseToken(position, filter));

			var currentPosition = position.Value;

			string value = null;

			var valOrOperator = ParseToken(position, filter);

			if (!Operator.GetByName(valOrOperator).Equals(Operator.Unknown))
			{
				// move back
				position.Set(currentPosition);
			}
			else
			{
				value = valOrOperator;
			}

			if (filterOperator.Equals(Operator.And) || filterOperator.Equals(Operator.Or))
			{
				throw new InvalidOperationException("Invalid predicate in filter, expected a non branching operator but found " + filterOperator);
			}

			return new TerminalNode(filterOperator) { Attribute = attribute, Value = value };
		}

		private static string ParseToken(Position pos, string filter)
		{
			var foundNonSpace = false;

			var sb = new StringBuilder();

			var prevChar = ' ';
			var isEscaped = false;
			var startQuote = false;
			var endQuote = false;
			while (pos.Value < filter.Length)
			{
				var c = filter[pos.Value];
				pos.Increment();

				if ((prevChar == '\\') && (c != '\\'))
				{
					isEscaped = true;
				}

				if (c == '"')
				{
					if (!isEscaped)
					{
						if (startQuote)
						{
							endQuote = true;
						}
						else
						{
							startQuote = true;
						}

						continue;
					}
				}

				switch (c)
				{
					case ' ':
						if (!foundNonSpace || (startQuote && !endQuote))
						{
							continue;
						}
						return sb.ToString();
					default:
						sb.Append(c);
						foundNonSpace = true;
						break;
				}

				prevChar = c;
			}

			return sb.ToString();
		}

		// Gives the complete string present inside the open and end parentheses
		// pos the current position in filter
		// filter the filter expression
		private static string GetWithinParenthesis(Position pos, string filter)
		{
			var start = -1;
			var end = -1;

			var stack = new Stack<int>();

			var prevChar = ' ';
			var startQuote = false;
			var endQuote = false;
			var stop = false;

			while (!stop && (pos.Value < filter.Length))
			{
				var c = filter[pos.Value];

				switch (c)
				{
					case '"':
						if (startQuote && prevChar != '\\')
						{
							endQuote = true;
						}
						else if (!startQuote)
						{
							startQuote = true;
						}
						break;

					case '(':
						if (!startQuote)
						{
							if (start == -1)
							{
								start = pos.Value + 1;
							}
							stack.Push(pos.Value);
						}
						break;

					case ')':
						if (!startQuote)
						{
							if (stack.Count == 1)
							{
								end = pos.Value;
								stop = true;
							}
							else
							{
								stack.Pop();
							}
						}
						break;
				}

				if (endQuote)
				{
					startQuote = false;
					endQuote = false;
				}

				prevChar = c;
				pos.Increment();
			}

			return filter.Substring(start, end - start);
		}

		//Internal class for keeping track of the index position while parsing the filter.
		private class Position
		{
			public Position()
				: this(0)
			{
			}

			private Position(int pos)
			{
				this.Value = pos;
			}

			public int Value { get; private set; }

			public void Increment()
			{
				this.Value++;
			}

			public void Set(int pos)
			{
				this.Value = pos;
			}

			public void Decrement()
			{
				this.Value--;
			}
		}
	}
}