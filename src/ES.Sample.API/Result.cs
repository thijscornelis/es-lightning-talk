using System.Text.Json.Serialization;

public record Result<T>
{
	/// <summary>Gets the query result.</summary>
	/// <value>The query result.</value>
	public T QueryResult { get; private set; }

	/// <summary>Gets the elapsed time.</summary>
	/// <value>The elapsed time.</value>
	public TimeSpan ElapsedTime { get; private set; }

	/// <summary>Gets the exception.</summary>
	/// <value>The exception.</value>
	[JsonIgnore]
	public Exception Exception { get; private set; }

	/// <summary>Gets the exception message.</summary>
	/// <value>The exception message.</value>
	public string ExceptionMessage => Exception?.GetBaseException().Message;

	/// <summary>Gets a value indicating whether this instance has executed successfully.</summary>
	/// <value><c>true</c> if this instance has executed successfully; otherwise, <c>false</c>.</value>
	public bool HasExecutedSuccessfully => Exception == null && ElapsedTime != TimeSpan.Zero;

	internal void SetResult(T result) => QueryResult = result;
	internal void SetException(Exception exception) => Exception = exception;
	internal void SetElapsedTime(TimeSpan elapsedTime) => ElapsedTime = elapsedTime;
}