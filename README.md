# Project Overview
This project demonstrates a microservices architecture with OpenTelemetry for observability, integrating several components:

 - **PocApi, PocConsumer and PocProducer**: Proof of Concept applications for demonstrating API and message consumption functionalities.
 - **SqlServer**: A SQL Server instance for database services.
 - **Otel-Collector**: An OpenTelemetry Collector for telemetry data collection and export.
 - **Elasticsearch**: A search and analytics engine for storing and querying telemetry data.
 - **RabbitMQ**: A message broker for asynchronous messaging between services.

## Prerequisites
 - **Docker**
 - **Docker Compose**

Ensure Docker and Docker Compose are installed on your system. Visit Docker's official documentation and Docker Compose documentation for installation guides.

## Project Structure
- **PocApi/**: Contains the Dockerfile and source code for the API application.
- **PocConsumer**/: Contains the Dockerfile and source code for the consumer application.
- **microDB**/: Contains the Dockerfile and initialization scripts for setting up SqlServer.
- **otel-collector-config.yml**: Configuration file for the OpenTelemetry Collector.
- **docker-compose.yml**: Orchestrates the setup and networking of all services.
- **kibana.yml**: Initiates kibana with fleet a other configurations.

## Setup and Running
### Clone the Repository
Clone this repository to your local machine:
```sh
git clone https://github.com/joe-kiang/NetOtelPoc.git
```
### Start the Services
Navigate to the project directory and start all services with Docker Compose:

```sh
docker-compose up --build
```
This command builds the images for each component and starts the containers.

### Accessing the Services
 - PocApi: Accessible at http://localhost:8860/swagger
 - PocConsumer: Communicates with RabbitMQ and SqlServer internally.
 - Elasticsearch: Accessible at https://localhost:5601
   - Login with the elastic username and the password with the credentials specified in docker-compose.yml
 - RabbitMQ Management Console: Accessible at http://localhost:15672
   - Login with user guest and pass guest
 - SQL Server: Connect using SQL clients at localhost:1433 with the credentials specified in docker-compose.yml.
 - OpenTelemetry Collector: Receives telemetry data on various endpoints configured in otel-collector-config.yml.
   
### Customization
Modify the docker-compose.yml and component-specific configuration files as needed to adapt to your environment or requirements.

### Stopping the Project
To stop and remove all running services, use:
```sh
docker-compose down
```
