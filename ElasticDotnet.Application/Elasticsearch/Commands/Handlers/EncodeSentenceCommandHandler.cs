using ElasticDotnet.Domain.Config;
using MediatR;
using Microsoft.Extensions.Options;
using Refit;

namespace ElasticDotnet.Application.Elasticsearch.Commands.Handlers;

public sealed class EncodeSentenceCommandHandler : IRequestHandler<EncodeSentenceCommand, List<float>>
{
    private readonly IPythonMicroservice _pythonMicroservice;

    public EncodeSentenceCommandHandler(IOptions<PythonMicroserviceOptions> options)
    {
        _pythonMicroservice = RestService.For<IPythonMicroservice>(options.Value.Url);
    }

    public async Task<List<float>> Handle(EncodeSentenceCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var sentenceRequest = new SentenceRequest { Sentence = request.Sentence };
            var response = await _pythonMicroservice.EncodeSentenceAsync(sentenceRequest);
            return response.QueryVector;
        }
        catch (ApiException apiException)
        {
            throw new HttpRequestException($"API request failed, Reason: {apiException.Message}", apiException);
        }
    }
}