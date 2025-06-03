using LiveLib.Application.Commom.ResultWrapper;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace LiveLib.Api.Common
{
    public class ControllerApiBase : ControllerBase
    {
        [NonAction]
        public IActionResult ToActionResult(Result result)
        {
            if (result.IsSuccess)
            {
                return result.SuccessInfo!.Code switch
                {
                    SuccessCode.Ok => Ok(result.SuccessInfo.Message),
                    SuccessCode.Created => Created(string.Empty, result.SuccessInfo.Message),
                    SuccessCode.Accepted => Accepted(result.SuccessInfo.Message),
                    SuccessCode.NoContent => NoContent(),
                    _ => Ok(result.SuccessInfo.Message ?? string.Empty)
                };
            }

            return result.ErrorInfo!.Code switch
            {
                ErrorCode.Conflict => Conflict(ToProblemDetails(result.ErrorInfo, HttpStatusCode.Conflict)),
                ErrorCode.Forbiden => Forbid(ToProblemDetails(result.ErrorInfo, HttpStatusCode.Forbidden)),
                ErrorCode.NotFound => NotFound(ToProblemDetails(result.ErrorInfo, HttpStatusCode.NotFound)),
                ErrorCode.ServerError => Problem(
                    detail: result.ErrorInfo.Message,
                    statusCode: (int)HttpStatusCode.InternalServerError),
                ErrorCode.BadRequest => BadRequest(ToProblemDetails(result.ErrorInfo, HttpStatusCode.BadRequest)),
                _ => StatusCode((int)HttpStatusCode.InternalServerError,
                    ToProblemDetails(result.ErrorInfo, HttpStatusCode.InternalServerError))
            };
        }

        [NonAction]
        public IActionResult ToActionResult<T>(Result<T> result)
        {
            if (result.IsSuccess)
            {
                if (result.Value is null)
                {
                    return NoContent();
                }

                return result.SuccessInfo!.Code switch
                {
                    SuccessCode.Ok => Ok(result.Value),
                    SuccessCode.NoContent => NoContent(),
                    SuccessCode.Accepted => Accepted(result.Value),
                    SuccessCode.Created => Created(string.Empty, result.Value),
                    _ => Ok(result.Value)
                };
            }

            return result.ErrorInfo!.Code switch
            {
                ErrorCode.NotFound => NotFound(ToProblemDetails(result.ErrorInfo, HttpStatusCode.NotFound)),
                ErrorCode.Conflict => Conflict(ToProblemDetails(result.ErrorInfo, HttpStatusCode.Conflict)),
                ErrorCode.ServerError => Problem(
                    detail: result.ErrorInfo.Message,
                    statusCode: (int)HttpStatusCode.InternalServerError),
                ErrorCode.Forbiden => Forbid(ToProblemDetails(result.ErrorInfo, HttpStatusCode.Forbidden)),
                ErrorCode.BadRequest => BadRequest(ToProblemDetails(result.ErrorInfo, HttpStatusCode.BadRequest)),
                _ => StatusCode((int)HttpStatusCode.InternalServerError,
                    ToProblemDetails(result.ErrorInfo, HttpStatusCode.InternalServerError))
            };
        }

        [NonAction]
        protected virtual ProblemDetails ToProblemDetails(ErrorInfo errorInfo, HttpStatusCode statusCode)
        {
            return new ProblemDetails
            {
                Title = errorInfo.Code.ToString(),
                Detail = errorInfo.Message,
                Status = (int)statusCode,
                Type = $"https://httpstatuses.com/{(int)statusCode}"
            };
        }
    }
}