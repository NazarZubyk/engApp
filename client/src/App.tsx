import { BrowserRouter, Navigate, Route, Routes } from 'react-router-dom'
import { AuthProvider } from './features/auth/AuthProvider'
import { LoginPage } from './features/auth/LoginPage'
import { RequireAdmin } from './features/auth/RequireAdmin'
import { RequireAuth } from './features/auth/RequireAuth'
import { GrammarImportPage } from './features/grammar-admin/GrammarImportPage'
import { GrammarProgressPage } from './features/grammar/GrammarProgressPage'
import { GrammarTopicsPage } from './features/grammar/GrammarTopicsPage'
import { TopicDetailPage } from './features/grammar/TopicDetailPage'
import { MainMenuPage } from './features/main-menu/MainMenuPage'
import './App.css'

function App() {
  return (
    <BrowserRouter>
      <AuthProvider>
        <Routes>
          <Route path="/login" element={<LoginPage />} />
          <Route
            path="/menu"
            element={
              <RequireAuth>
                <MainMenuPage />
              </RequireAuth>
            }
          />
          <Route
            path="/grammar"
            element={
              <RequireAuth>
                <GrammarTopicsPage />
              </RequireAuth>
            }
          />
          <Route
            path="/grammar/progress"
            element={
              <RequireAuth>
                <GrammarProgressPage />
              </RequireAuth>
            }
          />
          <Route
            path="/grammar/:slug"
            element={
              <RequireAuth>
                <TopicDetailPage />
              </RequireAuth>
            }
          />
          <Route
            path="/admin/grammar/import"
            element={
              <RequireAdmin>
                <GrammarImportPage />
              </RequireAdmin>
            }
          />
          <Route path="*" element={<Navigate to="/menu" replace />} />
        </Routes>
      </AuthProvider>
    </BrowserRouter>
  )
}

export default App
