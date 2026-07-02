# Auth

JWT authentication for admin and users, with admin-only user CRUD.

## Backend

_Key files:_
- `back/Features/Auth/AuthController.cs` — login endpoints
- `back/Features/Auth/UsersController.cs` — user CRUD (admin only)
- `back/Features/Auth/AuthService.cs`, `UsersService.cs`
- `back/Infrastructure/Auth/` — JWT, password hashing
- `back/Infrastructure/Persistence/AdminRepository.cs`, `UserRepository.cs`
- `devops/migrations/migrations/0001_auth.sql` — auth DB migration

_Endpoints:_ see handoff file.

## Frontend

Not yet — see `handoff/auth-api.md`.

## Handoff

API contract for back ↔ front: `handoff/auth-api.md`

## Do / Don't

- Do: run `npm run migrate` in `devops/migrations/` before using auth endpoints
- Do: send `Authorization: Bearer <token>` for admin user CRUD
- Don't: expose passwords in API responses
