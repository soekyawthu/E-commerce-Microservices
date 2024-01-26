import {Basket, BasketItem} from "../../app/models/basket.ts";
import {createAsyncThunk, createSlice, isAnyOf} from "@reduxjs/toolkit";
import agent from "../../app/api/agent.ts";

interface BasketState{
    basket: Basket | null;
    status: string
}

const initialState: BasketState = {
    basket: null,
    status: 'idle'
}

export const fetchBasketAsync = createAsyncThunk<Basket>(
    'basket/fetchBasketAsync',
    async (_, thunkAPI) => {
        try {
            const remoteBasket = await agent.basket.get();
            console.log("Remote Basket => ", remoteBasket)
            return remoteBasket
        }catch (error: any) {
            return thunkAPI.rejectWithValue({error: error.data})
        }
    }
)

export const addBasketItemAsync = createAsyncThunk<Basket, BasketItem>(
    'basket/addBasketItemAsync',
    async (item: BasketItem, {getState, rejectWithValue}) => {
        try {
            const states = getState()
            const basketState = states.basket
            const readOnlyBasket = basketState.basket as Basket
            const basketObjString = JSON.stringify(readOnlyBasket)
            const writeableBasket = JSON.parse(basketObjString) as Basket
            const existedItem = writeableBasket.items.find(x => x.productId === item.productId);
            if (existedItem){
                existedItem.quantity += item.quantity
            }else {
                writeableBasket.items.push(item)
            }
            console.log("Basket => ", writeableBasket)
            await agent.basket.addItem(writeableBasket)
            //return writeableBasket
            const remoteBasket = await agent.basket.get();
            console.log("Remote Basket => ", remoteBasket)
            return remoteBasket
        }catch (error: any){
            return rejectWithValue({error: error})
        }
    }
)

export const removeBasketItemAsync = createAsyncThunk<Basket,
    {productId : string, quantity: number, name?: string}>(
    'basket/removeBasketAsync',
    async ( { productId, quantity}, thunkAPI) => {
        try {
            const states = thunkAPI.getState() as any
            const basketState = states.basket as any
            const readOnlyBasket = basketState.basket as Basket
            const basketObjString = JSON.stringify(readOnlyBasket)
            const writeableBasket = JSON.parse(basketObjString) as Basket
            const existedItem = writeableBasket.items.find(x => x.productId === productId);
            if (existedItem){
                existedItem.quantity -= quantity
                if (existedItem.quantity <= 0){
                    writeableBasket.items = writeableBasket.items.filter(x => x.productId !== productId)
                }
            }
            console.log("Basket => ", writeableBasket)
            await agent.basket.remove()
            return writeableBasket
        }catch (error){
            return thunkAPI.rejectWithValue({error: error.data})
        }
    }
)

export const basketSlice = createSlice({
    name: 'basket',
    initialState,
    reducers: {
        setBasket: (state, action) => {
            state.basket = action.payload
        },
        clearBasket: (state) => {
            state.basket = null
        }
    },
    extraReducers: builder => {
        builder.addCase(addBasketItemAsync.pending, (state, action) => {
            state.status = `PendingBasketAdded-${action.meta.arg.productId}`
            //state.basket = action.payload
        });
        
        builder.addCase(removeBasketItemAsync.pending, (state, _) => {
            state.status = "Pending basket removing"
        })
        builder.addCase(removeBasketItemAsync.fulfilled, (state, action) => {
            state.status = "Removed basket"
            state.basket = action.payload
        })
        builder.addCase(removeBasketItemAsync.rejected, (state, _) => {
            state.status = "Failed basket removing"
        })

        builder.addMatcher(isAnyOf(addBasketItemAsync.fulfilled, fetchBasketAsync.fulfilled), (state, action) => {
            state.basket = action.payload;
            state.status = 'idle success add & fetch';
        });
        
        builder.addMatcher(isAnyOf(addBasketItemAsync.rejected, fetchBasketAsync.rejected), (state, action) => {
            state.status = 'idle fail add & fetch';
            console.log(action.payload);
        });
    }
})

export const {setBasket, clearBasket} = basketSlice.actions;