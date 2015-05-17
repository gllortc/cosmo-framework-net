using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;

namespace Cosmo.Utils.Json
{

   #region class JSONWriter

   /// <summary>
   /// Clase que permite manejar cadenas de texto en formato JSON.
   /// </summary>
   internal class JSONWriter
   {
      public static string Enquote(string s)
      {
         if (s == null || s.Length == 0)
            return "\"\"";

         return Enquote(s, null).ToString();
      }

      public static StringBuilder Enquote(string s, StringBuilder sb)
      {
         int length = Mask.NullString(s).Length;

         if (sb == null)
            sb = new StringBuilder(length + 4);

         sb.Append('"');

         char last;
         char ch = '\0';

         for (int index = 0; index < length; index++)
         {
            last = ch;
            ch = s[index];

            switch (ch)
            {
               case '\\':
               case '"':
                  {
                     sb.Append('\\');
                     sb.Append(ch);
                     break;
                  }

               case '/':
                  {
                     if (last == '<')
                        sb.Append('\\');
                     sb.Append(ch);
                     break;
                  }

               case '\b': sb.Append("\\b"); break;
               case '\t': sb.Append("\\t"); break;
               case '\n': sb.Append("\\n"); break;
               case '\f': sb.Append("\\f"); break;
               case '\r': sb.Append("\\r"); break;

               default:
                  {
                     if (ch < ' ')
                     {
                        sb.Append("\\u");
                        sb.Append(((int)ch).ToString("x4", CultureInfo.InvariantCulture));
                     }
                     else
                     {
                        sb.Append(ch);
                     }

                     break;
                  }
            }
         }

         return sb.Append('"');
      }

      /// <summary>
      /// Return the characters up to the next close quote character.
      /// Backslash processing is done. The formal JSON format does not
      /// allow strings in single quotes, but an implementation is allowed to
      /// accept them.
      /// </summary>
      /// <param name="input">carácteres de entrada.</param>
      /// <param name="quote">The quoting character, either " or '</param>
      /// <returns>A String.</returns>
      internal static string Dequote(BufferedCharReader input, char quote)
      {
         return Dequote(input, quote, null).ToString();
      }

      internal static StringBuilder Dequote(BufferedCharReader input, char quote, StringBuilder output)
      {
         Debug.Assert(input != null);

         if (output == null)
            output = new StringBuilder();

         char[] hexDigits = null;

         while (true)
         {
            char ch = input.Next();

            if ((ch == BufferedCharReader.EOF) || (ch == '\n') || (ch == '\r'))
               throw new FormatException("Unterminated string.");

            if (ch == '\\')
            {
               ch = input.Next();

               switch (ch)
               {
                  case 'b': output.Append('\b'); break; // Backspace
                  case 't': output.Append('\t'); break; // Horizontal tab
                  case 'n': output.Append('\n'); break; // Newline
                  case 'f': output.Append('\f'); break; // Form feed
                  case 'r': output.Append('\r'); break; // Carriage return 

                  case 'u':
                     {
                        if (hexDigits == null)
                           hexDigits = new char[4];

                        output.Append(ParseHex(input, hexDigits));
                        break;
                     }

                  default:
                     output.Append(ch);
                     break;
               }
            }
            else
            {
               if (ch == quote)
                  return output;

               output.Append(ch);
            }
         }
      }

      /// <summary>
      /// Eats the next four characters, assuming hex digits, and converts
      /// into the represented character value.
      /// </summary>
      /// <returns>The parsed character.</returns>
      private static char ParseHex(BufferedCharReader input, char[] hexDigits)
      {
         Debug.Assert(input != null);
         Debug.Assert(hexDigits != null);
         Debug.Assert(hexDigits.Length == 4);

         hexDigits[0] = input.Next();
         hexDigits[1] = input.Next();
         hexDigits[2] = input.Next();
         hexDigits[3] = input.Next();

         return (char)ushort.Parse(new string(hexDigits), NumberStyles.HexNumber);
      }
   }

   #endregion

   #region class BufferedCharReader

   internal sealed class BufferedCharReader
   {
      private TextReader _reader;
      private char[] _buffer;
      private int _index;
      private int _end;
      private bool _backed;
      private char _backup;
      private int _bufferSize;

      public const char EOF = (char)0;

      public BufferedCharReader(TextReader reader) : this(reader, 0) { }

      public BufferedCharReader(TextReader reader, int bufferSize)
      {
         Debug.Assert(reader != null);

         _reader = reader;
         _bufferSize = Math.Max(256, bufferSize);
      }

      /// <summary>
      /// Back up one character. This provides a sort of lookahead capability,
      /// so that one can test for a digit or letter before attempting to,
      /// for example, parse the next number or identifier.
      /// </summary>
      /// <remarks>
      /// This implementation currently does not support backing up more
      /// than a single character (the last read).
      /// </remarks>
      public void Back()
      {
         Debug.Assert(!_backed);

         if (_index > 0)
         {
            _backup = _buffer[_index - 1];
            _backed = true;
         }
      }

      /// <summary>
      /// Determine if the source string still contains characters that Next()
      /// can consume.
      /// </summary>
      /// <returns>true if not yet at the end of the source.</returns>
      public bool More()
      {
         if (_index == _end)
         {
            if (_buffer == null) _buffer = new char[_bufferSize];

            _index = 0;
            _end = _reader.Read(_buffer, 0, _buffer.Length);

            if (_end == 0) return false;
         }

         return true;
      }

      /// <summary>
      /// Get the next character in the source string.
      /// </summary>
      /// <returns>The next character, or 0 if past the end of the source string.</returns>
      public char Next()
      {
         if (_backed)
         {
            _backed = false;
            return _backup;
         }

         if (!More()) return EOF;

         char ch = _buffer[_index++];
         return ch;
      }
   }

   #endregion

   #region class Mask

   /// <summary>
   /// Provides masking services where one value masks another given a test.
   /// </summary>
   internal sealed class Mask
   {
      public static string NullString(string actual)
      {
         return actual == null ? string.Empty : actual;
      }

      public static string NullString(string actual, string mask)
      {
         return actual == null ? mask : actual;
      }

      public static string EmptyString(string actual, string emptyValue)
      {
         return Mask.NullString(actual).Length == 0 ? emptyValue : actual;
      }

      private Mask()
      {
         throw new NotSupportedException();
      }
   }

   #endregion

}
