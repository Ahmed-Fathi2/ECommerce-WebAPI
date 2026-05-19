# E-Commerce Web API — Production-Ready Cloud Native Backend

A production-ready E-Commerce Web API built with ASP.NET Core and Clean Architecture principles.  
This project started as an ITI assignment and evolved into a complete cloud-native backend ecosystem by integrating Microsoft Azure services, observability tools, distributed caching, DevOps automation, and secure payment processing.

---

# Live Demo

- Swagger UI: https://ecommerce-api-service-g2d2g7fwhub3dphe.spaincentral-01.azurewebsites.net/Swagger/index.html
- GitHub Repository: https://github.com/Ahmed-Fathi2/ECommerce-WebAPI-Azure.git

---

# Cloud & DevOps Highlights

## Microsoft Azure Integration

- Azure App Service Deployment
- Azure SQL Database
- Azure Blob Storage
- Azure Application Insights

## Storage System

Implemented a flexible storage abstraction supporting:

- Local File Storage
- Azure Blob Storage

## Local Cloud Development

- Azurite (Azure Storage Emulator)

Used Azurite to develop and test Azure Blob Storage features locally before deploying to Azure.

## Monitoring & Observability

Implemented structured logging and production monitoring using:

- Serilog
- Seq (Docker)
- Azure Application Insights

## Performance Optimization

Implemented multi-layer caching:

- In-Memory Cache
- Redis Distributed Cache
- Redis running with Docker

## CI/CD Automation

Professional CI/CD pipeline using GitHub Actions:

### Continuous Integration (CI)

- Triggered automatically on Pull Requests
- Automated validation and quality checks

### Continuous Deployment (CD)

- Automatic deployment to Azure after merging

---

# Project Overview

This project was designed to simulate a real-world scalable backend system for modern e-commerce applications.

The focus was not only implementing business logic, but also applying production-grade engineering practices including:

- Cloud deployment
- Distributed caching
- Monitoring & observability
- DevOps automation
- Scalable storage management
- Secure payment processing

---

# Architecture

The project follows Clean Architecture principles to ensure scalability, maintainability, and separation of concerns.

## Design Patterns

- Clean Architecture
- Repository Pattern
- Unit of Work Pattern

---

# Core Features

## Authentication & Authorization

- JWT Authentication
- Role-Based Authorization
- Secure API Endpoints

## Product Management

- Products CRUD
- Categories CRUD
- Image Upload Support

## Order Management

- Create Orders
- Order Tracking
- Status Management

## Payment Integration

- Kashier Payment Gateway Integration
- Secure Online Transactions

---

# Tech Stack

## Backend

- ASP.NET Core Web API
- C#
- Entity Framework Core
- SQL Server

## Architecture

- Clean Architecture
- Repository Pattern
- Unit of Work

## Cloud & DevOps

- Microsoft Azure
- Azure App Service
- Azure SQL Database
- Azure Blob Storage
- Azure Application Insights
- Docker
- GitHub Actions

## Caching

- Redis
- In-Memory Cache

## Logging

- Serilog
- Seq

## Payments

- Kashier Payment Gateway

---

# Project Goal

The goal of this project was to move beyond basic CRUD implementation and build a production-oriented backend ecosystem that applies modern software engineering, cloud computing, and DevOps practices.

---
This project is for educational and portfolio purposes.
