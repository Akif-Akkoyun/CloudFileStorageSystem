# CloudFileStorageSystem
✅ About This Project – Cloud File Management System
This project is a multi-layered cloud file management system built with a microservices architecture, designed to provide secure file upload, storage, metadata management, and user-based access control. It includes advanced logging, authentication, and role-based authorization features. The system supports both public and private file visibility, user-to-user file sharing, and centralized logging using Serilog.
ChatGPT:
Evet, tüm süreci hatırlıyorum. Projen çok katmanlı, mikroservis mimarisiyle inşa edilmiş bir dosya yönetim sistemi. Hem teknik olarak güçlü hem de kullanıcı odaklı özellikler içeriyor. Aşağıda sana uygun, detaylı ve profesyonel bir About This Project yazısı hazırladım:

✅ About This Project – Cloud File Management System
This project is a multi-layered cloud file management system built with a microservices architecture, designed to provide secure file upload, storage, metadata management, and user-based access control. It includes advanced logging, authentication, and role-based authorization features. The system supports both public and private file visibility, user-to-user file sharing, and centralized logging using Serilog.

🔧 Technologies & Tools Used
.NET 8 – Entire backend infrastructure built using ASP.NET Core with latest LTS version

MediatR – Applied CQRS pattern for clean separation of commands and queries

Entity Framework Core – Used with both SQLite and PostgreSQL in different services

JWT (JSON Web Tokens) – Secured access to all APIs with authentication & role-based authorization

Serilog – Integrated structured, filtered logging in all services with per-event granularity

ASP.NET Core MVC – Used for the frontend client interface (Admin & Main UI)

HttpClient & IHttpClientFactory – For inter-service communication via REST

YARP (Yet Another Reverse Proxy) – Gateway routing and request forwarding

AutoMapper – For mapping between DTOs, ViewModels, and Entities

Bootstrap – Used for responsive and modern UI design in views

FluentValidation – Ensured input validation across DTOs

PostgreSQL & SQLite – Used independently in services (e.g. FileMetaData with PostgreSQL)

Logging – All services log only meaningful events with route-based filtering via Serilog

🧩 Architecture Overview
Auth.Api
Handles user registration, login, password reset and token generation.
Uses SQLite + JWT + Serilog.

FileMetaData.Api
Stores metadata for uploaded files: description, visibility, owner, share info.
Uses PostgreSQL + MediatR + Serilog + Role-based Authorization.

FileStorage.Api
Handles the actual upload/download of physical files to disk.
Uses simple file system logic with validation and logging.

Gateway.Api
Central entry point using YARP for all routing.
Logs every request with success, failure, and unmatched route details.

Main.Mvc & Admin.Mvc
MVC applications where users can log in, view their files, upload/download, manage visibility, and share files.
Communicates with the APIs using HttpClient and passes the JWT token securely.

✅ Core Features
Secure user authentication (JWT) and password recovery

File uploading with auto-renaming and download support

Public/Private visibility toggles

File sharing with selected users and permission control

Real-time Serilog logging for observability and auditing

Clean architecture with proper separation of concerns and repositories

Well-structured codebase ready for scaling and deployment

💡 Why This Project?
The goal was to build a scalable, secure, and cleanly architected file storage system suitable for both personal and organizational use. Emphasis was placed on:

Security via token-based access

Extensibility via MediatR and CQRS

Observability with filtered logging

Maintainability via layered architecture and clear code boundaries

