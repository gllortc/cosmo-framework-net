/****** Object:  StoredProcedure [dbo].[sp_User_Logon]    Script Date: 21/06/2015 19:55:47 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_User_Logon] 
  @strLogin varchar(25)
AS
UPDATE USERS 
SET USRLOGONCOUNT = USRLOGONCOUNT+1, 
    USRLASTLOGON  = GetDate()
WHERE USRLOGIN = @strLogin
GO
/****** Object:  StoredProcedure [dbo].[sp_UserCancel]    Script Date: 21/06/2015 19:55:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_UserCancel]
  @lUserID int
AS

-- Elimina anuncios relacionados
DELETE FROM ANNOUNCES
WHERE ANNUSERID=@lUserID

-- Marca el usuario como eliminado
UPDATE USERS
SET USRMAIL      = '--' + USRMAIL + '--', 
    USRPWD       = '', 
    USRNAME      = '',
    USRCITY      = '', 
    USRCOUNTRYID = 0, 
    USRPHONE     = '', 
    USRMAIL2     = '', 
    USRDESC      = '', 
    USROPTIONS   = 0, 
    USRSTATUS    = 0
WHERE USRID=@lUserID
GO
/****** Object:  StoredProcedure [dbo].[sp_UserDelete]    Script Date: 21/06/2015 19:55:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_UserDelete]
  @lUserID int
AS

-- Elimina anuncios relacionados
DELETE FROM ANNOUNCES
WHERE ANNUSERID=@lUserID

-- Marca el usuario como eliminado
UPDATE USERS
SET USRMAIL      = '--' + USRMAIL + '--', 
    USRPWD       = '', 
    USRNAME      = '',
    USRCITY      = '', 
    USRCOUNTRYID = 0, 
    USRPHONE     = '', 
    USRMAIL2     = '', 
    USRDESC      = '', 
    USROPTIONS   = 0, 
    USRSTATUS    = 0
WHERE USRID=@lUserID
GO
/****** Object:  StoredProcedure [dbo].[sp_UserLogon]    Script Date: 21/06/2015 19:55:47 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_UserLogon]
  @strLogin    nvarchar(35), 
  @strPassword nvarchar(35)
AS
SELECT 
      USRID,
      USRLOGIN,
      USRMAIL,
      USRSTATUS
FROM 
      USERS
WHERE 
      LTrim(RTrim(Upper(USRLOGIN))) = LTrim(RTrim(Upper(@strLogin))) AND 
      LTrim(RTrim(USRPWD))          = LTrim(RTrim(@strPassword))
GO
/****** Object:  StoredProcedure [dbo].[sys_Users_AddNew]    Script Date: 21/06/2015 19:55:47 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
/* InforGEST ContentServer 1.2

sys_Users_AddNew login, password

Comprueba las credenciales de un usuario. Si son correctas, actualiza sus datos de acceso:
  USRID     - identificador de la cuenta
  USRLOGIN  - login
  USRMAIL   - cuenta de correo de contacto
  USRSTATUS - estado
*/
CREATE PROCEDURE [dbo].[sys_Users_AddNew]
  @USRLOGIN nvarchar(35),
  @USRMAIL nvarchar(255),
  @USRPWD nvarchar(35),
  @USRNAME nvarchar(64),
  @USRCITY nvarchar(64),
  @USRCOUNTRYID int,
  @USRPHONE nvarchar(25),
  @USRMAIL2 nvarchar(255),
  @USRDESC nvarchar(1024),
  @USROPTIONS int,
  @USRSTATUS int
AS

INSERT INTO USERS (USRLOGIN,USRMAIL,USRPWD,USRNAME,USRCITY,USRCOUNTRYID,USRPHONE,USRMAIL2,USRDESC,USROPTIONS,USRSTATUS) 
VALUES (@USRLOGIN,@USRMAIL,@USRPWD,@USRNAME,@USRCITY,@USRCOUNTRYID,@USRPHONE,@USRMAIL2,@USRDESC,@USROPTIONS,@USRSTATUS)
GO
/****** Object:  StoredProcedure [dbo].[sys_Users_Cancel]    Script Date: 21/06/2015 19:55:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/* InforGEST ContentServer 1.2

sys_Users_Cancel lUserID

Cancela una cuenta de usuario. Concretamente borra todos los datos menos el LOGIN 
(que queda ocupado) y memoriza el correo electrónico (para controles de seguridad).
*/
CREATE PROCEDURE [dbo].[sys_Users_Cancel]
  @lUserID int
