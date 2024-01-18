using ElasticDotnet.Application.Elasticsearch.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ElasticDotnet.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly ISender _sender;

    public ProductsController(ISender sender)
    {
        _sender = sender;
    }

    [Authorize]
    [HttpGet("cosine_search")]
    public async Task<IActionResult> GetAllProductsCosineSim([FromQuery(Name = "q")] string searchQuery)
    {
        var getProductsQuery = new GetProductsQuery(searchQuery);

        var response = await _sender.Send(getProductsQuery);

        if (response.IsValid)
        {
            return Ok(response.Documents);
        }

        Console.WriteLine("Error in search query: " + response.DebugInformation);
        return BadRequest();
    }
}