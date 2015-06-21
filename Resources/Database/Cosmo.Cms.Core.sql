/****** Object:  StoredProcedure [dbo].[cs_Ads_GetSectionAds]    Script Date: 21/06/2015 22:20:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[cs_Ads_GetSectionAds]
(@nSectionID int)
AS
SELECT ANNID,ANNUSERID,ANNDATE,ANNTITLE,ANNBODY,ANNPRICE,ANNNAME,ANNPHONE,ANNEMAIL,ANNURL
FROM  ANNOUNCES 
WHERE DateDiff(MONTH, ANNDATE, GetDate())<=1 And
      ANNFOLDERID=@nSectionID And 
      ANNDELETED=0
ORDER BY ANNDATE DESC

GO
/****** Object:  StoredProcedure [dbo].[cs_Ads_GetSections]    Script Date: 21/06/2015 22:20:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[cs_Ads_GetSections]
AS
SELECT ANNFLDRID,
       ANNFLDRNAME,
       ANNFLDRDESC, 
       (SELECT COUNT(*) AS NREGS FROM ANNOUNCES WHERE DateDiff(MONTH, ANNDATE, GetDate())<=1 And ANNFOLDERID=ANNFLDRID And ANNDELETED=0) AS NUMADS
FROM ANNFOLDERS
WHERE ANNFLDRENABLED=1
ORDER BY ANNFLDRNAME

GO
/****** Object:  StoredProcedure [dbo].[cs_Ads_GetUserContact]    Script Date: 21/06/2015 22:20:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/* InforGEST ContentServer 1.2

cs_Ads_GetUserContact userId

Obtiene los datos de contacto por defecto a mostrar para el autor registrado de un nuevo anuncio:

USRMAIL2 - cuenta de correo de contacto para anuncios
USRPHONE - teléfono de contacto para anuncios
*/
CREATE PROCEDURE [dbo].[cs_Ads_GetUserContact]
  @userId int
AS
SELECT USRMAIL2,USRPHONE
FROM USERS
WHERE USRID=@userId
GO
/****** Object:  StoredProcedure [dbo].[cs_Ads_SectionProperties]    Script Date: 21/06/2015 22:20:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/* InforGEST ContentServer 1.2

cs_Ads_SectionProperties sectionId

Obtiene las propiedades de una sección del tablón de anuncios:

ANNFLDRNAME - nombre de la sección
ANNFLDRDESC - descripción
*/
CREATE PROCEDURE [dbo].[cs_Ads_SectionProperties]
  @nSectionID int
AS
SELECT ANNFLDRNAME, ANNFLDRDESC
FROM AnnFolders
WHERE ANNFLDRID=@nSectionID
GO
/****** Object:  StoredProcedure [dbo].[cs_Docs_DocumentProperties]    Script Date: 21/06/2015 22:20:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/* InforGEST ContentServer 1.2

cs_Docs_DocumentProperties documentId

Obtiene las propiedades de un documento y actualiza las estadísticas de visualización:
  FOLDERNAME - nombre de la carpeta contenedora 
  DOCTITLE   - título
  DOCDESC    - descripción 
  DOCHTML    - contenido
  DOCDATE    - fecha de publicación
  DOCTYPE    - tipo
  DOCFILE    - archivo adjunto
  DOCPIC     - nombre de la imágen miniatura
*/
CREATE PROCEDURE [dbo].[cs_Docs_DocumentProperties]
    @documentid int
AS

-- Obtiene las propiedades del documento
SELECT 
  CMS_DOCFOLDERS.FOLDERNAME, 
  CMS_DOCS.DOCTITLE,
  CMS_DOCS.DOCDESC, 
  CMS_DOCS.DOCHTML, 
  CMS_DOCS.DOCDATE, 
  CMS_DOCS.DOCTYPE, 
  CMS_DOCS.DOCFILE,
  CMS_DOCS.DOCPIC
FROM 
  CMS_DOCFOLDERS INNER JOIN CMS_DOCS ON CMS_DOCFOLDERS.FOLDERID=CMS_DOCS.DOCFOLDER
WHERE 
  CMS_DOCS.DOCID=@documentid

-- Actualiza las estadísticas de visualización
UPDATE 
  CMS_DOCS
SET 
  DOCSHOWS=DOCSHOWS+1
WHERE 
  DOCID=@documentid
GO
/****** Object:  StoredProcedure [dbo].[cs_Docs_FolderProperties]    Script Date: 21/06/2015 22:20:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/* InforGEST ContentServer 1.2

cs_Docs_FolderProperties folderId

Obtiene las propiedades de una carpeta:
  FOLDERNAME - Nombre
  FOLDERDESC - descripción
*/
CREATE PROCEDURE [dbo].[cs_Docs_FolderProperties]
  @folderId int
AS 
SELECT
  FOLDERNAME,
  FOLDERDESC
FROM 
  CMS_DOCFOLDERS 
WHERE 
  FOLDERID=@folderId
GO
/****** Object:  StoredProcedure [dbo].[cs_Docs_GetDocumentImages]    Script Date: 21/06/2015 22:20:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/* InforGEST ContentServer 1.2

cs_Docs_GetDocumentImages documentId

Obtiene una lista de imágenes asociadas a un documento:
  IMGFILE     - nombre del archivo 
  IMGTHUMB    - nombre del archivo miniatura
  IMGTHWIDTH  - ancho de la imágen miniatura
  IMGTHHEIGTH - altura de la imágen miniatura
  IMGDESC     - pié de foto
  IMGAUTHORY  - firma digital
*/
CREATE PROCEDURE [dbo].[cs_Docs_GetDocumentImages]
  @DocumentID int
AS
SELECT 
  IMAGES.IMGFILE,
  IMAGES.IMGTHUMB,
  IMAGES.IMGTHWIDTH,
  IMAGES.IMGTHHEIGTH,
  IMAGES.IMGDESC,
  IMAGES.IMGAUTHORY 
FROM 
  CMS_DOCSIMAGES INNER JOIN Images ON CMS_DOCSIMAGES.IDIMGID=IMAGES.IMGID
WHERE 
  IDDOCID=@DocumentID
ORDER BY 
  IDORDER ASC
