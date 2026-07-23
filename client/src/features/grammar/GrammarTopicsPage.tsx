import { useEffect, useState } from 'react'
import { useAuth } from '../auth/AuthProvider'
import { ApiError } from '../../lib/apiClient'
import { fetchTopicTree } from './grammarApi'
import { GrammarLayout, TopicTreeItems } from './GrammarLayout'
import type { TopicTreeNode } from './types'
import './Grammar.css'

export function GrammarTopicsPage() {
  const { session } = useAuth()
  const [topics, setTopics] = useState<TopicTreeNode[]>([])
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
        const data = await fetchTopicTree(session!.token)
        if (!cancelled) {
          setTopics(data)
        }
      } catch (err) {
        if (!cancelled) {
          setError(err instanceof ApiError ? err.message : 'Failed to load topics')
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
    <GrammarLayout title="Grammar topics">
      {loading && <p className="grammar-muted">Loading topics…</p>}
      {error && <p className="grammar-error">{error}</p>}
      {!loading && !error && topics.length === 0 && (
        <p className="grammar-muted">No topics yet. Import content as admin to get started.</p>
      )}
      {!loading && !error && topics.length > 0 && <TopicTreeItems nodes={topics} />}
    </GrammarLayout>
  )
}
