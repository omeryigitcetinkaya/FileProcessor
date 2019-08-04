USE [UserList]
GO

/****** Object:  Table [dbo].[UserList]    Script Date: 30.07.2019 16:32:11 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[UserList](
	[UserID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](150) NULL,
	[Surname] [nvarchar](150) NULL,
	[Telephone] [nvarchar](150) NULL,
	[Address] [nvarchar](150) NULL,
	[RecordID] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[UserList]  WITH CHECK ADD  CONSTRAINT [FK_RecordID] FOREIGN KEY([RecordID])
REFERENCES [dbo].[Record] ([RecordID])
GO

ALTER TABLE [dbo].[UserList] CHECK CONSTRAINT [FK_RecordID]
GO


