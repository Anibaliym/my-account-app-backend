# My Account App

### General Description
**My Account App** is a full-stack application designed for user account management, including authentication, financial data management, and transaction tracking. It is built using modern technologies and follows a layered architecture ensuring scalability, separation of concerns, and maintainability.

### Features
- User authentication with Google Sign-In and an optional manual registration system with passwords.
- Secure management of accounts, cards, and notes with encrypted storage.
- Responsive API built with .NET 8.0 to handle requests efficiently.
- Integration with PostgreSQL for persistent data storage.

### Technologies Used
- **Backend**: .NET Core 8.0 (C#)
- **Database**: PostgreSQL
- **Authentication**: Google OAuth and manual password system.

### Architecture
The project follows a clean architecture pattern with the following layers:

1. **Api Layer**: Responsible for managing HTTP requests, directing them to the appropriate services, and returning responses to the client.
2. **Application Layer**: Contains application logic, including services, validations, and interfaces that connect the Core and Infrastructure layers. The layer includes:
   - **AutoMapper**: For object mapping between different layers.
   - **Interfaces**: Contract definitions for services.
   - **Responses**: Handlers for standardized responses to requests.
   - **Services**: Implementations of business logic acting as intermediaries between controllers and infrastructure.
   - **Validations**: Input and business logic validations.
   - **ViewModels**: Models representing the data sent to or received from the view.
3. **Core Layer**: Contains core business logic, domain entities, and services that do not depend on any specific infrastructure.
4. **Infrastructure Layer**: Handles data persistence and interactions with the database, as well as managing connections to external APIs. This is where repositories and integration services are located.

### External Libraries Used
The project uses several external libraries to facilitate development and add additional functionalities. The main ones are detailed below:

- **Entity Framework Core**: Used for database management, providing Object-Relational Mapping (ORM) to facilitate interaction with PostgreSQL. It also supports `Code First` migrations, making it easier to evolve the database schema without writing manual SQL scripts.

- **AutoMapper**: This library is used to simplify object mapping, such as converting database entities to DTOs (Data Transfer Objects) sent to the client, or vice versa. This helps maintain cleaner code and avoid duplication of logic for transforming data between layers.

- **FluentValidation**: Used to validate input data in the Application layer services. It allows defining validation rules in a declarative and consistent way, ensuring that the data is correct before being processed in the business logic.

- **ASP.NET Core Identity**: Provides a robust solution for managing users, roles, authentication, and authorization within the application. It is used for authentication with both Google OAuth and the manual password system, enabling secure user registration.

- **Google.Apis.Auth**: Used to integrate authentication with Google, allowing users to log into the application through their Google accounts, leveraging OAuth 2.0 to ensure credential security.

- **Newtonsoft.Json**: Used for JSON serialization and deserialization in the API. It simplifies the conversion of C# objects to JSON and vice versa, which is essential for communication between the frontend and backend.

### Best Practices
- **Dependency Injection**: Used to manage the application's services and repositories, allowing for better decoupling and facilitating unit testing.
- **Layered Architecture**: Each component has a single responsibility, following SOLID principles to improve maintainability and flexibility of the code.
- **Code First Migrations**: Using Entity Framework Core for database management, making it easier to evolve the data model through migrations.
- **Unit Testing**: The architecture allows for easy integration of unit tests for each layer, ensuring stability and correctness during development.

### Instructions to Start the Project
1. Clone the repository.
   ```bash
   git clone https://github.com/Anibaliym/my-account-app-backend
   ```

2. **Run the SQL script** (`script.sql`) provided to create the database and tables in PostgreSQL located at "my-account-app-backend\data-base\script.sql".

3. Run the project.

### Contributions
This project is under development, so any contribution is welcome. If you have ideas, improvements, or find errors, please open an issue or submit a pull request.

