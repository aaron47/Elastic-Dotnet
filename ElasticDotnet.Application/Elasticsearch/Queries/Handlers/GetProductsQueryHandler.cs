using ElasticDotnet.Application.Elasticsearch.Commands;
using ElasticDotnet.Domain.Models;
using Elasticsearch.Net;
using MediatR;
using Nest;

namespace ElasticDotnet.Application.Elasticsearch.Queries.Handlers;

public sealed class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, ISearchResponse<AxamProduct>>
{
    private const string IndexName = "axam_products";
    private readonly IElasticClient _elasticClient;
    private readonly ISender _sender;

    public GetProductsQueryHandler(IElasticClient elasticClient, ISender sender)
    {
        _elasticClient = elasticClient;
        _sender = sender;
    }

    public async Task<ISearchResponse<AxamProduct>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        var encodeCommand = new EncodeSentenceCommand(request.SearchQuery);
        var encodedQuery = await _sender.Send(encodeCommand, cancellationToken);

        var products = await KnnSearchAsync<AxamProduct>(encodedQuery, request.KnnSearchRequest);

        return products;
    }


    #region Knn Search Logic
    /// <summary>
    /// This search finds the global top k = 10 vector matches for DescriptionVecteur
    /// and the global k = 5 for the LabelProduitVecteur.
    /// These top values are then combined with the matches from the match query and the top-10 documents are returned.
    /// The multiple knn entries and the query matches are combined through a disjunction,
    /// as if you took a boolean or between them.
    /// The top k vector results represent the global nearest neighbors across all index shards.
    /// In our case, the way this calculates the score would be:
    /// score = (score_DescriptionVecteur + score_LabelProduitVecteur) / 2 granted that we do not provide any boosts.
    /// score_DescriptionVecteur is the result of the knnSearch on the score_DescriptionVecteur
    /// score_LabelProduitVecteur is the result of the knnSearch on the score_LabelProduitVecteur
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
    private async Task<ISearchResponse<T>> KnnSearchAsync<T>(List<float> queryVector, KnnSearchRequest knnSearchRequest) where T : class
    {
        var query = new
        {
            knn = new[]
            {
                new
                {
                    field = "LabelProduitVecteur",
                    query_vector = queryVector,
                    k = knnSearchRequest.TopResDesc,
                    num_candidates = knnSearchRequest.NumCandidatesDesc,
                },
                new
                {
                    field = "DescriptionVecteur",
                    query_vector = queryVector,
                    k = knnSearchRequest.TopResProdLabel,
                    num_candidates = knnSearchRequest.NumCandidatesProdLabel,
                },
            },
        };

        var response = await _elasticClient.LowLevel.SearchAsync<SearchResponse<T>>(IndexName, PostData.Serializable(query));
        return response;
    }
    #endregion
}