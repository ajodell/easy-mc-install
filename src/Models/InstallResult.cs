namespace CustomModpackInstaller.Models;

public sealed class InstallResult<T>
{
    public bool IsSuccess { get; }
    public T? Value { get; }
    public string? ErrorMessage { get; }

    private InstallResult(bool isSuccess, T? value, string? error)
    {
        IsSuccess = isSuccess;
        Value = value;
        ErrorMessage = error;
    }

    public static InstallResult<T> Success(T value) => new(true, value, null);
    public static InstallResult<T> Failure(string error) => new(false, default, error);
}