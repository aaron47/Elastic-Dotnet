using elastic_dotnet.Models;
using Elasticsearch.Net;
using Nest;

namespace elastic_dotnet.Services;

public class ProductsService(IElasticClient elasticClient, ISentenceEncoder sentenceEncoder) : IProductsService
{
	private const string INDEX_NAME = "french_products";

	public async Task<ISearchResponse<Product>> GetProductsAsync(string searchQuery)
	{
		List<float> encoded_query = await sentenceEncoder.EncodeAsync(searchQuery);
		var products = await KnnSearchAsync<Product>(encoded_query);

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

		var response = await elasticClient.LowLevel.SearchAsync<SearchResponse<T>>(INDEX_NAME, PostData.Serializable(query));
		return response;
	}



}