import useProducts from "../../app/hooks/useProducts.ts";
import {Grid, Paper} from "@mui/material";
import ProductSearch from "./ProductSearch.tsx";
import ProductList from "./ProductList.tsx";

export default function Catalog() {
    const {products} = useProducts()
    
    return (
        <Grid container columnSpacing={4}>
            <Grid item xs={3}>
                <Paper sx={{ mb: 2 }}>
                    <ProductSearch />
                </Paper>
            </Grid>
            <Grid item xs={9}>
                <ProductList products={products} />
            </Grid>
            <Grid item xs={3} />
        </Grid>
    )
}
