# Mabo Games 🎮

Full-stack online game store built with ASP.NET Core 8 and Angular.


## Tech Stack

- ASP.NET Core 8
- Entity Framework Core
- SQL Server
- Angular v21
- JWT Authentication
- Stripe Payments

---


## Features

- User authentication (register, login, logout)
- Forgot password flow (email code and security question recovery)
- Storefront with latest/cheapest game sections
- Browse games with search, sorting, genre filtering, and pagination
- Game details page with screenshots, ratings, and reviews display
- Add to cart from game details
- Cart management (remove items, clear cart, save for later)
- Stripe checkout flow (create order + redirect to Stripe + success return)
- User library page with search and genre filters
- Favourites support (add/remove favourites and highlight favourites in library)

### Features To Be Implemented

- Admin dashboard functionality
- Notifications system
- Achievements system
- User profile page
- Light mode
---


## Requirements

- .NET 8 SDK  
- SQL Server (local instance)  
- Node.js  
- Stripe CLI (for payment testing)

---


## Backend Setup

1. Open the **Backend** project in Visual Studio.
2. Run the Project.
3. The database will be created and seeded automatically on startup.

---


## Frontend Setup

1. Open the frontend folder in Visual Studio Code 
2. Open the terminal in the folder and run 
```bash 
npm install
```
```bash 
ng serve
```

Open: http://localhost:4200


## Stripe Setup (Required for Payment Testing)

To test purchases locally, you must configure Stripe in test mode(sandbox).

- Go to: https://stripe.com
- Create a free account
- Switch to **Test Mode** in the Stripe dashboard

---

#### Get Your API Keys

In the Stripe Dashboard settings:

- Go to **Developers → API Keys**
- Copy `Secret key`

Add it to your `appsettings.json` file:

```json
"Stripe": {
  "SecretKey": "sk_test_...",
  "WebhookSecret": "whsec_..."
}
```

---

#### Install Stripe CLI

Download from:
https://stripe.com/docs/stripe-cli

After installing, login:

```bash
stripe login
```

---

#### Start Webhook Forwarding

Run this in a separate terminal:

```bash
stripe listen --forward-to https://localhost:7260/api/webhooks/stripe
```

Stripe will output a webhook secret (`whsec_...`).

Add it to your `appsettings.json` file:

```json
"Stripe": {
  "SecretKey": "sk_test_...",
  "WebhookSecret": "whsec_..."
}
```

---

Now Stripe payments will work locally in test mode.

When entering card info, the test card number is `4242424242424242`


> ⚠️ This project is currently under active development. Some features are still being implemented and improved.
