using Back.Features.Grammar.Models;

namespace Back.Features.Grammar;

public interface IGrammarContentService
{
    Task<IReadOnlyList<TopicTreeNodeDto>> GetTopicTreeAsync(int? userId, CancellationToken cancellationToken = default);

    Task<TopicDetailDto?> GetTopicDetailAsync(string slug, int? userId, CancellationToken cancellationToken = default);

    Task<(ImportResponse? Result, string? Error)> ImportAsync(
        ImportRequest request,
        CancellationToken cancellationToken = default);
}
