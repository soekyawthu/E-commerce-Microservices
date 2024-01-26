import { LoadingButton } from "@mui/lab";
import { Avatar, Button, Card, CardActions, CardContent, CardHeader, CardMedia, Typography } from "@mui/material";
import { Link } from "react-router-dom";
import { Product } from "../../app/models/product";
import { useAppDispatch, useAppSelector } from "../../app/store/configureStore";
import { currencyFormat } from "../../app/utils/util";
import {addBasketItemAsync} from "../basket/basketSlice.ts";
import {BasketItem} from "../../app/models/basket.ts";

interface Props {
    product: Product;
}

export default function ProductCard({ product }: Props) {
    const {status} = useAppSelector(state => state.basket);
    const dispatch = useAppDispatch();

    return (
        <Card>
            <CardHeader
                avatar={
                    <Avatar sx={{ bgcolor: 'secondary.main' }}>
                        {product.name.charAt(0).toUpperCase()}
                    </Avatar>
                }
                title={product.name}
                titleTypographyProps={{
                    sx: { fontWeight: 'bold', color: 'primary.main' }
                }}
            />
            <CardMedia
                sx={{ height: 140, backgroundSize: 'contain', bgcolor: 'primary.light' }}
                image={product.image}
                title={product.name}
            />
            <CardContent>
                <Typography gutterBottom color='secondary' variant="h5">
                    {currencyFormat(product.price)}
                </Typography>
                <Typography variant="body2" color="text.secondary">
                    {product.category}
                </Typography>
            </CardContent>
            <CardActions>
                <LoadingButton
                    loading={status === `PendingBasketAdded-${product.id}`}
                    onClick={() => {
                        const item : BasketItem = {
                            productId: product.id,
                            productName: product.name,
                            pictureUrl: product.image,
                            color: "black",
                            price: product.price,
                            quantity: 1,
                        }
                        dispatch(addBasketItemAsync(item))
                    }}
                    size="small">Add to Cart</LoadingButton>
                <Button component={Link} to={`/catalog/${product.id}`}  size="small">View</Button>
            </CardActions>
        </Card>
    )
}