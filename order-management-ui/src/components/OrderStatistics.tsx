import React, { useEffect, useState } from 'react';
import {
    Card,
    CardContent,
    Grid,
    Typography,
    Box,
    CircularProgress
} from '@mui/material';
import { OrderStatistics as Stats, orderService } from '../services/api';

export default function OrderStatistics() {
    const [stats, setStats] = useState<Stats | null>(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string>('');

    const fetchStatistics = async () => {
        try {
            const data = await orderService.getStatistics();
            setStats(data);
        } catch (error) {
            setError('Failed to load statistics');
            console.error('Error fetching statistics:', error);
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        fetchStatistics();
        // Refresh statistics every minute
        const interval = setInterval(fetchStatistics, 60000);
        return () => clearInterval(interval);
    }, []);

    if (loading) {
        return (
            <Box display="flex" justifyContent="center" p={2}>
                <CircularProgress />
            </Box>
        );
    }

    if (error) {
        return (
            <Box p={2}>
                <Typography color="error">{error}</Typography>
            </Box>
        );
    }

    if (!stats) return null;

    return (
        <Box sx={{ mb: 4 }}>
            <Typography variant="h5" gutterBottom>
                Order Statistics
            </Typography>
            <Grid container spacing={3}>
                <Grid item xs={12} sm={6} md={3}>
                    <StatCard
                        title="Total Orders"
                        value={stats.totalOrders}
                    />
                </Grid>
                <Grid item xs={12} sm={6} md={3}>
                    <StatCard
                        title="Daily Orders"
                        value={stats.dailyOrders}
                    />
                </Grid>
                <Grid item xs={12} sm={6} md={3}>
                    <StatCard
                        title="Total Amount"
                        value={`$${stats.totalAmount.toFixed(2)}`}
                    />
                </Grid>
                <Grid item xs={12} sm={6} md={3}>
                    <StatCard
                        title="Daily Amount"
                        value={`$${stats.dailyAmount.toFixed(2)}`}
                    />
                </Grid>
            </Grid>
        </Box>
    );
}

interface StatCardProps {
    title: string;
    value: string | number;
}

function StatCard({ title, value }: StatCardProps) {
    return (
        <Card>
            <CardContent>
                <Typography color="textSecondary" gutterBottom>
                    {title}
                </Typography>
                <Typography variant="h5" component="div">
                    {value}
                </Typography>
            </CardContent>
        </Card>
    );
} 