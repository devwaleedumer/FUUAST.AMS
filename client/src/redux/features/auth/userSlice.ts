import { removeSessionCookies } from "@/lib/auth/tokenCookies";
import { PayloadAction, createSlice } from "@reduxjs/toolkit"


// initial state
const initialState = {
    token: "",
    user: ""
}

const userSlice = createSlice({
    name: "auth",
    initialState: initialState,
    reducers: {

        userLoggedIn: (state, action: PayloadAction<{ accessToken: string, user: string }>) => {
            state.token = action.payload.accessToken;
            state.user = action.payload.user
        },
        userLoggedOut: (state) => {
            state.token = "";
            state.user = "";
            removeSessionCookies()
        }
    }
})

export const { userLoggedIn, userLoggedOut } = userSlice.actions;
export default userSlice.reducer;