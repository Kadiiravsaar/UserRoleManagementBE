namespace URM.Core.Ultities.Results
{
	public interface IResult
	{
		bool Success { get; }
		string Message { get; }
	}
}
