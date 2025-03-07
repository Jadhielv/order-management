# Order Management System

A full-stack application for managing orders, built with .NET 9 and React.

## Project Structure

```
order-management/
├── OrderManagement.API/        # Backend API project
├── OrderManagement.Core/       # Core business logic and models
├── OrderManagement.Data/       # Data access layer
├── OrderManagement.API.Tests/  # API tests
├── OrderManagement.Core.Tests/ # Core logic tests
├── OrderManagement.Data.Tests/ # Data layer tests
├── database/                   # SQL scripts
│   └── scripts/
│       ├── 01_InitialSchema.sql
│       ├── 02_Triggers.sql
│       └── 03_InitialData.sql
└── order-management-ui/        # Frontend React application
```

## Prerequisites

- SQL Server 2019 or later
- .NET 9.0 SDK
- Node.js 18 or later
- npm 9 or later

## Backend Setup

### 1. Database Setup

1. Open SQL Server Management Studio (SSMS)
2. Connect to your SQL Server instance
3. Execute the SQL scripts in the following order:
   ```bash
   database/scripts/01_InitialSchema.sql
   database/scripts/02_Triggers.sql
   database/scripts/03_InitialData.sql
   ```

### 2. API Setup

1. Clone the repository
   ```bash
   git clone https://github.com/Jadhielv/order-management.git
   cd order-management
   ```

2. Update the connection string in `OrderManagement.API/appsettings.json`:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=localhost;Database=OrderManagementDB;Trusted_Connection=True;MultipleActiveResultSets=true"
     }
   }
   ```

3. Build and run the solution:
   ```bash
   dotnet build
   cd OrderManagement.API
   dotnet run
   ```

The API will be available at `http://localhost:5283`

### 3. Running Tests

```bash
dotnet test
```

## Frontend Setup

1. Navigate to the UI project:
   ```bash
   cd order-management-ui
   ```

2. Install dependencies:
   ```bash
   npm install
   ```

3. Start the development server:
   ```bash
   npm start
   ```

The application will be available at `http://localhost:3000`

## API Endpoints

- GET `/api/orders` - Get all orders
- GET `/api/orders/{id}` - Get order by ID
- POST `/api/orders` - Create new order
- PUT `/api/orders/{id}` - Update order
- DELETE `/api/orders/{id}` - Delete order
- GET `/api/orders/statistics` - Get order statistics

## Database Versioning

The database scripts are versioned and should be applied in sequence:

1. `01_InitialSchema.sql`: Creates the database and tables
2. `02_Triggers.sql`: Sets up triggers for statistics and auditing
3. `03_InitialData.sql`: Adds initial test data

When updating the database schema:
1. Create a new numbered script (e.g., `04_AddNewFeature.sql`)
2. Document the changes in this README
3. Apply the script to all environments in sequence

## Development Workflow

1. Pull the latest changes
2. Apply any new database scripts
3. Build and run the API
4. Start the React application
5. Make your changes
6. Run tests
7. Commit and push

## Troubleshooting

### Database Connection Issues
- Verify SQL Server is running
- Check connection string in appsettings.json
- Ensure database exists and user has proper permissions

### API Issues
- Check API logs in the console
- Verify correct ports are being used
- Ensure all dependencies are installed

### UI Issues
- Clear npm cache: `npm cache clean --force`
- Delete node_modules and reinstall: `rm -rf node_modules && npm install`
- Check browser console for errors

## Contributing

1. Create a feature branch
2. Make your changes
3. Run tests
4. Create a pull request

## License

This project is licensed under the [MIT License](LICENSE)
