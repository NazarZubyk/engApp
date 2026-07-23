import { useEffect, useState } from 'react'
import { Link, useParams } from 'react-router-dom'
import { useAuth } from '../auth/AuthProvider'
import { ApiError } from '../../lib/apiClient'
import { fetchTopicDetail } from './grammarApi'
import { formatMasteryLabel } from './grammarUtils'
import { GrammarLayout } from './GrammarLayout'
import type { TopicDetail } from './types'
import './Grammar.css'

export function TopicDetailPage() {
  const { slug } = useParams<{ slug: string }>()
  const { session } = useAuth()
  const [topic, setTopic] = useState<TopicDetail | null>(null)
  const [error, setError] = useState<string | null>(null)
  const [loading, setLoading] = useState(true)

  useEffect(() => {
    if (!session?.token || !slug) {
      return
    }

    let cancelled = false

    async function load() {
      setLoading(true)
      setError(null)

      try {
        const data = await fetchTopicDetail(session!.token, slug!)
        if (!cancelled) {
          setTopic(data)
        }
      } catch (err) {
        if (!cancelled) {
          if (err instanceof ApiError && err.status === 404) {
            setError('Topic not found.')
          } else {
            setError(err instanceof ApiError ? err.message : 'Failed to load topic')
          }
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
  }, [session?.token, slug])

  return (
    <GrammarLayout title={topic?.title ?? 'Topic'}>
      <p className="grammar-muted">
        <Link to="/grammar">← All topics</Link>
      </p>

      {loading && <p className="grammar-muted">Loading topic…</p>}
      {error && <p className="grammar-error">{error}</p>}

      {topic && (
        <>
          <div className="grammar-stats">
            <div className="grammar-stat">
              <div className="grammar-stat-label">Exercises</div>
              <div className="grammar-stat-value">{topic.exerciseCount}</div>
            </div>
            <div className="grammar-stat">
              <div className="grammar-stat-label">Mastery</div>
              <div className="grammar-stat-value">
                <span className={`grammar-badge ${topic.masteryLevel}`}>
                  {formatMasteryLabel(topic.masteryLevel)}
                </span>
              </div>
            </div>
            <div className="grammar-stat">
              <div className="grammar-stat-label">Accuracy</div>
              <div className="grammar-stat-value">{topic.accuracyPct}%</div>
            </div>
            <div className="grammar-stat">
              <div className="grammar-stat-label">Attempts</div>
              <div className="grammar-stat-value">
                {topic.totalCorrect}/{topic.totalAttempts}
              </div>
            </div>
          </div>

          {topic.explanationMd && (
            <section aria-label="Explanation">
              <h2>Explanation</h2>
              <div className="grammar-explanation">{topic.explanationMd}</div>
            </section>
          )}

          <div className="grammar-actions">
            <button type="button" className="grammar-primary-button" disabled>
              Start practice
            </button>
            <span className="grammar-muted" style={{ margin: 0 }}>
              Practice sessions arrive in backend phase A2.
            </span>
          </div>
        </>
      )}
    </GrammarLayout>
  )
}
