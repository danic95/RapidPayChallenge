# RapidPayChallenge

## Brief Description
RapidPayChallenge is a .NET Core application developed in C#. It includes an authentication system and a card management system. The application uses a Code-First approach to set up the database.

## Introduction
This application is designed to provide a simple authentication system along with basic card management functionality. It includes two main controllers:
- **AuthController**: Responsible for user authentication.
- **CardMngrController**: Manages card-related operations such as creating a new card, making payments, and checking card balances.

## Usage
### Authentication
To authenticate a user and obtain an access token, send a POST request to `/access-token` with the user's credentials (`User` and `Pass`) in the request body. The response will contain an access token and its expiration date.

Example request:
POST /access-token
Content-Type: application/json

{
  "User": "testuser",
  "Pass": "testpassword"
}


### Card Management
#### Create a New Card
To create a new card, send a POST request to `/CardMngrController/CreateCard` with the card details in the request body. This endpoint requires authentication and a valid JWT token in the request headers.

Example request:
POST /CardMngrController/CreateCard
Content-Type: application/json
Authorization: Bearer [JWT token]

{
  "Number": "1234567890123456",
  "Balance": 100,
  "CVC": "123",
  "ExpMonth": 12,
  "ExpYear": 2024,
  "Account": {
    "Email": "test@test.com",
    "FirstName": "John",
    "LastName": "Doe",
    "Pass": "password"
  }
}


#### Process Payment
To make a payment, send a PUT request to `/CardMngrController/Payment` with the card number and the amount to be paid in the request body. This endpoint also requires authentication and a valid JWT token.

Example request:
PUT /CardMngrController/Payment
Content-Type: application/json
Authorization: Bearer [JWT token]

{
  "Number": "1234567890123456",
  "Amount": 50
}


#### Check Card Balance
To check the balance of a card, send a GET request to `/CardMngrController/Balance/{cardNum}` where `{cardNum}` is the card number. This endpoint also requires authentication and a valid JWT token.

Example request:
GET /CardMngrController/Balance/1234567890123456
Authorization: Bearer [JWT token]

### Database Setup
The application uses Entity Framework Core with a Code-First approach to set up the database. To set up the database:
1. Open the `appsettings.json` file and configure your database connection string.
2. Simply run the application. Entity Framework Core will take care of creating the necessary database tables based on the application's models.

That's it! Your database is now set up and ready to use.

