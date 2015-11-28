namespace Klaims.Framework.Utility
{
	#region

	using System;
	using System.Diagnostics;

	using Klaims.Framework.Utility.Extensions;

	#endregion

	public sealed class Check
	{
		public static class Argument
		{
			#region Public Methods and Operators


			public static void IsNotNegative(int parameter, string parameterName)
			{
				if (parameter < 0)
				{
					throw new ArgumentOutOfRangeException(parameterName, Resources.ArgumentCannotBeNegative.FormatWith(parameterName));
				}
			}


			public static void IsNotNegative(long parameter, string parameterName)
			{
				if (parameter < 0)
				{
					throw new ArgumentOutOfRangeException(parameterName, Resources.ArgumentCannotBeNegative.FormatWith(parameterName));
				}
			}


			public static void IsNotNegative(float parameter, string parameterName)
			{
				if (parameter < 0)
				{
					throw new ArgumentOutOfRangeException(parameterName, Resources.ArgumentCannotBeNegative.FormatWith(parameterName));
				}
			}


			public static void IsNotNull(object parameter, string parameterName)
			{
				if (parameter == null)
				{
					throw new ArgumentNullException(parameterName, Resources.ArgumentCannotBeNull.FormatWith(parameterName));
				}
			}


			public static void IsNotNullOrEmpty(string parameter, string parameterName, bool trim = false)
			{
				if (string.IsNullOrEmpty(parameter))
				{
					throw new ArgumentException(Resources.ArgumentCannotBeNullOrEmpty.FormatWith(parameterName), parameterName);
				}
			}

			public static void IsNotZeroOrNegative(int parameter, string parameterName)
			{
				if (parameter <= 0)
				{
					throw new ArgumentOutOfRangeException(parameterName, Resources.ArgumentCannotBeNegativeOrZero.FormatWith(parameterName));
				}
			}


			public static void IsNotZeroOrNegative(long parameter, string parameterName)
			{
				if (parameter <= 0)
				{
					throw new ArgumentOutOfRangeException(parameterName, Resources.ArgumentCannotBeNegativeOrZero.FormatWith(parameterName));
				}
			}


			public static void IsNotZeroOrNegative(float parameter, string parameterName)
			{
				if (parameter <= 0)
				{
					throw new ArgumentOutOfRangeException(parameterName, Resources.ArgumentCannotBeNegativeOrZero.FormatWith(parameterName));
				}
			}

			#endregion
		}
	}
}