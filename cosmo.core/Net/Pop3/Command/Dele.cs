using System;

namespace Cosmo.Net.Pop3.Command
{

   /// <summary>
   /// Representa el comando DELE.
   /// </summary>
   public class Dele : Pop3Command
   {
      private Int64 zMailIndex = 1;

      /// <summary>
      /// Devuelve una instancia de Dele.
      /// </summary>
      public Dele(Int64 inMailIndex)
      {
         if (inMailIndex < 1)
         { throw new ArgumentException(); }
         this.zMailIndex = inMailIndex;
      }

      /// <summary>
      /// Nombre del comando.
      /// </summary>
      public override String Name
      {
         get { return "Dele"; }
      }

      /// <summary>
      /// Índice del correo electrónico.
      /// </summary>
      public Int64 MailIndex
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
   }

}
