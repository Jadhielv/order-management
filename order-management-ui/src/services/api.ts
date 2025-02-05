import axios from 'axios';

const API_BASE_URL = 'http://localhost:5283/api';

export interface Order {
    id: number;
    customerName: string;
    orderDate: string;
    totalAmount: number;
    createdAt?: string;
    updatedAt?: string;
}

export interface OrderStatistics {
    totalOrders: number;
    dailyOrders: number;
    totalAmount: number;
    dailyAmount: number;
    lastUpdated: string;
}

const api = axios.create({
    baseURL: API_BASE_URL,
    headers: {
        'Content-Type': 'application/json',
    },
});

export const orderService = {
    getAll: async (): Promise<Order[]> => {
        const response = await api.get('/orders');
        return response.data;
    },

    getById: async (id: number): Promise<Order> => {
        const response = await api.get(`/orders/${id}`);
        return response.data;
    },

    create: async (order: Order): Promise<Order> => {
        const response = await api.post('/orders', order);
        return response.data;
    },

    update: async (id: number, order: Order): Promise<Order> => {
        const response = await api.put(`/orders/${id}`, order);
        return response.data;
    },

    delete: async (id: number): Promise<void> => {
        await api.delete(`/orders/${id}`);
    },

    getStatistics: async (): Promise<OrderStatistics> => {
        const response = await api.get('/orders/statistics');
        return response.data;
    },
}; 