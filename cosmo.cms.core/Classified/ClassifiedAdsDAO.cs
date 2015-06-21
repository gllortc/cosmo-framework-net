using Cosmo.Diagnostics;
using Cosmo.Net;
using Cosmo.Security;
using Cosmo.Security.Auth;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Net.Mail;
using System.Web.UI.WebControls;

namespace Cosmo.Cms.Classified
{

   /// <summary>
   /// Permite acceder y gestionar todos los elementos del Tablón de anuncios.
   /// </summary>
   public class ClassifiedAdsDAO
   {
      // Declaración de variables internas
      private Workspace _ws = null;

      #region Constants

      /// <summary>Nombre público del módulo.</summary>
      public const string SERVICE_NAME = "Anuncios clasificados";
      
      /// <summary>Nombre de la persona que desea establecer contacto.</summary>
      public const string TAG_CONTACT_NAME = "{contact.name}";
      /// <summary>Correo electrónico de la persona que desea establecer contacto.</summary>
      public const string TAG_CONTACT_MAIL = "{contact.mail}";
      /// <summary>Texto proporcionado por la persona que desea establecer contacto.</summary>
      public const string TAG_CONTACT_MESSAGE = "{contact.msg}";
      /// <summary>Nombre a mostrar del autor del anuncio.</summary>
      public const string TAG_CONTACT_AUTHORDISPLAYNAME = "{author.displayname}";
      /// <summary>Título del anuncio.</summary>
      public const string TAG_CONTACT_ADTITLE = "{ad.title}";
      /// <summary>Texto del anuncio.</summary>
      public const string TAG_CONTACT_ADBODY = "{ad.body}";
      /// <summary>Enlace extra del anuncio.</summary>
      public const string TAG_CONTACT_ADLINK = "{ad.link}";
      /// <summary>Enlace a la página de gestión de anuncios clasificados del autor.</summary>
      public const string TAG_CONTACT_ADMANAGELINK = "{ad.manage.link}";
      /// <summary>Nombre del workspace.</summary>
      public const string TAG_CONTACT_WSNAME = "{ws.name}";

      // Variables de configuración
      private const string CONF_ADS_VALIDITYDAYS = "cs.ads.validitydays";
      private const string CONF_ADS_MAXPERUSER = "cs.ads.usermaxads";
      private const string CONF_ADS_CONTACTMAIL_SUBJECT = "cs.ads.contactmail.subject";
      private const string CONF_ADS_CONTACTMAIL_BODY = "cs.ads.contactmail.body";

      // Fragmentos SQL reaprovechables
      private const string SQL_OBJECTS_TABLE_NAME = "announces";
      private const string SQL_OBJECTS_FIELDS = "annid,annuserid,anndate,annfolderid,anntitle,annbody,annname,annphone,annemail,annurl,anndeleted,annowner,annprice,DateDiff(dd, anndate, GetDate())";
      private const string SQL_FOLDERS_TABLE_NAME = "annfolders";

      #endregion

      #region Constructors

      /// <summary>
      /// Gets a new instance of <see cref="ClassifiedAdsDAO"/>.
      /// </summary>
      /// <param name="workspace">Una instancia de <see cref="Cosmo.Cms.Cms"/> que representa el sitio actual.</param>
      public ClassifiedAdsDAO(Workspace workspace)
      {
         _ws = workspace;
      }

      #endregion

      #region Settings

      /// <summary>
      /// Dias de validez de un anuncio publicado
      /// </summary>
      /// <remarks>Corresponde a la entrada cs.ads.validitymonths del archivo de configuración.</remarks>
      public int ValidityDays
      {
         get { return _ws.Settings.GetInt(CONF_ADS_VALIDITYDAYS); }
      }

      /// <summary>
      /// Devuelve el número máximo de anuncios por persona.
      /// </summary>
      public int MaxAdsPerUser
      {
         get { return _ws.Settings.GetInt(CONF_ADS_MAXPERUSER, 15); }
      }

      #endregion

      #region Methods

      /// <summary>
      /// Agrega un nuevo anuncio a una carpeta concreta.
      /// </summary>
      /// <param name="ad">Una instáncia de CSAd que contanga los datos del objeto.</param>
      /// <returns>Un valor booleano que indica si la operación ha tenido éxito.</returns>
      public void Add(ClassifiedAd ad)
      {
         string sql = string.Empty;
         SqlCommand cmd = null;
         SqlParameter param = null;

         // Comprueba que el usuario no tenga más anuncios de los permitidos
         int max = _ws.Settings.GetInt(CONF_ADS_MAXPERUSER, 15);
         if ((max > 0) && (max <= this.Count(ad.UserID)))
         {
            throw new TooManyUserObjectsException("Un mismo usuario no puede disponer de más de " + max + " anuncios.");
         }

         try
         {
            sql = "INSERT INTO " + SQL_OBJECTS_TABLE_NAME + " (annuserid,anndate,annfolderid,anntitle,annbody,annprice,annname,annphone,annemail,annurl,anndeleted,annowner) " +
                  "VALUES (@annuserid,getdate(),@annfolderid,@anntitle,@annbody,@annprice,@annname,@annphone,@annemail,@annurl,0,'" + SecurityService.ACCOUNT_SUPER + "')";

            // string sql = "INSERT INTO announces (annuserid,anndate,annfolderid,anntitle,annbody,annname,annphone,annemail,adprice,annurl,anndeleted,annowner) " +
            //              "VALUES (@annuserid,getdate(),@annfolderid,@anntitle,@annbody,@annname,@annphone,@annemail,@adprice,@annurl,0,'" + UserAuthentication.ACCOUNT_SUPER + "')";

            _ws.DataSource.Connect();

            cmd = new SqlCommand(sql, _ws.DataSource.Connection);

            param = new SqlParameter("@annuserid", SqlDbType.Int);
            param.Value = ad.UserID;
            cmd.Parameters.Add(param);

            param = new SqlParameter("@annfolderid", SqlDbType.Int);
            param.Value = ad.FolderID;
            cmd.Parameters.Add(param);

            param = new SqlParameter("@anntitle", SqlDbType.NVarChar, 64);
            param.Value = ad.Title;
            cmd.Parameters.Add(param);

            param = new SqlParameter("@annbody", SqlDbType.NText);
            param.Value = ad.Body;
            cmd.Parameters.Add(param);

            param = new SqlParameter("@annprice", SqlDbType.Money);
            param.Value = ad.Price;
            cmd.Parameters.Add(param);

            param = new SqlParameter("@annname", SqlDbType.NVarChar, 64);
            param.Value = ad.UserLogin;
            cmd.Parameters.Add(param);

            param = new SqlParameter("@annphone", SqlDbType.NVarChar, 12);
            param.Value = ad.Phone;
            cmd.Parameters.Add(param);

            param = new SqlParameter("@annemail", SqlDbType.NVarChar, 255);
            param.Value = ad.Mail;
            cmd.Parameters.Add(param);

            // param = new SqlParameter("@adprice", SqlDbType.Money);
            // param.Value = ad.Price;
            // cmd.Parameters.Add(param);

            param = new SqlParameter("@annurl", SqlDbType.NVarChar, 1024);
            param.Value = ad.URL;
            cmd.Parameters.Add(param);

            cmd.ExecuteNonQuery();
         }
         catch (Exception ex)
         {
            _ws.Logger.Add(new LogEntry(Cms.ProductName,
                                        GetType().Name + ".Add()",
                                        ex.Message,
                                        LogEntry.LogEntryType.EV_ERROR));
            throw ex;
         }
         finally
         {
            cmd.Dispose();
            _ws.DataSource.Disconnect();
         }
      }

