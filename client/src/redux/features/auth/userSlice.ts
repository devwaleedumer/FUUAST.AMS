import { PayloadAction, createSlice } from "@reduxjs/toolkit"
import { IUser } from "../../../types/auth";

// initial state
type UserState = {
    user: Partial<IUser> | null,
    isAuthenticated: boolean,
}
const initialState: UserState = {
    user: null,
    isAuthenticated: false
}

const userSlice = createSlice({
    name: "auth",
    initialState: initialState,
    reducers: {

        userLoggedIn: (state, action: PayloadAction<UserState>) => {
            state.user = action.payload.user;
            state.isAuthenticated = true;
        },
        userLoggedOut: (state) => {
            state.user = null;
            state.isAuthenticated = false;
        }
    }
})

export const { userLoggedIn, userLoggedOut } = userSlice.actions;
export default userSlice.reducer;