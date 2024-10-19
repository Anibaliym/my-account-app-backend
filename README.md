﻿my-account-app - Back-end

my-account-app (MiCuentaApp) es el back-end de una aplicación de gestión de cuentas personales. Esta API, construida con .NET Core 8.0, se encarga de gestionar las operaciones de negocio, el almacenamiento de datos en PostgreSQL y la autenticación de usuarios.

Tecnologías Utilizadas

- 	.NET Core 8.0
- `	`PostgreSQL como base de datos relacional
- `	`Entity Framework Core como ORM
- `	`AutoMapper para mapear entidades y DTOs
- `	`FluentValidation para la validación de datos
- `	`JWT (JSON Web Tokens) para la autenticación y autorización

\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_

Librerías de Terceros

El proyecto utiliza varias librerías externas para facilitar el desarrollo:

1. `	`Entity Framework Core: Para el manejo de la base de datos y las operaciones CRUD.

bash

Copiar código

dotnet add package Microsoft.EntityFrameworkCore

dotnet add package Microsoft.EntityFrameworkCore.Design

dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL

1. `	`AutoMapper: Para mapear entre entidades y Data Transfer Objects (DTOs), lo que permite una separación clara entre los modelos de dominio y las respuestas que se envían a los clientes.

bash

Copiar código

dotnet add package AutoMapper.Extensions.Microsoft.DependencyInjection

1. `	`FluentValidation: Para la validación de los datos que se reciben en las solicitudes.

bash

Copiar código

dotnet add package FluentValidation.AspNetCore

1. `	`JWT Bearer Authentication: Para implementar autenticación mediante tokens JWT.

bash

Copiar código

dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer

\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_

Configuración de la Base de Datos

Esta aplicación utiliza PostgreSQL como sistema de gestión de base de datos. Asegúrate de tener PostgreSQL instalado y configurado.

1. Configurar la Cadena de Conexión

La cadena de conexión debe configurarse en el archivo appsettings.json dentro del proyecto back-end. Asegúrate de ajustar los valores de acuerdo con tu entorno local:

json

Copiar código

"ConnectionStrings": {

"DefaultConnection": "Host=<HOST>;Database=<DB\_NAME>;Username=<USERNAME>;Password=<PASSWORD>"

}

1. Aplicar Migraciones

Para inicializar la base de datos, ejecuta los siguientes comandos para aplicar las migraciones y crear las tablas necesarias:

bash

Copiar código

dotnet ef migrations add InitialCreate

dotnet ef database update

Esto creará las tablas en la base de datos PostgreSQL, basadas en los modelos definidos.

\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_

Configuración del Proyecto

Requisitos Previos

Asegúrate de tener instalados los siguientes programas:

- 	.NET Core SDK 8.0: Descargar .NET Core SDK
- `	`PostgreSQL: Descargar PostgreSQL

Instalación

1. `	`Clonar el repositorio:

bash

Copiar código

git clone <url-del-repositorio>

cd MiCuentaApp/back-end

1. `	`Restaurar las dependencias del proyecto:

bash

Copiar código

dotnet restore

1. `	`Configurar la conexión a la base de datos en el archivo appsettings.json.
1. `	`Aplicar las migraciones para inicializar la base de datos:

bash

Copiar código

dotnet ef database update

1. `	`Ejecutar la aplicación:

bash

Copiar código

dotnet run

\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_

Autenticación

El sistema de autenticación está basado en JSON Web Tokens (JWT). Para acceder a los recursos protegidos, los usuarios deben autenticarse y recibir un token JWT que luego se utilizará en el encabezado de las solicitudes HTTP (Authorization: Bearer <token>).

\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_

Uso

Una vez el back-end esté ejecutándose, puedes interactuar con la API a través de herramientas como Postman o cURL para realizar las operaciones CRUD sobre las cuentas y usuarios.

\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_

Contribuir

Si deseas contribuir al desarrollo del back-end, por favor sigue los siguientes pasos:

1. `	`Haz un fork del repositorio.
1. `	`Crea una nueva rama para tu funcionalidad (git checkout -b feature/nueva-funcionalidad).
1. `	`Realiza los cambios y haz commit (git commit -m 'Añadir nueva funcionalidad').
1. `	`Empuja la rama a tu fork (git push origin feature/nueva-funcionalidad).
1. `	`Crea un Pull Request en el repositorio principal.

\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_

Licencia

Este proyecto está licenciado bajo la MIT License - ver el archivo LICENSE para más detalles.

\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_

Este README está centrado exclusivamente en el back-end de la aplicación. Si deseas modificar algún detalle o agregar información más específica sobre alguna funcionalidad, no dudes en decírmelo.

