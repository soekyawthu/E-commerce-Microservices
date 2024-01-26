import {createAsyncThunk, createEntityAdapter, createSlice} from "@reduxjs/toolkit";
import agent from "../../app/api/agent";
import {MetaData} from "../../app/models/pagination";
import {Product, ProductParams} from "../../app/models/product";
import {RootState} from "../../app/store/configureStore";

interface CatalogState {
    productsLoaded: boolean;
    filtersLoaded: boolean;
    status: string;
    categories: string[];
    productParams: ProductParams;
    metaData: MetaData | null;
}

const productsAdapter = createEntityAdapter<Product>();

export const fetchProductsAsync = createAsyncThunk<Product[], void, {state: RootState}>(
    'catalog/fetchProductsAsync',
    async (_, thunkAPI) => {
        //const params = getAxiosParams(thunkAPI.getState().catalog.productParams)
        console.log("fetching products")
        try {
            const response = await agent.catalog.list();
            thunkAPI.dispatch(setMetaData(response.metaData));
            console.log(response)
            return response;
        } catch (error: any) {
            console.log(error.data)
            return thunkAPI.rejectWithValue({error: error.data})
        }
    }
)

export const fetchProductAsync = createAsyncThunk<Product, string>(
    'catalog/fetchProductAsync',
    async (productId, thunkAPI) => {
        try {
            return await agent.catalog.details(productId);
        } catch (error: any) {
            return thunkAPI.rejectWithValue({error: error.data})
        }
    }
)

function initParams(): ProductParams {
    return {
        pageNumber: 1,
        pageSize: 6,
        orderBy: 'name',
        categories: [],
    }
}

export const catalogSlice = createSlice({
    name: 'catalog',
    initialState: productsAdapter.getInitialState<CatalogState>({
        productsLoaded: false,
        filtersLoaded: false,
        status: 'idle',
        categories: [],
        productParams: initParams(),
        metaData: null
    }),
    reducers: {
        setProductParams: (state, action) => {
            state.productsLoaded = false;
            state.productParams = {...state.productParams, ...action.payload, pageNumber: 1}
        },
        setPageNumber: (state, action) => {
            state.productsLoaded = false;
            state.productParams = {...state.productParams, ...action.payload}
        },
        setMetaData: (state, action) => {
            state.metaData = action.payload
        },
        resetProductParams: (state) => {
            state.productParams = initParams()
        },
        setProduct: (state, action) => {
            productsAdapter.upsertOne(state, action.payload);
            state.productsLoaded = false;
        },
        removeProduct: (state, action) => {
            productsAdapter.removeOne(state, action.payload);
            state.productsLoaded = false;
        }
    },
    extraReducers: (builder => {
        builder.addCase(fetchProductsAsync.pending, (state, _) => {
            state.status = 'pendingFetchProducts'
        });
        builder.addCase(fetchProductsAsync.fulfilled, (state, action) => {
            console.log(action.payload)
            productsAdapter.setAll(state, action.payload);
            state.status = 'idle';
            state.productsLoaded = true;
        });
        builder.addCase(fetchProductsAsync.rejected, (state, action) => {
            console.log(action.payload);
            state.status = 'idle';
        });
        builder.addCase(fetchProductAsync.pending, (state) => {
            state.status = 'pendingFetchProduct';
        });
        builder.addCase(fetchProductAsync.fulfilled, (state, action) => {
            productsAdapter.upsertOne(state, action.payload);
            state.status = 'idle';
        });
        builder.addCase(fetchProductAsync.rejected, (state, action) => {
            console.log(action);
            state.status = 'idle';
        });
    })
})

export const productSelectors = productsAdapter.getSelectors((state: RootState) => state.catalog);

export const {setProductParams, resetProductParams, setMetaData, setPageNumber, setProduct, removeProduct} = catalogSlice.actions;