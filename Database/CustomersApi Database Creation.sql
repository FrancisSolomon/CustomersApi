USE [CustomersApi]
GO

/****** Object:  Table [dbo].[Customer]    Script Date: 2019-09-30 21:33:34 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Customer](
	[CustomerId] [bigint] IDENTITY(1,1) primary key NOT NULL,
	[Name] nvarchar(100) NOT NULL,
	IsActive bit not null default 1,
	[LastOrderDate] [datetime2](7) NULL,
	[TotalOrderValue] [money] NULL,
	[NumberOfOrders] [bigint] NOT NULL default 0
)
GO

create table dbo.Contact (
	ContactId bigint identity(1,1) not null primary key,
	FirstName nvarchar(50) not null,
	LastName nvarchar(50) not null,
	CustomerId bigint not null,
	ContactType nvarchar(50) not null,
	constraint FK_Customer_CustomerId foreign key (CustomerId) references dbo.Customer(CustomerId)
)
go

create table dbo.Address (
	AddressId bigint identity(1, 1) not null primary key,
	StreetAddress nvarchar(200) not null,
	City nvarchar(50) not null,
	[State] nvarchar(50) not null,
	PostalCode nvarchar(50) not null,
	Country nvarchar(50) not null,
	ContactId bigint not null,
	constraint FK_Contact_ContactId foreign key (ContactId) references dbo.Contact(ContactId)
)
go
