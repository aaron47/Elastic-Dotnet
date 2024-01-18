using MediatR;

namespace ElasticDotnet.Application.Elasticsearch.Commands;

public record EncodeSentenceCommand(string Sentence) : IRequest<List<float>>;
