# Torneo E-Sports

Sistema web de gestión de un torneo de e-sports desarrollado con **ASP.NET Core MVC** (sin Razor Pages) y **Entity Framework Core** sobre SQL Server. Permite administrar torneos, equipos, jugadores, partidas, sedes y árbitros, y consultar reportes del torneo.

Proyecto de la materia **Programación Web III**.

## Integrantes

| Integrante | Usuario de GitHub |
|---|---|
| Ronny Severiche Mariaca | `sm21fron05-crypto` |
| Jorge Daniel Gutiérrez España | _(completar)_ |
| Andrés Santiago Rojas Coria | `santiprod1` |

## Requisitos

- .NET SDK 10.0
- SQL Server LocalDB (incluido con Visual Studio) o cualquier instancia de SQL Server
- Visual Studio 2022 o superior (recomendado) o la CLI de .NET

## Cómo ejecutar la aplicación

1. Clona el repositorio:
   ```bash
   git clone https://github.com/sm21fron05-crypto/TorneoEsports.git
   cd TorneoEsports
   ```
2. Revisa la cadena de conexión `conn` en `appsettings.json`. Por defecto usa LocalDB:
   ```
   Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=TorneoEsports;Integrated Security=True;TrustServerCertificate=True
   ```
   Si usas otra instancia de SQL Server, cámbiala ahí. No se incluyen usuarios ni contraseñas en el repositorio.
3. Crea la base de datos (ver la siguiente sección).
4. Ejecuta la aplicación:
   ```bash
   dotnet run
   ```
   o presiona **F5** en Visual Studio. Se abre en `https://localhost:7244`.

## Base de datos

La base de datos se crea con las migraciones de Entity Framework Core (carpeta `Migrations/`).

- **Visual Studio** (Consola del Administrador de paquetes):
  ```
  Update-Database
  ```
- **CLI de .NET**:
  ```bash
  dotnet tool install --global dotnet-ef
  dotnet ef database update
  ```

Para generar una migración nueva después de cambiar los modelos: `Add-Migration NombreDeLaMigracion` y luego `Update-Database`.

## Estructura del proyecto

```
Controllers/   Controladores MVC (Home, Equipos, Jugadores, ...)
Models/        Entidades que corresponden al DER
ViewModels/    Modelos específicos para formularios y vistas
Data/          TorneoEsportsContext (DbContext y relaciones con Fluent API)
Views/         Vistas Razor (.cshtml) por controlador, y Shared/ con el layout
Migrations/    Migraciones de la base de datos
```

## Modelo de datos

- **Equipo** 1:N **Jugador**
- **Torneo** 1:N **Partida**
- **Sede** 1:N **Partida**
- **Torneo** N:N **Equipo** (tabla `Participa`)
- **Partida** N:N **Arbitro** (tabla `Arbitraje`)
- **Equipo** N:N **Partida** con atributo `Resultado` (entidad `EnfrentamientoEquipoPartida`)

## Reportes

_(completar cuando estén implementados: nombre de cada reporte, filtros disponibles y su utilidad para los usuarios.)_

1. **Reporte 1:** _pendiente_
2. **Reporte 2:** _pendiente_

### Aportes - Andrés Santiago Rojas Coria (GitHub: `santiprod1`)
* CRUD de Sedes y Árbitros con validación restrictiva de borrado relacional.
* CRUD de Partidas con validación de horarios, disponibilidad de recintos y cruce de equipos.
* Reporte 2: Calendario de partidas. Permite filtrar dinámicamente por torneo, sede y fechas, incluyendo totalizadores estadísticos mediante agrupaciones LINQ.