using System.Text.Json.Serialization;

namespace ElasticDotnet.Domain.Models;

public class Product
{
	[JsonPropertyName("ProductName")]
	public required string ProductName { get; set; }

	[JsonPropertyName("Description")]
	public required string Description { get; set; }
}