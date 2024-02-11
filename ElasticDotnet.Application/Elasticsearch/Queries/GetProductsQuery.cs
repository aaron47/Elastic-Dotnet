using ElasticDotnet.Domain.Models;
using MediatR;

namespace ElasticDotnet.Application.Elasticsearch.Queries;

public record GetProductsQuery(string SearchQuery, KnnSearchRequest KnnSearchRequest) : IRequest<Nest.ISearchResponse<AxamProduct>>;