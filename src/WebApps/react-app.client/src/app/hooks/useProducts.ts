import {useAppDispatch, useAppSelector} from "../store/configureStore.ts";
import {fetchProductsAsync, productSelectors} from "../../features/catalog/catalogSlice.ts";
import {useEffect} from "react";

export default function useProducts(){
    const products = useAppSelector(productSelectors.selectAll)
    const {categories, metaData, productsLoaded} = useAppSelector(state => state.catalog)
    const dispatch = useAppDispatch()
    
    useEffect(() => {
        console.log("use product fetch products")
        dispatch(fetchProductsAsync())
    }, [dispatch]);
    
    return {
        products,
        categories,
        metaData,
        productsLoaded
    }
}