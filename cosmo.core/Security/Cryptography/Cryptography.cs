using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Cosmo.Security.Cryptography
{
   /// <summary>
   /// Implementa una clase para la encriptación/desecriptación de datos seguros
   /// </summary>
   public class Cryptography
   {
      /// <summary>
      /// Cifra (encripta) una cadena de texto.
      /// </summary>
      /// <param name="toEncrypt">Cadena de texto a cifrar.</param>
      /// <param name="key">Clave usada para encriptar los datos</param>
      /// <param name="useHashing">Activa un cifrado más fuerte (seguro).</param>
      /// <returns>El texto original cifrado.</returns>
      public static string Encrypt(string toEncrypt, string key, bool useHashing)
      {
         byte[] keyArray;
         byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);

         if (useHashing)
         {
            MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
            keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
            hashmd5.Clear();
         }
         else
            keyArray = UTF8Encoding.UTF8.GetBytes(key);

         TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
         tdes.Key = keyArray;
         tdes.Mode = CipherMode.ECB;
         tdes.Padding = PaddingMode.PKCS7;

         ICryptoTransform cTransform = tdes.CreateEncryptor();
         byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
         tdes.Clear();
         return Convert.ToBase64String(resultArray, 0, resultArray.Length);
      }

      /// <summary>
      /// Descifra (desencripta) una cadena cifrada anteriormente con el método Encrypt().
      /// </summary>
      /// <param name="cipherString">Cadena cifrada.</param>
      /// <param name="key">Clave usada para desencriptar los datos</param>
      /// <param name="useHashing">Activar si para el cifrado se usó Hashing.</param>
      /// <returns>La cadena original descrifrada.</returns>
      public static string Decrypt(string cipherString, string key, bool useHashing)
      {
         byte[] keyArray;
         byte[] toEncryptArray = Convert.FromBase64String(cipherString);

         if (useHashing)
         {
            MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
            keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
            hashmd5.Clear();
         }
         else
            keyArray = UTF8Encoding.UTF8.GetBytes(key);

         TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
         tdes.Key = keyArray;
         tdes.Mode = CipherMode.ECB;
         tdes.Padding = PaddingMode.PKCS7;

         ICryptoTransform cTransform = tdes.CreateDecryptor();
         byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

         tdes.Clear();
         return UTF8Encoding.UTF8.GetString(resultArray);
      }
   }
}