import React, { useEffect, useState } from 'react';
import {
    DataGrid,
    GridColDef,
    GridRenderCellParams,
    GridValueFormatter
} from '@mui/x-data-grid';
import {
    Box,
    Button,
    IconButton,
    Typography,
    Container
} from '@mui/material';
import DeleteIcon from '@mui/icons-material/Delete';
import EditIcon from '@mui/icons-material/Edit';
import { Order, orderService } from '../services/api';
import OrderForm from './OrderForm';
import ErrorSnackbar from './ErrorSnackbar';
import OrderStatistics from './OrderStatistics';

export default function OrderList() {
    const [orders, setOrders] = useState<Order[]>([]);
    const [loading, setLoading] = useState(true);
    const [openForm, setOpenForm] = useState(false);
    const [selectedOrder, setSelectedOrder] = useState<Order | null>(null);
    const [error, setError] = useState<string>('');

    const fetchOrders = async () => {
        try {
            const data = await orderService.getAll();
            setOrders(data);
        } catch (error) {
            setError('Failed to load orders');
            console.error('Error fetching orders:', error);
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        fetchOrders();
    }, []);

    const handleDelete = async (id: number) => {
        try {
            await orderService.delete(id);
            await fetchOrders();
        } catch (error) {
            setError('Failed to delete order');
            console.error('Error deleting order:', error);
        }
    };

    const handleEdit = (order: Order) => {
        setSelectedOrder(order);
        setOpenForm(true);
    };

    const handleCloseForm = () => {
        setSelectedOrder(null);
        setOpenForm(false);
        fetchOrders();
    };

    const columns: GridColDef[] = [
        {
            field: 'id',
            headerName: 'ID',
            width: 90
        },
        {
            field: 'customerName',
            headerName: 'Customer Name',
            width: 200
        },
        {
            field: 'orderDate',
            headerName: 'Order Date',
            width: 150,
            valueFormatter: (value: any) => 
                value ? new Date(value as string).toLocaleDateString() : ''
        },
        {
            field: 'totalAmount',
            headerName: 'Total Amount',
            width: 150,
            valueFormatter: (value: any) =>
                value ? `$${Number(value).toFixed(2)}` : ''
        },
        {
            field: 'actions',
            headerName: 'Actions',
            width: 120,
            renderCell: (params: GridRenderCellParams<Order>) => (
                <>
                    <IconButton
                        color="primary"
                        onClick={() => handleEdit(params.row)}
                        size="small"
                    >
                        <EditIcon />
                    </IconButton>
                    <IconButton
                        color="error"
                        onClick={() => handleDelete(params.row.id)}
                        size="small"
                    >
                        <DeleteIcon />
                    </IconButton>
                </>
            )
        }
    ];

    return (
        <Container maxWidth="lg">
            <Box sx={{ mt: 4, mb: 4 }}>
                <Typography variant="h4" component="h1" gutterBottom sx={{ textAlign: 'center' }}>
                    Order Management
                </Typography>
                
                <OrderStatistics />
                
                <Box sx={{ mb: 2, display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
                    <Typography variant="h5">Orders</Typography>
                    <Button 
                        variant="contained" 
                        color="primary" 
                        onClick={() => setOpenForm(true)}
                    >
                        Add New Order
                    </Button>
                </Box>

                <DataGrid
                    rows={orders}
                    columns={columns}
                    loading={loading}
                    autoHeight
                    pageSizeOptions={[5, 10, 25]}
                    initialState={{
                        pagination: { paginationModel: { pageSize: 10 } },
                    }}
                />
                
                <OrderForm 
                    open={openForm}
                    onClose={handleCloseForm}
                    order={selectedOrder}
                    onError={(message) => setError(message)}
                />
                
                <ErrorSnackbar
                    open={!!error}
                    message={error}
                    onClose={() => setError('')}
                />
            </Box>
        </Container>
    );
} 