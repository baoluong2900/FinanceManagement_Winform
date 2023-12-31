USE [dbFinanceManagement]
GO
/****** Object:  Table [dbo].[Analysis]    Script Date: 11/26/2023 11:35:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Analysis](
	[analysisID] [int] IDENTITY(1,1) NOT NULL,
	[userID] [int] NULL,
	[analysisName] [nvarchar](100) NULL,
	[analysisDescription] [nvarchar](max) NULL,
	[createDate] [datetime] NULL,
	[modifiedDate] [datetime] NULL,
 CONSTRAINT [PK_Analysis] PRIMARY KEY CLUSTERED 
(
	[analysisID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Budgets]    Script Date: 11/26/2023 11:35:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Budgets](
	[budgetID] [int] IDENTITY(1,1) NOT NULL,
	[userID] [int] NULL,
	[budgetName] [nvarchar](100) NULL,
	[targetSavings] [decimal](18, 2) NULL,
	[startDate] [datetime] NULL,
	[endDate] [datetime] NULL,
	[createDate] [datetime] NULL,
	[modifiedDate] [datetime] NULL,
 CONSTRAINT [PK__Budgets__1E2B71168CAEF7C1] PRIMARY KEY CLUSTERED 
(
	[budgetID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Debts]    Script Date: 11/26/2023 11:35:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Debts](
	[debtID] [int] IDENTITY(1,1) NOT NULL,
	[userID] [int] NULL,
	[debtName] [nvarchar](100) NULL,
	[debtAmount] [decimal](18, 2) NULL,
	[debtDueDate] [date] NULL,
	[description] [nvarchar](max) NULL,
	[isDebt] [bit] NULL,
	[debtStatus] [int] NULL,
	[repaymentPlan] [int] NULL,
	[createDate] [datetime] NULL,
	[modifiedDate] [datetime] NULL,
 CONSTRAINT [PK__Debts__020CB8EE33ABBC06] PRIMARY KEY CLUSTERED 
(
	[debtID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Expenses]    Script Date: 11/26/2023 11:35:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Expenses](
	[expenseID] [int] IDENTITY(1,1) NOT NULL,
	[userID] [int] NULL,
	[expenseName] [nvarchar](100) NULL,
	[expenseAmount] [decimal](18, 2) NULL,
	[expenseDate] [date] NULL,
	[description] [nvarchar](max) NULL,
	[categoryID] [int] NULL,
	[createDate] [datetime] NULL,
	[modifiedDate] [datetime] NULL,
 CONSTRAINT [PK_Expenses] PRIMARY KEY CLUSTERED 
(
	[expenseID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FinancialPlan]    Script Date: 11/26/2023 11:35:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FinancialPlan](
	[financialPlanID] [int] IDENTITY(1,1) NOT NULL,
	[userID] [int] NULL,
	[financialPlanName] [nvarchar](100) NULL,
	[endDate] [date] NULL,
	[startDate] [datetime] NULL,
	[description] [nvarchar](max) NULL,
	[progress] [decimal](18, 2) NULL,
	[status] [int] NULL,
	[cateogryID] [int] NULL,
	[createDate] [datetime] NULL,
	[modifiedDate] [datetime] NULL,
 CONSTRAINT [PK__Financia__1C9B4ECD53B32963] PRIMARY KEY CLUSTERED 
(
	[financialPlanID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FinancialPlanDetail]    Script Date: 11/26/2023 11:35:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FinancialPlanDetail](
	[financialPlanDetailID] [int] NOT NULL,
	[financialPlanID] [int] NULL,
	[taskID] [int] NULL,
 CONSTRAINT [PK_FinancialPlanDetail] PRIMARY KEY CLUSTERED 
(
	[financialPlanDetailID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[IncomeSources]    Script Date: 11/26/2023 11:35:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[IncomeSources](
	[incomeSourceID] [int] IDENTITY(1,1) NOT NULL,
	[userID] [int] NULL,
	[incomeSourceName] [nvarchar](100) NULL,
	[incomeSourceAmount] [decimal](18, 2) NULL,
	[categoryID] [int] NULL,
	[description] [nvarchar](max) NULL,
	[incomeSourceDate] [datetime] NULL,
	[createDate] [datetime] NULL,
	[modifiedDate] [datetime] NULL,
 CONSTRAINT [PK__IncomeSo__D4BE1DBFD19E34C1] PRIMARY KEY CLUSTERED 
(
	[incomeSourceID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Locations]    Script Date: 11/26/2023 11:35:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Locations](
	[locationID] [int] IDENTITY(1,1) NOT NULL,
	[locationNo] [nchar](255) NULL,
	[name] [nvarchar](50) NULL,
	[parent] [nchar](255) NULL,
	[levels] [int] NULL,
	[slug] [nvarchar](100) NULL,
	[nameWithType] [nvarchar](100) NULL,
	[type] [int] NULL,
	[description] [nvarchar](max) NULL,
	[createDate] [datetime] NULL,
	[modifiedDate] [datetime] NULL,
 CONSTRAINT [PK__Location__30646B0EBEC2B3D7] PRIMARY KEY CLUSTERED 
(
	[locationID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Reminders]    Script Date: 11/26/2023 11:35:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Reminders](
	[reminderID] [int] IDENTITY(1,1) NOT NULL,
	[userID] [int] NOT NULL,
	[reminderType] [nvarchar](50) NULL,
	[reminderDate] [datetime] NULL,
	[description] [nvarchar](max) NULL,
	[createDate] [datetime] NULL,
	[modifiedDate] [datetime] NULL,
 CONSTRAINT [PK__Reminder__09DAAA832E696EA5] PRIMARY KEY CLUSTERED 
(
	[reminderID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RoleCateogry]    Script Date: 11/26/2023 11:35:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RoleCateogry](
	[roleCategoryID] [int] IDENTITY(1,1) NOT NULL,
	[roleCategoryName] [nvarchar](50) NOT NULL,
	[description] [nvarchar](max) NULL,
	[createDate] [datetime] NULL,
	[modifiedDate] [datetime] NULL,
 CONSTRAINT [PK__RoleCate__38CA9BA9573FD40E] PRIMARY KEY CLUSTERED 
(
	[roleCategoryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Roles]    Script Date: 11/26/2023 11:35:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Roles](
	[roleID] [int] IDENTITY(1,1) NOT NULL,
	[roleName] [nvarchar](50) NOT NULL,
	[description] [nvarchar](max) NULL,
	[createDate] [datetime] NULL,
	[modifiedDate] [datetime] NULL,
 CONSTRAINT [PK__Roles__CD98460A941BBCF3] PRIMARY KEY CLUSTERED 
(
	[roleID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tasks]    Script Date: 11/26/2023 11:35:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tasks](
	[taskID] [int] IDENTITY(1,1) NOT NULL,
	[userID] [int] NULL,
	[taskName] [nvarchar](255) NOT NULL,
	[taskDescription] [nvarchar](max) NULL,
	[endDate] [datetime] NULL,
	[startDate] [datetime] NULL,
	[isCompleted] [bit] NOT NULL,
	[createDate] [datetime] NULL,
	[modifiedDate] [datetime] NULL,
 CONSTRAINT [PK_ToDo] PRIMARY KEY CLUSTERED 
(
	[taskID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Transactions]    Script Date: 11/26/2023 11:35:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Transactions](
	[transactionID] [int] IDENTITY(1,1) NOT NULL,
	[transactionName] [nvarchar](255) NULL,
	[userID] [int] NULL,
	[description] [nvarchar](max) NULL,
	[categoryID] [int] NULL,
	[createDate] [datetime] NULL,
 CONSTRAINT [PK__Transact__9B57CF5233093E80] PRIMARY KEY CLUSTERED 
(
	[transactionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 11/26/2023 11:35:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[userID] [int] IDENTITY(1,1) NOT NULL,
	[userName] [nvarchar](50) NOT NULL,
	[passWord] [nvarchar](255) NOT NULL,
	[firstName] [nvarchar](50) NULL,
	[lastName] [nvarchar](50) NULL,
	[sex] [nvarchar](10) NULL,
	[locationID] [nchar](255) NULL,
	[address] [nvarchar](255) NULL,
	[phone] [nchar](15) NULL,
	[birthday] [datetime] NULL,
	[avatar] [nvarchar](max) NULL,
	[district] [nchar](255) NULL,
	[ward] [nchar](255) NULL,
	[active] [bit] NULL,
	[saltPassword] [nchar](255) NULL,
	[roleID] [int] NULL,
	[createDate] [datetime] NULL,
	[lastLogin] [datetime] NULL,
	[modifiedDate] [datetime] NULL,
 CONSTRAINT [PK__Users__CB9A1CDF4D4B22AC] PRIMARY KEY CLUSTERED 
(
	[userID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