GO
/****** Object:  StoredProcedure [dbo].[cs_Docs_GetFolderDocs]    Script Date: 21/06/2015 22:20:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/* InforGEST ContentServer 1.2

cs_Docs_GetFolderDocs folderId

Obtiene un listado de documentos contenidos en una carpeta:
  DOCID      - identificador del documento 
  DOCSECTION - --deprecated--
  DOCFOLDER  - identificador de la carpeta contenedora
  DOCTITLE   - título
  DOCVIEWER  - script de visualización
  DOCPIC     - nombre del archivo de imágen miniatura
  DOCDESC    - descripción
  DOCFILE    - nombre del archivo adjunto
  DOCTYPE    - tipo
  DOCDATE    - fecha de creación
*/
CREATE PROCEDURE [dbo].[cs_Docs_GetFolderDocs]
  @folder_id int
AS 
SELECT 
  DOCID, 
  DOCSECTION, 
  @folder_id AS DOCFOLDER, 
  DOCTITLE,
  DOCVIEWER,
  DOCPIC, 
  DOCDESC,
  DOCFILE,
  DOCTYPE,
  DOCDATE
FROM
  CMS_DOCS
WHERE
  CMS_DOCS.DOCFOLDER=@folder_id And
  CMS_DOCS.DOCENABLED=1
ORDER BY
  CMS_DOCS.DOCDATE DESC
GO
/****** Object:  StoredProcedure [dbo].[cs_Docs_GetFolderHighlighted]    Script Date: 21/06/2015 22:20:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/* InforGEST ContentServer 1.2

cs_Docs_GetFolderHighlighted folderId

Obtiene una lista de los documentos destacados de una carpeta y subcarpetas:
  DOCID      - identificador
  DOCVIEWER  - script de visualización
  FOLDERNAME - nombre de la carpeta contenedora
  DOCPIC     - nombre del archivo de la imágen miniatura
  DOCTITLE   - título
  DOCDESC    - descripción
*/
CREATE PROCEDURE [dbo].[cs_Docs_GetFolderHighlighted]
  @folderId int
AS 
SELECT 
  CMS_DOCS.DOCID, 
  CMS_DOCS.DOCTITLE, 
  CMS_DOCS.DOCDESC,
  CMS_DOCS.DOCPIC, 
  CMS_DOCS.DOCVIEWER, 
  CMS_DOCFOLDERS.FOLDERNAME
FROM 
  CMS_DOCS INNER Join CMS_DOCFOLDERS On (CMS_DOCS.DOCFOLDER=CMS_DOCFOLDERS.FOLDERID)
WHERE 
  CMS_DOCS.DOCHIGHLIGHT=1 And
  CMS_DOCS.DOCENABLED=1 And 
  (CMS_DOCFOLDERS.FOLDERPARENTID=@folderId Or CMS_DOCFOLDERS.FOLDERID=@folderId)
ORDER BY 
  CMS_DOCS.DOCSECTION, 
  CMS_DOCS.DOCDATE DESC
GO
/****** Object:  StoredProcedure [dbo].[cs_Docs_GetHighlighted]    Script Date: 21/06/2015 22:20:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[cs_Docs_GetHighlighted]
AS
SELECT 
  DOCID,
  DOCFOLDER,
  DOCTITLE,
  DOCDESC,
  DOCPIC,
  DOCVIEWER,
  DOCDATE
FROM 
  CMS_DOCS
WHERE 
  DOCHIGHLIGHT=1 And 
  DOCENABLED=1
ORDER BY 
  DOCDATE DESC,
  DOCFOLDER ASC
GO
/****** Object:  StoredProcedure [dbo].[cs_Docs_GetSubfolders]    Script Date: 21/06/2015 22:20:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[cs_Docs_GetSubfolders]
(@parentId int)
AS
SELECT 
  FOLDERID, 
  FOLDERNAME,
  count(*) As NREGS
FROM 
  CMS_DOCS Inner Join CMS_DOCFOLDERS On (CMS_DOCS.DOCFOLDER=CMS_DOCFOLDERS.FOLDERID) 
WHERE 
  CMS_DOCFOLDERS.FOLDERPARENTID=@parentId And
  CMS_DOCFOLDERS.FOLDERENABLED=1
GROUP BY 
  FOLDERID, 
  FOLDERNAME
ORDER BY 
  FOLDERNAME ASC
GO
/****** Object:  StoredProcedure [dbo].[cs_Forum_ChannelGetThreads]    Script Date: 21/06/2015 22:20:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/* InforGEST ContentServer 1.2

cs_Forum_ChannelGetThreads channelId

Obtiene las propiedades de un thread:
 MSGCLOSED  - abierto/cerrado (1/0)
 FLDDATE    - fecha de creación
 FLDAUTO    - identificador del thread
 FLDTITLE   - título
 MSGNUMMSGS - número de respuestas
 FLDNAME    - nombre del autor (login)
*/
CREATE PROCEDURE [dbo].[cs_Forum_ChannelGetThreads]
  @channel_id int
AS
SELECT MSGCLOSED, FLDDATE, FLDAUTO, FLDTITLE, MSGNUMMSGS, FLDNAME, MSGLASTREPLY
FROM FORUM
WHERE FLDREPLY=0 AND MSGFORUMID=@channel_id
ORDER BY MSGLASTREPLY DESC, FLDDATE DESC

GO
/****** Object:  StoredProcedure [dbo].[cs_Forum_ChannelNumMsgs]    Script Date: 21/06/2015 22:20:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/* InforGEST ContentServer 1.2

cs_Forum_ChannelNumMsgs channelId

Obtiene las propiedades de un thread:
 NREGS  - número de mensajes de un canal
*/
CREATE PROCEDURE [dbo].[cs_Forum_ChannelNumMsgs]
  @ChannelID int
AS
SELECT Count(*) AS NREGS 
FROM FORUM
WHERE MSGFORUMID=@ChannelID AND FLDREPLY=0
GO
/****** Object:  StoredProcedure [dbo].[cs_Forum_ChannelProperties]    Script Date: 21/06/2015 22:20:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/* InforGEST ContentServer 1.2

cs_Forum_ChannelProperties channelId

Obtiene las propiedades de un canal:
 FORUMNAME    - nombre del canal
 FORUMDESC    - descripción del canal
 FORUMENABLED - abierto/cerrado (1/0)
*/
CREATE PROCEDURE [dbo].[cs_Forum_ChannelProperties] 
  @ChannelID int
