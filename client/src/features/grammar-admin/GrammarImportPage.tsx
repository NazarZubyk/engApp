import { useState, type FormEvent } from 'react'
import { Link } from 'react-router-dom'
import { useAuth } from '../auth/AuthProvider'
import { ApiError } from '../../lib/apiClient'
import { importGrammarContent } from '../grammar/grammarApi'
import type { ImportMode, ImportRequest, ImportResponse } from '../grammar/types'
import '../grammar/Grammar.css'

const SAMPLE_IMPORT: ImportRequest = {
  importMode: 'create',
  topic: {
    slug: 'present-perfect',
    parentSlug: null,
    title: 'Present perfect',
    explanationMd: '## When we use it\nWe use the present perfect for past actions connected to now.',
    sortOrder: 10,
  },
  exercises: [
    {
      prompt: 'I ___ (see) that film.',
      slots: [
        {
          accepted: ["have seen", "'ve seen"],
          distractors: ['saw', 'see', 'am seeing'],
        },
      ],
    },
  ],
}

export function GrammarImportPage() {
  const { session } = useAuth()
  const [importMode, setImportMode] = useState<ImportMode>('create')
  const [jsonText, setJsonText] = useState(() => JSON.stringify(SAMPLE_IMPORT, null, 2))
  const [error, setError] = useState<string | null>(null)
  const [result, setResult] = useState<ImportResponse | null>(null)
  const [submitting, setSubmitting] = useState(false)

  function handleFileChange(event: React.ChangeEvent<HTMLInputElement>) {
    const file = event.target.files?.[0]
    if (!file) {
      return
    }

    const reader = new FileReader()
    reader.onload = () => {
      if (typeof reader.result === 'string') {
        setJsonText(reader.result)
        setError(null)
        setResult(null)
      }
    }
    reader.readAsText(file)
  }

  async function handleSubmit(event: FormEvent) {
    event.preventDefault()
    if (!session?.token) {
      return
    }

    setSubmitting(true)
    setError(null)
    setResult(null)

    try {
      const parsed = JSON.parse(jsonText) as ImportRequest
      const payload: ImportRequest = {
        ...parsed,
        importMode,
      }

      const response = await importGrammarContent(session.token, payload)
      setResult(response)
    } catch (err) {
      if (err instanceof SyntaxError) {
        setError('Invalid JSON.')
      } else if (err instanceof ApiError) {
        setError(err.message)
      } else {
        setError('Import failed.')
      }
    } finally {
      setSubmitting(false)
    }
  }

  return (
    <main className="grammar-page">
      <div className="grammar-page-header">
        <div>
          <h1>Import grammar</h1>
        </div>
        <nav className="grammar-nav" aria-label="Admin navigation">
          <Link to="/menu">Menu</Link>
          <Link to="/grammar">Topics</Link>
        </nav>
      </div>

      <p className="grammar-muted">
        Admin only. Paste JSON or upload a file. Modes: create, append, replace.
      </p>

      <form className="grammar-form" onSubmit={(event) => void handleSubmit(event)}>
        <label>
          Import mode
          <select
            value={importMode}
            onChange={(event) => setImportMode(event.target.value as ImportMode)}
          >
            <option value="create">create — new topic</option>
            <option value="append">append — add exercises</option>
            <option value="replace">replace — replace all exercises</option>
          </select>
        </label>

        <label>
          JSON file
          <input type="file" accept="application/json,.json" onChange={handleFileChange} />
        </label>

        <label>
          Payload
          <textarea
            value={jsonText}
            onChange={(event) => setJsonText(event.target.value)}
            spellCheck={false}
          />
        </label>

        {error && <p className="grammar-error">{error}</p>}

        {result && (
          <p className="grammar-success">
            Imported topic #{result.topicId}: {result.exercisesCreated} new exercises,{' '}
            {result.exercisesTotal} total, {result.slotsCreated} slots.
          </p>
        )}

        <button type="submit" className="grammar-primary-button" disabled={submitting}>
          {submitting ? 'Importing…' : 'Import'}
        </button>
      </form>
    </main>
  )
}
