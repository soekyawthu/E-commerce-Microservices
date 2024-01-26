import {configureStore} from "@reduxjs/toolkit";
import {catalogSlice} from "../../features/catalog/catalogSlice.ts";
import {TypedUseSelectorHook, useDispatch, useSelector} from "react-redux";
import {basketSlice} from "../../features/basket/basketSlice.ts";

export const store = configureStore({
    reducer: {
        catalog: catalogSlice.reducer,
        basket: basketSlice.reducer
    }
})

export type RootState = ReturnType<typeof store.getState>
export type AppDispatch = typeof store.dispatch

export const useAppDispatch = () => useDispatch<AppDispatch>()
export const useAppSelector: TypedUseSelectorHook<RootState> = useSelector