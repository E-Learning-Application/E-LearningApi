# E-Learning API

A comprehensive language learning platform backend that facilitates real-time communication between users for collaborative language practice and learning.

---

## Overview

The E-Learning API serves as the backend infrastructure for a language learning platform that connects users for collaborative language practice. The system provides:

- User management  
- Language matching algorithms  
- Real-time communication via WebSocket  
- Structured learning sessions  
- Content moderation features  

---

## Technology Stack

- **Framework**: ASP.NET Core 9.0 
- **Database**: SQL Server with Entity Framework Core
- **Authentication**: JWT Bearer Tokens
- **Real-time Communication**: SignalR
- **API Documentation**: Swagger/OpenAPI 
- **Object Mapping**: AutoMapper 
- **Containerization**: Docker 

---

## Architecture

The project follows a layered architecture:

- **API Layer**: Controllers, middleware, SignalR hubs  
- **CORE Layer**: Business services, DTOs, AutoMapper profiles  
- **DATA Layer**: EF models, repositories, Unit of Work pattern

---

## Key Features

### User Management
- Registration and authentication via ASP.NET Identity
- Profile management with image uploads  
- Soft delete for user data  

### Language Learning
- Language preference with proficiency levels
- Interest-based user matching  
- Multi-language system support  

### Real-time Communication
- WebSocket-based instant messaging
- Live user matching and status tracking  
- WebRTC signaling for voice/video calls  

### Session Management
- Structured sessions with feedback
- User ratings and comments  
- Match scoring algorithm  

---

## Running with Docker

You can run the E-Learning API using Docker and Docker Compose for easy setup and isolation.

### Prerequisites

- [Docker](https://www.docker.com/get-started)
- [Docker Compose](https://docs.docker.com/compose/)

### Steps to Run

1. **Clone the repository**  
   ```bash
   git clone https://github.com/E-Learning-Application/E-LearningApi.git
2. **Build and start the containers**
   
    From the root directory (where the `docker-compose.yml` file is located), run:
    ```bash
    docker-compose up --build
3. Access the API
   
   Once the containers are up, the API will be available at:
   ```bash
   http://localhost:8080
4. Swagger UI

   Navigate to:
   ```bash
   http://localhost:8080/swagger
   ```
   to explore and test the available API endpoints.
### Notes

- **Database Connection**:  
  The SQL Server instance runs in a separate container and is accessible from the API container using `Server=db`.

- **Environment Variables**:  
  Connection strings and secrets should be managed using environment variables as shown in the `docker-compose.yml`.

- **Volumes**:  
  A named volume `dbdata` is used to persist SQL Server data across container restarts.

- **Ports**:
  - **API**: `8080:8080`
  - **SQL Server**: `1433:1433`

