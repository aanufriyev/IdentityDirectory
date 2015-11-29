namespace IdentityDirectory.Scim.Query
{
	public abstract class FilterExpresstion
	{
		protected FilterExpresstion(ExpresssionOperator filterOperator)
		{
			this.Operator = filterOperator;
		}

		public ExpresssionOperator Operator { get; private set; }
	}
}
