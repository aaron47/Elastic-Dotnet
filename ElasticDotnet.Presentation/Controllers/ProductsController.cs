using ElasticDotnet.Application.Elasticsearch.Queries;
using ElasticDotnet.Domain.Models;
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
    public async Task<IActionResult> GetAllProductsCosineSim(
        [FromQuery(Name = "q")] string searchQuery,
        [FromQuery(Name = "num_candidates_desc")] int numCandidatesDesc = 150,
        [FromQuery(Name = "num_candidates_prodname")] int numCandidatesProdName = 150,
        [FromQuery(Name = "top_res_desc")] int topResDesc = 20,
        [FromQuery(Name = "top_res_prodname")] int topResProdName = 10
    )
    {
        var knnSearchRequest = new KnnSearchRequest(
            NumCandidatesDesc: numCandidatesDesc,
            NumCandidatesProdName: numCandidatesProdName,
            TopResDesc: topResDesc,
            TopResProdName: topResProdName
        );
        var getProductsQuery = new GetProductsQuery(searchQuery, knnSearchRequest);

        var response = await _sender.Send(getProductsQuery);

        if (response.IsValid)
        {
            return Ok(response.Documents);
        }

        Console.WriteLine("Error in search query: " + response.DebugInformation);
        return BadRequest();
    }
}