namespace Klaims.Scim.Resources
{
	public class TypedEnum
	{
		private readonly string name;

		protected TypedEnum(string name)
		{
			this.name = name;
		}

		public override bool Equals(object obj)
		{
			var other = obj as TypedEnum;
			return other != null && other.name.Equals(this.name);
		}

		public override int GetHashCode()
		{
			return this.name.GetHashCode();
		}

		public override string ToString()
		{
			return this.name;
		}
	}
}