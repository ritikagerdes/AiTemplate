using System.Collections.Generic;

namespace Utils;

public class PromptTemplateService
{
	private readonly Dictionary<string, string> _Templates = new()
	{
		["Summarize"] = "Summarize the following text in 3 bullet points:\n\n{input}",
		["ExplainCode"] = "Explain this C# code:\n\n{input}",
		["GenerateIdea"] = "Propose 3 product ideas for:\n\n{input}"
	};

	public string Build(string key, string input)
	{
		if (!this._Templates.TryGetValue(key, out string? template))
		{
			throw new KeyNotFoundException($"Template '{key}' not found.");
		}

		return template.Replace("{input}", input);
	}
}
