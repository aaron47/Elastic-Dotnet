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
        var encodedQuery = await _sender.Send(encodeCommand, cancellationToken);

        var products = await KnnSearchAsync<Product>(encodedQuery);

        return products;
    }


    #region Knn Search Logic
    /// <summary>
    /// This search finds the global top k = 10 vector matches for DescriptionVector
    /// and the global k = 5 for the ProductNameVector.
    /// These top values are then combined with the matches from the match query and the top-10 documents are returned.
    /// The multiple knn entries and the query matches are combined through a disjunction,
    /// as if you took a boolean or between them.
    /// The top k vector results represent the global nearest neighbors across all index shards.
    /// In our case, the way this calculates the score would be:
    /// score = (score_DescriptionVector + score_ProductNameVector) / 2 granted that we do not provide any boosts.
    /// score_DescriptionVector is the result of the knnSearch on the DescriptionVector
    /// score_ProductNameVector is the result of the knnSearch on the ProductNameVector
    /// We divide by 2 because we are searching through 2 vectors that we're going to combine.  
    /// </summary>

    /// <typeparam name="T">Generic type for the type of document you want to return.
    /// In our example above, we return a list of Product
    /// </typeparam>

    /// <param name="queryVector">The encoded vector of the user's search query.</param>
    /// <returns>A SearchResponse of the type of document returned. Which includes fields such as:
    /// - The documents themselves
    /// - Whether the result is valid or not.
    /// </returns>
    private async Task<ISearchResponse<T>> KnnSearchAsync<T>(List<float> queryVector) where T : class
    {
        var query = new
        {
            knn = new[]
            {
                new
                {
                    field = "DescriptionVector",
                    query_vector = queryVector,
                    k = 20,
                    num_candidates = 150,
                },
                new
                {
                    field = "ProductNameVector",
                    query_vector = queryVector,
                    k = 10,
                    num_candidates = 150,
                },
            },
            _source = new string[] { "ProductName", "Description" },
        };

        var response = await _elasticClient.LowLevel.SearchAsync<SearchResponse<T>>(IndexName, PostData.Serializable(query));
        return response;
    }
    #endregion
}