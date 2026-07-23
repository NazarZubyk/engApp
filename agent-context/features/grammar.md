# Grammar

Grammar topics with bulk import, explanations, and exercise pools for practice sessions.

## Backend

_Key files:_
- `back/Features/Grammar/GrammarTopicsController.cs` — topic tree and detail
- `back/Features/Grammar/GrammarImportController.cs` — admin bulk import
- `back/Features/Grammar/GrammarContentService.cs`
- `back/Features/Grammar/Models/GrammarDtos.cs`
- `back/Features/Grammar/DependencyInjection.cs`
- `back/Infrastructure/Persistence/GrammarContentRepository.cs`
- `devops/migrations/migrations/0002_grammar.sql`

_Endpoints (phase A1):_ see handoff file. Practice/session endpoints — phase A2.

## Frontend

_Key files:_
- `client/src/lib/apiClient.ts` — Bearer token fetch helper
- `client/src/features/grammar/types.ts`, `grammarApi.ts`, `grammarUtils.ts`
- `client/src/features/grammar/GrammarTopicsPage.tsx` — topic tree
- `client/src/features/grammar/TopicDetailPage.tsx` — explanation + stats
- `client/src/features/grammar/GrammarProgressPage.tsx` — progress table
- `client/src/features/grammar-admin/GrammarImportPage.tsx` — admin JSON import
- `client/src/features/auth/RequireAdmin.tsx` — admin route guard

_Routes:_
- `/grammar` — topic browser
- `/grammar/progress` — mastery / accuracy per topic
- `/grammar/:slug` — topic detail (practice button waits for A2)
- `/admin/grammar/import` — admin import

## Handoff

API contract for back ↔ front: `handoff/grammar-api.md`

## Do / Don't

- Do: run `npm run migrate` in `devops/migrations/` before using grammar endpoints
- Do: send `Authorization: Bearer <token>` — User role for topics, Admin for import
- Don't: import with duplicate slug in `create` mode
