import { createSlice } from '@reduxjs/toolkit'
import type { PayloadAction } from '@reduxjs/toolkit'

export interface UserState {
  darkMode: boolean
}

const initialState: UserState = {
  darkMode: false
}

export const themeSlice = createSlice({
  name: 'theme',
  initialState,
  reducers: {
    setDarkMode: (state, action: PayloadAction<boolean>) => {
      state.darkMode = action.payload
    }
  }
})

export const { setDarkMode } = themeSlice.actions

export default themeSlice.reducer
