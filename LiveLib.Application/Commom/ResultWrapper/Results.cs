namespace LiveLib.Application.Commom.ResultWrapper
{
    public enum ErrorCode
    {
        Conflict,
        Forbiden, 
        Common,
        NotFound,
        ServerError,
        BadRequest,
        Validation,
    }

    public enum SuccessCode
    {
        Ok,
        Created,
        Accepted,
        NoContent,
    }

    public record Error(ErrorCode Code, string Message)
    {
        public static Error Conflict(string? message = null) => new(ErrorCode.Conflict, message ?? "Resource already exists");
        public static Error Forbiden(string? message = null) => new(ErrorCode.Forbiden, message ?? "Forbid access");
        public static Error Common(string? message = null) => new(ErrorCode.Common, message ?? "Error has occrured");
        public static Error NotFound(string? message = null) => new(ErrorCode.NotFound, message ?? "Resource not found");
        public static Error Validation(string? message = null) => new(ErrorCode.Validation, message ?? "Validation error");
        public static Error ServerError(string? message = null) => new(ErrorCode.ServerError, message ?? "Internal server error");
        public static Error BadRequest(string? message = null) => new(ErrorCode.BadRequest, message ?? "Bad request");
    }

    public record Success(SuccessCode Code, string Message)
    {
        public static Success Ok(string? message = null) => new(SuccessCode.Ok, message ?? "Success");
        public static Success Created(string? message = null) => new(SuccessCode.Created, message ?? "Created");
        public static Success Accepted(string? message = null) => new(SuccessCode.Accepted, message ?? "Accepted");
        public static Success NoContent(string? message = null) => new(SuccessCode.NoContent, message ?? "No content");
    }

    public class Result
    {
        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;
        public Error? ErrorInfo { get; }
        public Success? SuccessInfo { get; }

        protected Result(bool isSuccess, Error? errorStatus = null, Success? successStatus = null)
        {
            if (isSuccess && errorStatus != null)
                throw new InvalidOperationException("Success result cannot have an error");
            if (!isSuccess && successStatus != null)
                throw new InvalidOperationException("Failure result must have an error");

            IsSuccess = isSuccess;
            ErrorInfo = errorStatus;
            SuccessInfo = successStatus;
        }

        public static Result Success(Success success) => new(true, successStatus: success);
        public static Result Ok(string? message = null) => new(true, successStatus: Success.Ok(message));
        public static Result Created(string? message = null) => new(true, successStatus: Success.Created(message));
        public static Result Accepted(string? message = null) => new(true, successStatus: Success.Accepted(message));
        public static Result NoContent(string? message = null) => new(true, successStatus: Success.NoContent(message));
        public static Result<T> Success<T>(T value, Success? success = null) => Result<T>.Success(value, success ?? Success.Ok());

        public static Result Failure(Error error) => new(false, errorStatus: error);
        public static Result Failure(string? message = null) => new(false, errorStatus: Error.Common(message));
        public static Result Conflict(string? message = null) => new(false, errorStatus: Error.Conflict(message));
        public static Result Forbiden(string? message = null) => new(false, errorStatus: Error.Forbiden(message));
        public static Result Common(string? message = null) => new(false, errorStatus: Error.Common(message));
        public static Result Validation(string? message = null) => new(false, errorStatus: Error.Validation(message));
        public static Result NotFound(string? message = null) => new(false, errorStatus: Error.NotFound(message));
        public static Result ServerError(string? message = null) => new(false, errorStatus: Error.ServerError(message));
        public static Result BadRequest(string? message = null) => new(false, errorStatus: Error.BadRequest(message));
    }

    public class Result<T> : Result
    {
        public T? Value { get; }

        protected internal Result(T? value, bool isSuccess, Error? errorStatus = null, Success? successStatus = null)
            : base(isSuccess, errorStatus, successStatus)
        {
            Value = value;
        }

        public static Result<T> Success(T value, Success success) => new(value, true, successStatus: success);
        public static Result<T> Success(T value) => new(value, true, successStatus: Success.Ok());

        public static new Result<T> Ok(string? message = null) => new(default!, true, successStatus: Success.Ok(message));
        public static new Result<T> Created(string? message = null) => new(default!, true, successStatus: Success.Created(message));
        public static new Result<T> Accepted(string? message = null) => new(default!, true, successStatus: Success.Accepted(message));
        public static new Result<T> NoContent(string? message = null) => new(default!, true, successStatus: Success.NoContent(message));

        public static new Result<T> Failure(Error error) => new(default!, false, errorStatus: error);
        public static new Result<T> Failure(string? error = null) => new(default!, false, errorStatus: Error.Common(error));
        public static new Result<T> Conflict(string? message = null) => new(default!, false, errorStatus: Error.Conflict(message));
        public static new Result<T> Forbiden(string? message = null) => new(default!, false, errorStatus: Error.Forbiden(message));
        public static new Result<T> NotFound(string? message = null) => new(default!, false, errorStatus: Error.NotFound(message));
        public static new Result<T> Validation(string? message = null) => new(default!, false, errorStatus: Error.Validation(message));
        public static new Result<T> ServerError(string? message = null) => new(default!, false, errorStatus: Error.ServerError(message));
        public static new Result<T> BadRequest(string? message = null) => new(default!, false, errorStatus: Error.BadRequest(message));
    }
}