      /// <summary>
      /// Actualiza los datos de un anuncio.
      /// </summary>
      /// <param name="ad">Una instáncia de CSAd que contanga los datos actualizados.</param>
      /// <remarks>No permite asignar el anuncio a otro usuario.</remarks>
      public void Update(ClassifiedAd ad)
      {
         string sql = "";
         SqlCommand cmd = null;

         try
         {
            sql = "UPDATE " + SQL_OBJECTS_TABLE_NAME + " " +
                  "SET anndate     = @anndate, " +
                  "    annfolderid = @annfolderid," +
                  "    anntitle    = @anntitle," +
                  "    annbody     = @annbody," +
                  "    annprice    = @annprice," +
                  "    annphone    = @annphone," +
                  "    annemail    = @annemail," +
                  "    annurl      = @annurl, " +
                  "    anndeleted  = @anndeleted " +
                  "WHERE annid = @annid";

            _ws.DataSource.Connect();

            cmd = new SqlCommand(sql, _ws.DataSource.Connection);
            cmd.Parameters.Add(new SqlParameter("@anndate", ad.Created));
            cmd.Parameters.Add(new SqlParameter("@annfolderid", ad.FolderID));
            cmd.Parameters.Add(new SqlParameter("@anntitle", ad.Title));
            cmd.Parameters.Add(new SqlParameter("@annbody", ad.Body));
            cmd.Parameters.Add(new SqlParameter("@annprice", ad.Price));
            cmd.Parameters.Add(new SqlParameter("@annphone", ad.Phone));
            cmd.Parameters.Add(new SqlParameter("@annemail", ad.Mail));
            cmd.Parameters.Add(new SqlParameter("@annurl", ad.URL));
            cmd.Parameters.Add(new SqlParameter("@anndeleted", false));
            cmd.Parameters.Add(new SqlParameter("@annid", ad.ID));
            cmd.ExecuteNonQuery();
         }
         catch (Exception ex)
         {
            _ws.Logger.Add(new LogEntry(Cms.ProductName,
                                        GetType().Name + ".Update()",
                                        ex.Message,
                                        LogEntry.LogEntryType.EV_ERROR));
            throw ex;
         }
         finally
         {
            cmd.Dispose();
            _ws.DataSource.Disconnect();
         }
      }

      /// <summary>
      /// Elimina un anuncio.
      /// </summary>
      /// <param name="adId">Identificador del anuncio.</param>
      /// <param name="deleteRow">Indica si sólo se marca como eliminado o se elimina el registro.</param>
      /// <returns>Un valor booleano que indica si la operación ha tenido éxito.</returns>
      public void Delete(int adId, bool deleteRow)
      {
         string sql;
         SqlCommand cmd = null;

         try
         {
            if (!deleteRow)
            {
               sql = "UPDATE " + SQL_OBJECTS_TABLE_NAME + " " +
                     "SET anndeleted=1 " +
                     "WHERE annid=@annid";
            }
            else
            {
               sql = "DELETE FROM " + SQL_OBJECTS_TABLE_NAME + " " +
                     "WHERE annid=@annid";
            }

            _ws.DataSource.Connect();
            cmd = new SqlCommand(sql, _ws.DataSource.Connection);
            cmd.Parameters.Add(new SqlParameter("@annid", adId));
            cmd.ExecuteNonQuery();
         }
         catch (Exception ex)
         {
            _ws.Logger.Add(new LogEntry(Cms.ProductName,
                                        GetType().Name + ".Delete()",
                                        ex.Message,
                                        LogEntry.LogEntryType.EV_ERROR));
            throw ex;
         }
         finally
         {
            cmd.Dispose();
            _ws.DataSource.Disconnect();
         }
      }

      /// <summary>
      /// Elimina un anuncio. Si está activo, lo marca como eliminado. Si está caducado, lo elimina definitivamente.
      /// </summary>
      /// <param name="adId">Identificador del anuncio.</param>
      /// <returns>Un valor booleano que indica si la operación ha tenido éxito.</returns>
      public void Delete(int adId)
      {
         string sql;
         SqlCommand cmd = null;

         try
         {
            // Obtiene las propiedades del anuncio
            ClassifiedAd ad = this.Item(adId);

            if (ad.Deleted)
            {
               sql = "DELETE FROM " + SQL_OBJECTS_TABLE_NAME + " " +
                     "WHERE annid=@annid";
            }
            else if (Cosmo.Utils.Calendar.DateDiff(Cosmo.Utils.Calendar.DateInterval.Day, ad.Created, DateTime.Now) > this.ValidityDays)
            {
               sql = "DELETE FROM " + SQL_OBJECTS_TABLE_NAME + " " +
                     "WHERE annid=@annid";
            }
            else
            {
               sql = "UPDATE " + SQL_OBJECTS_TABLE_NAME + " " +
                     "SET anndeleted=1 " +
                     "WHERE annid=@annid";
            }

            _ws.DataSource.Connect();

            cmd = new SqlCommand(sql, _ws.DataSource.Connection);
            cmd.Parameters.Add(new SqlParameter("@annid", adId));
            cmd.ExecuteNonQuery();
         }
         catch (Exception ex)
         {
            _ws.Logger.Add(new LogEntry(Cms.ProductName,
                                        GetType().Name + ".Delete()",
                                        ex.Message,
                                        LogEntry.LogEntryType.EV_ERROR));
            throw ex;
         }
         finally
         {
            cmd.Dispose();
            _ws.DataSource.Disconnect();
         }
      }

