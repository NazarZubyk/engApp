# Grammar API

Grammar topics, bulk exercise import, and (phase A2) practice sessions.

## Implemented API

### Phase A1 — content

| Method | Path | Auth | Request body | Response |
|--------|------|------|--------------|----------|
| GET | `/api/grammar/topics` | User or Admin JWT | — | `[TopicTreeNode]` (nested) |
| GET | `/api/grammar/topics/{slug}` | User or Admin JWT | — | `TopicDetail` |
| POST | `/api/grammar/import` | Admin JWT | `ImportRequest` | `ImportResponse` |

**TopicTreeNode:** `{ slug, title, sortOrder, exerciseCount, masteryLevel, accuracyPct, children[] }`

**TopicDetail:** `{ slug, title, explanationMd, exerciseCount, totalAttempts, totalCorrect, accuracyPct, masteryLevel, weakExerciseCount }`

**masteryLevel:** `not_started` | `learning` | `practicing` | `strong`

**ImportRequest:**

```json
{
  "importMode": "create",
  "topic": {
    "slug": "present-perfect",
    "parentSlug": null,
    "title": "Present perfect",
    "explanationMd": "## When we use it\n...",
    "sortOrder": 10
  },
  "exercises": [
    {
      "prompt": "I ___ (see) that film.",
      "notes": null,
      "slots": [
        {
          "label": null,
          "accepted": ["have seen", "'ve seen"],
          "distractors": ["saw", "see", "am seeing"]
        }
      ]
    }
  ]
}
```

**importMode:** `create` (new topic) | `append` (add exercises) | `replace` (replace all exercises, keep topic)

Example import response:

```json
{
  "topicId": 1,
  "exercisesCreated": 2,
  "exercisesTotal": 2,
  "slotsCreated": 3,
  "warnings": []
}
```

Example topic tree response:

```json
[
  {
    "slug": "present-perfect",
    "title": "Present perfect",
    "sortOrder": 10,
    "exerciseCount": 2,
    "masteryLevel": "not_started",
    "accuracyPct": 0,
    "children": []
  }
]
```

### Phase A2 — practice (not implemented yet)

| Method | Path | Auth | Notes |
|--------|------|------|-------|
| POST | `/api/grammar/sessions` | User JWT | Start practice session |
| GET | `/api/grammar/sessions/{id}/next` | User JWT | Next exercise |
| POST | `/api/grammar/sessions/{id}/attempt` | User JWT | Submit answer |
| POST | `/api/grammar/sessions/{id}/end` | User JWT | End session |
| GET | `/api/grammar/progress` | User JWT | Topic progress summary |
| GET | `/api/grammar/topics/{slug}/exercises/stats` | User JWT | Per-exercise stats |

## Expected API

_Not applicable._

## Notes

- Auth: JWT Bearer (`Authorization: Bearer <token>`)
- Apply migrations: `cd devops/migrations && npm run migrate` (includes `0002_grammar.sql`)
- Import errors: `400` validation, `409` slug conflict / append to missing topic
- Accepted answer variants (`don't` / `do not`) — upload both explicitly; normalized on import
