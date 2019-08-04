USE [UserList]
GO

/****** Object:  Table [dbo].[Record]    Script Date: 30.07.2019 16:31:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Record](
	[RecordID] [int] IDENTITY(1,1) NOT NULL,
	[ProcessingTime] [datetime] NULL,
	[RowCount] [int] NULL,
	[SuccessRow] [int] NULL,
	[FileName] [nvarchar](150) NULL,
	[FilePath] [nvarchar](150) NULL,
	[Status] [int] NULL,
	[ErrorMessage] [nvarchar](200) NULL,
 CONSTRAINT [PK__Record__FBDF78C92DCD00DF] PRIMARY KEY CLUSTERED 
(
	[RecordID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


