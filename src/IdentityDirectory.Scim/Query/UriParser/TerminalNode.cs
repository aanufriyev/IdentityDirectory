namespace Klaims.Scim.Query.Filter
{
	public class TerminalNode : FilterNode
	{
		public TerminalNode(Operator filterOperation)
			: base(filterOperation)
		{
		}

		public string Attribute { get; set; }

		public string Value { get; set; }

		public override string ToString()
		{
			return string.Format("({0} {1} {2})", this.Attribute, this.Operator, this.Value);
		}
	}
}