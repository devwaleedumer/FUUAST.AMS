import { combineSlices, configureStore } from "@reduxjs/toolkit";
import { apiSlice } from "./features/apiSlice";
import userSlice from "./features/auth/userSlice";
import wizardSlice from "./features/applicant/applicationWizardSlice";
import { rtkQueryErrorLogger } from "@/lib/services/rtkErrorMiddleware";
// import { counterSlice } from "./features/counter/counterSlice";
// import { quotesApiSlice } from "./features/quotes/quotesApiSlice";

// `combineSlices` automatically combines the reducers using
// their `reducerPath`s, therefore we no longer need to call `combineReducers`.
// Infer the `RootState` type from the root reducer
// export type RootState = ReturnType<typeof rootReducer>;

// `makeStore` encapsulates the store configuration to allow
// creating unique store instances, which is particularly important for
// server-side rendering (SSR) scenarios. In SSR, separate store instances
// are needed for each request to prevent cross-request state pollution.
export const store = configureStore({
    reducer: {
        [apiSlice.reducerPath]: apiSlice.reducer,
        auth: userSlice,
        wizard: wizardSlice
    },
    devTools: false,
    // Adding the api middleware enables caching, invalidation, polling,
    // and other useful features of `rtk-query`.
    middleware: (getDefaultMiddleware) => {
        return getDefaultMiddleware().concat(rtkQueryErrorLogger).concat(apiSlice.middleware);
    },
});

// const initializeApp = async () => await store.dispatch(apiSlice.endpoints.loadUser.initiate(null, { forceRefetch: true }))
// initializeApp();

export type RootState = ReturnType<typeof store.getState>
export type AppDispatch = typeof store.dispatch
