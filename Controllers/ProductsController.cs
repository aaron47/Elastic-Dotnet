using elastic_dotnet.Models;
using elastic_dotnet.Services;
using Elasticsearch.Net;
using Microsoft.AspNetCore.Mvc;
using Nest;

namespace elastic_dotnet.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{

	private readonly IElasticClient _elasticClient;
	private readonly ISentenceEncoder _sentenceEncoder;
	private static readonly string INDEX_NAME = "french_products";

	public ProductsController(IElasticClient elasticClient, ISentenceEncoder sentenceEncoder)
	{
		_elasticClient = elasticClient;
		_sentenceEncoder = sentenceEncoder;
	}

	[HttpGet("cosine_search")]
	public async Task<IActionResult> GetAllProductsCosineSim([FromQuery] string searchQuery)
	{
		var encodedQuery = await _sentenceEncoder.EncodeAsync(searchQuery);

		var response = await KnnSearchAsync<Product>(encodedQuery);

		if (response.IsValid)
		{
			return Ok(response.Documents);
		}
		else
		{
			Console.WriteLine("Error in search query: " + response.DebugInformation);
			return BadRequest();
		}

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

		var response = await _elasticClient.LowLevel.SearchAsync<SearchResponse<T>>("french_products", PostData.Serializable(query));
		return response;
	}
}