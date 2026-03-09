using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BizCover.Application.Abstractions {
    public class Result<T> {
        public bool IsSuccess { get; private set; }
        public T? Data { get; private set; }
        public int StatusCode { get; private set; } = (int)HttpStatusCode.OK;
        public string Message { get; private set; } = "OK";

        public static Result<T> Success(T data) {
            return new Result<T> { IsSuccess = true, Data = data };
        }
        public static Result<T> Failure(string errorMessage) {
            return new Result<T> { IsSuccess = false, Message = errorMessage, StatusCode = (int)HttpStatusCode.BadRequest };
        }
        public static Result<T> NotValid(string errorMessage, T Data) {
            return new Result<T> { IsSuccess = false, Message = errorMessage, Data = Data, StatusCode = (int)HttpStatusCode.UnprocessableEntity };
        }
        public static Result<T> NotFound(string errorMessage) {
            return new Result<T> { IsSuccess = false, Message = errorMessage, StatusCode = (int)HttpStatusCode.NotFound };
        }
    }
}
