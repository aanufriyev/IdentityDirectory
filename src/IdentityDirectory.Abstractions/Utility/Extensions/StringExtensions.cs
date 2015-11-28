namespace Klaims.Framework.Utility.Extensions
{
	using System;
	using System.Diagnostics;
	using System.Globalization;

	public static class StringExtensions
	{
		public static string FormatWith(this string template, params object[] args)
		{
			if (string.IsNullOrEmpty(template))
			{
				// Cant use format with and check here
				throw new ArgumentException(string.Format(Resources.ArgumentCannotBeNullOrEmpty, "template"), nameof(template));
			}

			return string.Format(template, args);
		}
		public static bool HasText(this string template)
		{
			return template != null && template.Trim().Length != 0;
		}
	}
}