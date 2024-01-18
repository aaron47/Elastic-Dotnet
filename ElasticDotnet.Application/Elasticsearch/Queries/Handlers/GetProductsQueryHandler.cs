using ElasticDotnet.Application.Elasticsearch.Commands;
using ElasticDotnet.Domain.Models;
using Elasticsearch.Net;
using MediatR;
using Nest;

namespace ElasticDotnet.Application.Elasticsearch.Queries.Handlers;

public sealed class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, ISearchResponse<Product>>
{
    private const string IndexName = "french_products";
    private readonly IElasticClient _elasticClient;
    private readonly ISender _sender;

    public GetProductsQueryHandler(IElasticClient elasticClient, ISender sender)
    {
        _elasticClient = elasticClient;
        _sender = sender;
    }

    public async Task<ISearchResponse<Product>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        var encodeCommand = new EncodeSentenceCommand(request.SearchQuery);
        var encodedQuery = await _sender.Send(encodeCommand);

        var products = await KnnSearchAsync<Product>(encodedQuery);

        return products;
    }

    private async Task<ISearchResponse<T>> KnnSearchAsync<T>(List<float> queryVector) where T : class
    {
        var query = new
        {
            knn = new
            {
                field = "DescriptionVector",
                query_vector = queryVector,
                k = 10,
                num_candidates = 500
            },
            _source = new string[] { "ProductName", "Description" },
        };

        var response = await _elasticClient.LowLevel.SearchAsync<SearchResponse<T>>(IndexName, PostData.Serializable(query));
        return response;
    }
}