using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SchedulerBot.Infrastructure.Interfaces.Application;

namespace SchedulerBot.Middleware
{
	/// <summary>
	/// The middleware supposed to update the application context to the actual state upon requests.
	/// </summary>
	public class ApplicationContextMiddleware
	{
		private readonly RequestDelegate next;

		/// <summary>
		/// Initializes a new instance of the <see cref="ApplicationContextMiddleware"/> class.
		/// </summary>
		/// <param name="next">The delegate to be executed after this middleware.</param>
		public ApplicationContextMiddleware(RequestDelegate next)
		{
			this.next = next;
		}

		/// <summary>
		/// Called upon request to update the application context to the actual state.
		/// </summary>
		/// <param name="httpContext">The HTTP context.</param>
		/// <param name="applicationContext">The application context.</param>
		/// <returns>The result of the next delegate execution.</returns>
		public Task InvokeAsync(HttpContext httpContext, IApplicationContext applicationContext)
		{
			applicationContext.Host = httpContext.Request.Host.ToUriComponent();
			applicationContext.Protocol = httpContext.Request.Scheme;

			return next(httpContext);
		}
	}
}
