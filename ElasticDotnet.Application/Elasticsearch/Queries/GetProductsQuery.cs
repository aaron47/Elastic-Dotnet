using ElasticDotnet.Domain.Models;
using MediatR;

namespace ElasticDotnet.Application.Elasticsearch.Queries;

public record GetProductsQuery(string SearchQuery) : IRequest<Nest.ISearchResponse<Product>>;