import { useEffect, useState } from 'react'
import { Link } from 'react-router-dom'
import { useAuth } from '../auth/AuthProvider'
import { ApiError } from '../../lib/apiClient'
import { fetchTopicTree } from './grammarApi'
import { flattenTopicTree, formatMasteryLabel } from './grammarUtils'
import { GrammarLayout } from './GrammarLayout'
import type { TopicProgressRow } from './types'
import './Grammar.css'

export function GrammarProgressPage() {
  const { session } = useAuth()
  const [rows, setRows] = useState<TopicProgressRow[]>([])
  const [error, setError] = useState<string | null>(null)
  const [loading, setLoading] = useState(true)

  useEffect(() => {
    if (!session?.token) {
      return
    }

    let cancelled = false

    async function load() {
      setLoading(true)
      setError(null)

      try {
        const tree = await fetchTopicTree(session!.token)
        if (!cancelled) {
          setRows(flattenTopicTree(tree))
        }
      } catch (err) {
        if (!cancelled) {
          setError(err instanceof ApiError ? err.message : 'Failed to load progress')
        }
      } finally {
        if (!cancelled) {
          setLoading(false)
        }
      }
    }

    void load()

    return () => {
      cancelled = true
    }
  }, [session?.token])

  return (
    <GrammarLayout title="Progress">
      {loading && <p className="grammar-muted">Loading progress…</p>}
      {error && <p className="grammar-error">{error}</p>}

      {!loading && !error && rows.length === 0 && (
        <p className="grammar-muted">No topics to track yet.</p>
      )}

      {!loading && !error && rows.length > 0 && (
        <div className="grammar-table-wrap">
          <table className="grammar-table">
            <thead>
              <tr>
                <th>Topic</th>
                <th>Exercises</th>
                <th>Mastery</th>
                <th>Accuracy</th>
              </tr>
            </thead>
            <tbody>
              {rows.map((row) => (
                <tr key={row.slug}>
                  <td>
                    <Link to={`/grammar/${row.slug}`} style={{ paddingLeft: row.depth * 16 }}>
                      {row.title}
                    </Link>
                  </td>
                  <td>{row.exerciseCount}</td>
                  <td>
                    <span className={`grammar-badge ${row.masteryLevel}`}>
                      {formatMasteryLabel(row.masteryLevel)}
                    </span>
                  </td>
                  <td>{row.accuracyPct}%</td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      )}
    </GrammarLayout>
  )
}
