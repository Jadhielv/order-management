import React from 'react';
import { render, screen } from '@testing-library/react';
import App from './App';

test('renders Order Management System heading', () => {
  render(<App />);
  const headingElement = screen.getByText(/Order Management System/i);
  expect(headingElement).toBeInTheDocument();
});
