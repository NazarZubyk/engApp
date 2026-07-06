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

_Key files:_
- `client/src/features/auth/LoginPage.tsx` — sign-in form (user or admin)
- `client/src/features/auth/AuthProvider.tsx` — session state, login/logout
- `client/src/features/auth/authApi.ts` — login API calls
- `client/src/features/auth/authStorage.ts` — JWT in `localStorage`
- `client/src/features/auth/RequireAuth.tsx` — route guard
- `client/src/features/main-menu/MainMenuPage.tsx` — post-login landing (placeholder)

_Routes:_
- `/login` — sign in
- `/menu` — main menu (protected)

## Handoff

API contract for back ↔ front: `handoff/auth-api.md`

## Do / Don't

- Do: run `npm run migrate` in `devops/migrations/` before using auth endpoints
- Do: send `Authorization: Bearer <token>` for admin user CRUD
- Don't: expose passwords in API responses
