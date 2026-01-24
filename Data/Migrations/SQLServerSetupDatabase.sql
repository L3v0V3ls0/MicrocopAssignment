-- Create Users table
CREATE TABLE Users (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    UserName NVARCHAR(50) UNIQUE NOT NULL,
    FullName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(254) UNIQUE NOT NULL,
    MobileNumber NVARCHAR(20) NULL,
    Language NVARCHAR(3) DEFAULT 'en',
    Culture NVARCHAR(5) DEFAULT 'en-US',
    PasswordHash NVARCHAR(256) NOT NULL,
    CreatedAt DATETIME2 DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2 DEFAULT GETUTCDATE(),
    IsActive BIT DEFAULT 1
);

-- Create ApiClients table
CREATE TABLE ApiClients (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    ClientName NVARCHAR(100) NOT NULL,
    ApiKeyhash NVARCHAR(256) UNIQUE NOT NULL,
    IsActive BIT DEFAULT 1,
    CreatedAt DATETIME2 DEFAULT GETUTCDATE()
);
GO
-- 1. Unique constraints (most important)
CREATE UNIQUE INDEX UIX_Users_UserName ON Users(UserName) WHERE IsActive = 1;
CREATE UNIQUE INDEX UIX_Users_Email ON Users(Email) WHERE IsActive = 1;
GO

-- 2. Performance indexes for common queries
CREATE INDEX IX_Users_IsActive ON Users(IsActive);
CREATE UNIQUE INDEX IX_ApiClients_ApiKey ON ApiClients(ApiKey);
GO