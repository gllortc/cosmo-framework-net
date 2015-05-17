using System;
using System.Collections.Generic;
using System.Text;

namespace Cosmo.Net.Pop3.Command
{

   /// <summary>
   /// Representa el comando TOP.
   /// </summary>
   public class Top : Pop3Command
   {
      private Int64 zMailIndex = 1;
      private Int32 zLineCount = 0;

      /// <summary>
      /// Devuelve una instancia de Top.
      /// </summary>
      public Top(Int64 inMailIndex)
      {
         if (inMailIndex < 1)
         { throw new ArgumentException(); }
         this.zMailIndex = inMailIndex;
      }

      /// <summary>
      /// Devuelve una instancia de Top.
      /// </summary>
      public Top(Int64 inMailIndex, Int32 inLineCount)
      {
         if (inMailIndex < 1)
         { throw new ArgumentException(); }
         this.zMailIndex = inMailIndex;
         this.zLineCount = inLineCount;
      }

      /// <summary>
      /// Nombre del comando.
      /// </summary>
      public override String Name
      {
         get { return "Top"; }
      }

      /// <summary>
      /// Índice del correo electrónico.
      /// </summary>
      public Int64 MailIndex
      {
         get { return this.zMailIndex; }
      }

      /// <summary>
      /// Devuelve el número de líneas del mensaje.
      /// </summary>
      public Int32 LineCount
      {
         get { return this.zLineCount; }
      }

      /// <summary>
      /// Obtiene el texto de ejecución del comando.
      /// </summary>
      public override String GetCommandString()
      {
         return String.Format("{0} {1} {2}", this.Name, this.zMailIndex, this.zLineCount);
      }
   }

}