AS
SELECT FORUMNAME, FORUMDESC, FORUMENABLED
FROM FORUMS 
WHERE FORUMID=@ChannelID
GO
/****** Object:  StoredProcedure [dbo].[cs_Forum_GetChannels]    Script Date: 21/06/2015 22:20:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/* InforGEST ContentServer 3.0

cs_Forum_GetChannels

Obtiene las propiedades de los canales activos del foro:
 FORUMID    - identificador del canal
 FORUMNAME  - nombre del canal
 FORUMDESC  - descripción del canal
 NMSGS      - número de mensajes del canal
*/
CREATE PROCEDURE [dbo].[cs_Forum_GetChannels]
     @forumstatus bit
AS

SELECT FORUMS.FORUMID,
        FORUMS.FORUMNAME,
        FORUMS.FORUMDESC,
        FORUMS.FORUMDATE,
        FORUMS.FORUMENABLED,
        FORUMS.FORUMOWNER,
        (SELECT Count(*) FROM FORUM WHERE FORUM.MSGFORUMID=FORUMS.FORUMID) as items 
FROM FORUMS 
WHERE FORUMENABLED=@forumstatus



GO
/****** Object:  StoredProcedure [dbo].[cs_Forum_GetThread]    Script Date: 21/06/2015 22:20:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- ContentServer 3.0
-- Author: Gerard Llort (G2)
-- Fecha:  01/06/2010
-- =============================================
CREATE PROCEDURE [dbo].[cs_Forum_GetThread] 
	@threadId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	select FORUM.fldAuto,		-- 0
	       FORUM.MSGUSERID,		-- 1
	       FORUM.MSGFORUMID,	-- 2
	       FORUM.MSGNUMMSGS,	-- 3
	       FORUM.FLDREPLY,		-- 4
	       FORUM.FLDNAME,		-- 5
	       FORUM.FLDTITLE,		-- 6
	       FORUM.FLDBODY,		-- 7
	       FORUM.FLDIP,			-- 8
	       FORUM.FLDDATE,		-- 9
	       FORUM.MSGLASTREPLY,	-- 10
	       FORUM.MSGTHREAD,		-- 11
	       FORUM.MSGIDENT,		-- 12
	       FORUM.MSGCLOSED,		-- 13
	       FORUM.MSGBBCODES		-- 14
	from FORUM
	where FORUM.fldAuto=@threadId or 
	      FORUM.FLDREPLY=@threadId
	order by FORUM.fldAuto asc
END
GO
/****** Object:  StoredProcedure [dbo].[cs_Forum_GetThreadsByPage]    Script Date: 21/06/2015 22:20:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- ContentServer 3.0
-- Obtiene los hilos de una página del foro.
-- Autor: Gerard Llort
-- Fecha: 01/07/2010
-- =============================================
CREATE PROCEDURE [dbo].[cs_Forum_GetThreadsByPage] 
	@channelId int, 
    @page int = 1,
    @itemsPerPage int = 40
AS
BEGIN
    
    -- for each column in your sort, you need a variable to hold
    -- the "starting values". In our case, we need two:
    declare @startingDate datetime;

    -- again, we want to returns results from row @a to row @b:
    declare @a int;
    declare @b int;

    set @a = ((@page - 1) * @itemsPerPage) + 1 -- start at row 200
    set @b = @a + @itemsPerPage                -- end at row 250

    -- get the starting date and starting ID to return results:
    set rowcount @a

    select @StartingDate = FORUM.MSGLASTREPLY
    from FORUM
    where FORUM.FLDREPLY = 0 AND FORUM.MSGFORUMID=@channelId 
    order by FORUM.MSGLASTREPLY DESC

    -- find out how many rows to return, and set the rowcount:
    set @b = 1 + @b - @a
    set rowcount @b

    -- now return the results:
    select FORUM.FLDAUTO,
           FORUM.MSGFORUMID,
           FORUM.FLDTITLE,
           FORUM.FLDDATE,
           FORUM.MSGLASTREPLY,
           FORUM.MSGNUMMSGS,
           FORUM.FLDNAME,
           FORUM.MSGUSERID,
           FORUM.MSGCLOSED 
    from FORUM
    where FORUM.FLDREPLY=0 AND 
          FORUM.MSGFORUMID=@channelId AND
          FORUM.MSGLASTREPLY <= @StartingDate 
    order by FORUM.MSGLASTREPLY DESC

    -- clean up:
    set rowcount 0 
    
END


GO
/****** Object:  StoredProcedure [dbo].[cs_Forum_NewThread]    Script Date: 21/06/2015 22:20:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/* InforGEST ContentServer 1.2

cs_Forum_NewThread Title, Body, Login, Mail, IPAdress, channelId, userID

Genera un nuevo thread en un determinado canal

*/
CREATE PROCEDURE [dbo].[cs_Forum_NewThread]
  @sTitle   nvarchar(512),
  @sBody    ntext,
  @sUser    nvarchar(32),
  @sMail    nvarchar(255),
  @sIPAdd   nvarchar(25),
  @iChannel int,
  @iUserID  int,
  @bbcodes  bit
AS

-- Inserta el registro padre correspondiente al thread 
INSERT INTO FORUM (MSGFORUMID,MSGUSERID,MSGNUMMSGS,FLDREPLY,FLDNAME,FLDEMAIL,FLDTITLE,FLDBODY,FLDIP,FLDDATE,MSGLASTREPLY,MSGTHREAD,MSGIDENT,MSGBBCODES)
VALUES (@iChannel,@iUserID,1,0,@sUser,@sMail,@sTitle,@sBody,@sIPAdd,GetDate(),GetDate(),0,0,@bbcodes)

-- Actualiza la información del ID del thread
UPDATE FORUM
SET MSGTHREAD = (SELECT Max(FLDAUTO) AS THREAD_ID FROM FORUM WHERE MSGFORUMID=@iChannel)
WHERE FLDAUTO = (SELECT Max(FLDAUTO) AS THREAD_ID FROM FORUM WHERE MSGFORUMID=@iChannel)

GO
/****** Object:  StoredProcedure [dbo].[cs_Forum_ThreadAddPost]    Script Date: 21/06/2015 22:20:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/* InforGEST ContentServer 1.2

cs_Forum_ThreadAddPost threadId, body, login, mail, IPAdress, channelId, userID

Agrega una nueva respuesta a un determinado thread

*/
CREATE PROCEDURE [dbo].[cs_Forum_ThreadAddPost]
  @iThread  int,
  @sBody    ntext,
  @sUser    nvarchar(32),
  @sMail    nvarchar(255),
  @sIPAdd   nvarchar(25),
  @iChannel int,
  @iUserID  int,
  @bbcodes  bit
