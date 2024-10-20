# My Account App

### Descripción General
**My Account App** es una aplicación full-stack diseñada para la gestión de cuentas de usuario, incluyendo autenticación, manejo de datos financieros y seguimiento de transacciones. Está construida utilizando tecnologías modernas y sigue una arquitectura por capas que garantiza escalabilidad, separación de responsabilidades y mantenibilidad.

### Funcionalidades
- Autenticación de usuarios con Google Sign-In y sistema opcional de registro manual con contraseñas.
- Gestión segura de cuentas, tarjetas y viñetas con almacenamiento encriptado.
- API receptiva construida con .NET 8.0 para manejar solicitudes de manera eficiente.
- Integración con PostgreSQL para almacenamiento persistente de datos.

### Tecnologías Utilizadas
- **Backend**: .NET Core 8.0 (C#)
- **Base de datos**: PostgreSQL
- **Autenticación**: OAuth de Google y sistema manual de contraseñas.

### Arquitectura
El proyecto sigue un patrón de arquitectura limpia con las siguientes capas:

1. **Capa Api**: Encargada de gestionar las solicitudes HTTP, dirigirlas a los servicios adecuados y devolver respuestas al cliente.
2. **Capa Application**: Esta capa contiene la lógica de aplicación, incluidos los servicios, las validaciones y las interfaces que conectan las capas de Core e Infrastructure. La capa incluye:
   - **AutoMapper**: Para el mapeo de objetos entre distintas capas.
   - **Interfaces**: Definiciones de contratos para los servicios.
   - **Responses**: Manejadores de respuestas estandarizadas para las solicitudes.
   - **Services**: Implementaciones de la lógica de negocio que actúan como intermediarios entre el controlador y la infraestructura.
   - **Validations**: Validaciones de entrada y lógica de negocio.
   - **ViewModels**: Modelos que representan los datos que se envían a la vista o se reciben de ella.
3. **Capa Core**: Contiene la lógica de negocio central, las entidades de dominio y los servicios que no dependen de ninguna infraestructura específica.
4. **Capa Infrastructure**: Maneja la persistencia de datos y las interacciones con la base de datos, además de gestionar las conexiones con APIs externas. Aquí se encuentran los repositorios y los servicios de integración.

### Librerías Externas Utilizadas
El proyecto hace uso de varias librerías externas para facilitar el desarrollo y agregar funcionalidades adicionales. A continuación se detallan las principales:

- **Entity Framework Core**: Utilizado para la gestión de la base de datos, permite realizar mapeo objeto-relacional (ORM) para facilitar la interacción con PostgreSQL. Además, se usa para implementar migraciones `Code First`, lo que facilita la evolución del esquema de la base de datos sin necesidad de escribir scripts SQL manuales.
  
- **AutoMapper**: Esta librería se usa para simplificar el mapeo entre objetos, por ejemplo, convertir entidades de la base de datos en DTOs (Data Transfer Objects) que son enviados al cliente, o viceversa. Esto permite mantener un código más limpio y evitar la duplicación de lógica para transformar datos entre capas.

- **FluentValidation**: Empleada para validar los datos de entrada en los servicios de la capa Application. Permite definir reglas de validación de manera declarativa y consistente, asegurando que los datos sean correctos antes de ser procesados en la lógica de negocio.

- **ASP.NET Core Identity**: Proporciona una solución robusta para la gestión de usuarios, roles, autenticación y autorización dentro de la aplicación. Se utiliza para la autenticación tanto con Google OAuth como con el sistema manual de contraseñas, permitiendo el registro seguro de usuarios.

- **Google.Apis.Auth**: Se utiliza para integrar la autenticación con Google, permitiendo a los usuarios iniciar sesión en la aplicación a través de sus cuentas de Google, aprovechando OAuth 2.0 para garantizar la seguridad de las credenciales.

- **Newtonsoft.Json**: Utilizado para serializar y deserializar datos JSON en la API. Facilita la conversión de objetos C# a JSON y viceversa, lo cual es esencial para la comunicación entre el frontend y el backend.

### Buenas Prácticas
- **Inyección de Dependencias**: Se utiliza para gestionar los servicios y repositorios de la aplicación, permitiendo mejor desacoplamiento y facilitando las pruebas unitarias.
- **Arquitectura por Capas**: Cada componente tiene una única responsabilidad, siguiendo los principios SOLID para mejorar la mantenibilidad y flexibilidad del código.
- **Migraciones Code First**: Se usa Entity Framework Core para la gestión de la base de datos, lo que facilita la evolución del modelo de datos mediante migraciones.
- **Pruebas Unitarias**: La arquitectura permite fácilmente integrar pruebas unitarias para cada capa, garantizando estabilidad y corrección en el desarrollo.

### Instrucciones para Iniciar el Proyecto
1. Clona el repositorio.
2. **Ejecuta el script SQL** (`script.sql`) proporcionado para crear la base de datos y las tablas en PostgreSQL que está en la ruta "my-account-app-backend\data-base\script.sql":
3. Ejecuta el proyecto.

### Contribuciones
Este proyecto está en desarrollo, por lo que cualquier contribución es bienvenida. Si tienes ideas, mejoras o encuentras errores, por favor abre un issue o envía un pull request.

