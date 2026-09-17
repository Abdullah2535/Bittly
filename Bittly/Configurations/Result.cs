namespace Bittly.Configurations
{
    public class Result<T>
    {
        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;
        public T? Value { get; }
        public Error Error { get; }

        private Result(bool isSuccess, T? value, Error error)
        {
            if (isSuccess && error != Error.None)
            {
                throw new InvalidOperationException("Successful results cannot have an error.");
            }
            if (!isSuccess && error == Error.None)
            {
                throw new InvalidOperationException("Failed results must contain an error.");
            }

            IsSuccess = isSuccess;
            Value = value;
            Error = error;
        }

        public static Result<T> Success(T value) => new Result<T>(true, value, Error.None);

        public static Result<T> Failure(Error error) => new Result<T>(false, default, error);
    }
}
