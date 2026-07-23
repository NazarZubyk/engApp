import type { MasteryLevel, TopicProgressRow, TopicTreeNode } from './types'

export function flattenTopicTree(
  nodes: TopicTreeNode[],
  depth = 0,
): TopicProgressRow[] {
  const rows: TopicProgressRow[] = []

  for (const node of nodes) {
    rows.push({
      slug: node.slug,
      title: node.title,
      exerciseCount: node.exerciseCount,
      masteryLevel: node.masteryLevel,
      accuracyPct: node.accuracyPct,
      depth,
    })

    if (node.children.length > 0) {
      rows.push(...flattenTopicTree(node.children, depth + 1))
    }
  }

  return rows
}

export function formatMasteryLabel(level: MasteryLevel): string {
  switch (level) {
    case 'not_started':
      return 'Not started'
    case 'learning':
      return 'Learning'
    case 'practicing':
      return 'Practicing'
    case 'strong':
      return 'Strong'
  }
}
