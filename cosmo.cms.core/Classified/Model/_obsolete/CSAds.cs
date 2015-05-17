using Cosmo.Communications;
using Cosmo.Diagnostics;
using Cosmo.Net;
using Cosmo.Net.Rss;
using Cosmo.Security;
using Cosmo.Security.Auth;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Net.Mail;
using System.Web.UI.WebControls;

namespace Cosmo.Cms.Classified.Model
{
   /// <summary>
   /// Permite acceder y gestionar todos los elementos del Tablón de anuncios.
   /// </summary>
   public class CSAds
   {
      // Declaración de variables internas
      private Workspace _ws;

      // public const string PARAMETER_OBJECTID = "id";
      // public const string PARAMETER_FOLDERID = "fid";
      // public const string PARAMETER_EDITMODE = "m";

      /// <summary>Nombre público del módulo.</summary>
      public const string SERVICE_NAME = "Anuncios clasificados";
      
      /// <summary>Script de portada del módulo.</summary>
      public const string URL_HOME = "cs_ads.aspx";
      /// <summary>Script que muestra el contenido de una carpeta del módulo.</summary>
      public const string URL_FOLDER = "cs_ads_folder.aspx";
      /// <summary>Script que muestra el editor de objetos del módulo.</summary>
      public const string URL_EDITOR = "cs_usr_ads_edit.aspx";
      /// <summary>Script de gestión de objetos del módulo.</summary>
      public const string URL_MANAGE = "cs_usr_ads.aspx";
      /// <summary>Script de gestión de objetos despublicados del módulo.</summary>
      public const string URL_MANAGE_UNPUBLISHED = "cs_usr_ads_upub.aspx";
      /// <summary>Script de visualización de anuncios.</summary>
      public const string URL_VIEWER = "cs_ads_viewer_std.aspx";

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

      /// <summary>
      /// Enumera los tipos de acción sobre los objetos gestionados por este módulo.
      /// </summary>
      public enum AdEditMode : int
      {
         /// <summary>Eliminar objeto.</summary>
         Delete = 0,
         /// <summary>Republicar objeto. Actualiza la fecha de publicación.</summary>
         Refresh = 1,
         /// <summary>Nuevo objeto.</summary>
         AddNew = 2,
         /// <summary>Actualizar objeto.</summary>
         Update = 3
      }

      /// <summary>
      /// Devuelve una instancia de <see cref="CSAds"/>.
      /// </summary>
      /// <param name="ws">Una instancia de <see cref="Workspace"/> que representa el sitio actual.</param>
      public CSAds(Workspace ws)
      {
         Initialize();

         _ws = ws;
      }

      /// <summary>
      /// Dias de validez de un anuncio publicado
      /// </summary>
      /// <remarks>Corresponde a la entrada cs.ads.validitymonths del archivo de configuración.</remarks>
      public int ValidityDays
      {
         get { return _ws.Properties.GetInt(Cms.AdsValidityDays); }
      }

      /// <summary>
      /// Devuelve el número máximo de anuncios por persona.
      /// </summary>
      public int MaxAdsPerUser
      {
         get { return _ws.Properties.GetInt(Cms.AdsMaxByUser, 15); }
      }

