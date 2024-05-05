using Microsoft.AspNetCore.Mvc.RazorPages;
using StackExchange.Redis;

namespace Valuator.Pages;

public class RedisPageModel(ConnectionMultiplexer redis): PageModel
{
    private string hostName = "localhost";
    private int port = 6379;
    public IDatabase RedisDatabase => redis.GetDatabase();
    public IServer RedisServer => redis.GetServer(hostName, port);
}