using System;
using System.Text.RegularExpressions;

namespace Cosmo.Net.Pop3.Command
{

   /// <summary>
   /// Representa el comando STAT.
   /// </summary>
   public class Stat : Pop3Command
   {
      /// <summary>
      /// Función de callback.
      /// </summary>
      public delegate void Callback(Stat.Result inResult);

      /// <summary>
      /// Nombre del comando.
      /// </summary>
      public override String Name
      {
         get { return "Stat"; }
      }

      /// <summary>
      /// Obtiene el texto de ejecución del comando.
      /// </summary>
      public override String GetCommandString()
      {
         return this.Name;
      }

      /// <summary>
      /// Analyze response single line and get total mail count of mailbox.
      /// </summary>
      public static Int64 GetTotalMessageCount(String inText)
      {
         return Int64.Parse(Regex.Replace(inText.Replace("\r\n", string.Empty)
             , @"^.*\+OK[ |	]+([0-9]+)[ |	]+.*$", "$1"));
      }

      /// <summary>
      /// Analyze response single line and get total mail size of mailbox.
      /// </summary>
      public static Int64 GetTotalSize(String inText)
      {
         return Int64.Parse(Regex.Replace(inText.Replace("\r\n", string.Empty)
             , @"^.*\+OK[ |	]+[0-9]+[ |	]+([0-9]+).*$", "$1"));
      }

      /// <summary>
      /// Represents result of stat command.
      /// </summary>
      public class Result
      {
         private Int64 zTotalMessageCount = 0;
         private Int64 zTotalSize = 0;

         /// <summary>
         /// Número de mensajes.
         /// </summary>
         public Int64 TotalMessageCount
         {
            get { return this.zTotalMessageCount; }
         }

         /// <summary>
         /// Tamaño total.
         /// </summary>
         public Int64 TotalSize
         {
            get { return this.zTotalSize; }
         }

         /// <summary>
         /// Resultado.
         /// </summary>
         public Result(String inText)
         {
            this.zTotalMessageCount = Stat.GetTotalMessageCount(inText);
            this.zTotalSize = Stat.GetTotalSize(inText);
         }
      }
   }

}
