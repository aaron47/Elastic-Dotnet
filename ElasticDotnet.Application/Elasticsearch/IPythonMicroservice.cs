using System.Text.Json.Serialization;
using Refit;

namespace ElasticDotnet.Application.Elasticsearch;

[Headers("Accept: */*")]
public interface IPythonMicroservice
{
    [Post("/encode")]
    Task<EncodedResponse> EncodeSentenceAsync([Body(true)] SentenceRequest sentenceRequest);
}


public class EncodedResponse
{
    [JsonPropertyName("query_vector")]
    public required List<float> QueryVector { get; set; }
}

public class SentenceRequest
{
    public required string Sentence { get; set; }
}
