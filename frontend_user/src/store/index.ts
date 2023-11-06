import { configureStore } from '@reduxjs/toolkit'
import themeReducer from './reducers/theme'
import userReducer from './reducers/user'

export const store = configureStore({
  reducer: {
    themeReducer,
    userReducer
  }
})

export type RootState = ReturnType<typeof store.getState>
export type AppDispatch = typeof store.dispatch
