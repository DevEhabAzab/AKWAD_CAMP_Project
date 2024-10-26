USE [master]
GO
/****** Object:  Database [Sales]    Script Date: 4/18/2022 11:25:41 PM ******/
CREATE DATABASE [Sales]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'Sales', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.MSSQLSERVER\MSSQL\DATA\Sales.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'Sales_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.MSSQLSERVER\MSSQL\DATA\Sales_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [Sales] SET COMPATIBILITY_LEVEL = 140
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Sales].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Sales] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [Sales] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [Sales] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [Sales] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [Sales] SET ARITHABORT OFF 
GO
ALTER DATABASE [Sales] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [Sales] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [Sales] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [Sales] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [Sales] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [Sales] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [Sales] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [Sales] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [Sales] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [Sales] SET  DISABLE_BROKER 
GO
ALTER DATABASE [Sales] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [Sales] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [Sales] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [Sales] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [Sales] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [Sales] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [Sales] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [Sales] SET RECOVERY FULL 
GO
ALTER DATABASE [Sales] SET  MULTI_USER 
GO
ALTER DATABASE [Sales] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [Sales] SET DB_CHAINING OFF 
GO
ALTER DATABASE [Sales] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [Sales] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [Sales] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [Sales] SET QUERY_STORE = OFF
GO
USE [Sales]
GO
/****** Object:  Table [dbo].[Items_Category]    Script Date: 4/18/2022 11:25:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Items_Category](
	[Category_Id] [int] IDENTITY(1,1) NOT NULL,
	[Parent_Id] [int] NULL,
	[Category_Name] [nvarchar](50) NULL,
	[Category_Number] [nvarchar](max) NULL,
	[Category_Desc] [nvarchar](max) NULL,
	[Last_Update_User] [nvarchar](max) NULL,
	[Last_Update_Date] [datetime] NULL,
	[Status_Id] [int] NULL,
	[Begin_Date] [datetime] NULL,
	[End_Date] [datetime] NULL,
	[Active] [bit] NOT NULL,
	[Deleted] [bit] NOT NULL,
 CONSTRAINT [PK_Items_Category] PRIMARY KEY CLUSTERED 
(
	[Category_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Items_Details]    Script Date: 4/18/2022 11:25:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Items_Details](
	[Item_Id] [int] IDENTITY(1,1) NOT NULL,
	[Item_number] [nvarchar](50) NULL,
	[Item_Name] [nvarchar](50) NULL,
	[Item_Desc] [nvarchar](max) NULL,
	[Unit_Id] [int] NULL,
	[Category_Id] [int] NULL,
	[Sub_Category_Id] [int] NULL,
	[Item_Price] [float] NULL,
	[Item_Exp_Date] [date] NULL,
	[Last_Update_User] [nvarchar](max) NULL,
	[Last_Update_Date] [datetime] NULL,
	[Status_Id] [int] NULL,
	[Begin_Date] [datetime] NULL,
	[End_Date] [datetime] NULL,
	[Active] [bit] NOT NULL,
	[Deleted] [bit] NOT NULL,
 CONSTRAINT [PK_Items_Details] PRIMARY KEY CLUSTERED 
(
	[Item_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Product_items]    Script Date: 4/18/2022 11:25:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Product_items](
	[Product_Item_Id] [int] IDENTITY(1,1) NOT NULL,
	[Product_Id] [int] NULL,
	[Item_Id] [int] NULL,
	[Unit_Id] [int] NULL,
	[Quantity] [float] NULL,
	[Price] [float] NULL,
	[Last_Update_User] [nvarchar](max) NULL,
	[Last_Update_Date] [datetime] NULL,
	[Included] [bit] NOT NULL,
 CONSTRAINT [PK_Product_items] PRIMARY KEY CLUSTERED 
(
	[Product_Item_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Products_Details]    Script Date: 4/18/2022 11:25:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Products_Details](
	[Product_Id] [int] IDENTITY(1,1) NOT NULL,
	[Product_Name] [nvarchar](50) NULL,
	[Product_Number] [nvarchar](50) NULL,
	[Product_Desc] [nvarchar](max) NULL,
	[Product_Notes] [nvarchar](max) NULL,
	[Unit_Id] [int] NULL,
	[Last_Update_User] [nvarchar](max) NULL,
	[Last_Update_Date] [datetime] NULL,
	[Status_Id] [int] NULL,
	[Begin_Date] [datetime] NULL,
	[End_Date] [datetime] NULL,
	[Active] [bit] NOT NULL,
	[Deleted] [bit] NOT NULL,
 CONSTRAINT [PK_Products_Details] PRIMARY KEY CLUSTERED 
(
	[Product_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Status]    Script Date: 4/18/2022 11:25:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Status](
	[Status_Id] [int] IDENTITY(1,1) NOT NULL,
	[Status_Code] [nvarchar](50) NOT NULL,
	[Status_Name] [nvarchar](50) NOT NULL,
	[Status_Desc] [nvarchar](max) NOT NULL,
	[Last_Update_User] [nvarchar](max) NULL,
	[Last_Update_Date] [datetime] NULL,
	[Active] [bit] NOT NULL,
	[Deleted] [bit] NOT NULL,
 CONSTRAINT [PK_Status] PRIMARY KEY CLUSTERED 
(
	[Status_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TEMPLATES]    Script Date: 4/18/2022 11:25:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TEMPLATES](
	[Template_Id] [int] IDENTITY(1,1) NOT NULL,
	[Product_Number] [int] NULL,
	[Item_Number] [int] NULL,
	[Unit_Id] [int] NULL,
	[Quantity] [float] NULL,
	[Price] [float] NULL,
	[Last_Update_User] [nvarchar](max) NULL,
	[Last_Update_Date] [datetime] NULL,
	[Included] [bit] NOT NULL,
	[Optional] [bit] NOT NULL,
 CONSTRAINT [PK_TEMPLATES] PRIMARY KEY CLUSTERED 
(
	[Template_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Units]    Script Date: 4/18/2022 11:25:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Units](
	[Unit_Id] [int] IDENTITY(1,1) NOT NULL,
	[Unit_Name] [nvarchar](50) NULL,
	[Unit_Desc] [nvarchar](max) NULL,
	[Unit_Short_Name] [nvarchar](50) NULL,
	[Deleted] [bit] NULL,
	[Last_Update_User] [nvarchar](max) NULL,
	[Last_Update_Date] [datetime] NULL,
	[Active] [bit] NOT NULL,
 CONSTRAINT [PK_Units] PRIMARY KEY CLUSTERED 
(
	[Unit_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[Items_Category]  WITH CHECK ADD  CONSTRAINT [FK_Items_Category_Items_Category] FOREIGN KEY([Parent_Id])
REFERENCES [dbo].[Items_Category] ([Category_Id])
GO
ALTER TABLE [dbo].[Items_Category] CHECK CONSTRAINT [FK_Items_Category_Items_Category]
GO
ALTER TABLE [dbo].[Items_Category]  WITH CHECK ADD  CONSTRAINT [FK_Items_Category_Status] FOREIGN KEY([Status_Id])
REFERENCES [dbo].[Status] ([Status_Id])
GO
ALTER TABLE [dbo].[Items_Category] CHECK CONSTRAINT [FK_Items_Category_Status]
GO
ALTER TABLE [dbo].[Items_Details]  WITH CHECK ADD  CONSTRAINT [FK_Items_Details_Items_Category] FOREIGN KEY([Category_Id])
REFERENCES [dbo].[Items_Category] ([Category_Id])
GO
ALTER TABLE [dbo].[Items_Details] CHECK CONSTRAINT [FK_Items_Details_Items_Category]
GO
ALTER TABLE [dbo].[Items_Details]  WITH CHECK ADD  CONSTRAINT [FK_Items_Details_Items_Category1] FOREIGN KEY([Sub_Category_Id])
REFERENCES [dbo].[Items_Category] ([Category_Id])
GO
ALTER TABLE [dbo].[Items_Details] CHECK CONSTRAINT [FK_Items_Details_Items_Category1]
GO
ALTER TABLE [dbo].[Items_Details]  WITH CHECK ADD  CONSTRAINT [FK_Items_Details_Status] FOREIGN KEY([Status_Id])
REFERENCES [dbo].[Status] ([Status_Id])
GO
ALTER TABLE [dbo].[Items_Details] CHECK CONSTRAINT [FK_Items_Details_Status]
GO
ALTER TABLE [dbo].[Items_Details]  WITH CHECK ADD  CONSTRAINT [FK_Items_Details_Units] FOREIGN KEY([Unit_Id])
REFERENCES [dbo].[Units] ([Unit_Id])
GO
ALTER TABLE [dbo].[Items_Details] CHECK CONSTRAINT [FK_Items_Details_Units]
GO
ALTER TABLE [dbo].[Product_items]  WITH CHECK ADD  CONSTRAINT [FK_Product_items_Items_Details] FOREIGN KEY([Item_Id])
REFERENCES [dbo].[Items_Details] ([Item_Id])
GO
ALTER TABLE [dbo].[Product_items] CHECK CONSTRAINT [FK_Product_items_Items_Details]
GO
ALTER TABLE [dbo].[Product_items]  WITH CHECK ADD  CONSTRAINT [FK_Product_items_Products_Details] FOREIGN KEY([Product_Id])
REFERENCES [dbo].[Products_Details] ([Product_Id])
GO
ALTER TABLE [dbo].[Product_items] CHECK CONSTRAINT [FK_Product_items_Products_Details]
GO
ALTER TABLE [dbo].[Product_items]  WITH CHECK ADD  CONSTRAINT [FK_Product_items_Units] FOREIGN KEY([Unit_Id])
REFERENCES [dbo].[Units] ([Unit_Id])
GO
ALTER TABLE [dbo].[Product_items] CHECK CONSTRAINT [FK_Product_items_Units]
GO
ALTER TABLE [dbo].[Products_Details]  WITH CHECK ADD  CONSTRAINT [FK_Products_Details_Status] FOREIGN KEY([Status_Id])
REFERENCES [dbo].[Status] ([Status_Id])
GO
ALTER TABLE [dbo].[Products_Details] CHECK CONSTRAINT [FK_Products_Details_Status]
GO
ALTER TABLE [dbo].[Products_Details]  WITH CHECK ADD  CONSTRAINT [FK_Products_Details_Units] FOREIGN KEY([Unit_Id])
REFERENCES [dbo].[Units] ([Unit_Id])
GO
ALTER TABLE [dbo].[Products_Details] CHECK CONSTRAINT [FK_Products_Details_Units]
GO
ALTER TABLE [dbo].[TEMPLATES]  WITH CHECK ADD  CONSTRAINT [FK_TEMPLATES_Units] FOREIGN KEY([Unit_Id])
REFERENCES [dbo].[Units] ([Unit_Id])
GO
ALTER TABLE [dbo].[TEMPLATES] CHECK CONSTRAINT [FK_TEMPLATES_Units]
GO
USE [master]
GO
ALTER DATABASE [Sales] SET  READ_WRITE 
GO
