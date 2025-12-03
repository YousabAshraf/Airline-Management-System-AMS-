# ✈️ AMS Airlines - Airline Management System

![AMS Airlines Banner](Airline Management System (AMS)\readme_banner.png)

## 📋 Overview

**AMS Airlines** is a comprehensive, professional airline management system built with ASP.NET Core MVC. The platform provides a complete solution for managing flights, bookings, passengers, and user accounts with a modern, trustworthy user interface designed specifically for the airline industry.

## 🎨 Design Philosophy

Our design follows a professional airline industry aesthetic with:
- **Deep Navy Blue** (#002244) - Trust and reliability
- **Sky Blue** (#0077CC) - Aviation and freedom
- **Warm Orange** (#FF6600) - Energy and call-to-action
- **Clean Neutrals** - White and light gray for readability

The UI/UX is crafted to inspire confidence and provide a seamless booking experience.

## ✨ Key Features

### 🔐 Authentication & Authorization
- **User Registration** with email verification
- **Secure Login** with "Remember Me" functionality
- **Password Recovery** with verification codes
- **Role-Based Access Control** (Admin, User)
- **ASP.NET Core Identity** integration

### ✈️ Flight Management
- **Flight Search** with advanced filters
  - Origin and destination selection
  - Date range picker
  - Passenger count
  - Round trip / One-way options
- **Flight Listings** with real-time availability
- **Destination Browsing** with featured locations

### 🎫 Booking System
- **Easy Booking Process** with step-by-step flow
- **Booking Management** - View, modify, cancel
- **Booking History** with detailed information
- **E-Ticket Generation** (planned)

### 👤 User Dashboard
- **Personalized Welcome** with user statistics
- **Travel Stats** - Bookings, trips, destinations
- **Quick Actions** - Search, book, manage
- **Profile Management** - Update personal information
- **Payment Methods** management (planned)

### 🎯 Admin Dashboard
- **Flight Management** - CRUD operations
- **Passenger Management** - View and manage passengers
- **Booking Overview** - Monitor all bookings
- **System Analytics** (planned)

### 🌍 Additional Features
- **Popular Destinations** showcase
- **Services Section** highlighting airline benefits
- **Customer Testimonials**
- **Responsive Design** - Mobile, tablet, desktop
- **Smooth Animations** and transitions
- **Professional Forms** with validation

## 🛠️ Technology Stack

### Backend
- **Framework:** ASP.NET Core 8.0 MVC
- **Language:** C# 12
- **ORM:** Entity Framework Core
- **Database:** SQL Server
- **Authentication:** ASP.NET Core Identity

### Frontend
- **HTML5** - Semantic markup
- **CSS3** - Custom styling with modern features
- **JavaScript (ES6+)** - Interactive functionality
- **Bootstrap 5** - Responsive grid system
- **Font Awesome** - Icon library
- **Google Fonts** - Inter font family

### Architecture
- **MVC Pattern** - Model-View-Controller
- **Repository Pattern** (planned)
- **Dependency Injection**
- **ViewModels** for data transfer

## 📁 Project Structure

```
Airline Management System (AMS)/
├── Controllers/
│   ├── AccountController.cs      # Authentication & authorization
│   ├── HomeController.cs          # Home page & search
│   ├── AdminDashboardController.cs
│   ├── UserDashboardController.cs
│   └── [Other Controllers]
├── Models/
│   ├── ApplicationUser.cs         # Extended Identity user
│   ├── Flight.cs                  # Flight entity
│   ├── Booking.cs                 # Booking entity
│   ├── Passenger.cs               # Passenger entity
│   └── [Other Models]
├── ViewModels/
│   ├── LoginViewModel.cs
│   ├── RegisterViewModel.cs
│   ├── FlightSearchViewModel.cs
│   └── [Other ViewModels]
├── Views/
│   ├── Home/
│   │   └── Index.cshtml           # Landing page
│   ├── Account/
│   │   ├── Login.cshtml
│   │   ├── Register.cshtml
│   │   ├── ForgetPassword.cshtml
│   │   └── ResetPassword.cshtml
│   ├── UserDashboard/
│   │   └── Index.cshtml
│   ├── AdminDashboard/
│   │   └── Index.cshtml
│   └── Shared/
│       └── _Layout.cshtml         # Main layout
├── wwwroot/
│   ├── css/
│   │   ├── home.css               # Home page styles
│   │   └── site.css
│   ├── js/
│   │   └── home.js                # Home page interactivity
│   ├── images/
│   │   └── destinations/          # Destination images
│   └── lib/                       # Client libraries
├── Data/
│   └── ApplicationDbContext.cs    # EF Core context
└── Services/
    └── EmailSender.cs             # Email service
```

## 🚀 Getting Started

### Prerequisites
- **.NET 8.0 SDK** or later
- **SQL Server** (LocalDB, Express, or Full)
- **Visual Studio 2022** or **VS Code**
- **Git** (optional)

### Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/yourusername/airline-management-system.git
   cd airline-management-system
   ```

2. **Update Connection String**
   
   Edit `appsettings.json`:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=AirlineManagementDB;Trusted_Connection=true;MultipleActiveResultSets=true"
     }
   }
   ```

3. **Apply Database Migrations**
   ```bash
   dotnet ef database update
   ```

4. **Run the Application**
   ```bash
   dotnet run
   ```

5. **Access the Application**
   
   Open your browser and navigate to:
   - `https://localhost:5001` (HTTPS)
   - `http://localhost:5000` (HTTP)

## 📊 Database Schema

### Main Entities

**ApplicationUser** (extends IdentityUser)
- FirstName, LastName
- Email (from Identity)
- Role management

**Flight**
- FlightNumber (unique)
- Origin, Destination
- DepartureTime, ArrivalTime
- AvailableSeats, Price
- Status

**Booking**
- BookingReference (unique)
- UserId (FK to ApplicationUser)
- FlightId (FK to Flight)
- BookingDate, Status
- TotalAmount

**Passenger**
- FirstName, LastName
- PassportNumber (unique)
- DateOfBirth, Nationality
- Email, Phone

**Seat**
- SeatNumber
- FlightId (FK to Flight)
- Class (Economy, Business, First)
- IsAvailable

## 🎯 Features Roadmap

### ✅ Completed
- [x] User authentication and authorization
- [x] Professional UI/UX with airline theme
- [x] Home page with flight search
- [x] User dashboard
- [x] Admin dashboard structure
- [x] Responsive design
- [x] Form validation

### 🚧 In Progress
- [ ] Flight CRUD operations
- [ ] Booking system implementation
- [ ] Passenger management

### 📅 Planned
- [ ] Payment integration (Stripe)
- [ ] Seat selection UI
- [ ] E-ticket generation (PDF)
- [ ] Email notifications
- [ ] Multi-language support
- [ ] Flight status tracking
- [ ] Advanced search filters
- [ ] Loyalty program
- [ ] Mobile app (future)

## 🎨 UI/UX Highlights

### Color Palette
```css
/* Primary Colors */
--deep-navy: #002244;
--lighter-navy: #004488;

/* Secondary Colors */
--sky-blue: #0077CC;
--very-light-blue: #E1F0FA;

/* Accent Colors */
--warm-orange: #FF6600;
--darker-orange: #CC5200;

/* Neutrals */
--white: #FFFFFF;
--light-gray: #F5F7FA;
--dark-text: #1A1A1A;
--medium-text: #4A4A4A;
--border: #E1E4E8;
```

### Typography
- **Font Family:** Inter (Google Fonts)
- **Headings:** Bold, 700-800 weight
- **Body:** Regular, 400 weight
- **Buttons:** Semi-bold, 600 weight

### Components
- **Cards:** Clean white with subtle shadows
- **Buttons:** Orange primary, blue secondary
- **Forms:** Light gray inputs with blue focus
- **Animations:** Smooth transitions and hover effects

## 🔒 Security Features

- **Password Hashing** with ASP.NET Core Identity
- **Email Verification** for new accounts
- **CSRF Protection** on all forms
- **SQL Injection Prevention** via EF Core
- **XSS Protection** with Razor encoding
- **Secure Password Reset** with verification codes
- **Role-Based Authorization** for admin features

## 📱 Responsive Design

The application is fully responsive and optimized for:
- **Desktop** (1920px and above)
- **Laptop** (1366px - 1919px)
- **Tablet** (768px - 1365px)
- **Mobile** (320px - 767px)

## 🧪 Testing

### Manual Testing
- User registration and login flows
- Flight search functionality
- Form validation
- Responsive design across devices
- Browser compatibility (Chrome, Firefox, Edge, Safari)

### Automated Testing (Planned)
- Unit tests for business logic
- Integration tests for controllers
- End-to-end tests with Selenium

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 👥 Contributors

- **Your Name** - Initial work and development

## 🙏 Acknowledgments

- ASP.NET Core team for the excellent framework
- Bootstrap team for the responsive grid system
- Font Awesome for the icon library
- Google Fonts for the Inter font family
- The open-source community

## 📞 Support

For support, email support@amsairlines.com or open an issue in the GitHub repository.

## 🔗 Links

- **Documentation:** [Coming Soon]
- **Live Demo:** [Coming Soon]
- **API Documentation:** [Coming Soon]

---

**Made with ❤️ for the aviation industry**

*AMS Airlines - Your Journey Begins Here* ✈️
