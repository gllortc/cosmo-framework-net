using System;
using System.Net;

namespace Cosmo.Net.Smtp
{
   /// <summary>
   /// Implementa una excepción para el servicio SMTP.
   /// </summary>
	public class SmtpException : ApplicationException
	{
      /// <summary>
      /// Devuelve una instancia de SmtpException.
      /// </summary>
		public SmtpException() { }

      /// <summary>
      /// Devuelve una instancia de SmtpException.
      /// </summary>
		public SmtpException(String inMessage) : base(inMessage) { }
	}
}