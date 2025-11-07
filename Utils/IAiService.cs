namespace Utils;

public interface IAiService
{
	Task<string> Generate(string prompt);
	Task<float[]> GetEmbedding(string input);
}
