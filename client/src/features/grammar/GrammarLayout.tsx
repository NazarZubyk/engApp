import { Link } from 'react-router-dom'
import type { ReactNode } from 'react'
import { formatMasteryLabel } from './grammarUtils'
import type { TopicTreeNode } from './types'
import './Grammar.css'

function TopicTreeItems({ nodes, depth = 0 }: { nodes: TopicTreeNode[]; depth?: number }) {
  return (
    <ul className="grammar-topic-list" style={{ paddingLeft: depth > 0 ? 16 : 0 }}>
      {nodes.map((node) => (
        <li key={node.slug} className="grammar-topic-item">
          <Link className="grammar-topic-link" to={`/grammar/${node.slug}`}>
            <span className="grammar-topic-title">{node.title}</span>
            <span className="grammar-topic-meta">
              <span className={`grammar-badge ${node.masteryLevel}`}>
                {formatMasteryLabel(node.masteryLevel)}
              </span>
              <span>{node.exerciseCount} exercises</span>
              {node.accuracyPct > 0 && <span>{node.accuracyPct}%</span>}
            </span>
          </Link>
          {node.children.length > 0 && (
            <div style={{ padding: '0 8px 8px' }}>
              <TopicTreeItems nodes={node.children} depth={depth + 1} />
            </div>
          )}
        </li>
      ))}
    </ul>
  )
}

interface GrammarLayoutProps {
  title: string
  children: ReactNode
}

export function GrammarLayout({ title, children }: GrammarLayoutProps) {
  return (
    <main className="grammar-page">
      <div className="grammar-page-header">
        <div>
          <h1>{title}</h1>
        </div>
        <nav className="grammar-nav" aria-label="Grammar navigation">
          <Link to="/menu">Menu</Link>
          <Link to="/grammar">Topics</Link>
          <Link to="/grammar/progress">Progress</Link>
        </nav>
      </div>
      {children}
    </main>
  )
}

export { TopicTreeItems }
