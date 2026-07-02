# engApp — Agent Entry Point

Monorepo: .NET 10 Web API (`back/`) + React/Vite SPA (`client/`) + Postgres via Docker (`devops/`).

**Humans:** read [agent-context/GUIDE.md](agent-context/GUIDE.md) for explanations and maintenance rules.

## Hard rules

- Keep changes small — match existing patterns, do not refactor unrelated code.
- Backend features live in `back/Features/{Name}/` with their own `DependencyInjection.cs`.
- Register features in `back/Program.cs` via `Add{Name}Feature()` — do not register services inline.
- Do not commit unless the user explicitly asks.
- Do not read all context files — use navigation below.

## What to read next

| Task | Read (in order) |
|------|-----------------|
| Unsure where to look | `agent-context/_NAVIGATION.md` |
| Coding patterns / naming | `agent-context/conventions.md` |
| Repo layout, ports, run commands | `agent-context/monorepo.md` |
| Add or change an API feature | `conventions.md` → `back/architecture.md` → `features/_TEMPLATE.md` |
| **Start new backend feature** (user attaches entry) | `entry/backend-feature.md` |
| **Start new frontend feature** (user attaches entry) | `entry/frontend-feature.md` |
| API contract between back and front | `handoff/{feature-name}-api.md` |
| DB, DI, middleware | `devops/local-setup.md` → `back/infrastructure.md` |
| Frontend / API proxy | `client/architecture.md` |
| WeatherForecast sample | `features/weather-forecast.md` |
| Change or extend agent context system | `agent-context/meta.md` |

Full file catalog: `agent-context/_INDEX.md`
