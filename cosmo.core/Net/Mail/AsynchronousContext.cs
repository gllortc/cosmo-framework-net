using System;
using System.Text;

namespace Cosmo.Net.Mail
{

   /// <summary>
   /// Used as call back function that is called when request is completed and get response data.
   /// </summary>
   public delegate void EndGetResponse(String inResponseString);

   /// <summary>
   /// Represent context of request and response process and provide data about context.
   /// </summary>
   public class AsynchronousContext
   {
      private Byte[] zData = null;
      private StringBuilder zText = new StringBuilder();
      private Predicate<String> zIsCompleteFunction = null;
      private EndGetResponse zFunction = null;

      /// <summary>
      /// Devuelve una instancia de AsynchronousContext.
      /// </summary>
      public AsynchronousContext(Int32 inBufferSize, Predicate<String> inIsCompleteFunction, EndGetResponse inFunction)
      {
         this.zData = new Byte[inBufferSize];
         this.zIsCompleteFunction = inIsCompleteFunction;
         this.zFunction = inFunction;
      }

      /// <summary>
      /// Datos del contexto.
      /// </summary>
      public Byte[] Data
      {
         get { return this.zData; }
      }

      /// <summary>
      /// Convert bytes data to text.It will return true when complete receiving data.
      /// </summary>
      internal Boolean Parse(Int32 inCount)
      {
         String s = string.Empty;

         s = Encoding.ASCII.GetString(this.zData, 0, inCount);
         this.zText.Append(s);
         if (this.zIsCompleteFunction(s) == true)
         {
            this.zFunction(this.zText.ToString());
            return true;
         }
         return false;
      }
   }

}
