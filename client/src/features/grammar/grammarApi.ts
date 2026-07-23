import { apiFetch } from '../../lib/apiClient'
import type { ImportRequest, ImportResponse, TopicDetail, TopicTreeNode } from './types'

export function fetchTopicTree(token: string) {
  return apiFetch<TopicTreeNode[]>('/api/grammar/topics', { token })
}

export function fetchTopicDetail(token: string, slug: string) {
  return apiFetch<TopicDetail>(`/api/grammar/topics/${encodeURIComponent(slug)}`, {
    token,
  })
}

export function importGrammarContent(token: string, payload: ImportRequest) {
  return apiFetch<ImportResponse>('/api/grammar/import', {
    method: 'POST',
    token,
    body: JSON.stringify(payload),
  })
}
