# API Technical Specification and User Guide

## CREATE DATABASE
- ** This project is using SQL Server to store data.
- ** How to create database first to all in order to run API
- *** Step 0: Open appsettings.json to edit field: DefaultConnection point to alive SQL Server
- *** Step 1: Open CMD Project folder: ..../CLUserManagementAPICLUserManagementAPI
- *** Step 2: Run command line: 
		 dotnet ef migrations add InitialCreate
- *** Step 2: Run command line: 
		 dotnet ef database update
If two above command run was successfully then you can login to SQL Management Studio or any tool that access to SQL Server to check new Data just be created name: UserManagementDb

## Logging
The root folder store logging file is /Logs/logs-[datetime].txt. You can change this value in Program.cs file 

## API Endpoints

### 1. Get User by ID
- **Endpoint**: `GET /api/user/{id}`
- **Description**: Retrieves user information by their ID.
- **Authentication**: Required (JWT Token).
- **Authorization**: All authenticated users.
- **Request**:
  - **Path Parameter**:
  - **Path Parameter**:
    - `id` (integer): The ID of the user to retrieve.
- **Response**:
  - **Success (200 OK)**:
    ```json
		{
			"id": 7,
			"username": "string",
			"email": "user@example.com",
			"password": "$2a$12$swc9Bk.6af8lyiEM38DNve52vBn4FkXjohyCQGDg.QFm8g3NMTcA.",
			"products": []
		}
    ```
  - **Error (404 Not Found)**: User not found.
    ```json
	{
		"type": "https://tools.ietf.org/html/rfc9110#section-15.5.5",
		"title": "Not Found",
		"status": 404,
		"traceId": "00-189e5c9e89e7eb0b93bf65242fd77ac3-bf5ae7e741cb78aa-00"
	}
    ```

---

### 2. Register User
- **Endpoint**: `POST /api/user/register`
- **Description**: Registers a new user.
- **Authentication**: Not required.
- **Authorization**: None.
- **Request**:
  - **Body** (JSON):
    ```json
    {
      "username": "newuser",
      "password": "password123",
      "email": "newuser@example.com"
    }
    ```
- **Response**:
  - **Success (200 OK)**:
    ```json
    {
      "message": "User registered successfully!"
    }
    ```
  - **Error (400 Bad Request)**: Invalid input or username already exists.
    ```json
    {
      "message": "Username already exists."
    }
    ```

---

### 3. Login User
- **Endpoint**: `POST /api/user/login`
- **Description**: Authenticates a user and returns a JWT token.
- **Authentication**: Not required.
- **Authorization**: None.
- **Request**:
  - **Body** (JSON):
    ```json
    {
      "username": "admin",
      "password": "admin123"
    }
    ```
- **Response**:
  - **Success (200 OK)**:
    ```json
    {
      "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
    }
    ```
  - **Error (401 Unauthorized)**: Invalid username or password.
    ```json
    {
      "message": "Invalid username or password."
    }
    ```

---

### 4. Logout User
- **Endpoint**: `POST /api/user/logout`
- **Description**: Logs out the current user by invalidating their JWT token.
- **Authentication**: Required (JWT Token).
- **Authorization**: All authenticated users.
- **Request**:
  - **Headers**:
    - `Authorization`: `Bearer <JWT Token>`
- **Response**:
  - **Success (200 OK)**:
    ```json
    {
      "message": "User logged out successfully!"
    }
    ```

---

# API Technical Specification and User Guide for ProductController

## API Endpoints

### 1. Get Product by ID
- **Endpoint**: `GET /api/product/{id}`
- **Description**: Retrieves product information by its ID.
- **Authentication**: Not required.
- **Authorization**: None.
- **Request**:
  - **Path Parameter**:
    - `id` (integer): The ID of the product to retrieve.
- **Response**:
  - **Success (200 OK)**:
    ```json
    {
      "id": 1,
      "name": "Laptop",
      "description": "High-end laptop",
      "price": 999.99,
      "userId": 1
    }
    ```
  - **Error (404 Not Found)**: Product not found.
    ```json
    {
      "message": "Product not found."
    }
    ```

---

### 2. Create Product
- **Endpoint**: `POST /api/product`
- **Description**: Creates a new product.
- **Authentication**: Not required.
- **Authorization**: None.
- **Request**:
  - **Body** (JSON):
    ```json
    {
      "name": "Laptop",
      "description": "High-end laptop",
      "price": 999.99,
      "userId": 1
    }
    ```
- **Response**:
  - **Success (200 OK)**:
    ```json
    {
      "message": "Product created successfully!"
    }
    ```
  - **Error (400 Bad Request)**: Invalid input or missing fields.
    ```json
    {
      "message": "Invalid input."
    }
    ```

---

### 3. Update Product
- **Endpoint**: `PUT /api/product/{id}`
- **Description**: Updates an existing product by its ID.
- **Authentication**: Not required.
- **Authorization**: None.
- **Request**:
  - **Path Parameter**:
    - `id` (integer): The ID of the product to update.
  - **Body** (JSON):
    ```json
    {
      "name": "Laptop",
      "description": "High-end laptop",
      "price": 999.99,
      "userId": 1
    }
    ```
- **Response**:
  - **Success (200 OK)**:
    ```json
    {
      "message": "Product updated successfully!"
    }
    ```
  - **Error (404 Not Found)**: Product not found.
    ```json
    {
      "message": "Product not found."
    }
    ```

---

### 4. Delete Product
- **Endpoint**: `DELETE /api/product/{id}`
- **Description**: Deletes a product by its ID.
- **Authentication**: Not required.
- **Authorization**: None.
- **Request**:
  - **Path Parameter**:
    - `id` (integer): The ID of the product to delete.
- **Response**:
  - **Success (200 OK)**:
    ```json
    {
      "message": "Product deleted successfully!"
    }
    ```
  - **Error (404 Not Found)**: Product not found.
    ```json
    {
      "message": "Product not found."
    }
    ```

---

### 5. Get Products
- **Endpoint**: `GET /api/product`
- **Description**: Retrieves a list of products, optionally filtered by a search term.
- **Authentication**: Not required.
- **Authorization**: None.
- **Request**:
  - **Query Parameter**:
    - `searchTerm` (string, optional): A term to filter products by name or description.
- **Response**:
  - **Success (200 OK)**:
    ```json
    [
      {
        "id": 1,
        "name": "Laptop",
        "description": "High-end laptop",
        "price": 999.99,
        "userId": 1
      },
      {
        "id": 2,
        "name": "Mouse",
        "description": "Wireless mouse",
        "price": 29.99,
        "userId": 1
      }
    ]
    ```

---