      /// <summary>
      /// Elimina todos los anuncios de un usuario.
      /// </summary>
      /// <param name="uid">Identificador del usuario.</param>
      /// <returns>Un valor booleano que indica si la operación ha tenido éxito.</returns>
      public void DeleteAllByUser(int uid)
      {
         string sql = string.Empty;
         SqlCommand cmd = null;

         try
         {
            _ws.DataSource.Connect();

            sql = "DELETE FROM " + SQL_OBJECTS_TABLE_NAME + " " +
                  "WHERE annuserid=@annuserid";

            cmd = new SqlCommand(sql, _ws.DataSource.Connection);
            cmd.Parameters.Add(new SqlParameter("@annuserid", uid));
            cmd.ExecuteNonQuery();
         }
         catch (Exception ex)
         {
            _ws.Logger.Add(new LogEntry(Cms.ProductName,
                                        GetType().Name + ".DeleteAll()",
                                        ex.Message,
                                        LogEntry.LogEntryType.EV_ERROR));
            throw ex;
         }
         finally
         {
            cmd.Dispose();
            _ws.DataSource.Disconnect();
         }
      }

      /// <summary>
      /// Recupera un anuncio.
      /// </summary>
      /// <param name="adId">Identificador del anuncio.</param>
      /// <returns>Una instáncia de la clase CSAd.</returns>
      public ClassifiedAd Item(int adId)
      {
         string sql = string.Empty;
         ClassifiedAd ad = null;
         SqlCommand cmd = null;
         SqlDataReader reader = null;

         try
         {
            _ws.DataSource.Connect();

            sql = "SELECT " + SQL_OBJECTS_FIELDS + " " +
                  "FROM " + SQL_OBJECTS_TABLE_NAME + " " +
                  "WHERE annid=@annid";

            // Obtiene las carpetas
            cmd = new SqlCommand(sql, _ws.DataSource.Connection);
            cmd.Parameters.Add(new SqlParameter("@annid", adId));
            reader = cmd.ExecuteReader();
            if (reader.Read())
            {
               ad = ReadClassifiedAd(reader);
            }
            reader.Close();

            return ad;
         }
         catch (Exception ex)
         {
            _ws.Logger.Add(new LogEntry(Cms.ProductName,
                                        GetType().Name + ".Item()",
                                        ex.Message,
                                        LogEntry.LogEntryType.EV_ERROR));
            throw ex;
         }
         finally
         {
            reader.Dispose();
            cmd.Dispose();
            _ws.DataSource.Disconnect();
         }
      }

      /// <summary>
      /// Cuenta todos los anuncios del servicio.
      /// </summary>
      /// <returns>Devuelve un número entero que representa número total de anuncios clasificados.</returns>
      public int Count()
      {
         string sql = string.Empty;
         SqlCommand cmd = null;

         try
         {
            _ws.DataSource.Connect();

            sql = "SELECT Count(*) As regs " +
                  "FROM " + SQL_OBJECTS_TABLE_NAME;

            cmd = new SqlCommand(sql, _ws.DataSource.Connection);
            return (int)cmd.ExecuteScalar();
         }
         catch (Exception ex)
         {
            _ws.Logger.Add(new LogEntry(Cms.ProductName,
                                        GetType().Name + ".Count()",
                                        ex.Message,
                                        LogEntry.LogEntryType.EV_ERROR));
            throw ex;
         }
         finally
         {
            cmd.Dispose();
            _ws.DataSource.Disconnect();
         }
      }

      /// <summary>
      /// Recupera los anuncios contenidos en una carpeta.
      /// </summary>
      /// <param name="enabled">Indica si se recuperan sólo los activos (true) o todos (false).</param>
      /// <returns>Un array de instáncias CSAds.</returns>
      public List<ClassifiedAd> Items(bool enabled)
      {
         string sql = string.Empty;
         List<ClassifiedAd> ads = new List<ClassifiedAd>();
         SqlCommand cmd = null;
         SqlDataReader reader = null;

         try
         {
            _ws.DataSource.Connect();

            sql = "SELECT " + SQL_OBJECTS_FIELDS + " " +
                  "FROM " + SQL_OBJECTS_TABLE_NAME + " " +
                  (enabled ? "WHERE anndeleted=0 AND DateDiff(mm, anndate, GetDate())<=@months" : string.Empty) +
                  "ORDER BY annid Desc";

            cmd = new SqlCommand(sql, _ws.DataSource.Connection);
            if (enabled) cmd.Parameters.Add(new SqlParameter("@months", this.ValidityDays));
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
               ads.Add(ReadClassifiedAd(reader));
            }
            reader.Close();

            return ads;
         }
         catch (Exception ex)
         {
            _ws.Logger.Add(new LogEntry(Cms.ProductName,
                                        GetType().Name + ".Items()",
                                        ex.Message,
                                        LogEntry.LogEntryType.EV_ERROR));
            throw ex;
         }
         finally
         {
            reader.Dispose();
            cmd.Dispose();
            _ws.DataSource.Disconnect();
         }
      }

