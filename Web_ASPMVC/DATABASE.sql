USE [master]
GO
/****** Object:  Database [WebASPMVC]    Script Date: 2021-07-29 11:47:23 ******/
CREATE DATABASE [WebASPMVC]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'WebASPMVC', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.MSSQLSERVER\MSSQL\DATA\WebASPMVC.mdf' , SIZE = 3264KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'WebASPMVC_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.MSSQLSERVER\MSSQL\DATA\WebASPMVC_log.ldf' , SIZE = 832KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [WebASPMVC] SET COMPATIBILITY_LEVEL = 120
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [WebASPMVC].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [WebASPMVC] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [WebASPMVC] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [WebASPMVC] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [WebASPMVC] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [WebASPMVC] SET ARITHABORT OFF 
GO
ALTER DATABASE [WebASPMVC] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [WebASPMVC] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [WebASPMVC] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [WebASPMVC] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [WebASPMVC] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [WebASPMVC] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [WebASPMVC] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [WebASPMVC] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [WebASPMVC] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [WebASPMVC] SET  ENABLE_BROKER 
GO
ALTER DATABASE [WebASPMVC] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [WebASPMVC] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [WebASPMVC] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [WebASPMVC] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [WebASPMVC] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [WebASPMVC] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [WebASPMVC] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [WebASPMVC] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [WebASPMVC] SET  MULTI_USER 
GO
ALTER DATABASE [WebASPMVC] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [WebASPMVC] SET DB_CHAINING OFF 
GO
ALTER DATABASE [WebASPMVC] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [WebASPMVC] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
ALTER DATABASE [WebASPMVC] SET DELAYED_DURABILITY = DISABLED 
GO
USE [WebASPMVC]
GO
/****** Object:  Table [dbo].[Category]    Script Date: 2021-07-29 11:47:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Category](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](31) NULL,
	[Status] [bit] NULL CONSTRAINT [DF_Category_Status]  DEFAULT ((1)),
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Customer]    Script Date: 2021-07-29 11:47:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Customer](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Username] [varchar](50) NULL,
	[Password] [varchar](50) NOT NULL,
	[Email] [varchar](100) NULL,
	[Address] [nvarchar](200) NULL,
	[Phone] [varchar](50) NULL,
	[Birthday] [datetime] NULL,
 CONSTRAINT [PK_Customer] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Order]    Script Date: 2021-07-29 11:47:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Order](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Payment_Status] [bit] NULL,
	[Order_Status] [bit] NULL,
	[Order_Time] [datetime] NULL,
	[Ship_Time] [datetime] NULL,
	[IdCustomer] [int] NULL,
 CONSTRAINT [PK_Order] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[OrderDetail]    Script Date: 2021-07-29 11:47:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderDetail](
	[IdOrder] [int] NOT NULL,
	[IdProduct] [int] NOT NULL,
	[Qty] [int] NOT NULL,
	[Price] [decimal](18, 0) NOT NULL,
	[Color] [nvarchar](50) NULL,
 CONSTRAINT [PK_OrderDetails] PRIMARY KEY CLUSTERED 
(
	[IdOrder] ASC,
	[IdProduct] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Product]    Script Date: 2021-07-29 11:47:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Product](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NULL,
	[Thumbnail] [varchar](100) NULL,
	[Description] [ntext] NULL CONSTRAINT [DF_Product_Description]  DEFAULT (N'Đang cập nhật'),
	[Status] [bit] NULL CONSTRAINT [DF_Product_Status]  DEFAULT ((1)),
	[IdCategory] [int] NULL,
	[Title] [ntext] NULL CONSTRAINT [DF_Product_Title]  DEFAULT (N'Đang cập nhật'),
	[Group] [nvarchar](50) NULL,
	[Color] [nvarchar](50) NULL,
	[UpdateTime] [datetime] NULL,
	[Price] [decimal](18, 0) NULL,
	[Items_Left] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Slide]    Script Date: 2021-07-29 11:47:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Slide](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Thumbnail] [nvarchar](100) NULL,
	[Status] [bit] NULL DEFAULT ((1)),
	[IdProduct] [int] NULL,
	[Title] [ntext] NULL CONSTRAINT [DF_Slide_Title]  DEFAULT (N'Đang cập nhật'),
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[User]    Script Date: 2021-07-29 11:47:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[User](
	[ID] [int] NOT NULL,
	[UserName] [varchar](50) NOT NULL,
	[PassWord] [varchar](50) NOT NULL,
	[Name] [nvarchar](50) NULL,
	[Avatar] [nvarchar](100) NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[Category] ON 

INSERT [dbo].[Category] ([ID], [Name], [Status]) VALUES (1, N'Iphone', 1)
INSERT [dbo].[Category] ([ID], [Name], [Status]) VALUES (2, N'Xiaomi', 1)
INSERT [dbo].[Category] ([ID], [Name], [Status]) VALUES (3, N'Oppo', 1)
INSERT [dbo].[Category] ([ID], [Name], [Status]) VALUES (4, N'Sam Sung', 1)
INSERT [dbo].[Category] ([ID], [Name], [Status]) VALUES (5, N'Phụ Kiện', 1)
SET IDENTITY_INSERT [dbo].[Category] OFF
SET IDENTITY_INSERT [dbo].[Customer] ON 

INSERT [dbo].[Customer] ([ID], [Name], [Username], [Password], [Email], [Address], [Phone], [Birthday]) VALUES (1, N'Lâm xuân nguyên', N'xuannguyen3107', N'e10adc3949ba59abbe56e057f20f883e', N'xuannguyenmaxpro@gmail.com', N'Tân Thành_Hàm Thuận Nam_Bình Thuận', N'0332093438', CAST(N'2000-07-31 00:00:00.000' AS DateTime))
SET IDENTITY_INSERT [dbo].[Customer] OFF
SET IDENTITY_INSERT [dbo].[Order] ON 

INSERT [dbo].[Order] ([ID], [Payment_Status], [Order_Status], [Order_Time], [Ship_Time], [IdCustomer]) VALUES (1, 0, 0, CAST(N'2021-07-28 02:06:02.060' AS DateTime), NULL, 1)
INSERT [dbo].[Order] ([ID], [Payment_Status], [Order_Status], [Order_Time], [Ship_Time], [IdCustomer]) VALUES (2, 0, 0, CAST(N'2021-07-29 10:50:48.173' AS DateTime), NULL, 1)
INSERT [dbo].[Order] ([ID], [Payment_Status], [Order_Status], [Order_Time], [Ship_Time], [IdCustomer]) VALUES (3, 0, 0, CAST(N'2021-07-29 10:55:22.623' AS DateTime), NULL, 1)
INSERT [dbo].[Order] ([ID], [Payment_Status], [Order_Status], [Order_Time], [Ship_Time], [IdCustomer]) VALUES (4, 0, 0, CAST(N'2021-07-29 10:58:18.937' AS DateTime), NULL, 1)
INSERT [dbo].[Order] ([ID], [Payment_Status], [Order_Status], [Order_Time], [Ship_Time], [IdCustomer]) VALUES (5, 0, 0, CAST(N'2021-07-29 11:06:30.437' AS DateTime), NULL, 1)
INSERT [dbo].[Order] ([ID], [Payment_Status], [Order_Status], [Order_Time], [Ship_Time], [IdCustomer]) VALUES (6, 0, 0, CAST(N'2021-07-29 11:08:39.027' AS DateTime), NULL, 1)
SET IDENTITY_INSERT [dbo].[Order] OFF
INSERT [dbo].[OrderDetail] ([IdOrder], [IdProduct], [Qty], [Price], [Color]) VALUES (1, 1, 2, CAST(10000000 AS Decimal(18, 0)), N'Trắng')
INSERT [dbo].[OrderDetail] ([IdOrder], [IdProduct], [Qty], [Price], [Color]) VALUES (2, 5, 1, CAST(33000000 AS Decimal(18, 0)), N'Trắng')
INSERT [dbo].[OrderDetail] ([IdOrder], [IdProduct], [Qty], [Price], [Color]) VALUES (3, 5, 1, CAST(33000000 AS Decimal(18, 0)), N'Trắng')
INSERT [dbo].[OrderDetail] ([IdOrder], [IdProduct], [Qty], [Price], [Color]) VALUES (4, 5, 1, CAST(33000000 AS Decimal(18, 0)), N'Đỏ')
INSERT [dbo].[OrderDetail] ([IdOrder], [IdProduct], [Qty], [Price], [Color]) VALUES (5, 5, 1, CAST(33000000 AS Decimal(18, 0)), N'Đỏ')
INSERT [dbo].[OrderDetail] ([IdOrder], [IdProduct], [Qty], [Price], [Color]) VALUES (6, 5, 1, CAST(33000000 AS Decimal(18, 0)), N'Đỏ')
SET IDENTITY_INSERT [dbo].[Product] ON 

INSERT [dbo].[Product] ([ID], [Name], [Thumbnail], [Description], [Status], [IdCategory], [Title], [Group], [Color], [UpdateTime], [Price], [Items_Left]) VALUES (1, N'Iphone 11Pro Max', N'iPhone-11-Pro-Max.jpg', N'<h2 style="text-align:center"><span style="font-family:Times New Roman,Times,serif">Th&ocirc;ng số kỹ thuật</span></h2>

<table align="center" border="1" cellpadding="1" cellspacing="1" style="width:500px">
	<tbody>
		<tr>
			<td style="text-align:center"><strong><span style="font-size:16px"><span style="font-family:Times New Roman,Times,serif">Bộ nhớ trong</span></span></strong></td>
			<td style="text-align:center"><strong><span style="font-size:16px"><span style="font-family:Times New Roman,Times,serif">64 GB</span></span></strong></td>
		</tr>
		<tr>
			<td style="text-align:center"><strong><span style="font-size:16px"><span style="font-family:Times New Roman,Times,serif">Camera ch&iacute;nh</span></span></strong></td>
			<td style="text-align:center"><strong><span style="font-size:16px"><span style="font-family:Times New Roman,Times,serif">Triple 12Mp + 12Mp + 12Mp</span></span></strong></td>
		</tr>
		<tr>
			<td style="text-align:center"><strong><span style="font-size:16px"><span style="font-family:Times New Roman,Times,serif">Độ ph&acirc;n giải m&agrave;n h&igrave;nh</span></span></strong></td>
			<td style="text-align:center"><strong><span style="font-size:16px"><span style="font-family:Times New Roman,Times,serif">1242 x 2688 Pixels</span></span></strong></td>
		</tr>
		<tr>
			<td style="text-align:center"><strong><span style="font-size:16px"><span style="font-family:Times New Roman,Times,serif">Dung lượng pin</span></span></strong></td>
			<td style="text-align:center"><strong><span style="font-size:16px"><span style="font-family:Times New Roman,Times,serif">Li-Ion 3969mAh</span></span></strong></td>
		</tr>
		<tr>
			<td style="text-align:center"><strong><span style="font-size:16px"><span style="font-family:Times New Roman,Times,serif">Hệ điều h&agrave;nh</span></span></strong></td>
			<td style="text-align:center"><strong><span style="font-size:16px"><span style="font-family:Times New Roman,Times,serif">iOS 13</span></span></strong></td>
		</tr>
		<tr>
			<td style="text-align:center"><strong><span style="font-size:16px"><span style="font-family:Times New Roman,Times,serif">K&iacute;ch thước m&agrave;n h&igrave;nh</span></span></strong></td>
			<td style="text-align:center"><strong><span style="font-size:16px"><span style="font-family:Times New Roman,Times,serif">6.5&quot;</span></span></strong></td>
		</tr>
		<tr>
			<td style="text-align:center"><strong><span style="font-size:16px"><span style="font-family:Times New Roman,Times,serif">Thẻ sim</span></span></strong></td>
			<td style="text-align:center"><strong><span style="font-size:16px"><span style="font-family:Times New Roman,Times,serif">1 nano SIM + 1 eSIM</span></span></strong></td>
		</tr>
		<tr>
			<td style="text-align:center"><strong><span style="font-size:16px"><span style="font-family:Times New Roman,Times,serif">Camera phụ</span></span></strong></td>
			<td style="text-align:center"><strong><span style="font-size:16px"><span style="font-family:Times New Roman,Times,serif">12 MP</span></span></strong></td>
		</tr>
		<tr>
			<td style="text-align:center"><strong><span style="font-size:16px"><span style="font-family:Times New Roman,Times,serif">CPU</span></span></strong></td>
			<td style="text-align:center"><strong><span style="font-size:16px"><span style="font-family:Times New Roman,Times,serif">Apple A13 Bionic (7 nm+)</span></span></strong></td>
		</tr>
		<tr>
			<td style="text-align:center"><strong><span style="font-size:16px"><span style="font-family:Times New Roman,Times,serif">RAM</span></span></strong></td>
			<td style="text-align:center"><strong><span style="font-size:16px"><span style="font-family:Times New Roman,Times,serif">4 GB</span></span></strong></td>
		</tr>
	</tbody>
</table>

<p>&nbsp;</p>
', 1, 1, N'Raw denim you probably haven''t heard of them jean shorts Austin. Nesciunt tofu stumptown aliqua, retro synth master cleanse. Mustache cliche tempor, williamsburg carles vegan helvetica. Reprehenderit butcher retro keffiyeh dreamcatcher synth. Cosby sweater eu banh mi, qui irure terry richardson ex squid. Aliquip placeat salvia cillum iphone. Seitan aliquip quis cardigan american apparel, butcher voluptate nisi qui.', N'Mới', N'Đen', NULL, CAST(10000000 AS Decimal(18, 0)), 13)
INSERT [dbo].[Product] ([ID], [Name], [Thumbnail], [Description], [Status], [IdCategory], [Title], [Group], [Color], [UpdateTime], [Price], [Items_Left]) VALUES (2, N'Iphone Xs Max', N'xsmax.jpg', N'<h2 style="text-align:center">Th&ocirc;ng số kỹ thuật</h2>

<table align="center" border="1" cellpadding="1" cellspacing="1">
	<tbody>
		<tr>
			<td><strong>Bộ nhớ trong</strong></td>
			<td><strong>64 GB</strong></td>
		</tr>
		<tr>
			<td><strong>Camera ch&iacute;nh</strong></td>
			<td>Dual 12 MP</td>
		</tr>
		<tr>
			<td><strong>Độ ph&acirc;n giải m&agrave;n h&igrave;nh</strong></td>
			<td>1242 x 2688 Pixels</td>
		</tr>
		<tr>
			<td><strong>Dung lượng pin</strong></td>
			<td>Li-Ion 3174 mAh</td>
		</tr>
		<tr>
			<td><strong>Hệ điều h&agrave;nh</strong></td>
			<td><strong>iOS 13</strong></td>
		</tr>
		<tr>
			<td><strong>K&iacute;ch thước m&agrave;n h&igrave;nh</strong></td>
			<td><strong>6.5&quot;</strong></td>
		</tr>
		<tr>
			<td><strong>Thẻ sim</strong></td>
			<td>1 nano SIM</td>
		</tr>
		<tr>
			<td><strong>Camera phụ</strong></td>
			<td>7.0 MP</td>
		</tr>
		<tr>
			<td><strong>CPU</strong></td>
			<td>Apple A12 Bionic 6 nh&acirc;n</td>
		</tr>
		<tr>
			<td><strong>RAM</strong></td>
			<td><strong>4 GB</strong></td>
		</tr>
		<tr>
			<td>Sạc nhanh</td>
			<td>Quick charge 3.0</td>
		</tr>
	</tbody>
</table>
', 1, 1, N' Apple iPhone XS Max cũng giống như truyền thống của Apple. Hầu như không thay đổi thiết kế bên ngoài và chỉ nâng cấp nhiều về phần cứng bên trong. iPhone XS Max giống như là dòng Plus cũ. Tức là một chiếc iPhone XS to hơn và đây cũng là chiếc iPhone có màn hình lớn nhất từ trước đến nay. iPhone XS Max được trang bị kính bảo vệ mới cho màn hình. Là loại kính bền nhất trên smartphone hiện nay. Máy đạt chuẩn chống nước bụi IP68.', N'Nổi bật', N'Đen', CAST(N'2021-07-28 00:00:00.000' AS DateTime), CAST(13390000 AS Decimal(18, 0)), 20)
INSERT [dbo].[Product] ([ID], [Name], [Thumbnail], [Description], [Status], [IdCategory], [Title], [Group], [Color], [UpdateTime], [Price], [Items_Left]) VALUES (3, N'SS galaxy S21 Plus 5G', N'galaxy-s21.jpg', N'<h2 style="text-align:center">Th&ocirc;ng số kỹ thuật</h2>

<table align="center" border="1" cellpadding="1" cellspacing="1">
	<tbody>
		<tr>
			<td><strong>Bộ nhớ trong</strong></td>
			<td><strong>128 GB</strong></td>
		</tr>
		<tr>
			<td><strong>Camera ch&iacute;nh</strong></td>
			<td>Triple Camera (64MP + 12MP + 12MP)</td>
		</tr>
		<tr>
			<td><strong>Độ ph&acirc;n giải m&agrave;n h&igrave;nh</strong></td>
			<td>Full HD+ (1080 x 2400 Pixels)</td>
		</tr>
		<tr>
			<td><strong>Dung lượng pin</strong></td>
			<td>Li-Po 4800mAh</td>
		</tr>
		<tr>
			<td><strong>Hệ điều h&agrave;nh</strong></td>
			<td>Android 11, OneUI 3.1</td>
		</tr>
		<tr>
			<td><strong>K&iacute;ch thước m&agrave;n h&igrave;nh</strong></td>
			<td>6.7 inches</td>
		</tr>
		<tr>
			<td><strong>Thẻ sim</strong></td>
			<td>2 nano SIM</td>
		</tr>
		<tr>
			<td><strong>Camera phụ</strong></td>
			<td>10MP (ToF)</td>
		</tr>
		<tr>
			<td><strong>CPU</strong></td>
			<td>Exynos 2100 (5nm)</td>
		</tr>
		<tr>
			<td><strong>RAM</strong></td>
			<td><strong>8 GB</strong></td>
		</tr>
		<tr>
			<td>Sạc nhanh</td>
			<td>Sạc nhanh 25W</td>
		</tr>
		<tr>
			<td>Bộ nhớ</td>
			<td>microSDXC</td>
		</tr>
	</tbody>
</table>
', 1, 4, N'Máy mới nguyên seal, IMEI trùng hộp. Bảo hành chính hãng 12 tháng, bao test 30 ngày', N'Nổi bật', N'Đen', NULL, CAST(16490000 AS Decimal(18, 0)), 10)
INSERT [dbo].[Product] ([ID], [Name], [Thumbnail], [Description], [Status], [IdCategory], [Title], [Group], [Color], [UpdateTime], [Price], [Items_Left]) VALUES (4, N'Oppo A92', N'oppo-a92.jpg', N'<h2 style="text-align:center">Th&ocirc;ng số kỹ thuật</h2>

<table align="center" border="1" cellpadding="1" cellspacing="1">
	<tbody>
		<tr>
			<td><strong>Bộ nhớ trong</strong></td>
			<td><strong>64 GB</strong></td>
		</tr>
		<tr>
			<td><strong>Camera ch&iacute;nh</strong></td>
			<td>Ch&iacute;nh 64 MP &amp; Phụ 8 MP, 2 MP</td>
		</tr>
		<tr>
			<td><strong>Độ ph&acirc;n giải m&agrave;n h&igrave;nh</strong></td>
			<td>AMOLED, 6.43&quot;, Full HD+</td>
		</tr>
		<tr>
			<td><strong>Dung lượng pin</strong></td>
			<td>4300 mAh, 65 W</td>
		</tr>
		<tr>
			<td><strong>Hệ điều h&agrave;nh</strong></td>
			<td>Android 11</td>
		</tr>
		<tr>
			<td><strong>K&iacute;ch thước m&agrave;n h&igrave;nh</strong></td>
			<td><strong>6.5&quot;</strong></td>
		</tr>
		<tr>
			<td><strong>Thẻ sim</strong></td>
			<td>2 Nano SIM, Hỗ trợ 5G</td>
		</tr>
		<tr>
			<td><strong>Camera phụ</strong></td>
			<td><strong>32 MP</strong></td>
		</tr>
		<tr>
			<td><strong>CPU</strong></td>
			<td>MediaTek Dimensity 900 5G</td>
		</tr>
		<tr>
			<td><strong>RAM</strong></td>
			<td><strong>8 GB</strong></td>
		</tr>
	</tbody>
</table>
', 1, 3, N'Được tích hợp thuật toán AI thông minh, tính năng chụp ảnh/quay phim chân dung Bokeh Flare biến ánh sáng xung quanh thành phông nền tuyệt diệu với những điểm sáng lấp lánh, khiến mỗi khoảnh khắc bạn ghi lại đều trở nên nổi bật và đặc biệt hơn bao giờ hết.  *Tính năng Video chân dung Bokeh Flare chỉ hỗ trợ trên OPPO Reno6 5G', N'Nổi bật', N'Đen', NULL, CAST(11990000 AS Decimal(18, 0)), 15)
INSERT [dbo].[Product] ([ID], [Name], [Thumbnail], [Description], [Status], [IdCategory], [Title], [Group], [Color], [UpdateTime], [Price], [Items_Left]) VALUES (5, N'Iphone 12Pro Max', N'iphone-12-pro-max.jpg', N'<h2 style="text-align:center">Th&ocirc;ng số kỹ thuật</h2>

<table align="center" border="1" cellpadding="1" cellspacing="1">
	<tbody>
		<tr>
			<td><strong>Bộ nhớ trong</strong></td>
			<td><strong>128 GB</strong></td>
		</tr>
		<tr>
			<td><strong>Camera ch&iacute;nh</strong></td>
			<td><strong>Triple 12Mp + 12Mp + 12Mp</strong></td>
		</tr>
		<tr>
			<td><strong>Độ ph&acirc;n giải m&agrave;n h&igrave;nh</strong></td>
			<td>1284 x 2778 Pixels</td>
		</tr>
		<tr>
			<td><strong>Dung lượng pin</strong></td>
			<td>3687 mAh</td>
		</tr>
		<tr>
			<td><strong>Hệ điều h&agrave;nh</strong></td>
			<td><strong>iOS 14</strong></td>
		</tr>
		<tr>
			<td><strong>K&iacute;ch thước m&agrave;n h&igrave;nh</strong></td>
			<td><strong>6.7&quot;</strong></td>
		</tr>
		<tr>
			<td><strong>Thẻ sim</strong></td>
			<td><strong>1 nano SIM + 1 eSIM</strong></td>
		</tr>
		<tr>
			<td><strong>Camera phụ</strong></td>
			<td><strong>3 camera 12 MP</strong></td>
		</tr>
		<tr>
			<td><strong>CPU</strong></td>
			<td><strong>Apple A14 Bionic 6 nh&acirc;n</strong></td>
		</tr>
		<tr>
			<td><strong>RAM</strong></td>
			<td><strong>6 GB</strong></td>
		</tr>
	</tbody>
</table>
', 1, 1, N'Iphone 12Pro Max là một siêu phẩm Smartphone đến từ Apple.', N'Mới', N'Đen', CAST(N'2021-07-28 00:00:00.000' AS DateTime), CAST(33000000 AS Decimal(18, 0)), 15)
INSERT [dbo].[Product] ([ID], [Name], [Thumbnail], [Description], [Status], [IdCategory], [Title], [Group], [Color], [UpdateTime], [Price], [Items_Left]) VALUES (6, N'Iphone SE', N'iphone-se-2020.jpg', N'<h2 style="text-align:center">Th&ocirc;ng số kỹ thuật</h2>

<table align="center" border="1" cellpadding="1" cellspacing="1">
	<tbody>
		<tr>
			<td><strong>Bộ nhớ trong</strong></td>
			<td><strong>64 GB</strong></td>
		</tr>
		<tr>
			<td><strong>Camera ch&iacute;nh</strong></td>
			<td><strong>7Mp</strong></td>
		</tr>
		<tr>
			<td><strong>Độ ph&acirc;n giải m&agrave;n h&igrave;nh</strong></td>
			<td><strong>HD (750 x 1334 Pixels)</strong></td>
		</tr>
		<tr>
			<td><strong>Dung lượng pin</strong></td>
			<td><strong>1821 mAh</strong></td>
		</tr>
		<tr>
			<td><strong>Hệ điều h&agrave;nh</strong></td>
			<td><strong>iOS 14</strong></td>
		</tr>
		<tr>
			<td><strong>K&iacute;ch thước m&agrave;n h&igrave;nh</strong></td>
			<td><strong>4.7&quot;</strong></td>
		</tr>
		<tr>
			<td><strong>Thẻ sim</strong></td>
			<td><strong>1 nano SIM + 1 eSIM</strong></td>
		</tr>
		<tr>
			<td><strong>Camera phụ</strong></td>
			<td><strong>12 MP</strong></td>
		</tr>
		<tr>
			<td><strong>CPU</strong></td>
			<td><strong>Apple A13 Bionic 6 nh&acirc;n</strong></td>
		</tr>
		<tr>
			<td><strong>RAM</strong></td>
			<td><strong>3 GB</strong></td>
		</tr>
	</tbody>
</table>
', 1, 1, N'Iphone SE bất ngờ ra mắt với thiết kế 4.7 inch nhỏ gọn và tiện lợi, chip A13 Bionic mạnh mẽ', N'Mới', N'Đen', CAST(N'2021-07-28 00:00:00.000' AS DateTime), CAST(14000000 AS Decimal(18, 0)), 20)
INSERT [dbo].[Product] ([ID], [Name], [Thumbnail], [Description], [Status], [IdCategory], [Title], [Group], [Color], [UpdateTime], [Price], [Items_Left]) VALUES (7, N'Oppo A94', N'oppo-a94.jpg', N'<p>Chi tiết</p>
', 1, 3, N'Mẫu Smartphone tầm trung của OPPO - đã được giới thiệu đển người dùng với nhiều cải tiến', N'Mới', N'Đen', CAST(N'2021-07-28 00:00:00.000' AS DateTime), CAST(7700000 AS Decimal(18, 0)), 20)
INSERT [dbo].[Product] ([ID], [Name], [Thumbnail], [Description], [Status], [IdCategory], [Title], [Group], [Color], [UpdateTime], [Price], [Items_Left]) VALUES (8, N'Oppo Reno5', N'oppo-reno5.jpg', N'<h2 style="text-align:center">Th&ocirc;ng số kỹ thuật</h2>

<table align="center" border="1" cellpadding="1" cellspacing="1">
	<tbody>
		<tr>
			<td><strong>Bộ nhớ trong</strong></td>
			<td><strong>128 GB</strong></td>
		</tr>
		<tr>
			<td><strong>Camera ch&iacute;nh</strong></td>
			<td>Ch&iacute;nh 64 MP &amp; Phụ 8 MP, 2 MP, 2 MP</td>
		</tr>
		<tr>
			<td><strong>Độ ph&acirc;n giải m&agrave;n h&igrave;nh</strong></td>
			<td>Full HD+ (1080 x 2400 Pixels)</td>
		</tr>
		<tr>
			<td><strong>Dung lượng pin</strong></td>
			<td>4310 mAh</td>
		</tr>
		<tr>
			<td><strong>Hệ điều h&agrave;nh</strong></td>
			<td>Android 11</td>
		</tr>
		<tr>
			<td><strong>K&iacute;ch thước m&agrave;n h&igrave;nh</strong></td>
			<td><strong>6.43&quot;</strong></td>
		</tr>
		<tr>
			<td><strong>Thẻ sim</strong></td>
			<td>2 Nano SIM, Hỗ trợ 5G</td>
		</tr>
		<tr>
			<td><strong>Camera phụ</strong></td>
			<td><strong>32 MP</strong></td>
		</tr>
		<tr>
			<td><strong>CPU</strong></td>
			<td>Snapdragon 720G 8 Nh&acirc;n</td>
		</tr>
		<tr>
			<td><strong>RAM</strong></td>
			<td><strong>8 GB</strong></td>
		</tr>
	</tbody>
</table>
', 1, 3, N'Oppo Reno5 là sự kết hợp ấn tượng giữa hiệu năng và thiết kế, mang đến cho người dùng nhiều công nghệ', N'Mới', N'Đen', CAST(N'2021-07-28 00:00:00.000' AS DateTime), CAST(8690000 AS Decimal(18, 0)), 20)
INSERT [dbo].[Product] ([ID], [Name], [Thumbnail], [Description], [Status], [IdCategory], [Title], [Group], [Color], [UpdateTime], [Price], [Items_Left]) VALUES (9, N'Sam Sung galaxy A22', N'samsung-galaxy-a22.jpg', N'<h2 style="text-align:center">Th&ocirc;ng số kỹ thuật</h2>

<table align="center" border="1" cellpadding="1" cellspacing="1">
	<tbody>
		<tr>
			<td><strong>Bộ nhớ trong</strong></td>
			<td><strong>128 GB</strong></td>
		</tr>
		<tr>
			<td><strong>Camera ch&iacute;nh</strong></td>
			<td>Ch&iacute;nh 48 MP &amp; phụ 8 MP, 2 MP, 2MP&nbsp;</td>
		</tr>
		<tr>
			<td><strong>Độ ph&acirc;n giải m&agrave;n h&igrave;nh</strong></td>
			<td>HD+ (720 x 1600 Pixels)</td>
		</tr>
		<tr>
			<td><strong>Dung lượng pin</strong></td>
			<td>5000mAh</td>
		</tr>
		<tr>
			<td><strong>Hệ điều h&agrave;nh</strong></td>
			<td>Android 11</td>
		</tr>
		<tr>
			<td><strong>K&iacute;ch thước m&agrave;n h&igrave;nh</strong></td>
			<td>6.4 inches</td>
		</tr>
		<tr>
			<td><strong>Thẻ sim</strong></td>
			<td>2 nano SIM</td>
		</tr>
		<tr>
			<td><strong>Camera phụ</strong></td>
			<td>10MP (ToF)</td>
		</tr>
		<tr>
			<td><strong>CPU</strong></td>
			<td>MediaTek MT6769V 8 nh&acirc;n</td>
		</tr>
		<tr>
			<td><strong>RAM</strong></td>
			<td><strong>6 GB</strong></td>
		</tr>
		<tr>
			<td>Sạc nhanh</td>
			<td>Sạc nhanh</td>
		</tr>
		<tr>
			<td>Bộ nhớ</td>
			<td>microSD</td>
		</tr>
	</tbody>
</table>
', 1, 4, N'Sam Sung chào đón hè 2021 bằng sự ra đời của Galaxy A22  với nhiều thông số ấn tượng', N'Mới', N'Đen', CAST(N'2021-07-28 00:00:00.000' AS DateTime), CAST(5890000 AS Decimal(18, 0)), 20)
INSERT [dbo].[Product] ([ID], [Name], [Thumbnail], [Description], [Status], [IdCategory], [Title], [Group], [Color], [UpdateTime], [Price], [Items_Left]) VALUES (10, N'Sam Sung galaxy NOTE 20', N'samsung-galaxy-note-20.jpg', N'<h2 style="text-align:center">Th&ocirc;ng số kỹ thuật</h2>

<table align="center" border="1" cellpadding="1" cellspacing="1">
	<tbody>
		<tr>
			<td><strong>Bộ nhớ trong</strong></td>
			<td><strong>256 GB</strong></td>
		</tr>
		<tr>
			<td><strong>Camera ch&iacute;nh</strong></td>
			<td>Ch&iacute;nh 12 MP &amp; phụ 64 MP, 12 MP</td>
		</tr>
		<tr>
			<td><strong>Độ ph&acirc;n giải m&agrave;n h&igrave;nh</strong></td>
			<td>Full HD+ (1080 x 2400 Pixels)</td>
		</tr>
		<tr>
			<td><strong>Dung lượng pin</strong></td>
			<td>4300mAh</td>
		</tr>
		<tr>
			<td><strong>Hệ điều h&agrave;nh</strong></td>
			<td>Android 10</td>
		</tr>
		<tr>
			<td><strong>K&iacute;ch thước m&agrave;n h&igrave;nh</strong></td>
			<td>6.7 inches</td>
		</tr>
		<tr>
			<td><strong>Thẻ sim</strong></td>
			<td>2 nano SIM hoặc 1 nano SIM + 1 eSIM</td>
		</tr>
		<tr>
			<td><strong>Camera phụ</strong></td>
			<td>10MP (ToF)</td>
		</tr>
		<tr>
			<td><strong>CPU</strong></td>
			<td>Exynos 999 8 nh&acirc;n</td>
		</tr>
		<tr>
			<td><strong>RAM</strong></td>
			<td><strong>8 GB</strong></td>
		</tr>
		<tr>
			<td>Sạc nhanh</td>
			<td>Sạc nhanh</td>
		</tr>
		<tr>
			<td>Bộ nhớ</td>
			<td>&nbsp;</td>
		</tr>
	</tbody>
</table>
', 1, 4, N'Galaxy note 20 chính thức lên kệ với thiết kế bắt mắt cùng nhiều chức năng', N'Mới', N'Đen', CAST(N'2021-07-28 00:00:00.000' AS DateTime), CAST(15000000 AS Decimal(18, 0)), 20)
INSERT [dbo].[Product] ([ID], [Name], [Thumbnail], [Description], [Status], [IdCategory], [Title], [Group], [Color], [UpdateTime], [Price], [Items_Left]) VALUES (11, N'Xiaomi Mi 11', N'xiaomi-mi-11.jpg', N'<h2 style="text-align:center">Th&ocirc;ng số kỹ thuật</h2>

<table align="center" border="1" cellpadding="1" cellspacing="1">
	<tbody>
		<tr>
			<td><strong>Bộ nhớ trong</strong></td>
			<td><strong>256 GB</strong></td>
		</tr>
		<tr>
			<td><strong>Camera ch&iacute;nh</strong></td>
			<td>Ch&iacute;nh 108 MP &amp; phụ 13 MP, 5 MP</td>
		</tr>
		<tr>
			<td><strong>Độ ph&acirc;n giải m&agrave;n h&igrave;nh</strong></td>
			<td>2K+ (1440 x 3200 Pixels)</td>
		</tr>
		<tr>
			<td><strong>Dung lượng pin</strong></td>
			<td>4600mAh</td>
		</tr>
		<tr>
			<td><strong>Hệ điều h&agrave;nh</strong></td>
			<td>Android 11</td>
		</tr>
		<tr>
			<td><strong>K&iacute;ch thước m&agrave;n h&igrave;nh</strong></td>
			<td>6.81 inches</td>
		</tr>
		<tr>
			<td><strong>Thẻ sim</strong></td>
			<td>2 nano SIM&nbsp;</td>
		</tr>
		<tr>
			<td><strong>Camera phụ</strong></td>
			<td>20 MP</td>
		</tr>
		<tr>
			<td><strong>CPU</strong></td>
			<td>Snapdragon 888 8 nh&acirc;n</td>
		</tr>
		<tr>
			<td><strong>RAM</strong></td>
			<td><strong>8 GB</strong></td>
		</tr>
		<tr>
			<td>Sạc</td>
			<td>Sạc nhanh, sạc kh&ocirc;ng gi&acirc;y</td>
		</tr>
		<tr>
			<td>Bộ nhớ</td>
		</tr>
	</tbody>
</table>
', 1, 2, N'Một siêu phẩm đến từ Xiaomi là Xiaomi Mi 11, máy cho trải nghiệm hàng đầu với vi xử lý cùng loạt công nghệ cao', N'Mới', N'Đen', CAST(N'2021-07-28 00:00:00.000' AS DateTime), CAST(18990000 AS Decimal(18, 0)), 20)
INSERT [dbo].[Product] ([ID], [Name], [Thumbnail], [Description], [Status], [IdCategory], [Title], [Group], [Color], [UpdateTime], [Price], [Items_Left]) VALUES (12, N'Xiaomi Note 10Pro', N'xiaomi-mi-note-10-pro.jpg', N'<h2>Th&ocirc;ng số kỹ thuật</h2>

<table align="center" border="1" cellpadding="1" cellspacing="1">
	<tbody>
		<tr>
			<td><strong>Bộ nhớ trong</strong></td>
			<td><strong>128 GB</strong></td>
		</tr>
		<tr>
			<td><strong>Camera ch&iacute;nh</strong></td>
			<td>Ch&iacute;nh 108 MP &amp; Phụ 8 MP, 5 MP, 2 MP</td>
		</tr>
		<tr>
			<td><strong>Độ ph&acirc;n giải m&agrave;n h&igrave;nh</strong></td>
			<td>Full HD+ (1080 x 2400 Pixels)</td>
		</tr>
		<tr>
			<td><strong>Dung lượng pin</strong></td>
			<td>5020mAh</td>
		</tr>
		<tr>
			<td><strong>Hệ điều h&agrave;nh</strong></td>
			<td>Android 11</td>
		</tr>
		<tr>
			<td><strong>K&iacute;ch thước m&agrave;n h&igrave;nh</strong></td>
			<td>6.67 inches</td>
		</tr>
		<tr>
			<td><strong>Thẻ sim</strong></td>
			<td>2 nano SIM&nbsp;</td>
		</tr>
		<tr>
			<td><strong>Camera phụ</strong></td>
			<td>20 MP</td>
		</tr>
		<tr>
			<td><strong>CPU</strong></td>
			<td>Snapdragon 732G 8 nh&acirc;n</td>
		</tr>
		<tr>
			<td><strong>RAM</strong></td>
			<td><strong>8 GB</strong></td>
		</tr>
		<tr>
			<td>Sạc</td>
			<td>Sạc nhanh, sạc kh&ocirc;ng gi&acirc;y, 33W</td>
		</tr>
		<tr>
			<td>Bộ nhớ</td>
		</tr>
	</tbody>
</table>
', 1, 2, N'Kế thừa và nâng cấp từ thế hệ trước, Xiaomi cho ra mắt dòng sản phẩm sở hữu thiết kế cao cấp', N'Mới', N'Đen', CAST(N'2021-07-28 00:00:00.000' AS DateTime), CAST(7500000 AS Decimal(18, 0)), 20)
INSERT [dbo].[Product] ([ID], [Name], [Thumbnail], [Description], [Status], [IdCategory], [Title], [Group], [Color], [UpdateTime], [Price], [Items_Left]) VALUES (13, N'Xiaomi Mi 10 Ultra', N'xiaomi-mi-10-ultra.jpg', N'<h2 style="text-align:center">Th&ocirc;ng số kỹ thuật</h2>

<table align="center" border="1" cellpadding="1" cellspacing="1">
	<tbody>
		<tr>
			<td><strong>Bộ nhớ trong</strong></td>
			<td><strong>256 GB</strong></td>
		</tr>
		<tr>
			<td><strong>Camera ch&iacute;nh</strong></td>
			<td>Ch&iacute;nh 48 MP &amp; Phụ 48 MP, 20 MP, 12 MP</td>
		</tr>
		<tr>
			<td><strong>Độ ph&acirc;n giải m&agrave;n h&igrave;nh</strong></td>
			<td>Full HD+ (1080 x 2340 Pixels)</td>
		</tr>
		<tr>
			<td><strong>Dung lượng pin</strong></td>
			<td>4500mAh</td>
		</tr>
		<tr>
			<td><strong>Hệ điều h&agrave;nh</strong></td>
			<td>Android 10</td>
		</tr>
		<tr>
			<td><strong>K&iacute;ch thước m&agrave;n h&igrave;nh</strong></td>
			<td>6.67 inches</td>
		</tr>
		<tr>
			<td><strong>Thẻ sim</strong></td>
			<td>2 nano SIM&nbsp;</td>
		</tr>
		<tr>
			<td><strong>Camera phụ</strong></td>
			<td>20 MP</td>
		</tr>
		<tr>
			<td><strong>CPU</strong></td>
			<td>Snapdragon 865 8 nh&acirc;n</td>
		</tr>
		<tr>
			<td><strong>RAM</strong></td>
			<td><strong>12 GB</strong></td>
		</tr>
		<tr>
			<td>Sạc</td>
			<td>Sạc nhanh, sạc kh&ocirc;ng gi&acirc;y</td>
		</tr>
		<tr>
			<td>Bộ nhớ</td>
		</tr>
	</tbody>
</table>
', 1, 2, N'Mới đây Xiaomi đã cho ra mắt mẫu flagship cao cấp có tên gọi là Xiaomi Mi 10 Ultra nhân dịp kỷ niệm 10 năm thành lập của hãng.', N'Mới', N'Đen', CAST(N'2021-07-28 00:00:00.000' AS DateTime), CAST(18000000 AS Decimal(18, 0)), 20)
INSERT [dbo].[Product] ([ID], [Name], [Thumbnail], [Description], [Status], [IdCategory], [Title], [Group], [Color], [UpdateTime], [Price], [Items_Left]) VALUES (14, N'Sạc dự phòng BPB011', N'BPB011.jpg', N'<h2 style="text-align:center">Th&ocirc;ng số kỹ thuật</h2>

<table align="center" border="1" cellpadding="1" cellspacing="1">
	<tbody>
		<tr>
			<td><strong>Dung lượng Pin</strong></td>
			<td>10.000 mAh</td>
		</tr>
		<tr>
			<td><strong>Hiệu suất</strong></td>
			<td>60%</td>
		</tr>
		<tr>
			<td><strong>Thời gian sạc</strong></td>
			<td>10 - 11 giờ (d&ugrave;ng Adapter 1A)5 - 6 giờ (d&ugrave;ng Adapter 2A)</td>
		</tr>
		<tr>
			<td><strong>Nguồn ra</strong></td>
			<td>Type-C: 5V - 3A, USB: 5V - 2.4A</td>
		</tr>
		<tr>
			<td><strong>Nguồn v&agrave;o</strong></td>
			<td>Type-C: 5V - 3A</td>
		</tr>
		<tr>
			<td><strong>K&iacute;ch thước</strong></td>
			<td>D&agrave;y 1.5 cm - Rộng 7 cm - D&agrave;i 13.5 cm</td>
		</tr>
		<tr>
			<td><strong>Thẻ sim</strong></td>
			<td>2 nano SIM&nbsp;</td>
		</tr>
		<tr>
			<td>Trọng lượng:</td>
			<td>222g</td>
		</tr>
		<tr>
			<td>Thương hiệu của:</td>
			<td>Mỹ</td>
		</tr>
	</tbody>
</table>
', 1, 5, N'Tốc độ sạc nhanh, an toàn với công nghệ sạc nhanh Power Delivery 15 W ', N'Phụ Kiện', N'Đen', CAST(N'2021-07-29 00:00:00.000' AS DateTime), CAST(790000 AS Decimal(18, 0)), 20)
INSERT [dbo].[Product] ([ID], [Name], [Thumbnail], [Description], [Status], [IdCategory], [Title], [Group], [Color], [UpdateTime], [Price], [Items_Left]) VALUES (15, N'Sạc dự phòng PA CK01', N'PA CK01.jpg', N'<h2 style="text-align:center">Th&ocirc;ng số kỹ thuật</h2>

<table align="center" border="1" cellpadding="1" cellspacing="1">
	<tbody>
		<tr>
			<td><strong>Dung lượng Pin</strong></td>
			<td>10.000 mAh</td>
		</tr>
		<tr>
			<td><strong>Hiệu suất</strong></td>
			<td>64%</td>
		</tr>
		<tr>
			<td><strong>Thời gian sạc</strong></td>
			<td>10 - 11 giờ (d&ugrave;ng Adapter 1A)5 - 6 giờ (d&ugrave;ng Adapter 2A)</td>
		</tr>
		<tr>
			<td><strong>Nguồn ra</strong></td>
			<td>USB: 5V - 2.1A</td>
		</tr>
		<tr>
			<td><strong>Nguồn v&agrave;o</strong></td>
			<td>Micro USB: 5V - 2A</td>
		</tr>
		<tr>
			<td><strong>K&iacute;ch thước</strong></td>
			<td>D&agrave;i 14.3 cm - ngang 7.5 cm - D&agrave;y 1.5 cm</td>
		</tr>
		<tr>
			<td>Trọng lượng:</td>
			<td>233g</td>
		</tr>
		<tr>
			<td>Thương hiệu của:</td>
			<td>Thế Giới Di Động</td>
		</tr>
	</tbody>
</table>
', 1, 5, N'Pin sạc dự phòng Polymer 10.000 mAh AVA PA CK01 có thiết kế đơn giản mà tinh tế với gam màu cổ điển', N'Phụ Kiện', N'Đen', CAST(N'2021-07-29 00:00:00.000' AS DateTime), CAST(490000 AS Decimal(18, 0)), 20)
INSERT [dbo].[Product] ([ID], [Name], [Thumbnail], [Description], [Status], [IdCategory], [Title], [Group], [Color], [UpdateTime], [Price], [Items_Left]) VALUES (16, N'Cáp sạc TypeC PM2002', N'PM2002.jpg', N'<h2 style="text-align:center">Th&ocirc;ng số kỹ thuật</h2>

<table align="center" border="1" cellpadding="1" cellspacing="1">
	<tbody>
		<tr>
			<td><strong>Chức năng</strong></td>
			<td>10.000 mAh</td>
		</tr>
		<tr>
			<td><strong>Đầu ra</strong></td>
			<td>Type C: 5V - 3A, 9V - 2.77A, 15V - 2A, 20V - 3A</td>
		</tr>
		<tr>
			<td><strong>Độ d&agrave;i</strong></td>
			<td>1 m</td>
		</tr>
		<tr>
			<td>C&ocirc;ng suất</td>
			<td>60 W</td>
		</tr>
		<tr>
			<td>Thương hiệu của:</td>
			<td>Mỹ</td>
		</tr>
	</tbody>
</table>
', 1, 5, N'Playa PM2002 thiết kế tối giản, màu đen - trắng đẹp mắt', N'Phụ Kiện', N'Đen', NULL, CAST(210000 AS Decimal(18, 0)), 20)
INSERT [dbo].[Product] ([ID], [Name], [Thumbnail], [Description], [Status], [IdCategory], [Title], [Group], [Color], [UpdateTime], [Price], [Items_Left]) VALUES (17, N'Cáp sạc TypeC PM2004', N'PM2004.jpg', N'<h2 style="text-align:center">Th&ocirc;ng số kỹ thuật</h2>

<table align="center" border="1" cellpadding="1" cellspacing="1">
	<tbody>
		<tr>
			<td><strong>Chức năng</strong></td>
			<td>Sạc, Truyền dữ liệu</td>
		</tr>
		<tr>
			<td><strong>Đầu ra</strong></td>
			<td>Type C: 5V - 3A, 9V - 2.77A, 15V - 2A, 20V - 3A</td>
		</tr>
		<tr>
			<td><strong>Độ d&agrave;i</strong></td>
			<td>1 m</td>
		</tr>
		<tr>
			<td>C&ocirc;ng suất</td>
			<td>60 W</td>
		</tr>
		<tr>
			<td>Sản xuất:</td>
			<td>Trung Quốc</td>
		</tr>
	</tbody>
</table>
', 1, 5, N'Thiết kế gọn nhẹ, độ dài dây cáp đến 1 m', N'Phụ Kiện', N'Đen', CAST(N'2021-07-29 00:00:00.000' AS DateTime), CAST(320000 AS Decimal(18, 0)), 20)
SET IDENTITY_INSERT [dbo].[Product] OFF
SET IDENTITY_INSERT [dbo].[Slide] ON 

INSERT [dbo].[Slide] ([ID], [Thumbnail], [Status], [IdProduct], [Title]) VALUES (1, N'Slide1.jpg', 1, NULL, N'Đang cập nhật')
INSERT [dbo].[Slide] ([ID], [Thumbnail], [Status], [IdProduct], [Title]) VALUES (2, N'Slide2.jpg', 1, NULL, N'Đang cập nhật')
INSERT [dbo].[Slide] ([ID], [Thumbnail], [Status], [IdProduct], [Title]) VALUES (3, N'Slide3.jpg', 1, NULL, N'Đang cập nhật')
SET IDENTITY_INSERT [dbo].[Slide] OFF
INSERT [dbo].[User] ([ID], [UserName], [PassWord], [Name], [Avatar]) VALUES (0, N'admin2', N'e10adc3949ba59abbe56e057f20f883e', N'Nguyễn Văn A', N'Avatar.jpg')
INSERT [dbo].[User] ([ID], [UserName], [PassWord], [Name], [Avatar]) VALUES (1, N'admin', N'e10adc3949ba59abbe56e057f20f883e', N'Lâm Xuân Nguyên', N'avatar_lxn.jpg')
ALTER TABLE [dbo].[Order]  WITH CHECK ADD  CONSTRAINT [FK_Order_Customer] FOREIGN KEY([IdCustomer])
REFERENCES [dbo].[Customer] ([ID])
GO
ALTER TABLE [dbo].[Order] CHECK CONSTRAINT [FK_Order_Customer]
GO
ALTER TABLE [dbo].[OrderDetail]  WITH CHECK ADD  CONSTRAINT [FK_OrderDetails_Order] FOREIGN KEY([IdOrder])
REFERENCES [dbo].[Order] ([ID])
GO
ALTER TABLE [dbo].[OrderDetail] CHECK CONSTRAINT [FK_OrderDetails_Order]
GO
ALTER TABLE [dbo].[OrderDetail]  WITH CHECK ADD  CONSTRAINT [FK_OrderDetails_Product] FOREIGN KEY([IdProduct])
REFERENCES [dbo].[Product] ([ID])
GO
ALTER TABLE [dbo].[OrderDetail] CHECK CONSTRAINT [FK_OrderDetails_Product]
GO
ALTER TABLE [dbo].[Product]  WITH CHECK ADD FOREIGN KEY([IdCategory])
REFERENCES [dbo].[Category] ([ID])
GO
ALTER TABLE [dbo].[Slide]  WITH CHECK ADD FOREIGN KEY([IdProduct])
REFERENCES [dbo].[Product] ([ID])
GO
USE [master]
GO
ALTER DATABASE [WebASPMVC] SET  READ_WRITE 
GO
