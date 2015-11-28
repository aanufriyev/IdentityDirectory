namespace Klaims.Framework.Utility
{
	using System;
	using System.Collections;
	using System.Diagnostics.CodeAnalysis;

	public sealed class Ensure
	{
		public static class Collection
		{
			[SuppressMessage("ReSharper", "RedundantAssignment")]
			public static void IsNotNull<T>(T collection) where T:IEnumerable
			{
				if (collection == null)
				{
					collection = Activator.CreateInstance<T>();
				}
			}
		} 
	}
}