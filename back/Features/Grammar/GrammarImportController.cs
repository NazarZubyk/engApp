using Back.Features.Grammar.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Back.Features.Grammar;

[ApiController]
[Route("api/grammar/import")]
[Authorize(Roles = "Admin")]
public class GrammarImportController : ControllerBase
{
    private readonly IGrammarContentService _contentService;

    public GrammarImportController(IGrammarContentService contentService)
    {
        _contentService = contentService;
    }

    [HttpPost]
    public async Task<ActionResult<ImportResponse>> Import(
        [FromBody] ImportRequest request,
        CancellationToken cancellationToken)
    {
        var (result, error) = await _contentService.ImportAsync(request, cancellationToken);
        if (error is not null)
        {
            if (error is "Topic slug already exists." or "Topic not found for append.")
            {
                return Conflict(new { message = error });
            }

            if (error.StartsWith("Invalid importMode", StringComparison.Ordinal)
                || error.StartsWith("Parent topic", StringComparison.Ordinal))
            {
                return BadRequest(new { message = error });
            }

            return BadRequest(new { message = error });
        }

        return Ok(result);
    }
}