AS

-- Inserta el registro padre correspondiente al thread
INSERT INTO FORUM (MSGFORUMID,MSGUSERID,MSGNUMMSGS,FLDREPLY,FLDNAME,FLDEMAIL,FLDTITLE,FLDBODY,FLDIP,FLDDATE,MSGLASTREPLY,MSGTHREAD,MSGIDENT,MSGBBCODES)
VALUES (@iChannel,@iUserID,0,@iThread,@sUser,@sMail,'',@sBody,@sIPAdd,GetDate(),GetDate(),@iThread,0,@bbcodes)

-- Actualiza la información de la cabecera del thread
UPDATE FORUM
SET MSGNUMMSGS   = MSGNUMMSGS + 1,
    MSGLASTREPLY = GetDate()
WHERE FLDAUTO = @iThread


GO
/****** Object:  StoredProcedure [dbo].[cs_Forum_ThreadGetMsgs]    Script Date: 21/06/2015 22:20:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/* InforGEST ContentServer 1.2

cs_Forum_ThreadGetMsgs channelId, threadId

Obtiene todos los mensajes de un hilo del foro:

FLDAUTO   - identificador del mensaje
MSGUSERID - identificador de la cuenta de usuario
FLDNAME   - login del autor
FLDBODY   - mensaje de texto
FLDDATE   - fecha/hora

*/
CREATE PROCEDURE [dbo].[cs_Forum_ThreadGetMsgs]
  @channel_id int,
  @thread_id  int
AS
SELECT FLDAUTO, MSGUSERID, FLDNAME, FLDBODY, FLDDATE 
FROM FORUM 
WHERE (MSGFORUMID=@channel_id And MSGTHREAD=@thread_id) Or 
      (FLDAUTO=@thread_id) 
ORDER BY FLDAUTO ASC
GO
/****** Object:  StoredProcedure [dbo].[cs_Forum_ThreadProperties]    Script Date: 21/06/2015 22:20:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/* InforGEST ContentServer 1.2

cs_Forum_ThreadProperties channelId, threadId

Obtiene las propiedades de un thread:
 FLDTITLE   - título
 FLDBODY    - mensaje original
 MSGCLOSED  - abierto/cerrado (1/0)
 FLDDATE    - fecha de creación
 NREGS      - respuestas
 FORUMNAME  - título del canal
*/
CREATE PROCEDURE [dbo].[cs_Forum_ThreadProperties]
  @ChannelID int,
  @ThreadID  int
AS
SELECT FLDTITLE,
       FLDBODY,
       MSGCLOSED,
       FLDDATE,
       (SELECT Count(*) FROM FORUM WHERE MSGTHREAD=@ThreadID) AS NREGS,
       (SELECT TOP 1 FORUMNAME FROM FORUMS WHERE FORUMID=@ChannelID) AS FORUMNAME
FROM FORUM
WHERE FLDAUTO=@ThreadID
GO
/****** Object:  StoredProcedure [dbo].[cs_Images_GetFolderImages]    Script Date: 21/06/2015 22:20:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/* InforGEST ContentServer 1.2

cs_Images_GetFolderImages folderId

Obtiene la lista de imágenes contenidas en una carpeta:
  IMGID       - identificador de la imágen
  IMGFILE     - nombre del archivo de la imágen
  IMGTEMPLATE - script de visualización
  IMGTHUMB    - nombre del archivo de la imágen miniatura
  IMGTHWIDTH  - anchura en píxels de la imágen miniatura
  IMGTHHEIGTH - altura en píxels de la imágen miniatura
  IMGDESC     - pié de foto
  IMGAUTHORY  - firma digital
*/
CREATE PROCEDURE [dbo].[cs_Images_GetFolderImages]
  @folderId int
AS
SELECT 
  IMGID,
  IMGTEMPLATE,
  IMGFILE,
  IMGTHUMB,
  IMGTHWIDTH,
  IMGTHHEIGTH,
  IMGDESC,
  IMGAUTHORY
FROM 
  IMAGES
WHERE 
  IMFOLDER=@folderId
ORDER BY 
  IMGDATE DESC
GO
/****** Object:  StoredProcedure [dbo].[cs_Images_GetNewImages]    Script Date: 21/06/2015 22:20:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/* InforGEST ContentServer 1.2

cs_Images_GetNewImages

Obtiene una lista de las fotografias añadidas los últimos 15 días:
  IMGFILE     - nombre del archivo de la imágen
  IMGTHUMB    - nombre del archivo de la imágen miniatura
  IMGTHWIDTH  - ancho de la imágen miniatura
  IMGTHHEIGTH - altura de la imágen miniatura
  IMGDESC     - pié de foto
  IMGAUTHORY  - firma digital
*/
CREATE PROCEDURE [dbo].[cs_Images_GetNewImages]
AS
SELECT 
  IMGFILE,
  IMGTHUMB,
  IMGTHWIDTH,
  IMGTHHEIGTH,
  IMGDESC,
  IMGAUTHORY
FROM 
  IMAGES
WHERE 
  DateDiff(Day, IMGDATE, GetDate()) <= 15
ORDER BY 
  IMGDATE DESC
GO
/****** Object:  StoredProcedure [dbo].[cs_Images_GetSubfolders]    Script Date: 21/06/2015 22:20:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[cs_Images_GetSubfolders]
(@folderId int)
AS
SELECT
  IFID,
  @folderId As IFPARENTID,
  IFLINK,
  IFNAME
FROM 
  IMAGEFOLDERS 
WHERE 
  IFENABLED=1 And 
  IFPARENTID=@folderId
ORDER BY 
  IFORDER ASC,
  IFNAME ASC
GO
/****** Object:  StoredProcedure [dbo].[cs_Images_ImageProperties]    Script Date: 21/06/2015 22:20:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/* InforGEST ContentServer 1.2

cs_Images_ImageProperties @imageId

Obtiene las propiedades de un documento y actualiza las estadísticas de visualización:
  IMGFILE    - nombre del archivo de imágen
  IMGWIDTH   - ancho en píxels
  IMGHEIGHT  - altura en pixels
  IMGDESC    - pié de foto
  IMGAUTHORY - firma digital
*/
CREATE PROCEDURE [dbo].[cs_Images_ImageProperties]
  @imageId int
