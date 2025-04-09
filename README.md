# SimpleSocialMediaApp 🧑‍🤝‍🧑

A simple social media platform built with ASP.NET Core and Entity Framework Core. Users can create posts, add friends, chat in real-time, and more.

---

## 🚀 Features

- 👤 User registration and login (Identity)
- 📝 Create, edit, delete posts
- 💬 Add comments to posts
- ❤️ Like posts
- 🖼️ Upload media (profile pictures & post images/videos)
- 🧑‍🤝‍🧑 Send and manage friend requests
- 🔍 Search users and chats
- 🔔 Notifications system
- 💬 Real-time chat with SignalR
- 👥 Manage chats (create and participate in group chats, add/remove users)
- 📜 infinite scroll
- 🛠️ Manage user profile (edit personal info, update picture)

---

## 🛠️ Tech Stack

### 🧩 Backend
- **ASP.NET Core MVC** – Core framework for building the web application following the MVC architectural pattern.
- **Entity Framework Core (EF Core)** – ORM for data access with:
  - **Code-First Approach** – Database schema is created from models.
  - **Migrations** – Database changes are tracked and updated incrementally.
  - **Microsoft SQL Server** – Primary relational database provider.

### 📦 Data & Models
- **Models** – Represent users, posts, comments, media, friendships, chats, etc.
- **Custom Seeder** – Seeds initial data like posts and users.
- **Bogus** – Generates realistic fake data (e.g., names).
- **Genderize API** – Determines gender based on first name for seeding.

### ⚙️ Architecture
- **Service Layer** – Decouples business logic from controllers.
  - Includes services for users, posts, media, chats, friendships, etc.
- **Custom Mapper** – Converts between domain models and view models.
- **Cloudinary Integration** – For media storage and management.
- **SignalR** – Real-time messaging system used for chat features.

### 💡 Features & UI
- **Infinite Scroll** – Smooth loading of additional content as user scrolls.
- **Search Functionality** – For discovering users by name.
- **Data Validation** – Both client-side and server-side validations implemented.

### 🧪 Testing
- **NUnit** 
- **Moq** 
- **In-Memory Database** 

### 📄 License
- **MIT License** – Open-source and free to use.