      /// <summary>
      /// Recupera los anuncios contenidos en una carpeta.
      /// </summary>
      /// <param name="folderId">Identificador de la carpeta.</param>
      /// <param name="enabled">Indica si se recuperan sólo los activos (true) o todos (false).</param>
      /// <returns>Un array de instáncias CSAds.</returns>
      public List<ClassifiedAd> Items(int folderId, bool enabled)
      {
         string sql = string.Empty;
         List<ClassifiedAd> ads = new List<ClassifiedAd>();
         SqlCommand cmd = null;
         SqlDataReader reader = null;

         try
         {
            _ws.DataSource.Connect();

            sql = "SELECT " + SQL_OBJECTS_FIELDS + " " +
                  "FROM " + SQL_OBJECTS_TABLE_NAME + " " +
                  "WHERE annfolderid=@annfolderid And " +
                  (enabled ? "(anndeleted=0 AND DateDiff(dd, anndate, GetDate())<=@days)" : "(anndeleted=1 Or DateDiff(dd, anndate, GetDate())>@days)") + " " +
                  "ORDER BY anndate Desc";

            cmd = new SqlCommand(sql, _ws.DataSource.Connection);
            cmd.Parameters.Add(new SqlParameter("@annfolderid", folderId));
            cmd.Parameters.Add(new SqlParameter("@days", this.ValidityDays));
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
               ads.Add(ReadClassifiedAd(reader));
            }
            reader.Close();

            return ads;
         }
         catch (Exception ex)
         {
            _ws.Logger.Add(new LogEntry(Cms.ProductName,
                                        GetType().Name + ".Items()",
                                        ex.Message,
                                        LogEntry.LogEntryType.EV_ERROR));
            throw ex;
         }
         finally
         {
            reader.Dispose();
            cmd.Dispose();
            _ws.DataSource.Disconnect();
         }
      }

      /// <summary>
      /// Recupera los anuncios publicados por un usuario.
      /// </summary>
      /// <param name="uid">Identificador de usuario del autor.</param>
      /// <param name="enabled">Indica si se recuperan sólo los activos (true) o todos (false).</param>
      /// <returns>Un array de instáncias CSAds.</returns>
      public List<ClassifiedAd> Items(bool enabled, int uid)
      {
         string sql = string.Empty;
         List<ClassifiedAd> ads = new List<ClassifiedAd>();
         SqlCommand cmd = null;
         SqlDataReader reader = null;

         try
         {
            _ws.DataSource.Connect();

            sql = "SELECT " + SQL_OBJECTS_FIELDS + " " +
                  "FROM " + SQL_OBJECTS_TABLE_NAME + " " +
                  "WHERE annuserid=@annuserid " +
                  (enabled ? " And (DateDiff(dd, anndate, GetDate())<=@days AND anndeleted=0)" : string.Empty) + " " + // "(DateDiff(dd, anndate, GetDate())>@days OR anndeleted=1)") + " " +
                  "ORDER BY annid Desc";

            cmd = new SqlCommand(sql, _ws.DataSource.Connection);
            cmd.Parameters.Add(new SqlParameter("@annuserid", uid));
            if (enabled) cmd.Parameters.Add(new SqlParameter("@days", this.ValidityDays));
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
               ads.Add(ReadClassifiedAd(reader));
            }
            reader.Close();

            return ads;
         }
         catch (Exception ex)
         {
            _ws.Logger.Add(new LogEntry(Cms.ProductName,
                                        GetType().Name + ".Items()",
                                        ex.Message,
                                        LogEntry.LogEntryType.EV_ERROR));
            throw ex;
         }
         finally
         {
            reader.Dispose();
            cmd.Dispose();
            _ws.DataSource.Disconnect();
         }
      }

      /// <summary>
      /// Actualiza la fecha de publicación por la actual y marca el anuncio como no eliminado.
      /// </summary>
      /// <param name="adId">Identificador del anuncio.</param>
      /// <returns>Un valor booleano que indica si la operación ha tenido éxito.</returns>
      public void Publish(int adId)
      {
         string sql = string.Empty;
         SqlCommand cmd = null;

         try
         {
            _ws.DataSource.Connect();

            sql = "UPDATE " + SQL_OBJECTS_TABLE_NAME + " " +
                  "SET anndeleted = 0, " +
                      "anndate = GetDate() " +
                  "WHERE annid=@annid";

            cmd = new SqlCommand(sql, _ws.DataSource.Connection);
            cmd.Parameters.Add(new SqlParameter("@annid", adId));
            cmd.ExecuteNonQuery();
         }
         catch (Exception ex)
         {
            _ws.Logger.Add(new LogEntry(Cms.ProductName,
                                        GetType().Name + ".Publish()",
                                        ex.Message,
                                        LogEntry.LogEntryType.EV_ERROR));
            throw ex;
         }
         finally
         {
            cmd.Dispose();
            _ws.DataSource.Disconnect();
         }
      }

      /// <summary>
      /// Obtiene las propiedades de una carpeta. La propiedad Objects contiene sólo los anuncios activos.
      /// </summary>
      /// <param name="folderId">Idetificador de la carpeta solicitada.</param>
      /// <returns>Una instáncia de CSAdsFolder.</returns>
      public ClassifiedAdsSection GetFolder(int folderId)
      {
         string sql = string.Empty;
         ClassifiedAdsSection folder = null;
         SqlCommand cmd = null;
         SqlDataReader reader = null;

         try
         {
            _ws.DataSource.Connect();

            sql = "SELECT annfldrid,annfldrname,annfldrdesc,annfldrenabled,annfldrlstdefault,annfldrnotselectable," +
                         "(SELECT Count(*) AS nregs FROM " + SQL_OBJECTS_TABLE_NAME + " WHERE annfolderid=" + SQL_FOLDERS_TABLE_NAME + ".annfldrid And anndeleted=0 And DateDiff(dd, anndate, getdate())<=@days) As objects " +
                  "FROM " + SQL_FOLDERS_TABLE_NAME + " " +
                  "WHERE annfldrid=@annfldrid";

            cmd = new SqlCommand(sql, _ws.DataSource.Connection);
            cmd.Parameters.Add(new SqlParameter("@annfldrid", folderId));
            cmd.Parameters.Add(new SqlParameter("@days", this.ValidityDays));
            reader = cmd.ExecuteReader();
            if (reader.Read())
            {
               folder = new ClassifiedAdsSection();
               folder.ID = folderId;
               folder.Name = !reader.IsDBNull(1) ? reader.GetString(1) : string.Empty;
               folder.Description = !reader.IsDBNull(2) ? reader.GetString(2) : string.Empty;
               folder.IsListDefault = reader.GetBoolean(4);
               folder.IsNotSelectable = reader.GetBoolean(5);
               folder.Enabled = reader.GetBoolean(3);
               folder.Objects = reader.GetInt32(6);
            }
            reader.Close();

            return folder;
         }
         catch (Exception ex)
         {
            _ws.Logger.Add(new LogEntry(Cms.ProductName,
                                        GetType().Name + ".GetFolder()",
                                        ex.Message,
                                        LogEntry.LogEntryType.EV_ERROR));
            throw ex;
         }
         finally
         {
            reader.Dispose();
            cmd.Dispose();
            _ws.DataSource.Disconnect();
         }
      }

      /// <summary>
      /// Obtiene las carpetas.
      /// </summary>
      /// <param name="published">Indica si están o no publicadas.</param>
      /// <returns>Un array de objetos CSAdsFolder.</returns>
      public List<ClassifiedAdsSection> GetFolders(bool published)
      {
         string sql = string.Empty;
         ClassifiedAdsSection folder = null;
         List<ClassifiedAdsSection> folders = new List<ClassifiedAdsSection>();
         SqlCommand cmd = null;
         SqlDataReader reader = null;

         try
         {
            _ws.DataSource.Connect();

            sql = "SELECT annfldrid,annfldrname,annfldrdesc,annfldrenabled,annfldrlstdefault,annfldrnotselectable, " +
                         "(SELECT Count(*) FROM " + SQL_OBJECTS_TABLE_NAME + " WHERE annfolderid=" + SQL_FOLDERS_TABLE_NAME + ".annfldrid " + (published ? " And anndeleted=0 And DateDiff(dd, anndate, getdate())<=@days" : string.Empty) + ") " +
                  "FROM " + SQL_FOLDERS_TABLE_NAME + " " +
                  "WHERE annfldrenabled=@annfldrenabled " +
                  "ORDER BY annfldrname ASC";

            cmd = new SqlCommand(sql, _ws.DataSource.Connection);
            cmd.Parameters.Add(new SqlParameter("@annfldrenabled", published));
            if (published) cmd.Parameters.Add(new SqlParameter("@days", this.ValidityDays));
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
               folder = new ClassifiedAdsSection();
               folder.ID = reader.GetInt32(0);
               folder.Name = reader.GetString(1);
               folder.Description = !reader.IsDBNull(2) ? reader.GetString(2) : string.Empty;
               folder.IsListDefault = reader.GetBoolean(4);
               folder.IsNotSelectable = reader.GetBoolean(5);
               folder.Enabled = reader.GetBoolean(3);
               folder.Objects = reader.GetInt32(6);
               folders.Add(folder);
            }
            reader.Close();

            return folders;
         }
         catch (Exception ex)
         {
            _ws.Logger.Add(new LogEntry(Cms.ProductName,
                                        GetType().Name + ".GetFolders()",
                                        ex.Message,
                                        LogEntry.LogEntryType.EV_ERROR));
            throw ex;
         }
         finally
         {
            reader.Dispose();
            cmd.Dispose();
            _ws.DataSource.Disconnect();
         }
      }

      /// <summary>
      /// Obtiene todas las carpetas.
      /// </summary>
      /// <returns>Un array de objetos CSAdsFolder.</returns>
      public List<ClassifiedAdsSection> GetFolders()
      {
         string sql = string.Empty;
         ClassifiedAdsSection folder = null;
         List<ClassifiedAdsSection> folders = new List<ClassifiedAdsSection>();
         SqlCommand cmd = null;
         SqlDataReader reader = null;

         try
         {
            _ws.DataSource.Connect();

            sql = "SELECT annfldrid,annfldrname,annfldrdesc,annfldrenabled,annfldrlstdefault,annfldrnotselectable, " +
                         "(SELECT Count(*) FROM " + SQL_OBJECTS_TABLE_NAME + " WHERE annfolderid=" + SQL_FOLDERS_TABLE_NAME + ".annfldrid) As objects " +
                  "FROM " + SQL_FOLDERS_TABLE_NAME + " " +
                  "ORDER BY annfldrname ASC";

            // Recupera las carpetas
            cmd = new SqlCommand(sql, _ws.DataSource.Connection);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
               folder = new ClassifiedAdsSection();
               folder.ID = reader.GetInt32(0);
               folder.Name = reader.GetString(1);
               folder.Description = !reader.IsDBNull(2) ? reader.GetString(2) : string.Empty;
               folder.IsListDefault = reader.GetBoolean(4);
               folder.IsNotSelectable = reader.GetBoolean(5);
               folder.Enabled = reader.GetBoolean(3);
               folder.Objects = reader.GetInt32(6);

               folders.Add(folder);
            }
            reader.Close();

            return folders;
         }
         catch (Exception ex)
         {
            _ws.Logger.Add(new LogEntry(Cms.ProductName,
                                        GetType().Name + ".GetFolders()",
                                        ex.Message,
                                        LogEntry.LogEntryType.EV_ERROR));
            throw ex;
         }
         finally
         {
            reader.Dispose();
            cmd.Dispose();
            _ws.DataSource.Disconnect();
         }
      }

      /// <summary>
      /// Rellena un control DropDownList con las carpetas de anuncios disponibles y preselecciona el elemento indicado.
      /// </summary>
      /// <param name="list">Control DropDownList.</param>
      /// <param name="defaultSelected">Identificador de la carpeta a preseleciconar.</param>
      public void CreateFoldersDropDownList(DropDownList list, int defaultSelected)
      {
         string sql = string.Empty;
         SqlCommand cmd = null;
         SqlDataReader reader = null;

         try
         {
            _ws.DataSource.Connect();

            sql = "SELECT annfldrid, annfldrname " +
                  "FROM " + SQL_FOLDERS_TABLE_NAME + " " +
                  "WHERE annfldrenabled=1 AND annfldrnotselectable=0 " +
                  "ORDER BY annfldrname ASC";

            // Rellena el control
            cmd = new SqlCommand(sql, _ws.DataSource.Connection);
            reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            list.DataSource = reader;
            list.DataTextField = "annfldrname";
            list.DataValueField = "annfldrid";
            list.DataBind();
            reader.Close();

            // Preselecciona el elemento
            if (defaultSelected > 0)
               list.Items.FindByValue(defaultSelected.ToString()).Selected = true;
         }
         catch (Exception ex)
         {
            _ws.Logger.Add(new LogEntry(Cms.ProductName,
                                        GetType().Name + ".CreateFoldersDropDownList()",
                                        ex.Message,
                                        LogEntry.LogEntryType.EV_ERROR));
            throw ex;
         }
         finally
         {
            reader.Dispose();
            cmd.Dispose();
            _ws.DataSource.Disconnect();
         }
      }

      /// <summary>
      /// Rellena un control DropDownList con las carpetas de anuncios disponibles.
      /// </summary>
      /// <param name="list">Control DropDownList.</param>
      public void CreateFoldersDropDownList(DropDownList list)
      {
         this.CreateFoldersDropDownList(list, 0);
      }

      /// <summary>
      /// Obtiene el número de elementos de una carpeta.
      /// </summary>
      /// <param name="folderId">Identificador de la carpeta contenedora.</param>
      /// <param name="enabled">Indica si sólo contempla los activos (true) o todos (false).</param>
      /// <returns>El número de objetos que contiene la carpeta.</returns>
      public int GetFolderItems(int folderId, bool enabled)
      {
         string sql = string.Empty;
         SqlCommand cmd = null;

         try
         {
            _ws.DataSource.Connect();

            sql = "SELECT Count(*) AS nregs " +
                  "FROM " + SQL_OBJECTS_TABLE_NAME + " " +
                  "WHERE annfolderid=@annfolderid " +
                  (enabled ? " And anndeleted=0 And DateDiff(dd, anndate, getdate())<=@days" : string.Empty);

            cmd = new SqlCommand(sql, _ws.DataSource.Connection);
            cmd.Parameters.Add(new SqlParameter("@annfolderid", folderId));
            if (enabled) cmd.Parameters.Add(new SqlParameter("@days", this.ValidityDays));

            return (int)cmd.ExecuteScalar();
         }
         catch (Exception ex)
         {
            _ws.Logger.Add(new LogEntry(Cms.ProductName,
                                        GetType().Name + ".GetFolderItems()",
                                        ex.Message,
                                        LogEntry.LogEntryType.EV_ERROR));
            throw ex;
         }
         finally
         {
            cmd.Dispose();
            _ws.DataSource.Disconnect();
         }
      }

      /// <summary>
      /// Devuelve el número de anuncios de un usuario.
      /// </summary>
      /// <param name="uid">Identificador de la cuenta (UID).</param>
      public int Count(int uid)
      {
         string sql = string.Empty;
         SqlCommand cmd = null;

         try
         {
            _ws.DataSource.Connect();

            sql = "SELECT Count(*) " +
                  "FROM " + SQL_OBJECTS_TABLE_NAME + " " +
                  "WHERE annuserid=@annuserid";

            // Rellena las entradas de noticias al canal
            cmd = new SqlCommand(sql, _ws.DataSource.Connection);
            cmd.Parameters.Add(new SqlParameter("annuserid", uid));
            
            return (int)cmd.ExecuteScalar();
         }
         catch (Exception ex)
         {
            _ws.Logger.Add(new LogEntry(Cms.ProductName,
                                        GetType().Name + ".Count()",
                                        ex.Message,
                                        LogEntry.LogEntryType.EV_ERROR));
            throw ex;
         }
         finally
         {
            cmd.Dispose();
            _ws.DataSource.Disconnect();
         }
      }

      /// <summary>
      /// Envia una petición de contacto al autor del anuncio.
      /// </summary>
      /// <param name="request">Una instancia de <see cref="ClassifiedContactRequest"/> que contiene la información para realizar el contacto.</param>
      public void SendContactRequest(ClassifiedContactRequest request)
      {
         SendContactRequest(request.ClassifiedAdId, 
                            request.Name,
                            request.Mail,
                            request.Message,
                            string.Empty);
      }

      /// <summary>
      /// Envia una petición de contacto al autor del anuncio.
      /// </summary>
      /// <param name="adId">Identificador único del mensaje para el que se desea mandar la petición de contacto.</param>
      /// <param name="name">Nombre de la persona que desea contactar.</param>
      /// <param name="mail">Cuenta de correo electrónico de la persona que desea contactar.</param>
      /// <param name="message">Mensaje de contacto.</param>
      /// <param name="ip">Dirección IP desde la que se efectuó la petición.</param>
      public void SendContactRequest(int adId, string name, string mail, string message, string ip)
      {
         try
         {
            // Obtiene el mensaje
            ClassifiedAd ad = this.Item(adId);

            // Obtiene el autor del mensaje
            User author = _ws.SecurityService.GetUser(ad.UserID);

            // Manda el mensaje de contacto
            MailMessage msg = new MailMessage();
            msg.To.Add(new MailAddress(author.Mail, author.GetDisplayName()));
            msg.ReplyToList.Add(new MailAddress(mail, name));
            msg.Subject = _ws.Settings.GetString(CONF_ADS_CONTACTMAIL_SUBJECT).
                                       Replace(ClassifiedAdsDAO.TAG_CONTACT_NAME, name).
                                       Replace(ClassifiedAdsDAO.TAG_CONTACT_ADTITLE, ad.Title);
            msg.Body = _ws.Settings.GetString(CONF_ADS_CONTACTMAIL_BODY).
                                    Replace(ClassifiedAdsDAO.TAG_CONTACT_NAME, name).
                                    Replace(ClassifiedAdsDAO.TAG_CONTACT_MAIL, mail).
                                    Replace(ClassifiedAdsDAO.TAG_CONTACT_MESSAGE, message).
                                    Replace(ClassifiedAdsDAO.TAG_CONTACT_AUTHORDISPLAYNAME, author.GetDisplayName()).
                                    Replace(ClassifiedAdsDAO.TAG_CONTACT_ADTITLE, ad.Title).
                                    Replace(ClassifiedAdsDAO.TAG_CONTACT_ADBODY, ad.Body).
                                    Replace(ClassifiedAdsDAO.TAG_CONTACT_ADLINK, GetAdUrl(ad)).
                                    Replace(ClassifiedAdsDAO.TAG_CONTACT_ADMANAGELINK, Cosmo.Net.Url.Combine(_ws.Url, "cs_usr_ads.aspx")).
                                    Replace(ClassifiedAdsDAO.TAG_CONTACT_WSNAME, _ws.Name);
            msg.IsBodyHtml = false;

            _ws.Communications.Send(msg);

            // Registra la petición de contacto
            _ws.Logger.Add(new LogEntry(Cms.ProductName,
                                        GetType().Name + ".SendContactRequest()",
                                        "Petición de contacto [" + name + "|" + mail + "] para [" + author.Login + "|" + author.ID + "] desde " + ip,
                                        LogEntry.LogEntryType.EV_INFORMATION));
         }
         catch (Exception ex)
         {
            _ws.Logger.Add(new LogEntry(Cms.ProductName,
                                        GetType().Name + ".SendContactRequest()",
                                        ex.Message,
                                        LogEntry.LogEntryType.EV_ERROR));
            throw ex;
         }
      }

      /// <summary>
      /// Devuelve la URL de un anuncio.
      /// </summary>
      /// <param name="ad">Una instancia de <see cref="Cosmo.Cms.Ads.CSAd"/> que contiene los datos del anuncio.</param>
      /// <returns>Una cadena que representa el enlace al anuncio.</returns>
      public string GetAdUrl(ClassifiedAd ad)
      {
         Url qs = new Url(Cosmo.Net.Url.Combine(_ws.Url, ad.Template));
         qs.AddParameter(Workspace.PARAM_OBJECT_ID, ad.ID);

         return qs.ToString(true, false);         
      }

      /// <summary>
      /// Agrega una nueva sección de anuncios clasificados.
      /// </summary>
      /// <param name="section">Una instáncia de <see cref="ClassifiedAdsSection"/>.</param>
      public void AddSection(ClassifiedAdsSection section)
      {
         SqlCommand cmd = null;
         SqlParameter param = null;

         try
         {
            string sql = "INSERT INTO " + SQL_FOLDERS_TABLE_NAME + " (annfldrname,annfldrdesc,annfldrenabled,annfldrlstdefault,annfldrnotselectable) " +
                         "VALUES (@annfldrname,@annfldrdesc,@annfldrenabled,@annfldrlstdefault,@annfldrnotselectable)";

            _ws.DataSource.Connect();

            cmd = new SqlCommand(sql, _ws.DataSource.Connection);

            param = new SqlParameter("@annfldrname", SqlDbType.NVarChar, 64);
            param.Value = section.Name;
            cmd.Parameters.Add(param);

            param = new SqlParameter("@annfldrdesc", SqlDbType.NVarChar, 255);
            param.Value = section.Description;
            cmd.Parameters.Add(param);

            param = new SqlParameter("@annfldrenabled", SqlDbType.Bit);
            param.Value = section.Enabled;
            cmd.Parameters.Add(param);

            param = new SqlParameter("@annfldrlstdefault", SqlDbType.Bit);
            param.Value = section.IsListDefault;
            cmd.Parameters.Add(param);

            param = new SqlParameter("@annfldrnotselectable", SqlDbType.Bit);
            param.Value = section.IsNotSelectable;
            cmd.Parameters.Add(param);

            cmd.ExecuteNonQuery();
         }
         catch (Exception ex)
         {
            _ws.Logger.Add(new LogEntry(Cms.ProductName,
                                        GetType().Name + ".AddSection()",
                                        ex.Message,
                                        LogEntry.LogEntryType.EV_ERROR));
            throw ex;
         }
         finally
         {
            cmd.Dispose();
            _ws.DataSource.Disconnect();
         }
      }

      /// <summary>
      /// Actualiza los datos de una sección de anuncios clasificados.
      /// </summary>
      /// <param name="ad">Una instáncia de <see cref="ClassifiedAdsSection"/> que contanga los datos actualizados.</param>
      public void UpdateSection(ClassifiedAdsSection section)
      {
         SqlCommand cmd = null;

         try
         {
            string sql = "UPDATE " + SQL_FOLDERS_TABLE_NAME + " " +
                         "SET annfldrname         =@annfldrname, " +
                             "annfldrdesc         =@annfldrdesc," +
                             "annfldrenabled      =@annfldrenabled," +
                             "annfldrlstdefault   =@annfldrlstdefault," +
                             "annfldrnotselectable=@annfldrnotselectable " +
                         "WHERE annfldrid=@annfldrid";

            _ws.DataSource.Connect();

            cmd = new SqlCommand(sql, _ws.DataSource.Connection);
            cmd.Parameters.Add(new SqlParameter("@annfldrname", section.Name));
            cmd.Parameters.Add(new SqlParameter("@annfldrdesc", section.Description));
            cmd.Parameters.Add(new SqlParameter("@annfldrenabled", section.Enabled));
            cmd.Parameters.Add(new SqlParameter("@annfldrlstdefault", section.IsListDefault));
            cmd.Parameters.Add(new SqlParameter("@annfldrnotselectable", section.IsNotSelectable));
            cmd.Parameters.Add(new SqlParameter("@annfldrid", section.ID));
            cmd.ExecuteNonQuery();
         }
         catch (Exception ex)
         {
            _ws.Logger.Add(new LogEntry(Cms.ProductName,
                                        GetType().Name + ".UpdateSection()",
                                        ex.Message,
                                        LogEntry.LogEntryType.EV_ERROR));
            throw ex;
         }
         finally
         {
            cmd.Dispose();
            _ws.DataSource.Disconnect();
         }
      }

      /// <summary>
      /// Elimina una sección de anuncios clasificados.
      /// </summary>
      /// <param name="sectionId">Identificador de la sección a eliminar.</param>
      public void DeleteSection(int sectionId)
      {
         string sql = string.Empty;
         SqlCommand cmd = null;

         try
         {
            _ws.DataSource.Connect();

            sql = "SELECT Count(*) As regs " +
                  "FROM " + SQL_OBJECTS_TABLE_NAME + " " +
                  "WHERE annfolderid=@annfolderid";

            cmd = new SqlCommand(sql, _ws.DataSource.Connection);
            cmd.Parameters.Add(new SqlParameter("@annfolderid", sectionId));
            if ((int)cmd.ExecuteScalar() > 0)
            {
               throw new NodeNotEmptyException("La sección #" + sectionId + " no se puede eliminar: contiene objetos asociados.");
            }

            sql = "DELETE FROM " + SQL_FOLDERS_TABLE_NAME + " " +
                  "WHERE annfldrid=@annfldrid";

            cmd = new SqlCommand(sql, _ws.DataSource.Connection);
            cmd.Parameters.Add(new SqlParameter("@annfldrid", sectionId));
            cmd.ExecuteNonQuery();
         }
         catch (Exception ex)
         {
            _ws.Logger.Add(new LogEntry(Cms.ProductName,
                                        GetType().Name + ".DeleteSection()",
                                        ex.Message,
                                        LogEntry.LogEntryType.EV_ERROR));
            throw ex;
         }
         finally
         {
            cmd.Dispose();
            _ws.DataSource.Disconnect();
         }
      }

      #endregion

      #region Private Members

      /// <summary>
      /// Lee un anuncio classificado de la posición actual del cursor en la base de datos.
      /// </summary>
      private ClassifiedAd ReadClassifiedAd(SqlDataReader reader)
      {
         ClassifiedAd ad = new ClassifiedAd();
         ad.ID = reader.GetInt32(0);
         ad.UserID = reader.GetInt32(1);
         ad.Created = reader.GetDateTime(2);
         ad.FolderID = reader.GetInt32(3);
         ad.Title = reader.IsDBNull(4) ? string.Empty : reader.GetString(4);
         ad.Body = reader.IsDBNull(5) ? string.Empty : reader.GetString(5);
         ad.UserLogin = reader.IsDBNull(6) ? string.Empty : reader.GetString(6);                     // Usuario de Workspace
         ad.Phone = reader.IsDBNull(7) ? string.Empty : reader.GetString(7);
         ad.Mail = reader.IsDBNull(8) ? string.Empty : reader.GetString(8);
         ad.URL = reader.IsDBNull(9) ? string.Empty : reader.GetString(9);
         ad.Deleted = reader.GetBoolean(10);
         ad.Owner = reader.IsDBNull(11) ? SecurityService.ACCOUNT_SUPER : reader.GetString(11);   // Usuario de Cosmo
         ad.Price = reader.GetDecimal(12);
         ad.PublishedDays = reader.GetInt32(13);

         return ad;
      }

      #endregion

      #region Disabled Code

      /*
       
      public const string CONF_RSS_CACHETIMEOUT = "cs.rss.rsscachetime";
      public const string CACHE_RSS_ADS = "cs.rss.channel.ads";
      
      /// <summary>
      /// Genera un canal RSS 2.0 que contiene los últimos anuncios publicados
      /// </summary>
      /// <returns>Devuelve una cadena en formato RSS</returns>
      public string ToRSS()
      {
         SqlCommand cmd = null;
         SqlDataReader reader = null;
         RssChannel rss = null;

         try
         {
            _ws.DataSource.Connect();

            // Implementa las propiedades del canal
            rss = new RssChannel();
            rss.Title = _ws.PropertyName + " - " + ClassifiedAdsDAO.SERVICE_NAME;
            rss.Copyright = "Copyright &copy; " + _ws.PropertyName;
            rss.Language = "es-es";
            rss.ManagingEditor = _ws.Mail;
            rss.LastBuildDate = DateTime.Now;
            rss.PubDate = DateTime.Now;
            rss.Link = _ws.Url;
            rss.Image = new RssChannelImage();
            rss.Image.Url = new Uri(Cosmo.Net.Url.Combine(_ws.Url, "images", "cs_rss_image.jpg"));
            rss.Image.Link = _ws.Url;

            // Rellena las entradas de noticias al canal
            cmd = new SqlCommand("cs_RSS_GetAds", _ws.DataSource.Connection);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
               Url qs = new Url(ClassifiedAdsDAO.URL_VIEWER);
               qs.AddParameter(Workspace.PARAM_OBJECT_ID, reader.GetInt32(0));
               qs.AddParameter(Workspace.PARAM_FOLDER_ID, reader.GetInt32(5));

               RssItem item = new RssItem();
               item.Link = Cosmo.Net.Url.Combine(_ws.Url, qs.ToString(true));
               item.Title = reader.GetString(1);
               item.Description = reader.GetString(2);
               item.PubDate = reader.GetDateTime(3);
               item.Guid = item.Link;
               item.Category = reader.GetString(6);
               rss.Items.Add(item);
            }
            reader.Close();

            return rss.ToXml();
         }
         catch (Exception ex)
         {
            _ws.Logger.Add(new LogEntry(Cms.ProductName,
                                        GetType().PropertyName + ".ToRSS()",
                                        ex.Message,
                                        LogEntry.LogEntryType.EV_ERROR));
            throw ex;
         }
         finally
         {
            reader.Dispose();
            cmd.Dispose();
            _ws.DataSource.Disconnect();
         }
      }
      
      /// <summary>
      /// Genera un canal RSS 2.0 que contiene los últimos anuncios publicados usando la caché de servidor web
      /// </summary>
      /// <param name="cache">La instáncia Cache del servidor web</param>
      /// <param name="forceUpdate">Indica si se debe forzar al refresco del contenido del canal si usar la caché.</param>
      /// <returns>Devuelve una cadena en formato RSS</returns>
      public string ToRSS(System.Web.Caching.Cache cache, bool forceUpdate)
      {
         // Calcula el número de segundos de validez
         int secs = _ws.Settings.GetInt(CONF_RSS_CACHETIMEOUT, 3600);

         // Guarda el feed en caché
         if (cache[CACHE_RSS_ADS] == null || forceUpdate)
            cache.Insert(CACHE_RSS_ADS, ToRSS(), null, DateTime.Now.AddSeconds(secs), TimeSpan.Zero);

         // Devuelve el contenido de la caché
         return cache[CACHE_RSS_ADS].ToString();
      } 
       
       */

      #endregion

   }
}