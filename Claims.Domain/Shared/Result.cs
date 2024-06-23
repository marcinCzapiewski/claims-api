namespace Claims.Domain.Shared;
public class Result
{
    protected internal Result(bool isSuccess, DomainError error)
    {
        if (isSuccess && error != DomainError.None)
        {
            throw new InvalidOperationException();
        }

        if (!isSuccess && error == DomainError.None)
        {
            throw new InvalidOperationException();
        }

        IsSuccess = isSuccess;
        Error = error;
    }

    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;

    public DomainError Error { get; }

    public static Result Success() => new(true, DomainError.None);

    public static Result<TValue> Success<TValue>(TValue value) => new(value, true, DomainError.None);

    public static Result Failure(DomainError error) => new(false, error);

    public static Result<TValue> Failure<TValue>(DomainError error) => new(default, false, error);

    public static Result<TValue> Create<TValue>(TValue? value) => value is not null ? Success(value) : Failure<TValue>(DomainError.NullValue);
}

