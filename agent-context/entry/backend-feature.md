# Start: backend feature

You are implementing a **new backend feature**. The user attached this file — follow it end to end.

## You need from the user

- **Feature name** — PascalCase, e.g. `Words`
- **Description** — one sentence what it does
- **Optional** — endpoints, DTO fields, business rules

If missing, ask briefly before coding.

## Read first (minimal)

1. `agent-context/conventions.md`
2. `agent-context/back/architecture.md`
3. `agent-context/features/weather-forecast.md` — reference pattern only

Do not read all context files.

## Implement

1. Create `back/Features/{Name}/`:
   - `{Name}Controller.cs`, `I{Name}Service.cs`, `{Name}Service.cs`
   - `Models/{Name}Dto.cs`, `DependencyInjection.cs`
2. Register `Add{Name}Feature()` in `back/Program.cs` + `using Back.Features.{Name};`
3. Add test lines to `back/back.http`

Match `WeatherForecast` patterns. Keep changes minimal.

## Before you finish (mandatory — for the next agent)

Do not skip. Write files, not a long chat summary.

1. **`agent-context/features/{kebab-name}.md`**
   - Copy from `features/_TEMPLATE.md`
   - Fill **Backend** section: key files, endpoints, patterns
   - **Frontend** section: `Not yet` + link to handoff file

2. **`agent-context/handoff/{kebab-name}-api.md`**
   - Copy from `handoff/_TEMPLATE-api.md`
   - Fill **Implemented API**: methods, paths, request/response, example JSON
   - Leave **Expected API** as `Not applicable` (backend went first)

3. **Update navigation**
   - Append row to `_NAVIGATION.md`: `Work on {Name} feature` → `features/{kebab}.md` + `handoff/{kebab}-api.md`
   - Append row to `_INDEX.md` for both new files

## Do / Don't

- Do: leave `handoff/{kebab}-api.md` accurate — frontend agent will read it
- Do: keep handoff short (tables + one JSON example)
- Don't: duplicate handoff content inside `features/{kebab}.md` — link to handoff file
- Don't: edit `AGENTS.md` per feature
- Don't: commit unless the user asks
