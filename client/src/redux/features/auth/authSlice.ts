import { PayloadAction, createSlice } from "@reduxjs/toolkit"


// initial state
const initialState = {
    token: "",
    user: ""
}

const authSlice = createSlice({
    name: "auth",
    initialState: initialState,
    reducers: {
        // storing jwt token that was generated for verifying opt
        userRegistration: (state, action: PayloadAction<{ token: string }>) => {
            state.token = action.payload.token
        },
        userLoggedIn: (state, action: PayloadAction<{ accessToken: string, user: string }>) => {
            state.token = action.payload.accessToken;
            state.user = action.payload.user
        },
        userLoggedOut: (state) => {
            state.token = "";
            state.user = ""
        }
    }
})

export const { userRegistration, userLoggedIn, userLoggedOut } = authSlice.actions;
export default authSlice.reducer;