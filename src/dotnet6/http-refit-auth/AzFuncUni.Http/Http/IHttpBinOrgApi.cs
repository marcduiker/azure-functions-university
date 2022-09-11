using Refit;

/// <summary>
/// This interface represents access to the HttpBin.org API.
/// </summary>
public interface IHttpBinOrgApi
{
	[Post("/post")]
	Task<GetRequestResponse> GetRequest(Stream content = null, [Query] IDictionary<string, string> query = default);
}