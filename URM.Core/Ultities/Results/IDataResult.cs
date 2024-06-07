namespace URM.Core.Ultities.Results
{
	public interface IDataResult<out T> : IResult
	{
		T Data { get; }
	}
}
