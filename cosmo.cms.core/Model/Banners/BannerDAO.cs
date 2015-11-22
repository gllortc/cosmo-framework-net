using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Cosmo.Cms.Model.Banners
{

   #region Enumeraciones

   public enum BannerPositions
   {
      CenterTop = 3,
      LeftBottom = 2,
      CenterMiddle = 1,
      CenterBottom = 4
   }

   #endregion

   /// <summary>
   /// Banner persistence manager.
   /// </summary>
   public class BannerDAO
   {
      // Internal data declarations
      private Workspace _ws;

      private const int BANNER_ACTION_SHOW = 0;
      private const int BANNER_ACTION_CLIC = 1;

      /// <summary>Nombre del módulo.</summary>
      public const string SERVICE_NAME = "Publicidad";

      /// <summary>Script de redirección.</summary>
      public const string URL_REDIRECT = "banner.do";

      /// <summary>Parametro URL que contiene el identificador del obeto.</summary>
      // public const string PARAM_OBJECTID = "id";

      public const string TAG_BANNERS_LEFTBOTTOM = "${cs.banners.left-bottom}";
      public const string TAG_BANNERS_RIGHTBOTTOM = "${cs.banners.right-bottom}";
      public const string TAG_BANNERS_CENTERTOP = "${cs.banners.center-top}";
      public const string TAG_BANNERS_CENTERMIDDLE = "${cs.banners.center-middle}";
      public const string TAG_BANNERS_CENTERBOTTOM = "${cs.banners.center.bottom}";

      #region Constructors

      /// <summary>
      /// Gets a new instance of <see cref="BannerDAO"/>.
      /// </summary>
      public BannerDAO(Workspace ws)
      {
         _ws = ws;
      }

      #endregion

      #region Methods

      /// <summary>
      /// Obtiene los banners a mostrar en una determinada posición
      /// </summary>
      /// <param name="Position">Posición para la que se solicitan los banners</param>
      /// <returns>Una lista de los banners</returns>
      public List<Banner> GetBanners(BannerPositions Position)
      {
         int numBanners = 0;
         SqlParameter param = null;
         List<Banner> banners = new List<Banner>();

         // TODO: De momento, el número de banners por posició se fija por código, aunque en próximas versiones se debe poder configurar
         switch (Position)
         {
            case BannerPositions.CenterTop: numBanners = 1; break;
            case BannerPositions.LeftBottom: numBanners = 2; break;
            case BannerPositions.CenterMiddle: numBanners = 1; break;
            case BannerPositions.CenterBottom: numBanners = 1; break;
            default: return banners;
         }

         try
         {
            _ws.DataSource.Connect();

            using (SqlCommand cmd = new SqlCommand("cs_Banners_GetBannersByPos", _ws.DataSource.Connection))
            {
               cmd.CommandType = CommandType.StoredProcedure;
               cmd.Parameters.Add(new SqlParameter("@bannerType", (int)Position));
               cmd.Parameters.Add(new SqlParameter("@numBanners", (int)numBanners));

               using (SqlDataReader reader = cmd.ExecuteReader())
               {
                  while (reader.Read())
                  {
                     // Agrega el banner
                     banners.Add(new Banner((Int32)reader["banid"],
                                              (string)reader["banclient"],
                                              _ws.FileSystemService.GetFilePath(new BannerFSID((Int32)reader["banid"]), (string)reader["banimage"]),
                                              _ws.FileSystemService.GetFileURL(new BannerFSID((Int32)reader["banid"]), (string)reader["banimage"]),
                                              (int)reader["banwidth"],
                                              (int)reader["banheight"]));
                  }
               }
            }
            
            // Contabiliza la visualización del banner
            foreach (Banner banner in banners)
            {
               using (SqlCommand cmd = new SqlCommand("cs_Banners_DoCount", _ws.DataSource.Connection))
               {
                  cmd.CommandType = CommandType.StoredProcedure;
                  // cmd.Parameters.Add(new SqlParameter("@banner_id", (int)banner.Id));

                  param = new SqlParameter("@banner_id", SqlDbType.Int);
                  param.Value = banner.ID;
                  cmd.Parameters.Add(param);

                  param = new SqlParameter("@click", SqlDbType.Int);
                  param.Value = BANNER_ACTION_SHOW;
                  cmd.Parameters.Add(param);

                  // cmd.Parameters.Add(new SqlParameter("@click", int.Parse(BANNER_ACTION_SHOW.ToString())));
                  cmd.ExecuteNonQuery();
               }
            }

            return banners;
         }
         catch (Exception ex)
         {
            _ws.Logger.Error(this, "GetBanners", ex);
            throw ex;
         }
         finally
         {
            _ws.DataSource.Disconnect();
         }
      }

      /// <summary>
      /// Responde a un click de un banner.
      /// </summary>
      /// <param name="BannerID">Identificador del banner.</param>
      /// <returns>Devuelve la URL dónde debe redirigirse el cliente.</returns>
      public string DoClick(int bannerId)
      {
         string url = string.Empty;

         try
         {
            _ws.DataSource.Connect();

            // Actualiza las estadísticas del banner
            using (SqlCommand cmd = new SqlCommand("cs_Banners_DoCount", _ws.DataSource.Connection))
            {
               cmd.CommandType = CommandType.StoredProcedure;
               cmd.Parameters.Add(new SqlParameter("@banner_id", bannerId));
               cmd.Parameters.Add(new SqlParameter("@click", 1));
               cmd.ExecuteNonQuery();
            }

            // Obtiene la dirección URL de destino
            using (SqlCommand cmd = new SqlCommand("cs_Banners_Properties", _ws.DataSource.Connection))
            {
               cmd.CommandType = CommandType.StoredProcedure;
               cmd.Parameters.Add(new SqlParameter("@bannerId", bannerId));

               using (SqlDataReader reader = cmd.ExecuteReader())
               {
                  if (reader.Read())
                  {
                     url = (string)reader["bandirecturl"].ToString().Trim().ToLower();
                     if (!url.StartsWith("http")) url = "http://" + url;
                  }
               }
            }

            return url;
         }
         catch (Exception ex)
         {
            _ws.Logger.Error(this, "DoClick()", ex);
            throw ex;
         }
         finally
         {
            _ws.DataSource.Disconnect();
         }
      }

      #endregion

   }
}