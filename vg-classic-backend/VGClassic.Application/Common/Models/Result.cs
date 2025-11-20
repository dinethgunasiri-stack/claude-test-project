using System.Collections.Generic;
using System.Linq;

namespace VGClassic.Application.Common.Models;

public class Result<T>
{
    public bool IsSuccess { get; set; }
    public T? Value { get; set; }
    public List<string> Errors { get; set; } = new();

    public static Result<T> Success(T value) => new() { IsSuccess = true, Value = value };
    public static Result<T> Failure(string error) => new() { IsSuccess = false, Errors = new List<string> { error } };
    public static Result<T> Failure(IEnumerable<string> errors) => new() { IsSuccess = false, Errors = errors.ToList() };
}

public class Result
{
    public bool IsSuccess { get; set; }
    public List<string> Errors { get; set; } = new();

    public static Result Success() => new() { IsSuccess = true };
    public static Result Failure(string error) => new() { IsSuccess = false, Errors = new List<string> { error } };
    public static Result Failure(IEnumerable<string> errors) => new() { IsSuccess = false, Errors = errors.ToList() };
}
