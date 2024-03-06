# Project Overview
This project demonstrates a microservices architecture with OpenTelemetry for observability, integrating several components:

 - PocApi and PocConsumer: Proof of Concept applications for demonstrating API and message consumption functionalities.
 - SqlServer: A SQL Server instance for database services.
 - Otel-Collector: An OpenTelemetry Collector for telemetry data collection and export.
 - Elasticsearch: A search and analytics engine for storing and querying telemetry data.
 - RabbitMQ: A message broker for asynchronous messaging between services.

## Prerequisites
Docker
Docker Compose
Ensure Docker and Docker Compose are installed on your system. Visit Docker's official documentation and Docker Compose documentation for installation guides.

## Project Structure
PocApi/: Contains the Dockerfile and source code for the API application.
PocConsumer/: Contains the Dockerfile and source code for the consumer application.
microDB/: Contains the Dockerfile and initialization scripts for setting up SqlServer.
script.sql: SQL script for creating the microDB database and the Orders table.
otel-collector-config.yml: Configuration file for the OpenTelemetry Collector.
docker-compose.yml: Orchestrates the setup and networking of all services.

## Setup and Running
### Clone the Repository
Clone this repository to your local machine:
```sh
git clone https://github.com/joe-kiang/NetOtelPoc
```
### Start the Services
Navigate to the project directory and start all services with Docker Compose:

```sh
cd path/to/project
docker-compose up --build
```
This command builds the images for each component and starts the containers.

### Accessing the Services
 - PocApi: Accessible at http://localhost:8860
 - PocConsumer: Communicates with RabbitMQ and SqlServer internally.
 - Elasticsearch: Accessible at http://localhost:9200
 - RabbitMQ Management Console: Accessible at http://localhost:15672
 - SQL Server: Connect using SQL clients at localhost:1433 with the credentials specified in docker-compose.yml.
 - OpenTelemetry Collector: Receives telemetry data on various endpoints configured in otel-collector-config.yml.
   
### Customization
Modify the docker-compose.yml and component-specific configuration files as needed to adapt to your environment or requirements.

### Stopping the Project
To stop and remove all running services, use:
```sh
docker-compose down
```
