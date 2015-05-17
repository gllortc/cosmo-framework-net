using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Cosmo.Security.Cryptography
{
   /// <summary>
   /// Implementa una clase para encriptar/desencriptar cadenas de texto correspondientes a URLs.
   /// </summary>
   public class UriCryptography
   {
      /// <summary>
      /// Encrypts any string using the Rijndael algorithm.
      /// </summary>
      /// <param name="originalText">The string to encrypt.</param>
      /// <param name="key">Clave de encriptación que se usará para encriptar el texto.</param>
      /// <returns>A Base64 encrypted string.</returns>
      public static string Encrypt(string originalText, string key)
      {
         RijndaelManaged cipher = new RijndaelManaged();
         byte[] plainText = Encoding.Unicode.GetBytes(originalText);
         PasswordDeriveBytes pwd = new PasswordDeriveBytes(key, Encoding.ASCII.GetBytes(key.Length.ToString()));

         using (ICryptoTransform encryptor = cipher.CreateEncryptor(pwd.GetBytes(32), pwd.GetBytes(16)))
         {
            using (MemoryStream memoryStream = new MemoryStream())
            {
               using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
               {
                  cryptoStream.Write(plainText, 0, plainText.Length);
                  cryptoStream.FlushFinalBlock();
                  return Convert.ToBase64String(memoryStream.ToArray());
               }
            }
         }
      }

      /// <summary>
      /// Decrypts a previously encrypted string.
      /// </summary>
      /// <param name="encryptedText">The encrypted string to decrypt.</param>
      /// <param name="key">Clave de encriptación usada para encriptar el texto.</param>
      /// <returns>A decrypted string.</returns>
      public static string Decrypt(string encryptedText, string key)
      {
         RijndaelManaged cipher = new RijndaelManaged();
         byte[] encryptedData = Convert.FromBase64String(encryptedText);
         PasswordDeriveBytes pwd = new PasswordDeriveBytes(key, Encoding.ASCII.GetBytes(key.Length.ToString()));

         using (ICryptoTransform decryptor = cipher.CreateDecryptor(pwd.GetBytes(32), pwd.GetBytes(16)))
         {
            using (MemoryStream memoryStream = new MemoryStream(encryptedData))
            {
               using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
               {
                  byte[] plainText = new byte[encryptedData.Length];
                  int decryptedCount = cryptoStream.Read(plainText, 0, plainText.Length);
                  return Encoding.Unicode.GetString(plainText, 0, decryptedCount);
               }
            }
         }
      }
   }
}
