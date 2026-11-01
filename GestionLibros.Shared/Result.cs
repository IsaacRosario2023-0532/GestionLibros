namespace GestionLibros.Shared;

public class Result
{
    public bool IsSuccess { get; set; }
    public bool IsFailure => !IsSuccess; 
    public string Message { get; set; } = string.Empty;
    public string Error => Message;

    public static Result Success(bool success = true, string message = "") =>
        new Result { IsSuccess = success, Message = message };

    public static Result Failure(string message) =>
        new Result { IsSuccess = false, Message = message };
}

public class Result<T> : Result
{
    public T? Data { get; set; }
    public T? Value => Data;

    public static Result<T> Success(T? data, string message = "") =>
        new Result<T> { Data = data, IsSuccess = true, Message = message };

    new public static Result<T> Failure(string message) =>
        new Result<T> { IsSuccess = false, Message = message };
}