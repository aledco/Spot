using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Spot.Business.Resources;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

#nullable disable

namespace Spot.Business.Models.Result
{
    public class OperationResult<T> : IActionResult
    {
        public T Result { get; set; }
        public IList<string> Errors { get; set; } = [];
        public bool IsValid { get => Errors.Count == 0; }
        public bool IsFatal { get; set; }

        private OperationResult(T result) : base()
        {
            Result = result;
        }

        protected OperationResult(IList<string> errors)
        {
            this.Errors = errors;
        }

        protected OperationResult(IList<string> errors, bool isFatal)
        {
            this.Errors = errors;
            this.IsFatal = isFatal;
        }

        public OperationResult<K> ErrorsAs<K>()
        {
            return new OperationResult<K>(this.Errors, this.IsFatal);
        }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            if (IsValid)
            {
                var content = JsonSerializer.Serialize(this.Result, options: new JsonSerializerOptions()
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

                context.HttpContext.Response.StatusCode = StatusCodes.Status200OK;
                await context.HttpContext.Response.WriteAsync(content);
            }
            else
            {
                var error = new ErrorResult
                {
                    Errors = this.Errors,
                    IsFatal = this.IsFatal,
                };

                var errorContent = JsonSerializer.Serialize(error, options: new JsonSerializerOptions()
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

                context.HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.HttpContext.Response.WriteAsync(errorContent);
            }
        }

        public static OperationResult<T> Success(T result)
        {
            return new OperationResult<T>(result);
        }

        public static OperationResult<T> Failed(IList<string> errors)
        {
            return new OperationResult<T>(errors);
        }

        public static OperationResult<T> Failed(string error)
        {
            var errors = new List<string>() { error };
            return new OperationResult<T>(errors);
        }

        public static OperationResult<T> Failed()
        {
            var errors = new List<string>() { ErrorMessages.UnknownError };
            return new OperationResult<T>(errors);
        }

        public static OperationResult<T> Fatal()
        {
            var errors = new List<string>() { ErrorMessages.FatalError };
            return new OperationResult<T>(errors, true);
        }
    }

    public class OperationResult : OperationResult<object>
    {
        protected OperationResult(IList<string> errors) : base(errors)
        {
        }

        protected OperationResult(IList<string> errors, bool isFatal) : base(errors, isFatal)
        {
        }

        public static OperationResult Success()
        {
            return new OperationResult(null);
        }

        public static new OperationResult Failed(IList<string> errors)
        {
            return new OperationResult(errors);
        }

        public static new OperationResult Failed(string error)
        {
            var errors = new List<string>() { error };
            return new OperationResult(errors);
        }

        public static new OperationResult Failed()
        {
            var errors = new List<string>() { ErrorMessages.UnknownError };
            return new OperationResult(errors);
        }

        public static new OperationResult Fatal()
        {
            var errors = new List<string>() { ErrorMessages.FatalError };
            return new OperationResult(errors, true);
        }

        public static OperationResult ErrorsFrom<T>(OperationResult<T> result)
        {
            return new OperationResult(result.Errors, result.IsFatal);
        }
    }
}
