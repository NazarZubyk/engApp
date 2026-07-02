# {FeatureName}

{One sentence — what this feature does.}

## Backend

_Key files:_
- `back/Features/{Name}/{Name}Controller.cs`
- `back/Features/{Name}/I{Name}Service.cs`
- `back/Features/{Name}/{Name}Service.cs`
- `back/Features/{Name}/Models/{Name}Dto.cs`
- `back/Features/{Name}/DependencyInjection.cs`

_Endpoints:_

| Method | Path | Returns |
|--------|------|---------|
| GET | `/api/{kebab}` | … |

_Or: Not yet — see handoff Expected API._

## Frontend

_Key files:_
- `client/src/features/{kebab}/…`

_Or: Not yet._

## Handoff

API contract for back ↔ front: `handoff/{kebab}-api.md`

## Do / Don't

- Do: keep Backend and Frontend sections updated when that side ships
- Do: put endpoint details in handoff file, not duplicated here
- Don't: …
