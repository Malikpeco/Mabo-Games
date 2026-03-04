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

## Requirements

- .NET 8 SDK  
- SQL Server (local instance)  
- Node.js  
- Stripe CLI (for payment testing)

---

## Backend Setup

1. Open the solution file (`MaboGames.sln`) in **Visual Studio**.

2. Go to:
   Tools → NuGet Package Manager → Package Manager Console

3. In Package Manager Console:
   - Set **Default Project** to `Market.Infrastructure`
   - Make sure the **Startup Project** is the API project
4. Run:

```powershell
Update-Database
```

---

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

Add it to your `appsettings.json`:

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

Add it to your configuration:

```json
"Stripe": {
  "SecretKey": "sk_test_...",
  "WebhookSecret": "whsec_..."
}
```

---

Now Stripe payments will work locally in test mode.



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

> ⚠️ This project is currently under active development. Some features are still being implemented and improved.