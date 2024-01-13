namespace elastic_dotnet.Utils;

public class ServiceResult<T>
{
	public bool Success { get; set; }
	public T? Data { get; set; }
	public string? Message { get; set; }
	public IEnumerable<string>? Errors { get; set; }

	public static ServiceResult<T> SuccessResult(T data, string message = "")
			=> new() { Success = true, Message = message, Data = data };

	public static ServiceResult<T> FailureResult(IEnumerable<string> errors)
			=> new() { Success = false, Errors = errors };
}