AS

-- Elimina anuncios relacionados
DELETE
FROM
  ANNOUNCES
WHERE
  ANNUSERID=@lUserID

-- Marca el usuario como eliminado
UPDATE
  USERS
SET
  USRMAIL      = '--' + USRMAIL + '--',
  USRPWD       = '',
  USRNAME      = '',
  USRCITY      = '',
  USRCOUNTRYID = 0,
  USRPHONE     = '',
  USRMAIL2     = '',
  USRDESC      = '',
  USROPTIONS   = 0,
  USRSTATUS    = 0
WHERE
  USRID=@lUserID
GO
/****** Object:  StoredProcedure [dbo].[sys_Users_DoLogin]    Script Date: 21/06/2015 19:55:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/* InforGEST ContentServer 1.2

sys_Users_DoLogin login, password

Comprueba las credenciales de un usuario. Si son correctas, actualiza sus datos de acceso:
  USRID     - identificador de la cuenta
  USRLOGIN  - login
  USRMAIL   - cuenta de correo de contacto
  USRSTATUS - estado
*/
CREATE PROCEDURE [dbo].[sys_Users_DoLogin]
  @strLogin    nvarchar(35), 
  @strPassword nvarchar(35)
AS

-- recupera el registro del usuario con las credenciales
SELECT 
  USRID,
  USRLOGIN,
  USRMAIL,
  USRSTATUS
FROM 
  USERS
WHERE 
  LTrim(RTrim(Upper(USRLOGIN))) = LTrim(RTrim(Upper(@strLogin))) And
  LTrim(RTrim(USRPWD))          = LTrim(RTrim(@strPassword))

-- Si encuentra un registro, actualiza los datos
IF @@rowcount>0
BEGIN
  UPDATE 
    USERS 
  SET 
    USRLOGONCOUNT = USRLOGONCOUNT+1, 
    USRLASTLOGON  = GetDate()
  WHERE 
    USRLOGIN = @strLogin
END
GO
/****** Object:  StoredProcedure [dbo].[sys_Users_FindData]    Script Date: 21/06/2015 19:55:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/* InforGEST ContentServer 1.2

sys_Users_FindData login, mail

Averigua el número de logins y mails coincidentes con los proporcionados:
 NLOGIN - número de logins existentes
 NMAIL  - número de mails existentes
*/
CREATE PROCEDURE [dbo].[sys_Users_FindData] 
  @login  nvarchar(64),
  @mail   nvarchar(255)
AS
SELECT 
  (SELECT COUNT(*) FROM USERS WHERE LOWER(USRLOGIN)=@login) AS NLOGIN,
  (SELECT COUNT(*) FROM USERS WHERE LOWER(USRMAIL)=@mail) AS NMAIL
GO
/****** Object:  StoredProcedure [dbo].[sys_Users_GetUserByMail]    Script Date: 21/06/2015 19:55:47 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
/* InforGEST ContentServer 1.2

sys_Users_GetUserByMail mail

Obtiene los datos de un usuario a partir de una cuenta de correo electrónico.
*/
CREATE PROCEDURE [dbo].[sys_Users_GetUserByMail]
  @mail   nvarchar(255)
AS

SELECT * 
FROM USERS 
WHERE Lower(USRMAIL)=Lower(@mail)
GO
/****** Object:  StoredProcedure [dbo].[sys_Users_TestLogin]    Script Date: 21/06/2015 19:55:47 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
/* InforGEST ContentServer 1.2

sys_Users_TestLogin @login

Devuelve el número de cuentas que tienen un determinado LOGIN de usuario..
*/
CREATE PROCEDURE [dbo].[sys_Users_TestLogin]
  @login nvarchar(64)
AS

