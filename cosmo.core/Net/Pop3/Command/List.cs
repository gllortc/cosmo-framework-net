using System;
using System.Text.RegularExpressions;

namespace Cosmo.Net.Pop3.Command
{

   /// <summary>
   /// Representa el comando LIST.
   /// </summary>
   public class List : Pop3Command
   {
      private Int64? zMailIndex = null;

      /// <summary>
      /// Devuelve una instancia de List.
      /// </summary>
      public List() { }

      /// <summary>
      /// Devuelve una instancia de List.
      /// </summary>
      public List(Int64 inMailIndex)
      {
         if (inMailIndex < 1)
         { throw new ArgumentException(); }
         this.zMailIndex = inMailIndex;
      }

      /// <summary>
      /// Función de callback.
      /// </summary>
      public delegate void Callback(List.Result[] inResult);

      /// <summary>
      /// Nombre del comando.
      /// </summary>
      public override String Name
      {
         get { return "List"; }
      }

      /// <summary>
      /// Índice.
      /// </summary>
      public Int64? MailIndex
      {
         get { return this.zMailIndex; }
         set { this.zMailIndex = value; }
      }

      /// <summary>
      /// Obtiene el texto de ejecución del comando.
      /// </summary>
      public override String GetCommandString()
      {
         return String.Format("{0} {1}", this.Name, this.zMailIndex);
      }

      /// <summary>
      /// Analyze response single line and get mail index.
      /// </summary>
      public static Int64 GetMessageIndex(String inLine)
      {
         if (inLine.ToLower().StartsWith("+ok") == true)
         {
            return Int64.Parse(Regex.Replace(inLine.Replace("\r\n", string.Empty)
                , @"^.*\+OK[ |	]+([0-9]+)[ |	]+.*$", "$1"));
         }
         else
         {
            return Int64.Parse(Regex.Replace(inLine.Replace("\r\n", string.Empty)
                , @"^([0-9]+)[ |	]+.*$", "$1"));
         }
      }

      /// <summary>
      /// Analyze response single line and get mail size.
      /// </summary>
      public static Int32 GetSize(String inLine)
      {
         if (inLine.ToLower().StartsWith("+ok") == true)
         {
            return Int32.Parse(Regex.Replace(inLine.Replace("\r\n", string.Empty)
                , @"^.*\+OK[ |	]+[0-9]+[ |	]+([0-9]+).*$", "$1"));
         }
         else
         {
            return Int32.Parse(Regex.Replace(inLine.Replace("\r\n", string.Empty)
                , @"^[0-9]+[ |	]+([0-9]+).*$", "$1"));
         }
      }

      /// <summary>
      /// Represents result of list command.
      /// </summary>
      public class Result
      {
         private Int64 zMailIndex = 0;
         private Int32 zSize = 0;

         /// <summary>
         /// Índice.
         /// </summary>
         public Int64 MailIndex
         {
            get { return this.zMailIndex; }
         }
         
         /// <summary>
         /// Tamaño.
         /// </summary>
         public Int32 Size
         {
            get { return this.zSize; }
         }
         
         /// <summary>
         /// Resultado.
         /// </summary>
         public Result(String inText)
         {
            this.zMailIndex = List.GetMessageIndex(inText);
            this.zSize = List.GetSize(inText);
         }
      }
   }

}
