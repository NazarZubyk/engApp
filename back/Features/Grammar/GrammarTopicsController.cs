using System.Security.Claims;
using Back.Features.Grammar.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Back.Features.Grammar;

[ApiController]
[Route("api/grammar/topics")]
[Authorize(Roles = "User,Admin")]
public class GrammarTopicsController : ControllerBase
{
    private readonly IGrammarContentService _contentService;

    public GrammarTopicsController(IGrammarContentService contentService)
    {
        _contentService = contentService;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<TopicTreeNodeDto>>> GetTree(CancellationToken cancellationToken)
    {
        var userId = GetUserId();
        var tree = await _contentService.GetTopicTreeAsync(userId, cancellationToken);
        return Ok(tree);
    }

    [HttpGet("{slug}")]
    public async Task<ActionResult<TopicDetailDto>> GetBySlug(string slug, CancellationToken cancellationToken)
    {
        var userId = GetUserId();
        var topic = await _contentService.GetTopicDetailAsync(slug, userId, cancellationToken);
        if (topic is null)
        {
            return NotFound();
        }

        return Ok(topic);
    }

    private int? GetUserId()
    {
        var sub = User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? User.FindFirstValue("sub");

        return int.TryParse(sub, out var userId) ? userId : null;
    }
}
