# 🛒 E-commerce ASP.NET Core MVC Project

This is a fully functional e-commerce web application developed using ASP.NET Core MVC during my internship. The application provides a great user experience for customers and efficient management tools for administrators. The project is built with Entity Framework Core for database management and ASP.NET Core Identity for user authentication and authorization.

---

## 🛠️ Project Overview

- **Project Type:** E-commerce Web Application  
- **Technology Stack:** ASP.NET Core MVC, C#, SQL Server, Entity Framework Core, ASP.NET Core Identity  
- **Purpose:** To create a scalable e-commerce platform with role-based access control (Admin and Customer) and a secure user authentication system.  
- **Status:** Completed

---

## 💡 Key Features

### 🔐 Role-Based User Authentication
- **Admin:** Full control over the product catalog, customer orders, and user management.
- **Customer:** Browse products, add items to the cart, and place orders.

### 📦 Product Management
- Admins can easily add, edit, or delete products from the catalog.
- Products are managed using Entity Framework Core and stored in SQL Server.

### 🛒 Shopping Cart
- Customers can add items to their cart, modify quantities, and proceed to checkout.

### 📑 Order Management
- Customers can view their orders, and Admins can manage and process them.

### 🔐 User Authentication
- Secure user registration, login, and logout using ASP.NET Core Identity.

### 📱 Responsive Design
- Fully responsive UI across devices (desktop, tablet, and mobile) using Bootstrap.

### 🔒 Role-Based Authorization
- Admin and Customer roles have separate access rights to ensure security and proper functionality.

---

## 📁 Technologies Used

- **ASP.NET Core MVC** – Framework used for building the web application and handling routing and views  
- **C#** – Programming language for backend logic  
- **SQL Server** – Database system for storing products, orders, and user data  
- **Entity Framework Core** – ORM for managing database operations  
- **ASP.NET Core Identity** – User authentication and role-based authorization  
- **Razor Views** – For rendering dynamic pages  
- **Bootstrap** – Responsive and mobile-friendly UI  
- **Font Awesome** – Icons used throughout the application  

---

## 📸 Application Screenshots

Here’s a visual walkthrough of the application features from both Admin and Customer perspectives:

### 🔐 Admin Dashboard  
![Admin Dashboard](images/admindashboard.png)



### 🏠 Admin Home
![Admin Home](images/adminview.png)



### 👥 Manage Users  
![Manage Users](images/manageuser.png)



### ➕ Add Product  
![Add Product](images/Uploadproduct.png)



### 📦 Manage Orders  
![Manage Orders](images/manageorders.png)



### 🏠 Customer Home  
![Customer Home](images/customerhome.png)



### 🛍️ Product View & Cart  
![Product View](images/productview.png)  
![Shopping Cart](images/shoppingcart.png)




### 💳 Checkout & Order Summary  
![Checkout](images/checkout.png)  
![Order Summary](images/ordersummary.png)



### 👤 Profile Management  
![Profile Management](images/profilemanage.png)




### 🌟 Featured Section  
![Featured](images/featuredproduct.png)

---
## 🚀 Deployment Notes

This project uses **Razor Views** and follows a **monolithic ASP.NET Core MVC architecture** with server-side rendering.  
Unlike frontend frameworks like React or Angular that support static deployment, this approach requires a full server environment.  

Deployment involves configuring:
- IIS or Kestrel hosting  
- SQL Server setup and migrations  
- ASP.NET Core Identity seeding and role management  
- Proper environment-specific settings in `appsettings.json`  

Due to this complexity, the application is not currently deployed live.  
Instead, detailed screenshots are provided above to demonstrate the complete functionality.
---

## 🚧 Project Status & Roadmap

The project is currently in its initial stages. Here are some future goals:

- 💳 Implement payment gateway (e.g., Stripe, PayPal)  
- 🎨 Improve UI and UX  
- 📊 Add advanced reporting for Admin  
- 🚀 Deploy to a production server (Azure, AWS, etc.)

---

## 🤝 Contributing

Contributions to this project are welcome!  
If you find a bug or have an idea for a feature, feel free to open an issue or submit a pull request.

---

## 🙏 Acknowledgments

- [ASP.NET Core Documentation](https://learn.microsoft.com/en-us/aspnet/core/?view=aspnetcore-7.0)  
- [Entity Framework Core Documentation](https://learn.microsoft.com/en-us/ef/core/)  
- [Bootstrap 5](https://getbootstrap.com/)  
- [Font Awesome](https://fontawesome.com/)

---




