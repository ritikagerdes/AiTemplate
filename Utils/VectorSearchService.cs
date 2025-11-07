using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Utils;

public class VectorSearchService
{
	private readonly IAiService _ai;
	private readonly List<(string Content, float[] Vector)> _store = new();

	public VectorSearchService(IAiService ai) => this._ai = ai;

	public async Task IndexTextAsync(string text)
	{
		float[] vector = await this._ai.GetEmbedding(text);
		this._store.Add((text, vector));
	}

	public async Task<List<string>> SearchSimilarAsync(string query, int topK = 3)
	{
		float[] queryVector = await this._ai.GetEmbedding(query);

		return this._store
			.Select(doc => (doc.Content, Score: CosineSimilarity(doc.Vector, queryVector)))
			.OrderByDescending(x => x.Score)
			.Take(topK)
			.Select(x => x.Content)
			.ToList();
	}

	private static float CosineSimilarity(float[] v1, float[] v2)
	{
		float dot = 0, mag1 = 0, mag2 = 0;
		for (int i = 0; i < v1.Length; i++)
		{
			dot += v1[i] * v2[i];
			mag1 += v1[i] * v1[i];
			mag2 += v2[i] * v2[i];
		}
		return dot / (float)((Math.Sqrt(mag1) * Math.Sqrt(mag2)) + 1e-8);
	}
}
