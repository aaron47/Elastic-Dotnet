namespace elastic_dotnet.Services;

public interface ISentenceEncoder
{
	public Task<List<float>> EncodeAsync(string sentence);
}