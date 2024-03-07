# Vue.js Product Management System

This project is a Vue.js-based Product Management System designed to showcase CRUD operations with a .NET backend, featuring functionalities like product listing, creation, editing, and deletion. It includes pagination and sorting capabilities for efficient data handling and display.

## Features

- **CRUD Operations**: Create, Read, Update, and Delete products.
- **Pagination**: Navigate through products in paginated format.
- **Sorting**: Sort products based on price.
- **Responsive Design**: Compatible with various screen sizes.
- **Image Upload**: Supports image upload for products.

## .NET Backend Application

The backend for this project is built using .NET. It provides RESTful APIs that the Vue.js frontend consumes. Here's how to set it up:

### Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/download) (version specified in your project)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (if your project uses SQL Server)

### Setup and Running

1. Navigate to the .NET project directory:
   ```bash
   cd path/to/your/netproject
   ```

2. Restore dependencies:
   ```bash
   dotnet restore
   ```
   
4. Apply migrations (if your project uses Entity Framework):
   ```bash
   dotnet ef database update
   ```
   
6. Run the application:
   ```bash
   dotnet run
   ```

This will start the .NET server, typically on https://localhost:7294.

## Connecting with the Vue.js Frontend
Make sure that the Vue.js application is set up to connect to the .NET backend by configuring the API endpoint URLs. You might need to adjust the Vue.js environment variables or API service configuration to point to the .NET backend server's address.

## Installation

Before you begin, ensure you have [Node.js](https://nodejs.org/) and [Vue CLI](https://cli.vuejs.org/) installed on your system.

To get the project up and running on your local machine, follow these steps:

1. Clone the repository:

   ```bash
   git clone https://github.com/LucasVanni/CRUD-VUE-DOTNET.git
   cd CRUD-VUE-DOTNET
   ```

2. Install dependencies:

   ```bash
    yarn
   ```

3. Serve the application:

   ```bash
    yarn dev
   ```

The application will be available at http://localhost:5173/

## Usage
The application allows you to:

View Products: Navigate through the list of products.
- **Create New Product**: Add a new product by providing the required information.
- **Edit Product**: Update product details.
- **Delete Product**: Remove a product from the list.
- **Sort Products**: Click on the 'Price' to sort products by their price.
- **Paginate Products**: Use the pagination controls to navigate between pages.

## Contributing
Contributions are welcome! Please feel free to submit a Pull Request or create an issue for any changes or additions.

## License
This project is licensed under the MIT License - see the LICENSE.md file for details.

## Acknowledgments
- Vue.js Team for the amazing frontend framework.
- The .NET community for backend support.
