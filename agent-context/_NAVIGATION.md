# Navigation — problem → file

Read `AGENTS.md` first, then open **one row** from this table.

| If you need to… | Read (in order) |
|-----------------|-----------------|
| **Start new backend feature** (entry attached) | `entry/backend-feature.md` |
| **Start new frontend feature** (entry attached) | `entry/frontend-feature.md` |
| Add a new API feature (no entry) | `conventions.md` → `back/architecture.md` → `features/_TEMPLATE.md` |
| Change an existing API feature | `features/{feature-name}.md` + `handoff/{feature-name}-api.md` |
| Fix DB connection or Postgres setup | `devops/local-setup.md` → `back/infrastructure.md` |
| Change DI, middleware, or infrastructure | `back/infrastructure.md` |
| Understand backend startup | `back/architecture.md` |
| Change frontend or API calls from client | `client/architecture.md` |
| Find ports, folders, or run commands | `monorepo.md` |
| Understand the WeatherForecast sample | `features/weather-forecast.md` |
| Cross-cutting naming or git rules | `conventions.md` |
| Change or extend the context system itself | `meta.md` |
