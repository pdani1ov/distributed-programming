using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace Valuator.Pages;

public class IndexModel : RedisPageModel
{
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(ILogger<IndexModel> logger, ConnectionMultiplexer redis): base(redis)
    {
        _logger = logger;
    }

    public void OnGet()
    {

    }

    public IActionResult? OnPost(string text)
    {
        if (String.IsNullOrEmpty(text))
        {
            return null;
        }
        
        _logger.LogDebug(text);

        string id = Guid.NewGuid().ToString();
        
        string similarityKey = "SIMILARITY-" + id;
        RedisDatabase.StringSet(similarityKey, GetSimilarity(text));

        string textKey = "TEXT-" + id;
        RedisDatabase.StringSet(textKey, text);

        string rankKey = "RANK-" + id;
        RedisDatabase.StringSet(rankKey, GetRank(text));

        return Redirect($"summary?id={id}");
    }

    private double GetRank(string text)
    {
        var notLetterCount = text.Count(ch => !char.IsLetter(ch));

        return (double) notLetterCount / text.Length;
    }

    private int GetSimilarity(string text)
    {
        var keys = RedisServer.Keys();
        var isSimilarText = keys.Any(key =>
            key.ToString().Substring(0, 5) == "TEXT-" && RedisDatabase.StringGet(key) == text);

        return  isSimilarText ? 1 : 0;
    }
}
