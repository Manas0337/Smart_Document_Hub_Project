📘 Smart Documentation Hub

A real-time collaborative documentation platform built using React.js, ASP.NET Core Web API, and MySQL, supporting secure authentication, document versioning, backend text extraction, and inline commenting.

🚀 Features

🔐 JWT Authentication (Login, Forgot & Reset Password)

📄 Document Metadata Management

📤 File Upload (PDF, DOCX, TXT)

🧠 Backend Text Extraction & Normalization

🕒 Document Versioning

💬 Inline Comments using Text Indexing

⚡ Real-time Collaboration (SignalR)

👤 User-based Access Control

🛠 Tech Stack

Frontend

React.js

Axios

SignalR Client

Rich Text Editor (Quill / Slate)

Backend

ASP.NET Core Web API (.NET 8)

Entity Framework Core

SignalR

JWT + BCrypt

Database

MySQL

🏗 Architecture
React Frontend
     ↓
ASP.NET Core Web API
     ↓
Service Layer
     ↓
EF Core
     ↓
MySQL


Real-time updates are handled using SignalR.

🔁 Workflow

User logs in (JWT issued)

Document metadata created

File uploaded → text extracted on backend

Document version created

Inline comments mapped via text indices

Real-time collaboration enabled

🔐 Security

JWT-based authorization

Password hashing using BCrypt

Secure, token-based password reset

Ownership enforced at backend

📂 Project Structure
frontend/   → React app
backend/    → ASP.NET Core API

🎓 Highlights

Clean architecture with DTOs & Services

Backend-driven validation & security

Real-time collaboration support

Designed for scalability and maintainability

📌 Future Enhancements

Role-based access

Cloud file storage

AI-based document insights

Advanced collaboration tools

📄 License

Academic / Educational Use
