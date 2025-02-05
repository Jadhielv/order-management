import React from 'react';
import { Snackbar, Alert } from '@mui/material';

interface ErrorSnackbarProps {
    open: boolean;
    message: string;
    onClose: () => void;
}

export default function ErrorSnackbar({ open, message, onClose }: ErrorSnackbarProps) {
    return (
        <Snackbar 
            open={open} 
            autoHideDuration={6000} 
            onClose={onClose}
            anchorOrigin={{ vertical: 'top', horizontal: 'right' }}
        >
            <Alert onClose={onClose} severity="error" sx={{ width: '100%' }}>
                {message}
            </Alert>
        </Snackbar>
    );
} 