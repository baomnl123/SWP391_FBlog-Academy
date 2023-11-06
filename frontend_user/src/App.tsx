import { ClerkProvider } from '@clerk/clerk-react'
import AppRoute from './routes'

function App() {
  return (
    <ClerkProvider publishableKey={import.meta.env.VITE_APP_CLERK_PUBLISHABLE_KEY}>
      <AppRoute />
    </ClerkProvider>
  )
}

export default App
