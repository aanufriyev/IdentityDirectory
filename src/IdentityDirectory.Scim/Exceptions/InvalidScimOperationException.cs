namespace Klaims.Scim.Exceptions
{
	using System;
	using System.Diagnostics.CodeAnalysis;

	public class InvalidScimOperationException : ScimException
	{
		public InvalidScimOperationException(string message, ScimType type, Exception innerException)
			: base(message, innerException)
		{
			this.Type = type;
		}

		public InvalidScimOperationException(string message, ScimType type)
			: base(message)
		{
			this.Type = type;
		}

		public ScimType Type { get; private set; }
	}

	public class ScimType
	{
		public static readonly ScimType InvalidFilter = new ScimType(
			"invalidFilter",
			"The specified filter syntax was invalid (does not comply with Figure 1) " + "or the specified attribute and filter comparison combination is not supported.");

		public static readonly ScimType TooMany = new ScimType(
			"tooMany",
			"The specified filter yields many more results than the server is willing calculate or process."
			+ " For example, a filter such as \"(userName pr)\" by itself would return all entries with a "
			+ "\"userName\" and MAY not be acceptable to the service provider.");

		public static readonly ScimType Uniqueness = new ScimType("uniqueness", "One or more of attribute values is already in use or is reserved.");

		public static readonly ScimType Mutability = new ScimType(
			"mutability",
			"The attempted modification is not compatible with the target attributes mutability or current"
			+ " state (e.g. modification of an immutable attribute with an existing value).");

		public static readonly ScimType InvalidSyntax = new ScimType(
			"invalidSyntax",
			"The request body message structure was invalid or did not conform to the request schema.");

		public static readonly ScimType InvalidPath = new ScimType("invalidPath", "The path attribute was invalid or malformed (see Figure 7).");

		public static readonly ScimType NoTarget = new ScimType(
			"noTarget",
			"The specified \"path\" did not yield an attribute or attribute value that could be operated on. "
			+ "This occurs when the specified \"path\" value contains a filter that yields no match.");

		public static readonly ScimType InvalidValue = new ScimType(
			"invalidValue",
			"A required value was missing, or the value specified was not compatible with the operation" + " or attribute type.");

		public static readonly ScimType InvalidVers = new ScimType("invalidVers", "The specified SCIM protocol version is not supported");

		// Want to keep description here. Even if it is not used in code.
		[SuppressMessage("ReSharper", "NotAccessedField.Local")]
		private readonly string description;

		private readonly string name;

		private ScimType(string name, string description)
		{
			this.name = name;
			this.description = description;
		}

		public override string ToString()
		{
			return this.name;
		}
	}
}