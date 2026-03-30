namespace ECommerce.BLL.Abstractions.ResultPattern
{
    public class Result
    {

        public bool IsSuccess { get;}
        public bool IsFailure => !IsSuccess;

        public IReadOnlyList<Error> Errors { get; } = Array.Empty<Error>();


        public Result(bool isSuccess, IReadOnlyList<Error> errors)
        {

            if((isSuccess&&errors is not null&&errors.Count>0 )||(!isSuccess && (errors is null || errors.Count == 0)))
                throw new InvalidOperationException("Invalid result state");

            IsSuccess = isSuccess;
            Errors = errors?? Array.Empty<Error>();

        }

        public static Result Success() => new Result(true, Array.Empty<Error>());

        public static Result Failure(Error error) => new Result(false, [error]);

        public static Result<TValue> Success<TValue>(TValue value) => new Result<TValue>(true, Array.Empty<Error>(),value);

        public static Result<TValue> Failure<TValue>(Error error) => new Result<TValue>(false, [error],default);
    }


    public class Result<TValue> : Result
    {
        private readonly TValue? _value;

        public Result(bool isSuccess, IReadOnlyList<Error> errors, TValue? value) : base(isSuccess, errors)
        {
            _value = value;
        }

        public TValue Value => IsSuccess
           ? _value!
           : throw new InvalidOperationException("Failure results cannot have value");


    
    }
}