      /// <summary>
      /// Agrega un nuevo anuncio a una carpeta concreta.
      /// </summary>
      /// <param name="ad">Una instáncia de CSAd que contanga los datos del objeto.</param>
      /// <returns>Un valor booleano que indica si la operación ha tenido éxito.</returns>
      public void Add(CSAd ad)
      {
         SqlCommand cmd = null;
         SqlParameter param = null;

         // Comprueba que el usuario no tenga más anuncios de los permitidos
         int max = _ws.Properties.GetInt(Cms.AdsMaxByUser, 15);
         if ((max > 0) && (max <= this.Count(ad.UserID)))
            throw new TooManyUserObjectsException("Un mismo usuario no puede disponer de más de " + max + " anuncios.");

         try
         {
            string sql = "INSERT INTO announces (annuserid,anndate,annfolderid,anntitle,annbody,annprice,annname,annphone,annemail,annurl,anndeleted,annowner) " +
                         "VALUES (@annuserid,getdate(),@annfolderid,@anntitle,@annbody,@annprice,@annname,@annphone,@annemail,@annurl,0,'" + AuthenticationService.ACCOUNT_SUPER + "')";

            // string sql = "INSERT INTO announces (annuserid,anndate,annfolderid,anntitle,annbody,annname,annphone,annemail,adprice,annurl,anndeleted,annowner) " +
            //              "VALUES (@annuserid,getdate(),@annfolderid,@anntitle,@annbody,@annname,@annphone,@annemail,@adprice,@annurl,0,'" + UserAuthentication.ACCOUNT_SUPER + "')";

            _ws.DataSource.Connect();

            cmd = new SqlCommand(sql, _ws.DataSource.Connection);

            param = new SqlParameter("@annuserid", SqlDbType.Int);
            param.Value = ad.UserID;
            cmd.Parameters.Add(param);

            param = new SqlParameter("@annfolderid", SqlDbType.Int);
            param.Value = ad.FolderId;
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
                                        GetType().Name + ".Add", 
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
      public void Update(CSAd ad)
      {
         SqlCommand cmd = null;

         try
         {
            string sql = "UPDATE announces " +
                         "SET anndate    =@anndate, " +
                             "annfolderid=@annfolderid," +
                             "anntitle   =@anntitle," +
                             "annbody    =@annbody," +
                             "annprice   =@annprice," +
                             "annphone   =@annphone," +
                             "annemail   =@annemail," +
                             "annurl     =@annurl, " +
                             "anndeleted =@anndeleted " +
                         "WHERE annid=@annid";

            _ws.DataSource.Connect();

            cmd = new SqlCommand(sql, _ws.DataSource.Connection);
            cmd.Parameters.Add(new SqlParameter("@anndate", ad.Created));
            cmd.Parameters.Add(new SqlParameter("@annfolderid", ad.FolderId));
            cmd.Parameters.Add(new SqlParameter("@anntitle", ad.Title));
            cmd.Parameters.Add(new SqlParameter("@annbody", ad.Body));
            cmd.Parameters.Add(new SqlParameter("@annprice", ad.Price));
            cmd.Parameters.Add(new SqlParameter("@annphone", ad.Phone));
            cmd.Parameters.Add(new SqlParameter("@annemail", ad.Mail));
            cmd.Parameters.Add(new SqlParameter("@annurl", ad.URL));
            cmd.Parameters.Add(new SqlParameter("@anndeleted", false));
            cmd.Parameters.Add(new SqlParameter("@annid", ad.Id));
            cmd.ExecuteNonQuery();
         }
         catch (Exception ex)
         {
            _ws.Logger.Add(new LogEntry(Cms.ProductName, 
                                        GetType().Name + ".Update", 
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
         string sql = string.Empty;
         SqlCommand cmd = null;

         try
         {
            if (!deleteRow)
               sql = "UPDATE announces " +
                     "SET anndeleted=1 " +
                     "WHERE annid=@annid";
            else
               sql = "DELETE FROM announces " +
                     "WHERE annid=@annid";

            _ws.DataSource.Connect();
            cmd = new SqlCommand(sql, _ws.DataSource.Connection);
            cmd.Parameters.Add(new SqlParameter("@annid", adId));
            cmd.ExecuteNonQuery();
         }
         catch (Exception ex)
         {
            _ws.Logger.Add(new LogEntry(Cms.ProductName, 
                                        GetType().Name + ".Delete", 
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
         string sql = string.Empty;
         SqlCommand cmd = null;

         try
         {
            // Obtiene las propiedades del anuncio
            CSAd ad = this.Item(adId);

            if (ad.Deleted)
               sql = "DELETE FROM announces " +
                     "WHERE annid=@annid";
            else if (Cosmo.Utils.Calendar.DateDiff(Cosmo.Utils.Calendar.DateInterval.Day, ad.Created, DateTime.Now) > this.ValidityDays)
               sql = "DELETE FROM announces " +
                     "WHERE annid=@annid";
            else
               sql = "UPDATE announces " +
                     "SET anndeleted=1 " +
                     "WHERE annid=@annid";

            _ws.DataSource.Connect();

            cmd = new SqlCommand(sql, _ws.DataSource.Connection);
            cmd.Parameters.Add(new SqlParameter("@annid", adId));
            cmd.ExecuteNonQuery();
         }
         catch (Exception ex)
         {
            _ws.Logger.Add(new LogEntry(Cms.ProductName, 
                                        GetType().Name + ".Delete", 
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
         SqlCommand cmd = null;

         try
         {
            _ws.DataSource.Connect();

            cmd = new SqlCommand("DELETE FROM announces WHERE annuserid=@annuserid", _ws.DataSource.Connection);
            cmd.Parameters.Add(new SqlParameter("@annuserid", uid));
            cmd.ExecuteNonQuery();
         }
         catch (Exception ex)
         {
            _ws.Logger.Add(new LogEntry(Cms.ProductName, 
                                        GetType().Name + ".DeleteAll", 
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
      public CSAd Item(int adId)
      {
         string sql = string.Empty;
         CSAd ad = null;
         SqlCommand cmd = null;
         SqlDataReader reader = null;

         try
         {
            _ws.DataSource.Connect();

            sql = "SELECT annid,annuserid,anndate,annfolderid,anntitle,annbody,annname,annphone,annemail,annurl,anndeleted,annowner,annprice " +
                  "FROM announces " +
                  "WHERE annid=@annid";

            // Obtiene las carpetas
            cmd = new SqlCommand(sql, _ws.DataSource.Connection);
            cmd.Parameters.Add(new SqlParameter("@annid", adId));
            reader = cmd.ExecuteReader();
            if (reader.Read())
            {
               ad = new CSAd();
               ad.Id = reader.GetInt32(0);
               ad.UserID = reader.GetInt32(1);
               ad.Created = reader.GetDateTime(2);
               ad.FolderId = reader.GetInt32(3);
               ad.Title = reader.IsDBNull(4) ? string.Empty : reader.GetString(4);
               ad.Body = reader.IsDBNull(5) ? string.Empty : reader.GetString(5);
               ad.UserLogin = reader.IsDBNull(6) ? string.Empty : reader.GetString(6);                   // Usuario de Workspace
               ad.Phone = reader.IsDBNull(7) ? string.Empty : reader.GetString(7);
               ad.Mail = reader.IsDBNull(8) ? string.Empty : reader.GetString(8);
               ad.URL = reader.IsDBNull(9) ? string.Empty : reader.GetString(9);
               ad.Deleted = reader.GetBoolean(10);
               ad.Owner = reader.IsDBNull(11) ? AuthenticationService.ACCOUNT_SUPER : reader.GetString(11); // Usuario de Cosmo
               ad.Price = reader.GetDecimal(12);
            }
            reader.Close();

            return ad;
         }
         catch (Exception ex)
         {
            _ws.Logger.Add(new LogEntry(Cms.ProductName, 
                                        GetType().Name + ".Item", 
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
      /// <param name="enabled">Indica si se recuperan sólo los activos (true) o todos (false).</param>
      /// <returns>Un array de instáncias CSAds.</returns>
      public List<CSAd> Items(bool enabled)
      {
         string sql = string.Empty;
         CSAd ad = null;
         List<CSAd> ads = new List<CSAd>();
         SqlCommand cmd = null;
         SqlDataReader reader = null;

         try
         {
            _ws.DataSource.Connect();

            sql = "SELECT annid,annuserid,anndate,annfolderid,anntitle,annbody,annname,annphone,annemail,annurl,anndeleted,annowner,annprice " +
                  "FROM announces " +
                  (enabled ? "WHERE anndeleted=0 AND DateDiff(mm, anndate, GetDate())<=@months" : "") +
                  "ORDER BY annid Desc";

            cmd = new SqlCommand(sql, _ws.DataSource.Connection);
            if (enabled) cmd.Parameters.Add(new SqlParameter("@months", this.ValidityDays));
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
               ad = new CSAd();
               ad.Id = reader.GetInt32(0);
               ad.UserID = reader.GetInt32(1);
               ad.Created = reader.GetDateTime(2);
               ad.FolderId = reader.GetInt32(3);
               ad.Title = reader.IsDBNull(4) ? string.Empty : reader.GetString(4);
               ad.Body = reader.IsDBNull(5) ? string.Empty : reader.GetString(5);
               ad.UserLogin = reader.IsDBNull(6) ? string.Empty : reader.GetString(6);                   // Usuario de Workspace
               ad.Phone = reader.IsDBNull(7) ? string.Empty : reader.GetString(7);
               ad.Mail = reader.IsDBNull(8) ? string.Empty : reader.GetString(8);
               ad.URL = reader.IsDBNull(9) ? string.Empty : reader.GetString(9);
               ad.Deleted = reader.GetBoolean(10);
               ad.Owner = reader.IsDBNull(11) ? AuthenticationService.ACCOUNT_SUPER : reader.GetString(11); // Usuario de Cosmo
               ad.Price = reader.GetDecimal(12);

               ads.Add(ad);
            }
            reader.Close();

            return ads;
         }
         catch (Exception ex)
         {
            _ws.Logger.Add(new LogEntry(Cms.ProductName, 
                                        GetType().Name + ".Items", 
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
      public List<CSAd> Items(int folderId, bool enabled)
      {
         string sql = string.Empty;
         CSAd ad = null;
         List<CSAd> ads = new List<CSAd>();
         SqlCommand cmd = null;
         SqlDataReader reader = null;

         try
         {
            _ws.DataSource.Connect();

            sql = "SELECT annid,annuserid,anndate,annfolderid,anntitle,annbody,annname,annphone,annemail,annurl,anndeleted,annowner,annprice " +
                  "FROM announces " +
                  "WHERE annfolderid=@annfolderid And " +
                  (enabled ? "(anndeleted=0 AND DateDiff(dd, anndate, GetDate())<=@days)" : "(anndeleted=1 Or DateDiff(dd, anndate, GetDate())>@days)") + " " +
                  "ORDER BY anndate Desc";

            cmd = new SqlCommand(sql, _ws.DataSource.Connection);
            cmd.Parameters.Add(new SqlParameter("@annfolderid", folderId));
            cmd.Parameters.Add(new SqlParameter("@days", this.ValidityDays));
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
               ad = new CSAd();
               ad.Id = reader.GetInt32(0);
               ad.UserID = reader.GetInt32(1);
               ad.Created = reader.GetDateTime(2);
               ad.FolderId = reader.GetInt32(3);
               ad.Title = reader.IsDBNull(4) ? string.Empty : reader.GetString(4);
               ad.Body = reader.IsDBNull(5) ? string.Empty : reader.GetString(5);
               ad.UserLogin = reader.IsDBNull(6) ? string.Empty : reader.GetString(6);                   // Usuario de Workspace
               ad.Phone = reader.IsDBNull(7) ? string.Empty : reader.GetString(7);
               ad.Mail = reader.IsDBNull(8) ? string.Empty : reader.GetString(8);
               ad.URL = reader.IsDBNull(9) ? string.Empty : reader.GetString(9);
               ad.Deleted = reader.GetBoolean(10);
               ad.Owner = reader.IsDBNull(11) ? AuthenticationService.ACCOUNT_SUPER : reader.GetString(11); // Usuario de Cosmo
               ad.Price = reader.GetDecimal(12);

               ads.Add(ad);
            }
            reader.Close();

            return ads;
         }
         catch (Exception ex)
         {
            _ws.Logger.Add(new LogEntry(Cms.ProductName, 
                                        GetType().Name + ".Items", 
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
      public List<CSAd> Items(bool enabled, int uid)
      {
         string sql = string.Empty;
         CSAd ad = null;
         List<CSAd> ads = new List<CSAd>();
         SqlCommand cmd = null;
         SqlDataReader reader = null;

         try
         {
            _ws.DataSource.Connect();

            sql = "SELECT annid,annuserid,anndate,annfolderid,anntitle,annbody,annname,annphone,annemail,annurl,anndeleted,annowner,annprice " +
                  "FROM announces " +
                  "WHERE annuserid=@annuserid And " +
                  (enabled ? "(DateDiff(dd, anndate, GetDate())<=@days AND anndeleted=0)" : "(DateDiff(dd, anndate, GetDate())>@days OR anndeleted=1)") + " " +
                  "ORDER BY annid Desc";

            cmd = new SqlCommand(sql, _ws.DataSource.Connection);
            cmd.Parameters.Add(new SqlParameter("@annuserid", uid));
            cmd.Parameters.Add(new SqlParameter("@days", this.ValidityDays));
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
               ad = new CSAd();
               ad.Id = reader.GetInt32(0);
               ad.UserID = reader.GetInt32(1);
               ad.Created = reader.GetDateTime(2);
               ad.FolderId = reader.GetInt32(3);
               ad.Title = reader.IsDBNull(4) ? string.Empty : reader.GetString(4);
               ad.Body = reader.IsDBNull(5) ? string.Empty : reader.GetString(5);
               ad.UserLogin = reader.IsDBNull(6) ? string.Empty : reader.GetString(6);                     // Usuario de Workspace
               ad.Phone = reader.IsDBNull(7) ? string.Empty : reader.GetString(7);
               ad.Mail = reader.IsDBNull(8) ? string.Empty : reader.GetString(8);
               ad.URL = reader.IsDBNull(9) ? string.Empty : reader.GetString(9);
               ad.Deleted = reader.GetBoolean(10);
               ad.Owner = reader.IsDBNull(11) ? AuthenticationService.ACCOUNT_SUPER : reader.GetString(11);   // Usuario de Cosmo
               ad.Price = reader.GetDecimal(12);

               ads.Add(ad);
            }
            reader.Close();

            return ads;
         }
         catch (Exception ex)
         {
            _ws.Logger.Add(new LogEntry(Cms.ProductName, 
                                        GetType().Name + ".Items", 
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

            sql = "UPDATE announces " +
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
                                        GetType().Name + ".Publish", 
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
      public CSAdsFolder GetFolder(int folderId)
      {
         string sql = string.Empty;
         CSAdsFolder folder = null;
         SqlCommand cmd = null;
         SqlDataReader reader = null;

         try
         {
            _ws.DataSource.Connect();

            sql = "SELECT annfldrid,annfldrname,annfldrdesc,annfldrenabled,annfldrlstdefault,annfldrnotselectable," +
                         "(SELECT Count(*) AS nregs FROM announces WHERE annfolderid=annfolders.annfldrid And anndeleted=0 And DateDiff(dd, anndate, getdate())<=@days) As objects " +
                  "FROM annfolders " +
                  "WHERE annfldrid=@annfldrid";

            cmd = new SqlCommand(sql, _ws.DataSource.Connection);
            cmd.Parameters.Add(new SqlParameter("@annfldrid", folderId));
            cmd.Parameters.Add(new SqlParameter("@days", this.ValidityDays));
            reader = cmd.ExecuteReader();
            if (reader.Read())
            {
               folder = new CSAdsFolder();
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
                                        GetType().Name + ".GetFolder", 
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
      public List<CSAdsFolder> GetFolders(bool published)
      {
         string sql = string.Empty;
         CSAdsFolder folder = null;
         List<CSAdsFolder> folders = new List<CSAdsFolder>();
         SqlCommand cmd = null;
         SqlDataReader reader = null;

         try
         {
            _ws.DataSource.Connect();

            sql = "SELECT annfldrid,annfldrname,annfldrdesc,annfldrenabled,annfldrlstdefault,annfldrnotselectable, " +
                         "(SELECT Count(*) FROM announces WHERE annfolderid=annfolders.annfldrid " + (published ? " And anndeleted=0 And DateDiff(dd, anndate, getdate())<=@days" : "") + ") " +
                  "FROM annfolders " +
                  "WHERE annfldrenabled=@annfldrenabled " +
                  "ORDER BY annfldrname ASC";

            cmd = new SqlCommand(sql, _ws.DataSource.Connection);
            cmd.Parameters.Add(new SqlParameter("@annfldrenabled", published));
            if (published) cmd.Parameters.Add(new SqlParameter("@days", this.ValidityDays));
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
               folder = new CSAdsFolder();
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
                                        GetType().Name + ".GetFolders", 
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
      public List<CSAdsFolder> GetFolders()
      {
         string sql = string.Empty;
         CSAdsFolder folder = null;
         List<CSAdsFolder> folders = new List<CSAdsFolder>();
         SqlCommand cmd = null;
         SqlDataReader reader = null;

         try
         {
            _ws.DataSource.Connect();

            sql = "SELECT annfldrid,annfldrname,annfldrdesc,annfldrenabled,annfldrlstdefault,annfldrnotselectable, " +
                         "(SELECT Count(*) FROM announces WHERE annfolderid=annfolders.annfldrid) As objects" +
                  "FROM annfolders " +
                  "ORDER BY annfldrname ASC";

            // Recupera las carpetas
            cmd = new SqlCommand(sql, _ws.DataSource.Connection);
            reader = cmd.ExecuteReader();
            if (reader.Read())
            {
               folder = new CSAdsFolder();
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
                                        GetType().Name + ".GetFolders", 
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
                  "FROM annfolders " +
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
                                        GetType().Name + ".CreateFoldersDropDownList", 
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
         string sql = "";
         SqlCommand cmd = null;

         try
         {
            _ws.DataSource.Connect();

            sql = "SELECT Count(*) AS nregs " +
                  "FROM announces " +
                  "WHERE annfolderid=@annfolderid " +
                  (enabled ? " And anndeleted=0 And DateDiff(dd, anndate, getdate())<=@days" : "");

            cmd = new SqlCommand(sql, _ws.DataSource.Connection);
            cmd.Parameters.Add(new SqlParameter("@annfolderid", folderId));
            if (enabled) cmd.Parameters.Add(new SqlParameter("@days", this.ValidityDays));

            return (int)cmd.ExecuteScalar();
         }
         catch (Exception ex)
         {
            _ws.Logger.Add(new LogEntry(Cms.ProductName, 
                                        GetType().Name + ".getFolderItems", 
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
            rss.Title = _ws.Name + " - " + CSAds.SERVICE_NAME;
            rss.Copyright = "Copyright &copy; " + _ws.Name;
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
               Url qs = new Url(CSAds.URL_VIEWER);
               qs.AddParameter(Cosmo.Workspace.PARAM_OBJECT_ID, reader.GetInt32(0));
               qs.AddParameter(Cosmo.Workspace.PARAM_FOLDER_ID, reader.GetInt32(5));

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
                                        GetType().Name + ".ToRSS", 
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
         int secs = _ws.Properties.GetInt(Cms.RSSCacheTimeout, 3600);

         // Guarda el feed en caché
         if (cache[Cms.CACHE_RSS_ADS] == null || forceUpdate)
            cache.Insert(Cms.CACHE_RSS_ADS, ToRSS(), null, DateTime.Now.AddSeconds(secs), TimeSpan.Zero);

         // Devuelve el contenido de la caché
         return cache[Cms.CACHE_RSS_ADS].ToString();
      }

      /// <summary>
      /// Devuelve el número de anuncios de un usuario.
      /// </summary>
      /// <param name="uid">Identificador de la cuenta (UID).</param>
      public int Count(int uid)
      {
         SqlCommand cmd = null;

         try
         {
            _ws.DataSource.Connect();

            // Rellena las entradas de noticias al canal
            cmd = new SqlCommand("SELECT Count(*) FROM announces WHERE annuserid=@annuserid", _ws.DataSource.Connection);
            cmd.Parameters.Add(new SqlParameter("annuserid", uid));
            
            return (int)cmd.ExecuteScalar();
         }
         catch (Exception ex)
         {
            _ws.Logger.Add(new LogEntry(Cms.ProductName, 
                                        GetType().Name + ".Count", 
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
      /// Envia una petición de contacto al autor de 
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
            CSAd ad = this.Item(adId);

            // Obtiene el autor del mensaje
            User author = _ws.AuthenticationService.GetUser(ad.UserID);

            // Manda el mensaje de contacto
            MailMessage msg = new MailMessage();
            msg.To.Add(new MailAddress(author.Mail, author.GetDisplayName()));
            msg.ReplyToList.Add(new MailAddress(mail, name));
            msg.Subject = _ws.Properties.GetString(Cms.AdsContactMailSubject).
                                         Replace(CSAds.TAG_CONTACT_NAME, name).
                                         Replace(CSAds.TAG_CONTACT_ADTITLE, ad.Title);
            msg.Body = _ws.Properties.GetString(Cms.AdsContactMailBody).
                                      Replace(CSAds.TAG_CONTACT_NAME, name).
                                      Replace(CSAds.TAG_CONTACT_MAIL, mail).
                                      Replace(CSAds.TAG_CONTACT_MESSAGE, message).
                                      Replace(CSAds.TAG_CONTACT_AUTHORDISPLAYNAME, author.GetDisplayName()).
                                      Replace(CSAds.TAG_CONTACT_ADTITLE, ad.Title).
                                      Replace(CSAds.TAG_CONTACT_ADBODY, ad.Body).
                                      Replace(CSAds.TAG_CONTACT_ADLINK, GetAdUrl(ad)).
                                      Replace(CSAds.TAG_CONTACT_ADMANAGELINK, Cosmo.Net.Url.Combine(_ws.Url, "cs_usr_ads.aspx")).
                                      Replace(CSAds.TAG_CONTACT_WSNAME, _ws.Name);
            msg.IsBodyHtml = false;

            _ws.Communications.Send(msg);

            // Registra la petición de contacto
            _ws.Logger.Add(new LogEntry(Cms.ProductName, 
                                        GetType().Name + ".SendContactRequest", 
                                        "Petición de contacto [" + name + "|" + mail + "] para [" + author.Login + "|" + author.ID + "] desde " + ip, 
                                        LogEntry.LogEntryType.EV_INFORMATION)); 
         }
         catch (Exception ex)
         {
            _ws.Logger.Add(new LogEntry(Cms.ProductName,
                                        GetType().Name + ".SendContactRequest", 
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
      public string GetAdUrl(CSAd ad)
      {
         Url qs = new Url(Cosmo.Net.Url.Combine(_ws.Url, ad.Template));
         qs.AddParameter(Cosmo.Workspace.PARAM_OBJECT_ID, ad.Id);

         return qs.ToString(true, false);         
      }

      /// <summary>
      /// Inicializa la instancia.
      /// </summary>
      private void Initialize()
      {
         _ws = null;
      }
   }
}