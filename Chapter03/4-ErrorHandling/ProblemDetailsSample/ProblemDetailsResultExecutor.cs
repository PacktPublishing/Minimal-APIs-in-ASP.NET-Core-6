using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace ProblemDetailsSample
{
    /// <summary>
    /// Executes an <see cref="ProblemDetailsResultExecutor"/> to write to the response.
    /// </summary>
    public class ProblemDetailsResultExecutor : IActionResultExecutor<ObjectResult>
    {
        /// <summary>
        /// Executes the <see cref="ProblemDetailsResultExecutor"/>.
        /// </summary>
        /// <param name="context">The <see cref="ActionContext"/> for the current request.</param>
        /// <param name="result">The <see cref="ObjectResult"/>.</param>
        /// <returns>
        /// A <see cref="Task"/> which will complete once the <see cref="ObjectResult"/> is written to the response.
        /// </returns>
        public virtual Task ExecuteAsync(ActionContext context, ObjectResult result)
        {
            ArgumentNullException.ThrowIfNull(context);
            ArgumentNullException.ThrowIfNull(result);

            var executor = Results.Json(result.Value, null, "application/problem+json", result.StatusCode);
            return executor.ExecuteAsync(context.HttpContext);
        }
    }
}
