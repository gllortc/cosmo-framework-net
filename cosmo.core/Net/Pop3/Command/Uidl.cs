using System;
using System.Text.RegularExpressions;

namespace Cosmo.Net.Pop3.Command
{

   /// <summary>
   /// Representa el comando UIDL.
   /// </summary>
   public class Uidl : Pop3Command
   {
      private Int64? zMailIndex = null;

      /// <summary>
      /// Devuelve una instancia de Uidl.
      /// </summary>
      public Uidl() { }

      /// <summary>
      /// Devuelve una instancia de Uidl.
      /// </summary>
      public Uidl(Int64 inMailIndex)
      {
         if (inMailIndex < 1)
         { throw new ArgumentException(); }
         this.zMailIndex = inMailIndex;
      }

      /// <summary>
      /// Función de callback.
      /// </summary>
      public delegate void Callback(Uidl.Result[] inResult);

      /// <summary>
      /// Nombre del comando.
      /// </summary>
      public override String Name
      {
         get { return "Uidl"; }
      }

      /// <summary>
      /// Índice del correo electrónico.
      /// </summary>
      public Int64? MailIndex
      {
         get { return this.zMailIndex; }
      }

      /// <summary>
      /// Obtiene el texto de ejecución del comando.
      /// </summary>
      public override String GetCommandString()
      {
         return String.Format("{0} {1}", this.Name, this.zMailIndex);
      }

      /// <summary>
      /// Devuelve el índice del mensaje.
      /// </summary>
      public static Int64 GetMessageIndex(String inLine)
      {
         return Int64.Parse(Regex.Replace(inLine.Replace("\r\n", string.Empty), @"^([0-9]+)[ |	]+.*$", "$1"));
      }

      /// <summary>
      /// Devuelve el UID (identificador) del mensaje.
      /// </summary>
      public static String GetUid(String inLine)
      {
         return Regex.Replace(inLine.Replace("\r\n", string.Empty), @"^[0-9]+[ |	]+([0-9a-fA-F]+).*$", "$1");
      }

      /// <summary>
      /// Implementa el resultado de la ejecución del comando UIDL.
      /// </summary>
      public class Result
      {
         private Int64 zMailIndex = 0;
         private String zUid = string.Empty;

         /// <summary>
         /// Devuelve una instancia de Result.
         /// </summary>
         /// <param name="inText"></param>
         public Result(String inText)
         {
            this.zMailIndex = Uidl.GetMessageIndex(inText);
            this.zUid = Uidl.GetUid(inText);
         }

         /// <summary>
         /// Devuelve el índice del mensaje.
         /// </summary>
         public Int64 MailIndex
         {
            get { return this.zMailIndex; }
         }

         /// <summary>
         /// Devuelve el identificador UID del mensaje.
         /// </summary>
         public String Uid
         {
            get { return this.zUid; }
         }

         /// <summary>
         /// Verifica el formato.
         /// </summary>
         public static Boolean CheckFormat(String inLine)
         {
            return Regex.IsMatch(inLine.Replace("\r\n", string.Empty), @"^([0-9]+)[ |	]+([\x21-\x7e]+).*$", RegexOptions.None);
         }
      }
   }

}
