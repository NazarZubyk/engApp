import { Link } from 'react-router-dom'
import { useAuth } from '../auth/AuthProvider'
import './MainMenuPage.css'

export function MainMenuPage() {
  const { session, logout } = useAuth()
  const isAdmin = session?.role === 'admin'

  return (
    <main className="main-menu-page">
      <section className="main-menu-card">
        <h1>Main menu</h1>
        <p className="main-menu-subtitle">Signed in as {isAdmin ? 'admin' : 'user'}.</p>

        <nav className="main-menu-links" aria-label="Main navigation">
          <Link to="/grammar">Grammar topics</Link>
          <Link to="/grammar/progress">Progress</Link>
          {isAdmin && <Link to="/admin/grammar/import">Import grammar</Link>}
        </nav>

        <button type="button" className="main-menu-logout" onClick={logout}>
          Log out
        </button>
      </section>
    </main>
  )
}
