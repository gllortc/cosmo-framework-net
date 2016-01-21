using Cosmo.Data.Lists;
using Cosmo.Net;
using Cosmo.Net.REST;
using System.Reflection;

namespace Cosmo.Web.Handlers
{
   /// <summary>
   /// Cosmo Data Services REST handler.
   /// </summary>
   public class DataRestHandler : RestHandler
   {
      /// <summary>parámetro de llamada: Nombre de archivo.</summary>
      public const string PARAM_FILENAME = "dsid";

      #region RestHandler Implementation

      /// <summary>
      /// Método invocado al recibir una petición.
      /// </summary>
      /// <param name="command">Comando a ejecutar pasado mediante el parámetro definido mediante <see cref="Properties.Workspace.PARAM_COMMAND"/>.</param>
      public override void ServiceRequest(string command)
      {
         switch (command)
         {
            case COMMAND_LIST:
               DataList(Parameters.GetString(Cosmo.Workspace.PARAM_OBJECT_ID));
               break;

            default:
               break;
         }
      }

      #endregion

      #region REST Method: DataList

      /// <summary>Comando para descargar un fichero asociado a un objeto.</summary>
      private const string COMMAND_LIST = "_list_";

      /// <summary>
      /// Gets an URL to invoke <c>DataList</c> REST handler method.
      /// </summary>
      /// <param name="dataListId">DataList unique identifier.</param>
      /// <returns>A string containing the requested URL.</returns>
      public static Url GetDataList(string dataListId)
      {
         return new Url(FileSystemRestHandler.ServiceUrl)
            .AddParameter(Cosmo.Workspace.PARAM_COMMAND, COMMAND_LIST)
            .AddParameter(Cosmo.Workspace.PARAM_OBJECT_ID, dataListId);
      }

      /// <summary>
      /// Get a DataList and send it to client.
      /// </summary>
      private void DataList(string dataListId)
      {
         AjaxResponse response;

         // Verifica la existencia del archivo
         IDataList list = Workspace.DataService.GetDataList(dataListId);

         if (list != null)
         {
            response = new AjaxResponse(list);
            response.Result = AjaxResponse.JsonResponse.Successful;
         }
         else
         {
            response = new AjaxResponse(AjaxResponse.ERRCODE_OBJECT_NOT_FOUND, 
                                        "DataList [" + dataListId + "] not found in workspace.");
            response.Result = AjaxResponse.JsonResponse.Fail;
         }

         SendResponse(response);
      }

      #endregion

      #region Static Members

      /// <summary>
      /// Devuelve la URL a la que se debe atacar para realizar operaciones REST sobre el servicio.
      /// </summary>
      public static string ServiceUrl
      {
         get { return MethodBase.GetCurrentMethod().DeclaringType.Name; }
      }

      #endregion

   }
}
