using elastic_dotnet.Config;
using elastic_dotnet.Utils;
using Microsoft.Extensions.Options;
using Refit;

namespace elastic_dotnet.Services;

public class SentenceEncoder : ISentenceEncoder
{
    private readonly IPythonMicroservice _pythonMicroservice;

    public SentenceEncoder(IOptions<PythonMicroserviceOptions> options)
    {
        var optionsValue = options.Value;
        _pythonMicroservice = RestService.For<IPythonMicroservice>(optionsValue.Url);
    }

    public async Task<List<float>> EncodeAsync(string sentence)
    {
        try
        {
            var request = new SentenceRequest { Sentence = sentence };
            var response = await _pythonMicroservice.EncodeSentenceAsync(request);
            return response.QueryVector;
        }
        catch (ApiException apiException)
        {
            throw new HttpRequestException($"API request failed, Reason: {apiException.Message}", apiException);
        }
    }
}
