
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using elastic_dotnet.Config;
using Microsoft.Extensions.Options;

namespace elastic_dotnet.Services;

public class SentenceEncoder : ISentenceEncoder
{

	private readonly HttpClient _httpClient;
	private readonly PythonMicroserviceOptions _options;

	public SentenceEncoder(HttpClient httpClient, IOptions<PythonMicroserviceOptions> options)
	{
		_httpClient = httpClient;
		_options = options.Value;
	}

	public async Task<List<float>> EncodeAsync(string sentence)
	{
		var json = JsonSerializer.Serialize(new { sentence });
		var content = new StringContent(json, Encoding.UTF8, "application/json");
		var response = await _httpClient.PostAsync(_options.Url, content);

		if (!response.IsSuccessStatusCode)
		{
			var errorContent = await response.Content.ReadAsStringAsync();
			throw new HttpRequestException($"Request failed with status {response.StatusCode}: {errorContent}");
		}


		var encodedSentence = await response.Content.ReadFromJsonAsync<EncodedResponse>();
		return encodedSentence!.QueryVector!;
	}
}


internal sealed class EncodedResponse
{
	[JsonPropertyName("query_vector")]
	public required List<float> QueryVector { get; set; }
}