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

