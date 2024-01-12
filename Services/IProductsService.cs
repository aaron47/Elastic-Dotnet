using elastic_dotnet.Models;
using Nest;

namespace elastic_dotnet.Services;

public interface IProductsService
{
	public Task<ISearchResponse<Product>> GetProductsAsync(string searchQuery);
}