import { TableContainer, Paper, Table, TableHead, TableRow, TableCell, TableBody, Button } from "@mui/material";
import { useEffect, useState } from "react";
import agent from "../../app/api/agent";
import LoadingComponent from "../../app/layout/LoadingComponent";
import { Order } from "../../app/models/order";
import { currencyFormat } from "../../app/utils/util";
import {Link} from "react-router-dom";

export default function Orders() {
    const [orders, setOrders] = useState<Order[] | null>(null);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        agent.order.list()
            .then(orders => setOrders(orders as Order[]))
            .catch(error => console.log(error))
            .finally(() => setLoading(false))
    }, [orders]);

    if (loading) return <LoadingComponent message="Loading orders..." />

    return (
        <TableContainer component={Paper}>
            <Table sx={{ minWidth: 650 }} aria-label="simple table">
                <TableHead>
                    <TableRow>
                        <TableCell>Order Number</TableCell>
                        <TableCell align="right">Total</TableCell>
                        <TableCell align="right">Order Date</TableCell>
                        <TableCell align="right"></TableCell>
                    </TableRow>
                </TableHead>
                <TableBody>
                    {orders?.map((order) => (
                        <TableRow
                            key={order.id}
                            sx={{ '&:last-child td, &:last-child th': { border: 0 } }}
                        >
                            <TableCell component="th" scope="row">
                                {order.id}
                            </TableCell>
                            <TableCell align="right">{currencyFormat(order.totalPrice)}</TableCell>
                            {/*<TableCell align="right">{{order.orderDate.split('T')[0]}}</TableCell>*/}
                            <TableCell align="right">{order.orderDate}</TableCell>
                            <TableCell align="right">
                                <Button component={Link} to={`${order.id}`}  size="small">View</Button>
                            </TableCell>
                        </TableRow>
                    ))}
                </TableBody>
            </Table>
        </TableContainer>
    )
}