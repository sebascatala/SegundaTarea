# Practica #2

# Table Of Content
1. [Overview](#overview)
2. [Installation & Steps](#installation--steps)
3. [Project Stucture](#Project-Structure)
4. [Business Logic](#Busines-Logic)
5. [API Endpoints](#API-Endpoints) 
6. [Metodología: Los 12 Factores](#como-se-usaron-los-12-factores)

# OverView
- Esta API permite la gestión de ciudadanos mediante un CRUD completo.
- Integra servicios externos para la asignación de activos personales y persistencia en archivos CSV.
- Genera tipo de sangre y activos personales de forma aleatoria.
- El proyecto está construido bajo los principios de aplicaciones nativas de la nube.


# Installation & Steps
## Step 1 Clone Repositori
- En el bash: git clone [https://github.com/sebascatala/SegundaTarea]
- cd CitizenApi

## Step 2 Restaurar y Ejecutar
- dotnet restore
- dotnet run


# Project Structure
- /Controllers: Contiene CitizenController.cs, donde reside la lógica de los endpoints de la API.
- /Models: Definición de las entidades del sistema:
- Citizen.cs: Modelo principal del ciudadano.
- PersonalAsset.cs: DTO para el mapeo de activos desde la API externa.
- /Services: PersonalAssetService.cs encargado del consumo de servicios externos mediante HttpClient.
- /Utils: CsvHelper.cs que encapsula la lógica de lectura y escritura de archivos CSV.
- /Properties: Configuración de lanzamiento (launchSettings.json).
- appsettings.json: Archivo central de configuración y variables de entorno.
- citizens.csv: Base de datos en archivo plano.
- log-{date}.log: Archivos de registro generados por Serilog.


# Business Logic
- La API automaticamente quetiona campos especificos al crear un nuevo ciudadano:
- **GetRandomBloodType**:  
    Asigna de forma aleatoria uno de los 8 tipos de sangre humanos (A+, O-, etc.) al momento de la creación.

- **GetRandomPersonalAsset**:  
  - Se conecta de forma asíncrona a una API externa de objetos.  
  - Recupera la lista de activos disponibles.  
  - Selecciona uno al azar para vincularlo al ciudadano, garantizando que cada registro tenga un activo del mundo real.


# API Endpoints
- Puesto que la API usa Swagger, es accesible en http://localhost:9070/swagger

| Método | Endpoint              | Descripción 
|--------|---------------------- |------------
| GET    | `/api/citizens`       | Retorna la lista de todos los ciudadanos registrados en el CSV. 
| GET    | `/api/citizens/{ci}`  | Busca y retorna un ciudadano específico por su número de CI. 
| POST   | `/api/citizens`       | Crea un ciudadano. Necesita `Ci`,`FirstName`,`LastName`. El sistema asigna `BloodType` y `PersonalAssets`.
| PUT    | `/api/citizens/{ci}`  | Actualiza los datos de un ciudadano existente identificado por su CI.
| DELETE | `/api/citizens/{ci}`  | Elimina permanentemente a un ciudadano del registro CSV.

# Como se usaron los 12 factores
## 1. Code Base
- Para cumplir con un codigo base, se creo un unico repositorio en GitHub con flujo de trabajo basado en ramas.
- Se uso un versionamiento en Git con Tres ramas: main, develop, P2-001-citizen-entity.

## 2. Dependencias
- Se utilizo NuGet para gestionar las dependencias externas.
- Serilog para la gestion de Logs.
- Swagger para ver el post, put, delete, update y que se autogeneren endpoints.
- Se almacenaron en appsettings.json.

## 3. Configuration
- Los parametros que pueden ser sensibles se almacenaron en el archivo appsettings.json.
- Rutas a CSV.
- Rutas URI.
- Rutas de servicios externos.

## 4. Backing Services
- En concreto se uso un servicio externo para activos personales.
- Esta en PersonalAssetService, y se trata de un recurso adjunto.
- Esto permite cambiar la URL sin modificar la logica principal.

## 5. Build/Release/Run
- El ciclo de vida esta separado.
- dotnet build para contruccion.
- dotnet run para la ejecucion.

## 6. Proceso/State Less
- Se utilizo el archivo citizen.csv para evitar estados.
- Al no tener un array que se reinice con cada refresh evitamos ser StateFull.
- Los datos no se guardan en memoria RAM.

## 7. Port Binding
- En el archivo launchSettings.json se establecio como puerto de salida el 9070.

## 8. Develop/ Production
- Se mantiene la paridad usando appsettings.Develop.json y appsettings.json
- Esto para que el entorno local sea lo mas parecido al de produccion.

## 9. Registro de Errores
- Se utilizo Serilog para tratar los log como flujos de eventos.
- Con ello se permitio depurar acciones en tiempo real.

## 10. Admin Proces
- Las tareas administrativas y de mantenimento se gestionan a traves de la documentacion.
- El manejo de errores y monitoreo se realiza mediante archivos de log generados por Serilog.
- Esto permite auditar el comportamiento del sistema sin afectar la ejecución de la API.

## 11. Disponibilidad
- Se implementaron procesos asincrónicos mediante "async" y  "await" para mejorar la respuesta de la API.
- Esto mejora la escalabilidad y evita cuellos de botella cuando se consumen servicios externos.

## 12. Concurrencia
- La aplicación soporta concurrencia mediante el uso de programación asincrónica.
- Esto permite manejar múltiples solicitudes al mismo tiempo sin necesidad de múltiples hilos explícitos.
- Aunque no se implementa paralelismo directo, la API puede atender varias operaciones de entrada o salida de forma concurrente.
