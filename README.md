# E-Commerce Platform

A robust, full-featured E-Commerce API and Web Platform built using ASP.NET Core, EF Core, and Clean Architecture.

## 🚀 Features Implemented

### 1. Architecture & Core
* **Clean Architecture & N-Tier Design:** Separation of concerns across Domain, DAL, BLL, API, and Common layers.
* **Unit of Work & Repository Pattern:** Centralized data access and atomic database transactions.
* **Standardized API Responses:** Consistent `ApiResponse<T>` wrapper for all endpoints.

### 2. Security & Authentication
* **JWT Authentication:** Secure user login and registration.
* **Policy-Based Authorization:** Strict access control based on user roles (`Admin` vs `User`).

### 3. Product & Category Management
* **Full CRUD Operations:** Manage products and categories efficiently.
* **Media Management:** Image upload functionality with real-time preview and drag-and-drop support.
* **Role-Based UI:** Administrative table views for staff and modern, responsive card-based grid layouts for customers.

### 4. Shopping Cart
* **User-Scoped Carts:** Secure cart management (Add, Update, Remove) tied to individual user accounts.

### 5. Order & Shipping Management
* **Order Lifecycle:** Complete flow from placing an order to fulfillment.
* **Shipping Workflow:** Tracking number and carrier assignments with read-only validation states once shipping starts.
* **Customer UI:** Responsive order status progress indicators and dynamic status filtering.

### 6. Payment & Kashier Integration 💳
* **Kashier Payment Gateway:** Full checkout integration with Kashier for secure online payments.
* **Refund System:** Integrated Kashier Refund API to handle both full and partial order refunds seamlessly from the admin dashboard.

---
*This README reflects the currently completed and functional features of the project.*