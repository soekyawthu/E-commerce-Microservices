import CheckoutPage from "./CheckoutPage";
import { useState } from "react";
import { useAppDispatch } from "../../app/store/configureStore";
import LoadingComponent from "../../app/layout/LoadingComponent";

export default function CheckoutWrapper() {
    const dispatch = useAppDispatch();
    const [loading, setLoading] = useState(true);

    
    if (loading) return <LoadingComponent message='Loading checkout' />

    return (
            <CheckoutPage />
    )
} 