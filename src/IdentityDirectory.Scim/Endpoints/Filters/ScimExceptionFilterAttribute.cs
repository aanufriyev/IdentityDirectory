using System;

namespace Klaims.Scim.Endpoints.Filters
{
	using Klaims.Scim.Services;

	using Microsoft.AspNet.Mvc;
	using Microsoft.AspNet.Mvc.Filters;
	using Microsoft.Net.Http.Headers;

	public class ScimExceptionFilterAttribute : ExceptionFilterAttribute
	{
		private readonly IScimErrorHandler errorHandler = new DefaultScimErrorHandler();
		// Filed an issue, because cannot get controller here.
		public override void OnException(ExceptionContext context)
		{
			var exception = context.Exception;
			if (exception != null)
			{
				var error = this.errorHandler.Handle(exception);
				var result = new ObjectResult(error) { StatusCode = error.Status };
				result.ContentTypes.Add(new MediaTypeHeaderValue(ScimConstants.ScimMediaType));
				context.Result = result;
			}
			else
			{
				base.OnException(context);
			}
		}
	}
}