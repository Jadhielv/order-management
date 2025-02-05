import React, { useEffect } from 'react';
import {
    Dialog,
    DialogTitle,
    DialogContent,
    DialogActions,
    Button,
    TextField,
    Box
} from '@mui/material';
import { Order, orderService } from '../services/api';
import { useForm, Controller } from 'react-hook-form';

interface OrderFormProps {
    open: boolean;
    onClose: () => void;
    order?: Order | null;
    onError?: (message: string) => void;
}

export default function OrderForm({ open, onClose, order, onError }: OrderFormProps) {
    const { control, handleSubmit, reset, formState: { errors } } = useForm<Order>({
        defaultValues: {
            customerName: '',
            orderDate: new Date().toISOString().split('T')[0],
            totalAmount: 0
        }
    });

    useEffect(() => {
        if (order) {
            reset({
                ...order,
                orderDate: new Date(order.orderDate).toISOString().split('T')[0]
            });
        }
    }, [order, reset]);

    const onSubmit = async (data: Order) => {
        try {
            if (order?.id) {
                await orderService.update(order.id, data);
            } else {
                await orderService.create(data);
            }
            onClose();
        } catch (error: any) {
            const errorMessage = error.response?.data?.Message || 'Failed to save order';
            onError?.(errorMessage);
            console.error('Error saving order:', error);
        }
    };

    return (
        <Dialog open={open} onClose={onClose} maxWidth="sm" fullWidth>
            <DialogTitle>
                {order ? 'Edit Order' : 'New Order'}
            </DialogTitle>
            <form onSubmit={handleSubmit(onSubmit)}>
                <DialogContent>
                    <Box sx={{ display: 'flex', flexDirection: 'column', gap: 2 }}>
                        <Controller
                            name="customerName"
                            control={control}
                            rules={{ required: 'Customer name is required', minLength: 3 }}
                            render={({ field }) => (
                                <TextField
                                    {...field}
                                    label="Customer Name"
                                    error={!!errors.customerName}
                                    helperText={errors.customerName?.message}
                                />
                            )}
                        />
                        <Controller
                            name="orderDate"
                            control={control}
                            rules={{ required: 'Order date is required' }}
                            render={({ field }) => (
                                <TextField
                                    {...field}
                                    type="date"
                                    label="Order Date"
                                    error={!!errors.orderDate}
                                    helperText={errors.orderDate?.message}
                                    InputLabelProps={{ shrink: true }}
                                />
                            )}
                        />
                        <Controller
                            name="totalAmount"
                            control={control}
                            rules={{ 
                                required: 'Total amount is required',
                                min: { value: 0.01, message: 'Amount must be greater than 0' }
                            }}
                            render={({ field }) => (
                                <TextField
                                    {...field}
                                    type="number"
                                    label="Total Amount"
                                    error={!!errors.totalAmount}
                                    helperText={errors.totalAmount?.message}
                                    InputProps={{ inputProps: { min: 0, step: 0.01 } }}
                                />
                            )}
                        />
                    </Box>
                </DialogContent>
                <DialogActions>
                    <Button onClick={onClose}>Cancel</Button>
                    <Button type="submit" variant="contained" color="primary">
                        {order ? 'Update' : 'Create'}
                    </Button>
                </DialogActions>
            </form>
        </Dialog>
    );
} 