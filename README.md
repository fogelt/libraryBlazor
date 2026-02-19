# Library Solution

A .NET 9 library management system built with C# using clean architecture principles. Manage books, DVDs, and magazines with a responsive Blazor UI, SQLite database, and features for searching, borrowing, and member tracking.

## Quick Start

### Prerequisites
- .NET 9 SDK
- SQLite (database file created automatically)

### Run the Application

Navigate to the Library.Web directory and start the development server:

```bash
cd Library.Web
dotnet watch
```

The application will open at `https://localhost:5023` with a responsive Blazor interface.

### Run Tests

```bash
dotnet test
```

## Features

- **Item Management**: Books, DVDs, and Magazines with availability tracking
- **Member Management**: Register and track library members
- **Borrowing System**: Borrow and return items with loan history
- **Search & Filtering**: Search library items by title, author, or ISBN
- **Responsive UI**: Works seamlessly on desktop and mobile devices

## Project Structure

- **Library.Core**: Business logic, models, and interfaces
- **Library.Data**: Data persistence layer with SQLite integration
- **Library.Web**: Responsive Blazor UI
- **Library.Tests**: Comprehensive unit tests

## Database Model

The system uses SQLite with the following main entities:

![Database structure](Library.Web/wwwroot/images/screenshots/DBtables.png)

- **LibraryItem**: Base class for Books, DVDs, and Magazines with properties like Title, Author, ISBN, Availability
- **Member**: Represents library members with name and contact information
- **Loan**: Tracks borrowed items with borrow date, due date, and return date

The database is automatically created and seeded with sample data on first run.
(example library.db is included in the repo)

## Screenshots

### Home Page
![Home Page](Library.Web/wwwroot/images/screenshots/homepage.png)

### Catalog
![Catalog Page](Library.Web/wwwroot/images/screenshots/catalogpage.png)

### Loans
![Loans Page](Library.Web/wwwroot/images/screenshots/loanspage.png)

### Members
![Members Page](Library.Web/wwwroot/images/screenshots/memberpage.png)

### Add Item
![Add Item Form](Library.Web/wwwroot/images/screenshots/newitemform.png)

### New Loan
![New Loan Form](Library.Web/wwwroot/images/screenshots/newloanform.png)

### Add Member
![Add Member Form](Library.Web/wwwroot/images/screenshots/newmemberform.png)
