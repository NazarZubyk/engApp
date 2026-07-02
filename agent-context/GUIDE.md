# Agent context — guide for you

**This file is for you, not for agents.** Agents should not read it unless you attach it on purpose.

Bookmark this page when you change code and need to know what rules to follow and what context files to update.

---

## Part 1 — I changed code. What do I update?

| You edited… | Update these context files | Code rules to follow |
|-------------|---------------------------|----------------------|
| You start a new backend feature | Attach `entry/backend-feature.md` + prompt | Agent creates code + context files |
| You start a new frontend feature | Attach `entry/frontend-feature.md` + prompt | Agent creates UI + handoff if needed |
| `back/Infrastructure/` | `back/infrastructure.md` only | Use extension methods on `IServiceCollection` / `WebApplication`. No feature logic here. |
| `back/Program.cs` | `back/architecture.md` (only if startup order changes) | Chain `.AddInfrastructure().Add{Feature}()`. Do not register services inline. |
| `client/src/` or `client/vite.config.ts` | `client/architecture.md` when you add routing, API client, or new patterns | Use `/api` prefix for API calls. Vite proxy points to backend HTTPS port. |
| `devops/` (Docker, Postgres, env) | `devops/local-setup.md` | Never commit `.env`. Document ports and env var names — not real passwords. |
| Project-wide naming, DI, or git habits | `conventions.md` | One place for rules that apply everywhere. |
| Ports, folders, or run commands | `monorepo.md` | Facts only — no long explanations. |
| Agent context system itself | `meta.md`, plus `_NAVIGATION.md` + `_INDEX.md` | Lean format, one fact per file. Update `AGENTS.md` only for new routes. |

**Rule:** one fact lives in one file. Update the matching file. Do not copy the same text into `AGENTS.md` or this `GUIDE.md`.

---

## Part 2 — How does this system work?

There are three layers. Each has a different job.

### Layer 1 — `AGENTS.md` (repo root)

- Cursor loads this automatically in every agent session.
- Keep it short (~40 lines). It only routes the agent to the right context file.
- Do not put architecture details here.

### Layer 2 — `agent-context/` (this folder)

- Modular files with facts about the codebase.
- Agents read them on demand — you `@` mention them or upload them.
- Files are short and token-friendly. No long tutorials.

**Important files:**

| File | Job |
|------|-----|
| `_NAVIGATION.md` | "If you need X, read Y" — fastest way to find the right file |
| `_INDEX.md` | Full list of all context files |
| `_TEMPLATE.md` | Copy when you add a new context file |
| `conventions.md` | Naming, DI, API patterns |
| `monorepo.md` | Folders, ports, run commands |
| `back/`, `client/`, `devops/` | Area-specific context |
| `features/` | One file per API feature |

### Layer 3 — `.cursor/rules/agent-behavior.mdc`

- Tiny rules that apply automatically (commit policy, minimal scope).
- Do not put architecture here — only behavior.

```mermaid
flowchart LR
    you[You edit code]
    guide[GUIDE.md]
    agents[AGENTS.md]
    nav[_NAVIGATION.md]
    topic[Topic file e.g. features/foo.md]

    you -->|check rules| guide
    agents -->|auto loaded| nav
    nav --> topic
```

---

## Part 3 — Checklist before you finish a task

- [ ] Code matches the pattern in `conventions.md` or the relevant `features/*.md`
- [ ] If you added a new feature, `features/{name}.md` and `handoff/{name}-api.md` exist
- [ ] `_NAVIGATION.md` and `_INDEX.md` have new rows (if you added a feature or new context file)
- [ ] You did **not** paste the same info into `AGENTS.md` or `GUIDE.md`
- [ ] No secrets in context files (use references to `appsettings.Development.json` or `.env.example`)

---

## Part 4 — How to start a new feature

No scripts — delegate to an agent. Attach one entry file + short prompt.

**Backend first:**
```
@agent-context/entry/backend-feature.md
Feature: Words. Description: vocabulary CRUD.
```

**Frontend first (or UI after API exists):**
```
@agent-context/entry/frontend-feature.md
Feature: Words. Description: word list page with add form.
```

**What the agent must leave for the next agent:**
- `features/{kebab}.md` — feature context
- `handoff/{kebab}-api.md` — API contract (Implemented or Expected API)
- Updated `_NAVIGATION.md` and `_INDEX.md`

**Backend went first → frontend later:** front agent reads `handoff/{kebab}-api.md` (Implemented API).

**Frontend went first → backend later:** back agent reads handoff (Expected API) and implements.

---

## Part 5 — Quick links

| Open this when… | File |
|-----------------|------|
| You forget what to update after editing code | `GUIDE.md` (this file) |
| You start a new agent and want to point it to the right file | `_NAVIGATION.md` |
| You need a full list of context files | `_INDEX.md` |
| You add a new context file | `_TEMPLATE.md` |
| Naming, DI, API routes | `conventions.md` |
| Ports, folders, how to run | `monorepo.md` |
| Backend structure, Program.cs | `back/architecture.md` |
| Database, middleware | `back/infrastructure.md` |
| Frontend, Vite proxy | `client/architecture.md` |
| Docker, Postgres | `devops/local-setup.md` |
| Reference feature example | `features/weather-forecast.md` |
| Start new backend feature (attach for agent) | `entry/backend-feature.md` |
| Start new frontend feature (attach for agent) | `entry/frontend-feature.md` |
| API contract back ↔ front | `handoff/{name}-api.md` |
| Handoff template | `handoff/_TEMPLATE-api.md` |
| New feature template | `features/_TEMPLATE.md` |
| Change or extend the context system (attach for agents) | `meta.md` |
| Agent entry point (auto-loaded) | `../AGENTS.md` |

---

## Part 6 — How to use this with a new agent

1. Start a new agent session — it will read `AGENTS.md` automatically.
2. **New feature:** attach `entry/backend-feature.md` or `entry/frontend-feature.md` + feature name in prompt.
3. **Existing feature:** `@` mention `features/{name}.md` and `handoff/{name}-api.md`.
4. Do **not** attach all files — that wastes tokens.
5. Attach `meta.md` to change the context system. `GUIDE.md` is for you only.