-- Marca el usuario como eliminado
SELECT Count(*) As NREGS 
FROM users 
WHERE Upper(USRLOGIN)=Upper(@login)
GO
/****** Object:  StoredProcedure [dbo].[sys_Users_TestMail]    Script Date: 21/06/2015 19:55:47 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
/* InforGEST ContentServer 1.2

sys_Users_TestMail @mail

Devuelve el número de cuentas que tienen un determinado MAIL de usuario..
*/
CREATE PROCEDURE [dbo].[sys_Users_TestMail]
  @mail nvarchar(512)
AS

-- Marca el usuario como eliminado
SELECT Count(*) As NREGS 
FROM users 
WHERE Lower(USRMAIL)=Lower(@mail)
GO
/****** Object:  Table [dbo].[COUNTRY]    Script Date: 21/06/2015 19:55:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[COUNTRY](
	[COUNTRYID] [int] NOT NULL,
	[COUNTRYNAME] [nvarchar](64) NOT NULL,
	[COUNTRYLSTDEF] [bit] NOT NULL,
 CONSTRAINT [PK_COUNTRY] PRIMARY KEY CLUSTERED 
(
	[COUNTRYID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[SYSFORMATRULES]    Script Date: 21/06/2015 19:55:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SYSFORMATRULES](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[AGENT] [nvarchar](64) NOT NULL,
	[DEVICETYPE] [int] NOT NULL,
	[FORMATID] [int] NOT NULL,
	[PRIORITY] [int] NOT NULL,
	[REDIRECTURL] [nvarchar](1024) NOT NULL,
	[FRDESC] [nvarchar](1024) NOT NULL,
	[FRDOWNLOAD] [bit] NOT NULL,
	[FRUPLOAD] [bit] NOT NULL,
	[FRJAVASCRIPT] [bit] NOT NULL,
	[FRCOOKIES] [bit] NOT NULL,
	[FRDEFAULT] [bit] NOT NULL,
 CONSTRAINT [PK_DMW_FORMATRULES] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[SYSLOG]    Script Date: 21/06/2015 19:55:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SYSLOG](
	[SLID] [int] IDENTITY(1,1) NOT NULL,
	[SLCONTEXT] [nvarchar](255) NOT NULL,
	[SLAPP] [nvarchar](255) NOT NULL,
	[SLERRCODE] [int] NOT NULL,
	[SLMESSAGE] [nvarchar](2048) NOT NULL,
	[SLWORKSPACE] [nvarchar](255) NOT NULL,
	[SLTYPE] [int] NOT NULL,
	[SLUSER] [nvarchar](64) NOT NULL,
	[SLDATE] [smalldatetime] NOT NULL,
 CONSTRAINT [PK_SYSLOG] PRIMARY KEY CLUSTERED 
(
	[SLID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[SYSTEMPLATEPARTS]    Script Date: 21/06/2015 19:55:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SYSTEMPLATEPARTS](
	[TEMPLATEID] [int] NOT NULL,
	[PART] [int] NOT NULL,
	[CONTENTS] [nvarchar](4000) NOT NULL,
 CONSTRAINT [PK_DMW_TEMPLATEPARTS] PRIMARY KEY CLUSTERED 
(
	[TEMPLATEID] ASC,
	[PART] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[SYSTEMPLATES]    Script Date: 21/06/2015 19:55:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SYSTEMPLATES](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[NAME] [nvarchar](128) NOT NULL,
	[DESCRIPTION] [nvarchar](2048) NOT NULL,
	[PLATFORMTYPE] [int] NOT NULL,
	[USEPAGEHEADDERS] [bit] NOT NULL,
	[RENDERER] [nvarchar](255) NOT NULL,
	[CREATED] [smalldatetime] NOT NULL,
	[UPDATED] [smalldatetime] NOT NULL,
	[OWNER] [nvarchar](64) NOT NULL,
 CONSTRAINT [PK_DMW_TEMPLATES] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[SYSUSERSMSG]    Script Date: 21/06/2015 19:55:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SYSUSERSMSG](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[FROMUSRID] [int] NOT NULL,
	[TOUSRID] [int] NOT NULL,
	[FROMIP] [nvarchar](20) NOT NULL,
	[SENDED] [smalldatetime] NULL,
	[SUBJECT] [nvarchar](512) NOT NULL,
	[STATUS] [int] NOT NULL,
	[BODY] [nvarchar](4000) NOT NULL,
	[THREADID] [int] NOT NULL,
 CONSTRAINT [PK_SYSUSERSMSG] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[SYSUSERSREL]    Script Date: 21/06/2015 19:55:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SYSUSERSREL](
	[USERID] [int] NOT NULL,
	[TOUSERID] [int] NOT NULL,
	[STATUS] [int] NOT NULL,
	[CREATED] [smalldatetime] NOT NULL,
	[UPDATED] [smalldatetime] NULL,
 CONSTRAINT [PK_SYSUSERSREL] PRIMARY KEY CLUSTERED 
(
	[USERID] ASC,
	[TOUSERID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[USERS]    Script Date: 21/06/2015 19:55:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[USERS](
	[USRID] [int] IDENTITY(1,1) NOT NULL,
	[USRLOGIN] [nvarchar](35) NOT NULL,
	[USRMAIL] [nvarchar](255) NOT NULL,
	[USRPWD] [nvarchar](35) NOT NULL,
	[USRNAME] [nvarchar](64) NOT NULL,
	[PIN] [nvarchar](128) NULL,
	[USRCITY] [nvarchar](64) NOT NULL,
	[USRCOUNTRYID] [int] NOT NULL,
	[USRPHONE] [nvarchar](25) NOT NULL,
	[USRMAIL2] [nvarchar](255) NOT NULL,
	[USRDESC] [nvarchar](1024) NOT NULL,
	[USROPTIONS] [int] NOT NULL,
	[USRSTATUS] [int] NOT NULL,
	[USRCREATED] [smalldatetime] NOT NULL,
	[USRLASTLOGON] [smalldatetime] NOT NULL,
	[USRLOGONCOUNT] [int] NOT NULL,
	[USROWNER] [nvarchar](35) NULL,
	[USRROLES] [nvarchar](1024) NOT NULL,
 CONSTRAINT [pk_Users_Login] PRIMARY KEY CLUSTERED 
(
	[USRLOGIN] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [idx_Users_Mail] UNIQUE NONCLUSTERED 
(
	[USRMAIL] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  View [dbo].[cs_sys_Today]    Script Date: 21/06/2015 19:55:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[cs_sys_Today] AS SELECT GETDATE( ) Today
GO
ALTER TABLE [dbo].[SYSFORMATRULES] ADD  CONSTRAINT [DF_DMW_FORMATRULES_AGENT]  DEFAULT ('') FOR [AGENT]
GO
ALTER TABLE [dbo].[SYSFORMATRULES] ADD  CONSTRAINT [DF_DMW_FORMATRULES_DEVICETYPE]  DEFAULT (0) FOR [DEVICETYPE]
GO
ALTER TABLE [dbo].[SYSFORMATRULES] ADD  CONSTRAINT [DF_DMW_FORMATRULES_PRIORITY]  DEFAULT (0) FOR [PRIORITY]
GO
ALTER TABLE [dbo].[SYSFORMATRULES] ADD  CONSTRAINT [DF_DMW_FORMATRULES_REDIRECT]  DEFAULT ('') FOR [REDIRECTURL]
GO
ALTER TABLE [dbo].[SYSFORMATRULES] ADD  CONSTRAINT [DF_SYSFORMATRULES_FRDESC]  DEFAULT ('') FOR [FRDESC]
GO
ALTER TABLE [dbo].[SYSFORMATRULES] ADD  CONSTRAINT [DF_SYSFORMATRULES_FRDOWNLOAD]  DEFAULT (0) FOR [FRDOWNLOAD]
GO
ALTER TABLE [dbo].[SYSFORMATRULES] ADD  CONSTRAINT [DF_SYSFORMATRULES_FRUPLOAD]  DEFAULT (0) FOR [FRUPLOAD]
GO
ALTER TABLE [dbo].[SYSFORMATRULES] ADD  CONSTRAINT [DF_SYSFORMATRULES_FRJAVASCRIPT]  DEFAULT (0) FOR [FRJAVASCRIPT]
GO
ALTER TABLE [dbo].[SYSFORMATRULES] ADD  CONSTRAINT [DF_SYSFORMATRULES_FRCOOKIES]  DEFAULT (0) FOR [FRCOOKIES]
GO
ALTER TABLE [dbo].[SYSFORMATRULES] ADD  CONSTRAINT [DF_SYSFORMATRULES_FRDEFAULT]  DEFAULT (0) FOR [FRDEFAULT]
GO
ALTER TABLE [dbo].[SYSLOG] ADD  CONSTRAINT [DF_SYSLOG_SLCONTEXT]  DEFAULT ('') FOR [SLCONTEXT]
GO
ALTER TABLE [dbo].[SYSLOG] ADD  CONSTRAINT [DF_SYSLOG_SLAPP]  DEFAULT ('') FOR [SLAPP]
GO
ALTER TABLE [dbo].[SYSLOG] ADD  CONSTRAINT [DF_SYSLOG_SLERRCODE]  DEFAULT (0) FOR [SLERRCODE]
GO
ALTER TABLE [dbo].[SYSLOG] ADD  CONSTRAINT [DF_SYSLOG_SLMESSAGE]  DEFAULT ('') FOR [SLMESSAGE]
GO
ALTER TABLE [dbo].[SYSLOG] ADD  CONSTRAINT [DF_SYSLOG_SLWORKSPACE]  DEFAULT ('') FOR [SLWORKSPACE]
GO
ALTER TABLE [dbo].[SYSLOG] ADD  CONSTRAINT [DF_SYSLOG_SLTYPE]  DEFAULT (0) FOR [SLTYPE]
GO
ALTER TABLE [dbo].[SYSLOG] ADD  CONSTRAINT [DF_SYSLOG_SLUSER]  DEFAULT ('SA') FOR [SLUSER]
GO
ALTER TABLE [dbo].[SYSLOG] ADD  CONSTRAINT [DF_SYSLOG_SLDATE]  DEFAULT (getdate()) FOR [SLDATE]
GO
ALTER TABLE [dbo].[SYSTEMPLATEPARTS] ADD  CONSTRAINT [DF_DMW_TEMPLATEPARTS_CONTENTS]  DEFAULT ('') FOR [CONTENTS]
GO
ALTER TABLE [dbo].[SYSTEMPLATES] ADD  CONSTRAINT [DF_DMW_TEMPLATES_DESCRIPTION]  DEFAULT ('') FOR [DESCRIPTION]
GO
ALTER TABLE [dbo].[SYSTEMPLATES] ADD  CONSTRAINT [DF_DMW_TEMPLATES_PLATFORMTYPE]  DEFAULT ((0)) FOR [PLATFORMTYPE]
GO
ALTER TABLE [dbo].[SYSTEMPLATES] ADD  CONSTRAINT [DF_DMW_TEMPLATES_USEPAGEHEADDERS]  DEFAULT ((1)) FOR [USEPAGEHEADDERS]
GO
ALTER TABLE [dbo].[SYSTEMPLATES] ADD  CONSTRAINT [DF_SYSTEMPLATES_RENDERER]  DEFAULT ('') FOR [RENDERER]
GO
ALTER TABLE [dbo].[SYSTEMPLATES] ADD  CONSTRAINT [DF_DMW_TEMPLATES_CREATED]  DEFAULT (getdate()) FOR [CREATED]
GO
ALTER TABLE [dbo].[SYSTEMPLATES] ADD  CONSTRAINT [DF_DMW_TEMPLATES_IPDATED]  DEFAULT (getdate()) FOR [UPDATED]
GO
ALTER TABLE [dbo].[SYSTEMPLATES] ADD  CONSTRAINT [DF_DMW_TEMPLATES_OWNER]  DEFAULT ('SA') FOR [OWNER]
GO
ALTER TABLE [dbo].[SYSUSERSMSG] ADD  CONSTRAINT [DF_SYSUSERSMSG_FROMIP]  DEFAULT ('') FOR [FROMIP]
GO
ALTER TABLE [dbo].[SYSUSERSMSG] ADD  CONSTRAINT [DF_SYSUSERSMSG_SENDED]  DEFAULT (getdate()) FOR [SENDED]
GO
ALTER TABLE [dbo].[SYSUSERSMSG] ADD  CONSTRAINT [DF_SYSUSERSMSG_SUBJECT]  DEFAULT ('') FOR [SUBJECT]
GO
ALTER TABLE [dbo].[SYSUSERSMSG] ADD  CONSTRAINT [DF_SYSUSERSMSG_STATUS]  DEFAULT ((0)) FOR [STATUS]
GO
ALTER TABLE [dbo].[SYSUSERSMSG] ADD  CONSTRAINT [DF_SYSUSERSMSG_BODY]  DEFAULT ('') FOR [BODY]
GO
ALTER TABLE [dbo].[SYSUSERSMSG] ADD  CONSTRAINT [DF_SYSUSERSMSG_THREADID]  DEFAULT ((0)) FOR [THREADID]
GO
ALTER TABLE [dbo].[SYSUSERSREL] ADD  CONSTRAINT [DF_SYSUSERSREL_STATUS]  DEFAULT ((0)) FOR [STATUS]
GO
ALTER TABLE [dbo].[SYSUSERSREL] ADD  CONSTRAINT [DF_SYSUSERSREL_CREATED]  DEFAULT (getdate()) FOR [CREATED]
GO
ALTER TABLE [dbo].[USERS] ADD  CONSTRAINT [DF_USERS_USRPUBNAME]  DEFAULT ('') FOR [USRNAME]
GO
ALTER TABLE [dbo].[USERS] ADD  CONSTRAINT [DF_USERS_PIN]  DEFAULT ('') FOR [PIN]
GO
ALTER TABLE [dbo].[USERS] ADD  CONSTRAINT [DF_USERS_USRPUBLOCATION]  DEFAULT ('') FOR [USRCITY]
GO
ALTER TABLE [dbo].[USERS] ADD  CONSTRAINT [DF_USERS_USRPUBCOUNTRYID]  DEFAULT (200) FOR [USRCOUNTRYID]
GO
ALTER TABLE [dbo].[USERS] ADD  CONSTRAINT [DF_USERS_USRPUBPHONE]  DEFAULT ('') FOR [USRPHONE]
GO
ALTER TABLE [dbo].[USERS] ADD  CONSTRAINT [DF_USERS_USRMAIL2]  DEFAULT ('') FOR [USRMAIL2]
GO
ALTER TABLE [dbo].[USERS] ADD  CONSTRAINT [DF_USERS_USRDESC]  DEFAULT ('') FOR [USRDESC]
GO
ALTER TABLE [dbo].[USERS] ADD  CONSTRAINT [DF_USERS_USRMAILING]  DEFAULT (1) FOR [USROPTIONS]
GO
ALTER TABLE [dbo].[USERS] ADD  CONSTRAINT [DF_USERS_USRMAILINGX]  DEFAULT (1) FOR [USRSTATUS]
GO
ALTER TABLE [dbo].[USERS] ADD  CONSTRAINT [DF_USERS_USRCREATED]  DEFAULT (getdate()) FOR [USRCREATED]
GO
ALTER TABLE [dbo].[USERS] ADD  CONSTRAINT [DF_USERS_USRLASTLOGON]  DEFAULT (getdate()) FOR [USRLASTLOGON]
GO
ALTER TABLE [dbo].[USERS] ADD  CONSTRAINT [DF_USERS_USRLOGONCOUNT]  DEFAULT (0) FOR [USRLOGONCOUNT]
GO
ALTER TABLE [dbo].[USERS] ADD  CONSTRAINT [DF_USERS_OWNER]  DEFAULT ('SA') FOR [USROWNER]
GO
ALTER TABLE [dbo].[USERS] ADD  CONSTRAINT [DF_USERS_USRROLES]  DEFAULT ('') FOR [USRROLES]
GO
