# PosManager API

Backend para administrar ventas y consultar información de productos, clientes, empleados y bancos. Está desarrollado en C# con ASP.NET Core y SQL Server.

## ¿Qué hace este proyecto?

Es un proyecto académico cuyo objetivo es simular, a nivel de software, la gestión de información relacionada con un banco y una compañía. El modelo contempla bancos y cuentas bancarias de la compañía, además de funcionalidades de un sistema de punto de venta (POS): clientes, empleados, productos y órdenes de venta.

La API permite consultar y administrar bancos, consultar entidades del sistema y crear, consultar, actualizar o eliminar ventas. Las solicitudes y respuestas usan JSON, y los datos se almacenan en SQL Server.

> Esta es una simulación académica de gestión de datos: no procesa pagos, transferencias ni movimientos de dinero reales.

## Organización del código

La solución separa responsabilidades en proyectos:

- **PosManager.WebApi:** endpoints HTTP y configuración de la API.
- **PosManager.Business:** reglas y validaciones de negocio.
- **PosManager.Data:** acceso a SQL Server.
- **PosManager.Domain:** entidades del sistema y DTOs para intercambiar datos.

Esta separación permite mantener diferenciadas la interfaz HTTP, las reglas de la aplicación, la persistencia y los modelos del dominio.

## Requisitos

- Windows.
- .NET 10 SDK.
- Visual Studio compatible con .NET 10, con la carga de trabajo **ASP.NET y desarrollo web** (opcional si prefieres usar la terminal).
- Acceso y credenciales autorizadas para la base de datos SQL Server configurada para el proyecto.

## Ejecutar con Visual Studio

1. Instala el SDK de .NET 10 y Visual Studio con la carga de trabajo **ASP.NET y desarrollo web**.
2. Clona el repositorio y abre `PosManager.Api.slnx` en Visual Studio.
3. En el Explorador de soluciones, haz clic derecho en **PosManager.WebApi** y selecciona **Establecer como proyecto de inicio**.
4. Configura la conexión a SQL Server como se explica en la sección siguiente.
5. Ejecuta el proyecto con el perfil **http** o **https**.

> **Importante:** `PosManager.WebApi` es el proyecto que contiene los endpoints del sistema. `PosManager.Api` es un proyecto aparte y no forma parte de la solución principal.

## Configurar la conexión a SQL Server

La API espera una cadena de conexión con la clave `Lenguajes_Equipo4`. No compartas ni subas contraseñas al repositorio. Puedes guardarla localmente con User Secrets desde la carpeta raíz:

```powershell
dotnet user-secrets init --project .\PosManager.WebApi\PosManager.WebApi.csproj
dotnet user-secrets set "ConnectionStrings:Lenguajes_Equipo4" "Server=SERVIDOR;Database=BASE_DE_DATOS;User Id=USUARIO;Password=CONTRASEÑA;Encrypt=True;TrustServerCertificate=True" --project .\PosManager.WebApi\PosManager.WebApi.csproj
```

Reemplaza los valores de ejemplo por los datos a los que tengas autorización de acceso. Para ejecutar la API con datos, también necesitas que la base de datos y su esquema estén disponibles.

## Ejecutar desde la terminal

Desde la carpeta raíz del repositorio:

```powershell
dotnet restore .\PosManager.Api.slnx
dotnet run --project .\PosManager.WebApi\PosManager.WebApi.csproj --launch-profile http
```

La API queda disponible en `http://localhost:5188`. El perfil `https` también está configurado en `https://localhost:7199`.

## Endpoints principales

| Método | Ruta | Descripción |
|---|---|---|
| GET | `/api/producto` | Consulta productos |
| GET | `/api/cliente` | Consulta clientes |
| GET | `/api/empleado` | Consulta empleados |
| GET | `/api/banco` | Consulta bancos |
| GET | `/api/banco/{id}` | Consulta un banco |
| GET | `/api/venta` | Consulta ventas |
| GET | `/api/venta/{numeroOrden}` | Consulta una venta |
| POST | `/api/venta` | Crea una venta |
| PUT | `/api/venta/{numeroOrden}` | Actualiza una venta |
| DELETE | `/api/venta/{numeroOrden}` | Elimina una venta |

En el entorno de desarrollo, el documento OpenAPI está disponible en:

```text
http://localhost:5188/openapi/v1.json
```

Por ejemplo, puedes comprobar la ruta de productos desde PowerShell:

```powershell
Invoke-RestMethod http://localhost:5188/api/producto
```

Las rutas que consultan o modifican datos requieren una conexión válida a SQL Server.