AS

-- Obtiene las propiedades
SELECT
  IMGFILE,
  IMGWIDTH,
  IMGHEIGHT,
  IMGDESC,
  IMGAUTHORY
FROM
  IMAGES
WHERE 
  IMGID=@imageId

-- Actualiza las estadísticas de visualización
UPDATE 
  IMAGES
SET
  IMGSHOWS=IMGSHOWS+1
WHERE
  IMGID=@imageId
GO
/****** Object:  UserDefinedFunction [dbo].[cs_Banners_GetCurrentShows]    Script Date: 21/06/2015 22:20:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[cs_Banners_GetCurrentShows] 
(
	@bannerid int
)
RETURNS int
AS
BEGIN

--==========================================================
-- Obtiene el número de veces que se ha mostrado un banner
-- en el dia actual.
------------------------------------------------------------
-- parámetros:
--   @bannerid - Identificador del banner.
------------------------------------------------------------
-- Autor:
--   Gerard Llort Casanova
-- Copyright (c) 2009 G2 Software
--==========================================================

	-- Declare the return variable here
	DECLARE @shows INT
	DECLARE @today SMALLDATETIME

	-- Obtiene el dia actual
	SELECT @today=today 
	FROM cs_sys_Today

	-- Obtiene el número de veces que se ha mostrado el banner
	select @shows=SHOWS
	from STD_BANNER 
	where DATEDIFF(DAY, STDDATE, @today) = 0 AND BANNERID=@bannerid

	-- Return the result of the function
	IF @@ROWCOUNT>0 RETURN @shows
	RETURN 0

END

GO
/****** Object:  UserDefinedFunction [dbo].[cs_Forum_GetThreadID]    Script Date: 21/06/2015 22:20:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[cs_Forum_GetThreadID] (@msgID int, @replyID int)  
RETURNS int AS  
BEGIN 

DECLARE @thread int

IF (@replyID = 0)
  SET @thread = @msgID
ELSE
  SET @thread = @replyID

RETURN @thread

END
GO
/****** Object:  UserDefinedFunction [dbo].[cs_Forum_GetThreadTitle]    Script Date: 21/06/2015 22:20:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[cs_Forum_GetThreadTitle] (@threadID int)  
RETURNS nvarchar(512) AS  
BEGIN 

DECLARE @Title  varchar(512)
DECLARE @Thread int

SELECT @Title = FLDTITLE, @Thread = FLDREPLY
FROM FORUM
WHERE FLDAUTO = @threadID

IF (@Title = '')
BEGIN
  SELECT @Title = FLDTITLE 
  FROM FORUM
  WHERE FLDAUTO = @Thread
END

RETURN @Title

END
GO
/****** Object:  Table [dbo].[ANNFOLDERS]    Script Date: 21/06/2015 22:20:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ANNFOLDERS](
	[ANNFLDRID] [int] IDENTITY(1,1) NOT NULL,
	[ANNFLDRNAME] [nvarchar](64) NOT NULL,
	[ANNFLDRDESC] [nvarchar](255) NOT NULL,
	[ANNFLDRENABLED] [bit] NOT NULL,
	[ANNFLDRLSTDEFAULT] [bit] NOT NULL,
	[ANNFLDRNOTSELECTABLE] [bit] NOT NULL,
 CONSTRAINT [PK_ANNFOLDERS] PRIMARY KEY CLUSTERED 
(
	[ANNFLDRID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ANNOUNCES]    Script Date: 21/06/2015 22:20:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ANNOUNCES](
	[ANNID] [int] IDENTITY(1,1) NOT NULL,
	[ANNDATE] [smalldatetime] NOT NULL,
	[ANNFOLDERID] [int] NOT NULL,
	[ANNTITLE] [nvarchar](64) NOT NULL,
	[ANNBODY] [ntext] NOT NULL,
	[ANNNAME] [nvarchar](64) NOT NULL,
	[ANNPHONE] [nvarchar](12) NOT NULL,
	[ANNEMAIL] [nvarchar](255) NOT NULL,
	[ANNURL] [nvarchar](1024) NOT NULL,
	[ANNPASSWORD] [nvarchar](32) NOT NULL,
	[ANNDELETED] [bit] NOT NULL,
	[ANNOWNER] [nvarchar](64) NOT NULL,
	[ANNUSERID] [int] NOT NULL,
	[ANNPRICE] [money] NOT NULL,
 CONSTRAINT [PK_ANNOUNCES] PRIMARY KEY CLUSTERED 
(
	[ANNID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[BANNERS]    Script Date: 21/06/2015 22:20:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BANNERS](
	[BANID] [int] IDENTITY(1,1) NOT NULL,
	[BANCLIENT] [nvarchar](64) NOT NULL,
	[BANIMAGE] [nvarchar](255) NOT NULL,
	[BANPAGE] [ntext] NOT NULL,
	[BANEMAIL] [nvarchar](255) NOT NULL,
	[BANDIRECTLINK] [bit] NOT NULL,
	[BANDIRECTURL] [nvarchar](255) NOT NULL,
	[BANDATE] [smalldatetime] NOT NULL,
	[BANTYPE] [int] NOT NULL,
	[BANSHOWS] [int] NOT NULL,
	[BANCLICKS] [int] NOT NULL,
	[BANENABLED] [bit] NOT NULL,
	[STARTDATE] [smalldatetime] NULL,
	[ENDDATE] [smalldatetime] NULL,
	[BANWIDTH] [int] NOT NULL,
	[BANHEIGHT] [int] NOT NULL,
 CONSTRAINT [PK_BANNERS] PRIMARY KEY CLUSTERED 
(
	[BANID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[CMS_DOCFOLDERS]    Script Date: 21/06/2015 22:20:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CMS_DOCFOLDERS](
	[FOLDERID] [int] IDENTITY(1,1) NOT NULL,
	[FOLDERPARENTID] [int] NOT NULL,
	[FOLDERNAME] [nvarchar](64) NOT NULL,
	[FOLDERDESC] [nvarchar](2048) NULL,
	[FOLDERSHOWTITLE] [bit] NOT NULL,
	[FOLDERSECTIONID] [int] NULL,
	[FOLDERORDER] [int] NOT NULL,
	[FOLDERCREATED] [smalldatetime] NOT NULL,
	[FOLDERENABLED] [bit] NOT NULL,
	[UPDATED] [smalldatetime] NOT NULL,
	[FOLDERMENU] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK_FOLDERS] PRIMARY KEY CLUSTERED 
(
	[FOLDERID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[CMS_DOCRELATIONS]    Script Date: 21/06/2015 22:20:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CMS_DOCRELATIONS](
	[DOCSOURCEID] [int] NOT NULL,
	[DOCDESTID] [int] NOT NULL,
 CONSTRAINT [PK_DOCRELATIONS] PRIMARY KEY CLUSTERED 
(
	[DOCSOURCEID] ASC,
	[DOCDESTID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[CMS_DOCS]    Script Date: 21/06/2015 22:20:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CMS_DOCS](
	[DOCID] [int] IDENTITY(1,1) NOT NULL,
	[DOCSECTION] [int] NOT NULL,
	[DOCFOLDER] [int] NOT NULL,
	[DOCTITLE] [nvarchar](64) NOT NULL,
	[DOCDESC] [nvarchar](255) NOT NULL,
	[DOCHTML] [ntext] NOT NULL,
	[DOCPIC] [nvarchar](255) NOT NULL,
	[DOCVIEWER] [nvarchar](255) NOT NULL,
	[DOCHIGHLIGHT] [bit] NOT NULL,
	[DOCENABLED] [bit] NOT NULL,
	[DOCDATE] [smalldatetime] NOT NULL,
	[DOCUPDATED] [smalldatetime] NOT NULL,
	[DOCSHOWS] [int] NOT NULL,
	[DOCTYPE] [int] NOT NULL,
	[DOCFILE] [nvarchar](255) NOT NULL,
	[DOCOWNER] [nvarchar](64) NOT NULL,
 CONSTRAINT [PK_DOCS] PRIMARY KEY CLUSTERED 
(
	[DOCID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[CMS_DOCSIMAGES]    Script Date: 21/06/2015 22:20:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CMS_DOCSIMAGES](
	[IDDOCID] [int] NOT NULL,
	[IDIMGID] [int] NOT NULL,
	[IDORDER] [int] NOT NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[FORUM]    Script Date: 21/06/2015 22:20:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FORUM](
	[FLDAUTO] [int] IDENTITY(1,1) NOT NULL,
	[MSGUSERID] [int] NOT NULL,
	[MSGFORUMID] [int] NOT NULL,
	[MSGNUMMSGS] [int] NOT NULL,
	[FLDREPLY] [int] NOT NULL,
	[FLDNAME] [nvarchar](50) NOT NULL,
	[FLDEMAIL] [nvarchar](50) NOT NULL,
	[FLDTITLE] [nvarchar](512) NOT NULL,
	[FLDBODY] [ntext] NOT NULL,
	[FLDIP] [nvarchar](25) NULL,
	[FLDDATE] [smalldatetime] NOT NULL,
	[MSGLASTREPLY] [smalldatetime] NULL,
	[MSGTHREAD] [int] NOT NULL,
	[MSGIDENT] [int] NOT NULL,
	[MSGCLOSED] [bit] NOT NULL,
	[MSGBBCODES] [bit] NOT NULL,
 CONSTRAINT [PK_FORUM] PRIMARY KEY CLUSTERED 
(
	[FLDAUTO] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[FORUMS]    Script Date: 21/06/2015 22:20:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FORUMS](
	[FORUMID] [int] IDENTITY(1,1) NOT NULL,
	[FORUMNAME] [nvarchar](64) NOT NULL,
	[FORUMDESC] [ntext] NOT NULL,
	[FORUMDATE] [smalldatetime] NOT NULL,
	[FORUMENABLED] [bit] NOT NULL,
	[FORUMOWNER] [nvarchar](64) NOT NULL,
 CONSTRAINT [PK_FORUMS] PRIMARY KEY CLUSTERED 
(
	[FORUMID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[IMAGEFOLDERS]    Script Date: 21/06/2015 22:20:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[IMAGEFOLDERS](
	[IFID] [int] IDENTITY(1,1) NOT NULL,
	[IFNAME] [nvarchar](128) NOT NULL,
	[IFPARENTID] [int] NOT NULL,
	[IFIMAGE] [nvarchar](255) NOT NULL,
	[IFIMGWIDTH] [int] NOT NULL,
	[IFIMGHEIGHT] [int] NOT NULL,
	[IFLINK] [bit] NOT NULL,
	[IFORDER] [int] NOT NULL,
	[IFHTML] [ntext] NOT NULL,
	[IFENABLED] [bit] NOT NULL,
	[IFOWNER] [nvarchar](64) NOT NULL,
	[IFFILEPATTERN] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK_IMAGEFOLDERS] PRIMARY KEY CLUSTERED 
(
	[IFID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[IMAGES]    Script Date: 21/06/2015 22:20:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[IMAGES](
	[IMGID] [int] IDENTITY(1,1) NOT NULL,
	[IMFOLDER] [int] NOT NULL,
	[IMGTEMPLATE] [nvarchar](255) NOT NULL,
	[IMGFILE] [nvarchar](255) NOT NULL,
	[IMGWIDTH] [int] NOT NULL,
	[IMGHEIGHT] [int] NOT NULL,
	[IMGTHUMB] [nvarchar](255) NOT NULL,
	[IMGTHWIDTH] [int] NOT NULL,
	[IMGTHHEIGTH] [int] NOT NULL,
	[IMGDESC] [nvarchar](4000) NOT NULL,
	[IMGAUTHORY] [nvarchar](1024) NOT NULL,
	[IMGUSERID] [int] NOT NULL,
	[IMGDATE] [smalldatetime] NOT NULL,
	[IMGSHOWS] [int] NOT NULL,
 CONSTRAINT [PK_IMAGES] PRIMARY KEY NONCLUSTERED 
(
	[IMGID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[STD_BANNER]    Script Date: 21/06/2015 22:20:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[STD_BANNER](
	[BANNERID] [int] NOT NULL,
	[STDDATE] [datetime] NOT NULL,
	[CLICKS] [int] NOT NULL,
	[SHOWS] [int] NOT NULL,
 CONSTRAINT [PK_STD_BANNER] PRIMARY KEY CLUSTERED 
(
	[BANNERID] ASC,
	[STDDATE] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[ANNOUNCES] ADD  CONSTRAINT [DF_ANNOUNCES_ANNDATE]  DEFAULT (getdate()) FOR [ANNDATE]
GO
ALTER TABLE [dbo].[ANNOUNCES] ADD  CONSTRAINT [DF_ANNOUNCES_ANNURL]  DEFAULT ('') FOR [ANNURL]
GO
ALTER TABLE [dbo].[ANNOUNCES] ADD  CONSTRAINT [DF_ANNOUNCES_ANNPASSWORD]  DEFAULT ('') FOR [ANNPASSWORD]
GO
ALTER TABLE [dbo].[ANNOUNCES] ADD  CONSTRAINT [DF_ANNOUNCES_ANNDELETED]  DEFAULT (0) FOR [ANNDELETED]
GO
ALTER TABLE [dbo].[ANNOUNCES] ADD  CONSTRAINT [DF_ANNOUNCES_ANNOWNER]  DEFAULT ('') FOR [ANNOWNER]
GO
ALTER TABLE [dbo].[ANNOUNCES] ADD  DEFAULT (0) FOR [ANNUSERID]
GO
ALTER TABLE [dbo].[ANNOUNCES] ADD  CONSTRAINT [DF_ANNOUNCES_ANNPRICE]  DEFAULT ((0)) FOR [ANNPRICE]
GO
ALTER TABLE [dbo].[BANNERS] ADD  CONSTRAINT [DF_BANNERS_BANCLIENT]  DEFAULT ('') FOR [BANCLIENT]
GO
ALTER TABLE [dbo].[BANNERS] ADD  CONSTRAINT [DF_BANNERS_BANIMAGE]  DEFAULT ('') FOR [BANIMAGE]
GO
ALTER TABLE [dbo].[BANNERS] ADD  CONSTRAINT [DF_BANNERS_BANPAGE]  DEFAULT ('') FOR [BANPAGE]
GO
ALTER TABLE [dbo].[BANNERS] ADD  CONSTRAINT [DF_BANNERS_BANEMAIL]  DEFAULT ('') FOR [BANEMAIL]
GO
ALTER TABLE [dbo].[BANNERS] ADD  CONSTRAINT [DF_BANNERS_BANDIRECTLINK]  DEFAULT (0) FOR [BANDIRECTLINK]
GO
ALTER TABLE [dbo].[BANNERS] ADD  CONSTRAINT [DF_BANNERS_BANDIRECTURL]  DEFAULT ('') FOR [BANDIRECTURL]
GO
ALTER TABLE [dbo].[BANNERS] ADD  CONSTRAINT [DF_BANNERS_BANDATE]  DEFAULT (getdate()) FOR [BANDATE]
GO
ALTER TABLE [dbo].[BANNERS] ADD  CONSTRAINT [DF_BANNERS_BANTYPE]  DEFAULT (0) FOR [BANTYPE]
GO
ALTER TABLE [dbo].[BANNERS] ADD  CONSTRAINT [DF_BANNERS_BANSHOWS]  DEFAULT (0) FOR [BANSHOWS]
GO
ALTER TABLE [dbo].[BANNERS] ADD  CONSTRAINT [DF_BANNERS_BANCLICKS]  DEFAULT (0) FOR [BANCLICKS]
GO
ALTER TABLE [dbo].[BANNERS] ADD  CONSTRAINT [DF_BANNERS_BANENABLED]  DEFAULT (0) FOR [BANENABLED]
GO
ALTER TABLE [dbo].[BANNERS] ADD  CONSTRAINT [DF_BANNERS_BANWIDTH]  DEFAULT ((0)) FOR [BANWIDTH]
GO
ALTER TABLE [dbo].[BANNERS] ADD  CONSTRAINT [DF_BANNERS_BANHEIGHT]  DEFAULT ((0)) FOR [BANHEIGHT]
GO
ALTER TABLE [dbo].[CMS_DOCFOLDERS] ADD  CONSTRAINT [DF_FOLDERS_FOLDERPARENTID]  DEFAULT ((0)) FOR [FOLDERPARENTID]
GO
ALTER TABLE [dbo].[CMS_DOCFOLDERS] ADD  CONSTRAINT [DF_FOLDERS_FOLDERDESC]  DEFAULT ('') FOR [FOLDERDESC]
GO
ALTER TABLE [dbo].[CMS_DOCFOLDERS] ADD  CONSTRAINT [DF_FOLDERS_FOLDERSHOWTITLE]  DEFAULT ((1)) FOR [FOLDERSHOWTITLE]
GO
ALTER TABLE [dbo].[CMS_DOCFOLDERS] ADD  CONSTRAINT [DF_FOLDERS_FOLDERSECTIONID]  DEFAULT ((0)) FOR [FOLDERSECTIONID]
GO
ALTER TABLE [dbo].[CMS_DOCFOLDERS] ADD  CONSTRAINT [DF_FOLDERS_FOLDERORDER]  DEFAULT ((0)) FOR [FOLDERORDER]
GO
ALTER TABLE [dbo].[CMS_DOCFOLDERS] ADD  CONSTRAINT [DF_FOLDERS_FOLDERCREATED]  DEFAULT (getdate()) FOR [FOLDERCREATED]
GO
ALTER TABLE [dbo].[CMS_DOCFOLDERS] ADD  CONSTRAINT [DF_FOLDERS_FOLDERENABLED]  DEFAULT ((1)) FOR [FOLDERENABLED]
GO
ALTER TABLE [dbo].[CMS_DOCFOLDERS] ADD  CONSTRAINT [DF_FOLDERS_UPDATED]  DEFAULT (getdate()) FOR [UPDATED]
GO
ALTER TABLE [dbo].[CMS_DOCFOLDERS] ADD  CONSTRAINT [DF_CMS_DOCFOLDERS_FOLDERMENU]  DEFAULT ('') FOR [FOLDERMENU]
GO
ALTER TABLE [dbo].[CMS_DOCS] ADD  CONSTRAINT [DF_DOCS_DOCFILE]  DEFAULT ('') FOR [DOCFILE]
GO
ALTER TABLE [dbo].[CMS_DOCS] ADD  CONSTRAINT [DF_DOCS_DOCOWNER]  DEFAULT ('sa') FOR [DOCOWNER]
GO
ALTER TABLE [dbo].[FORUM] ADD  CONSTRAINT [DF_FORUM_MSGUSERID]  DEFAULT (0) FOR [MSGUSERID]
GO
ALTER TABLE [dbo].[FORUM] ADD  CONSTRAINT [DF_FORUM_MSGFORUMID]  DEFAULT (1) FOR [MSGFORUMID]
GO
ALTER TABLE [dbo].[FORUM] ADD  CONSTRAINT [DF_FORUM_MSGNUMMSGS]  DEFAULT (0) FOR [MSGNUMMSGS]
GO
ALTER TABLE [dbo].[FORUM] ADD  CONSTRAINT [DF_FORUM_fldReply]  DEFAULT (0) FOR [FLDREPLY]
GO
ALTER TABLE [dbo].[FORUM] ADD  CONSTRAINT [DF_FORUM_fldDate_1]  DEFAULT (getdate()) FOR [FLDDATE]
GO
ALTER TABLE [dbo].[FORUM] ADD  CONSTRAINT [DF_FORUM_MSGLASTREPLY]  DEFAULT (getdate()) FOR [MSGLASTREPLY]
GO
ALTER TABLE [dbo].[FORUM] ADD  CONSTRAINT [DF_FORUM_msgIdent_1]  DEFAULT (0) FOR [MSGIDENT]
GO
ALTER TABLE [dbo].[FORUM] ADD  CONSTRAINT [DF_FORUM_MSGCLOSED]  DEFAULT (0) FOR [MSGCLOSED]
GO
ALTER TABLE [dbo].[FORUM] ADD  CONSTRAINT [DF_FORUM_MSGBBCODES]  DEFAULT ((0)) FOR [MSGBBCODES]
GO
ALTER TABLE [dbo].[FORUMS] ADD  CONSTRAINT [DF_FORUMS_FORUMDATE]  DEFAULT (getdate()) FOR [FORUMDATE]
GO
ALTER TABLE [dbo].[FORUMS] ADD  CONSTRAINT [DF_FORUMS_FORUMENABLED]  DEFAULT (1) FOR [FORUMENABLED]
GO
ALTER TABLE [dbo].[FORUMS] ADD  CONSTRAINT [DF_FORUMS_FORUMOWNER]  DEFAULT ('[SYS]') FOR [FORUMOWNER]
GO
ALTER TABLE [dbo].[IMAGEFOLDERS] ADD  CONSTRAINT [DF_IMAGEFOLDERS_IFHTML]  DEFAULT ('') FOR [IFHTML]
GO
ALTER TABLE [dbo].[IMAGEFOLDERS] ADD  CONSTRAINT [DF_IMAGEFOLDERS_IFENABLED]  DEFAULT (1) FOR [IFENABLED]
GO
ALTER TABLE [dbo].[IMAGEFOLDERS] ADD  CONSTRAINT [DF_IMAGEFOLDERS_IFOWNER]  DEFAULT ('SYS') FOR [IFOWNER]
GO
ALTER TABLE [dbo].[IMAGEFOLDERS] ADD  CONSTRAINT [DF_IMAGEFOLDERS_IFFILEPATTERN]  DEFAULT ('') FOR [IFFILEPATTERN]
GO
ALTER TABLE [dbo].[IMAGES] ADD  CONSTRAINT [DF_IMAGES_IMGDESC]  DEFAULT ('') FOR [IMGDESC]
GO
ALTER TABLE [dbo].[IMAGES] ADD  CONSTRAINT [DF_IMAGES_IMGAUTHORY]  DEFAULT ('') FOR [IMGAUTHORY]
GO
ALTER TABLE [dbo].[IMAGES] ADD  CONSTRAINT [DF_IMAGES_IMGUSERID]  DEFAULT ((0)) FOR [IMGUSERID]
GO
ALTER TABLE [dbo].[IMAGES] ADD  CONSTRAINT [DF_IMAGES_IMGDATE]  DEFAULT (getdate()) FOR [IMGDATE]
GO
ALTER TABLE [dbo].[IMAGES] ADD  CONSTRAINT [DF_IMAGES_IMGSHOWS]  DEFAULT ((0)) FOR [IMGSHOWS]
GO
ALTER TABLE [dbo].[STD_BANNER] ADD  CONSTRAINT [DF_STD_BANNER_CLICKS]  DEFAULT (0) FOR [CLICKS]
GO
ALTER TABLE [dbo].[STD_BANNER] ADD  CONSTRAINT [DF_STD_BANNER_SHOWS]  DEFAULT (0) FOR [SHOWS]
GO
ALTER TABLE [dbo].[ANNOUNCES]  WITH CHECK ADD  CONSTRAINT [FK_ANNOUNCES_ANNFOLDERS] FOREIGN KEY([ANNFOLDERID])
REFERENCES [dbo].[ANNFOLDERS] ([ANNFLDRID])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[ANNOUNCES] CHECK CONSTRAINT [FK_ANNOUNCES_ANNFOLDERS]
GO
ALTER TABLE [dbo].[CMS_DOCS]  WITH CHECK ADD  CONSTRAINT [FK_DOCS_FOLDERS] FOREIGN KEY([DOCFOLDER])
REFERENCES [dbo].[CMS_DOCFOLDERS] ([FOLDERID])
GO
ALTER TABLE [dbo].[CMS_DOCS] CHECK CONSTRAINT [FK_DOCS_FOLDERS]
GO
ALTER TABLE [dbo].[FORUM]  WITH CHECK ADD  CONSTRAINT [FK_FORUM_FORUMS] FOREIGN KEY([MSGFORUMID])
REFERENCES [dbo].[FORUMS] ([FORUMID])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[FORUM] CHECK CONSTRAINT [FK_FORUM_FORUMS]
GO
ALTER TABLE [dbo].[IMAGES]  WITH CHECK ADD  CONSTRAINT [FK_IMAGES_IMAGEFOLDERS] FOREIGN KEY([IMFOLDER])
REFERENCES [dbo].[IMAGEFOLDERS] ([IFID])
GO
ALTER TABLE [dbo].[IMAGES] CHECK CONSTRAINT [FK_IMAGES_IMAGEFOLDERS]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'IMAGES', @level2type=N'COLUMN',@level2name=N'IMGDATE'
GO
