namespace Klaims.Scim.Services
{
	using System;
	using System.Collections.Generic;
	using System.Net;

	using Klaims.Scim.Exceptions;
	using Klaims.Scim.Resources;

	public class DefaultScimErrorHandler : IScimErrorHandler
	{
		private static readonly Dictionary<Type, HttpStatusCode> BaseExceptionsMap = new Dictionary<Type, HttpStatusCode>();

		static DefaultScimErrorHandler()
		{
			BaseExceptionsMap.Add(typeof(ScimResourceConstraintException), HttpStatusCode.PreconditionFailed);
			BaseExceptionsMap.Add(typeof(ScimResourceConflictException), HttpStatusCode.Conflict);
			BaseExceptionsMap.Add(typeof(ScimResourceNotFoundException), HttpStatusCode.NotFound);
			BaseExceptionsMap.Add(typeof(InvalidScimOperationException), HttpStatusCode.BadRequest);
			BaseExceptionsMap.Add(typeof(NotImplementedException), HttpStatusCode.NotImplemented);
		}

		public ScimError Handle(Exception exception)
		{
			var exceptionType = exception.GetType();
			var detail = exception.Message;
			var statusCode = !BaseExceptionsMap.ContainsKey(exceptionType) ? HttpStatusCode.InternalServerError : BaseExceptionsMap[exceptionType];
			var invalidOperationException = exception as InvalidScimOperationException;

			return invalidOperationException != null ? new ScimError(statusCode, invalidOperationException.Type, detail) : new ScimError(statusCode, detail);
		}
	}
}