# ✈️ AMS Airlines - Airline Management System

![AMS Airlines Banner](readme_banner.png)

## 📋 Overview

**AMS Airlines** is a comprehensive, enterprise-grade Airline Management System built with **ASP.NET Core 8.0 MVC**. The platform provides a complete end-to-end solution for managing flights, seamless bookings, passenger check-ins, and administrative operations.

Designed with a focus on **User Experience (UX)** and **Architecture**, it features a reliable, secure, and modern interface tailored for the aviation industry.

---

## 🎨 Design Philosophy

Our design follows a professional airline industry aesthetic, prioritizing trust and clarity:

-   **Deep Navy Blue (#002244)**: Represents professional reliability and trust.
-   **Sky Blue (#0077CC)**: Evokes the freedom of aviation.
-   **Warm Orange (#FF6600)**: Used strategically for energetic calls-to-action.
-   **Clean Neutrals**: White and light gray backgrounds ensure maximum readability.

The interface is fully responsive, ensuring a seamless experience across desktop, tablet, and mobile devices.

---

## ✨ Key Features

### 🔐 Authentication & Security
-   **Secure Ops**: Built on top of **ASP.NET Core Identity**.
-   **Role-Based Access Control (RBAC)**: Distinct portals for **Admins** and **Customers**.
-   **User Registration**: Includes email verification and secure password hashing.
-   **Account Recovery**: "Forgot Password?" flow with verification codes.

### ✈️ Flight Management (Public & Admin)
-   **Advanced Flight Search**: Filter by Origin, Destination, Date, and Passenger Count.
-   **Real-Time Availability**: Search results reflect live seat inventory.
-   **Detailed Listings**: View duration, pricing, and flight status at a glance.
-   **Featured Destinations**: Dynamic showcase of popular routes.

### 🎫 Comprehensive Booking System
-   **Interactive Booking Flow**: Step-by-step wizard from search to confirmation.
-   **Seat Selection**: Visual interface to choose specific seats (Window, Aisle, Extra Legroom).
-   **Pricing Engine**: Automatic calculation of ticket prices based on seat class and selection.
-   **Booking Management**: 
    -   **My Bookings**: Customers can view their entire travel history.
    -   **Modifications**: Users can change seats or update booking details.
    -   **Cancellations**: Automated cancellation flow with inventory comparison.
-   **Email Notifications**: Instant confirmation emails sending branded HTML tickets to passengers upon booking, update, or cancellation.

### 👥 Passenger & Profile Management
-   **User Dashboard**: Personalized hub showing upcoming trips and stats.
-   **Profile Editing**: Update personal details, contact info, and passport data.
-   **Feedback System**: Customers can rate completed flights (1-5 stars) and leave detailed reviews.

### 🎯 Admin Dashboard
-   **Flight Control**: Full CRUD operations for scheduling new flights.
-   **Passenger Oversight**: Search, view, and manage passenger records.
-   **Booking Administration**: Admins can override, edit, or cancel any user booking.
-   **Feedback Monitoring**: Review customer satisfaction and flight ratings.

---

## 🛠️ Technology Stack

### Backend
-   **Framework**: ASP.NET Core 8.0 MVC
-   **Language**: C# 12
-   **Database**: SQL Server 2019+
-   **ORM**: Entity Framework Core (Code-First approach)
-   **Identity**: ASP.NET Core Identity for auth
-   **Services**: SMTP for Email Notifications

### Frontend
-   **Razor Views**: Server-side rendering for optimal performance.
-   **Styling**: Custom CSS3 variables with a specialized color palette.
-   **Frameworks**: Bootstrap 5 (Grid & Components), Font Awesome 6.
-   **Interactivity**: Vanilla JavaScript (ES6+).

---

## 🚀 Getting Started

Follow these steps to set up the project locally.

### Prerequisites
-   **.NET 8.0 SDK**
-   **SQL Server** (LocalDB or Express)
-   **Visual Studio 2022** (Recommended) or VS Code

### Installation

1.  **Clone the Repository**
    ```bash
    git clone https://github.com/yourusername/airline-management-system.git
    cd airline-management-system
    ```

2.  **Configure Database**
    Open `appsettings.json` and ensure the connection string points to your local SQL Server instance:
    ```json
    "ConnectionStrings": {
      "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=AirlineManagementDB;Trusted_Connection=true;MultipleActiveResultSets=true"
    }
    ```

3.  **Apply Migrations & Seed Data**
    Create the database schema and populate it with initial data (Passengers, Flights, Seats):
    ```bash
    dotnet ef database update
    ```
    *Note: The system includes a seeding script to populate initial flights and seats for testing.*

4.  **Run the Application**
    ```bash
    dotnet run
    ```
    Access the site at `https://localhost:5001` or `http://localhost:5000`.

---

## 📊 Database Schema Highlights

-   **ApplicationUser**: Extends IdentityUser with specific airline roles.
-   **Booking**: The central entity linking Passengers, Flights, and Seats.
-   **Passenger**: detailed profile including Passport and Nationality info.
-   **Seat**: Individual seat inventory with comprehensive status tracking (Available/Booked).
-   **Feedback**: Linked to specific Flights and Users for verified reviews.

---

## 🎯 Features Roadmap

### ✅ Completed
-   [x] **Core Architecture**: MVC setup, EF Core DB, Identity Auth.
-   [x] **Flight Search**: Real-time filtering and availability.
-   [x] **Booking Engine**: Complete flow with **Seat Selection**.
-   [x] **Passenger Portal**: Dashboard, History, Profile Management.
-   [x] **Admin Center**: Flight/Passenger/Booking management.
-   [x] **Communication**: Automated Email Notifications (Confirmation, Updates, Cancellations).
-   [x] **Feedback Loop**: Rating and review system.
-   [x] **UI/UX**: Responsive Airline Themed Design.

### 📅 Planned / Future
-   [ ] **Payment Gateway**: Integration with Stripe/PayPal.
-   [ ] **PDF Generation**: Downloadable E-Tickets.
-   [ ] **loyalty Program**: Miles and rewards system.
-   [ ] **Check-in API**: Mobile app integration endpoints.

---

## 📄 License
This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

---

**Made with ❤️ for the aviation industry.**
*AMS Airlines - Your Journey Begins Here* ✈️
