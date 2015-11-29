namespace Klaims.Framework.IdentityMangement
{
	using System;
	using System.Globalization;
	using System.Security.Cryptography;
	using System.Text;

	public class DefaultVersionGenerator : IVersionGenerator
	{
		public string Create()
		{
			var hashAlgorithm = SHA1.Create();
			return "\"" + hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(DateTime.UtcNow.ToString(CultureInfo.InvariantCulture))) + "\"";
		}
	}
}