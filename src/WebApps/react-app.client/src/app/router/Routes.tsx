import {createBrowserRouter, Navigate} from "react-router-dom";
import App from "../layout/App.tsx";
import Catalog from "../../features/catalog/Catalog.tsx";
import NotFound from "../errors/NotFound.tsx";
import BasketPage from "../../features/basket/BasketPage.tsx";
import ProductDetails from "../../features/catalog/ProductDetails.tsx";
import CheckoutPage from "../../features/checkout/CheckoutPage.tsx";
import Orders from "../../features/orders/Orders.tsx";
import OrderDetailed from "../../features/orders/OrderDetailed.tsx";

export const router = createBrowserRouter([
    {
        path: '/',
        element: <App/>,
        children: [
            {
                path: 'catalog',
                element: <Catalog/>
            },
            {
                path: 'catalog/:id', 
                element: <ProductDetails />
            },
            {
                path: 'basket',
                element: <BasketPage/>
            },
            {
                path: 'checkout',
                element: <CheckoutPage/>
            },
            {
                path: 'orders',
                element: <Orders/>
            },
            {
                path: 'orders/:id',
                element: <OrderDetailed/>
            },
            {
                path: 'not-found',
                element: <NotFound/>
            },
            {
                path: '*',
                element: <Navigate replace={true} to={'not-found'}/>
            }
        ]
    }
])