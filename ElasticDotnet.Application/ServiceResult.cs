namespace ElasticDotnet.Application;

public sealed class ServiceResult<T>
{
	public bool Success { get; private set; }
	public T? Data { get; private set; }
	public string? Message { get; private set; }
	public IEnumerable<string>? Errors { get; private set; }

	public static ServiceResult<T> SuccessResult(T data, string message = "")
			=> new() { Success = true, Message = message, Data = data };

	public static ServiceResult<T> FailureResult(IEnumerable<string> errors)
			=> new() { Success = false, Errors = errors };
}
