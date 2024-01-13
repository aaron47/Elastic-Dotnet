using elastic_dotnet.Services;
using Microsoft.AspNetCore.Mvc;

namespace elastic_dotnet.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
	private readonly IProductsService _productsService;
	public ProductsController(IProductsService productsService)
	{
		_productsService = productsService;
	}

	[HttpGet("cosine_search")]
	public async Task<IActionResult> GetAllProductsCosineSim([FromQuery(Name = "q")] string searchQuery)
	{
		var response = await _productsService.GetProductsAsync(searchQuery);

		if (response.IsValid)
		{
			return Ok(response.Documents);
		}

		Console.WriteLine("Error in search query: " + response.DebugInformation);
		return BadRequest();
	}
}