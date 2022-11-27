using System.Diagnostics;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;

namespace RegexTest.Controllers;

[ApiController]
[Route("[controller]")]
public class TestCompiledRegexController : ControllerBase
{
    private readonly ILogger<TestCompiledRegexController> _logger;

    private static readonly Regex Pattern = new("^(0|[1-9]\\d*)\\.(0|[1-9]\\d*)\\.(0|[1-9]\\d*)(?:-((?:0|[1-9]\\d*|\\d*[a-zA-Z-][0-9a-zA-Z-]*)(?:\\.(?:0|[1-9]\\d*|\\d*[a-zA-Z-][0-9a-zA-Z-]*))*))?(?:\\+([0-9a-zA-Z-]+(?:\\.[0-9a-zA-Z-]+)*))?$");

    public TestCompiledRegexController(ILogger<TestCompiledRegexController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetTestCompiledRegex")]
    public TestEndpoint Get([FromQuery(Name = "semVer")] string semVer, [FromQuery(Name = "iter")] int iter)
    {
        var times = new List<double>();

        foreach (var i in Enumerable.Range(0, iter))
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var _ = Pattern.Match(semVer);
            stopwatch.Stop();
            times.Add(stopwatch.Elapsed.TotalMilliseconds);
        }

        return new TestEndpoint { Name = "TestRuntimeCachedRegex", AverageProcessTime = times.Average() };
    }
}