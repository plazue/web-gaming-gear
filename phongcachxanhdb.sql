CREATE DATABASE PhongCachXanhDB;
GO

USE PhongCachXanhDB;
GO

-- 1. Categories Table (For managing categories in the store)
CREATE TABLE Categories (
    CategoryID INT IDENTITY(1,1) PRIMARY KEY,
    CategoryName NVARCHAR(100) NOT NULL,
    Slug NVARCHAR(100) NOT NULL
);

-- 2. Users Table (Supports Customer and Admin roles for login/registration)
CREATE TABLE Users (
    UserID INT IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(50) NOT NULL,
    Email NVARCHAR(100) NOT NULL UNIQUE,
    Password NVARCHAR(255) NOT NULL,
    Role NVARCHAR(20) DEFAULT 'Customer', -- Values: 'Customer' or 'Admin'
    Address NVARCHAR(255) NULL,
    PhoneNumber VARCHAR(20) NULL,
    CreatedAt DATETIME DEFAULT GETDATE()
);

-- 3. Products Table (Stores items displayed in categories and detail pages like SP1-SP4)
CREATE TABLE Products (
    ProductID INT IDENTITY(1,1) PRIMARY KEY,
    CategoryID INT NULL,
    Brand NVARCHAR(100) NOT NULL,
    Title NVARCHAR(255) NOT NULL,
    Price DECIMAL(18,2) NOT NULL,
    SalePrice DECIMAL(18,2) NULL,
    Color NVARCHAR(50) NULL,
    ProductType NVARCHAR(100) NULL,
    Description NVARCHAR(MAX) NULL,
    StockQuantity INT NULL,
    ProductImage NVARCHAR(MAX) NULL,
    ProductImages1 NVARCHAR(500) NULL,
    FOREIGN KEY (CategoryID) REFERENCES Categories(CategoryID)
);

-- 4. Product Images Table (Handles multiple gallery thumbnail images per product)
CREATE TABLE ProductImages (
    ImageID INT IDENTITY(1,1) PRIMARY KEY,
    ProductID INT NOT NULL,
    ImageURL NVARCHAR(MAX) NOT NULL,
    IsThumbnail BIT DEFAULT 0, -- 1 for main showcase image, 0 for extra thumbnails
    FOREIGN KEY (ProductID) REFERENCES Products(ProductID) ON DELETE CASCADE
);

-- 5. Product Specifications Table (For technical specs rows on product detail pages)
CREATE TABLE ProductSpecs (
    SpecKeyID INT IDENTITY(1,1) PRIMARY KEY,
    ProductID INT NOT NULL,
    SpecLabel NVARCHAR(100) NOT NULL,
    SpecValue NVARCHAR(MAX) NOT NULL,
    FOREIGN KEY (ProductID) REFERENCES Products(ProductID) ON DELETE CASCADE
);

-- 6. Orders Table (Checkout and order history tracking)
CREATE TABLE Orders (
    OrderID INT IDENTITY(1,1) PRIMARY KEY,
    UserID INT NULL,
    Subtotal DECIMAL(18,2) NOT NULL,
    OrderNote NVARCHAR(MAX),
    OrderStatus NVARCHAR(50) DEFAULT N'Đã xác nhận',
    PaymentType NVARCHAR(50) DEFAULT ('CASH') NULL,
    CreditCard NVARCHAR(MAX) NULL,
    CardHolderName NVARCHAR(100) NULL,      -- Tên chủ thẻ
    CardNumberLast4 NVARCHAR(4) NULL,       -- 4 số cuối của thẻ (đảm bảo bảo mật)
    CardExpiryDate NVARCHAR(10) NULL,       -- Ngày hết hạn thẻ (MM/YY)
    CardBrand NVARCHAR(50) NULL,            -- Loại thẻ (Visa, MasterCard, JCB,...)
    CreatedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (UserID) REFERENCES Users(UserID)
);

-- 7. Order Details Table
CREATE TABLE OrderDetails (
    OrderDetailID INT IDENTITY(1,1) PRIMARY KEY,
    OrderID INT NOT NULL,
    ProductID INT NOT NULL,
    Quantity INT NOT NULL,
    Price DECIMAL(18,2) NOT NULL,
    FOREIGN KEY (OrderID) REFERENCES Orders(OrderID) ON DELETE CASCADE,
    FOREIGN KEY (ProductID) REFERENCES Products(ProductID)
);
GO