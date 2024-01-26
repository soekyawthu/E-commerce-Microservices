import { Box, Typography, Button, Grid } from "@mui/material";
import { Order } from "../../app/models/order";
import BasketSummary from "../basket/BasketSummary";
import {useParams} from "react-router-dom";
import {useEffect, useState} from "react";
import agent from "../../app/api/agent.ts";
import OrderTable from "./OrderTable.tsx";
import LoadingComponent from "../../app/layout/LoadingComponent.tsx";

export default function OrderDetailed() {
    const { id } = useParams<{ id: string }>();
    const [order, setOrder] = useState<Order | null>(null);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        id && agent.order.fetch(id)
            .then(order => setOrder(order))
            .catch(error => console.log(error))
            .finally(() => setLoading(false))
    }, []);
    
    const subtotal = order?.items.reduce((sum, item) => sum + (item.quantity * item.price), 0) ?? 0;

    if (loading) return <LoadingComponent message="Loading Order Detail..." />
    
    return (
        <>
            <Box display='flex' justifyContent='space-between'>
                <Typography sx={{ p: 2 }} gutterBottom variant='h4'>Order# {order?.id} - {order?.status}</Typography>
                <Button sx={{ m: 2 }} size='large' variant='contained'>Back to orders</Button>
            </Box>
            <OrderTable items={order?.items} />
            <Grid container>
                <Grid item xs={6} />
                <Grid item xs={6}>
                    <BasketSummary subtotal={subtotal} />
                </Grid>
            </Grid>
        </>
    )
}