using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace OperatingManagement.Framework.Helper
{
    /// <summary>
    /// Provides a class for html operations
    /// </summary>
    public class HtmlHelper
    {
        private static Regex isWhitespace = new Regex("[^\\w&;#]", RegexOptions.Singleline | RegexOptions.Compiled);

        private static Regex htmlRegex = new Regex("<[^>]+>|\\&nbsp\\;", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        private static Regex spacer = new Regex(@"\s{2,}", RegexOptions.Compiled);

        /// <summary>
        /// Gets the substring from text(ignore html tags).
        /// </summary>
        /// <param name="text">Text</param>
        /// <param name="charLimit">Max length</param>
        /// <returns>string</returns>
        public static string MaxLength(string text, int charLimit)
        {
            if (string.IsNullOrEmpty(text))
                return string.Empty;

            if (charLimit >= text.Length)
                return text;

            Match match = isWhitespace.Match(text, charLimit);
            if (!match.Success)
                return text;
            else
                return text.Substring(0, match.Index) + "...";
        }

        /// <summary>
        /// Clear the html tags.
        /// </summary>
        /// <param name="html">HTML</param>
        /// <returns>string</returns>
        public static string RemoveHtml(string html)
        {
            return RemoveHtml(html, 0);
        }

        /// <summary>
        /// Clear the html tags and return text with limited length.
        /// </summary>
        /// <param name="html">HTML</param>
        /// <param name="charLimit">Max Length</param>
        /// <returns>string</returns>
        public static string RemoveHtml(string html, int charLimit)
        {
            if (string.IsNullOrEmpty(html))
                return string.Empty;

            string nonhtml = spacer.Replace(htmlRegex.Replace(html, " ").Trim(), " ");
            if (charLimit <= 0 || charLimit >= nonhtml.Length)
                return nonhtml;
            else
                return MaxLength(nonhtml, charLimit);
        }

        /// <summary>
        /// Strip all the html tags.
        /// </summary>
        /// <param name="stringToStrip"></param>
        /// <returns></returns>
        public static string StripAllTags(string stringToStrip)
        {
            return StripAllTags(stringToStrip, true);
        }

        /// <summary>
        /// Strip all the html tags.
        /// </summary>
        /// <param name="html">Html</param>
        /// <param name="enableMultiLine">Enable Multiline</param>
        /// <returns></returns>
        public static string StripAllTags(string stringToStrip, bool enableMultiLine)
        {
            if (enableMultiLine)
            {
                stringToStrip = Regex.Replace(stringToStrip, @"</p(?:\s*)>(?:\s*)<p(?:\s*)>", "\n\n", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                stringToStrip = Regex.Replace(stringToStrip, @"<br(?:\s*)/>", "\n", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            }
            else
            {
                stringToStrip = Regex.Replace(stringToStrip, @"</p(?:\s*)>(?:\s*)<p(?:\s*)>", string.Empty, RegexOptions.Compiled | RegexOptions.IgnoreCase);
                stringToStrip = Regex.Replace(stringToStrip, @"<br(?:\s*)/>", string.Empty, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            }
            stringToStrip = Regex.Replace(stringToStrip, "\"", "''", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            stringToStrip = Regex.Replace(stringToStrip, "&[^;]*;", string.Empty, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            stringToStrip = Regex.Replace(stringToStrip, "<[^>]+>", string.Empty, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            return stringToStrip;
        }


        /// <summary>
        /// Trim Html and return the substring.
        /// </summary>
        /// <param name="sourceHtml">HTML</param>
        /// <param name="charLimit">Max Length</param>
        /// <returns></returns>
        public static string TrimHtml(string sourceHtml, int charLimit)
        {
            if (string.IsNullOrEmpty(sourceHtml))
            {
                return string.Empty;
            }
            string stringTrim = StripAllTags(sourceHtml, false);
            if ((charLimit <= 0) || (charLimit >= stringTrim.Length))
            {
                return stringTrim;
            }
            return Trim(stringTrim, charLimit);
        }

        /// <summary>
        /// Trim Html and return the substring end with '...' whether maxLength less then the Length of stringTrim
        /// </summary>
        /// <param name="stringTrim">HTML</param>
        /// <param name="maxLength">Max Length</param>
        /// <returns></returns>
        public static string Trim(string stringTrim, int maxLength)
        {
            return Trim(stringTrim, maxLength, "...");
        }

        /// <summary>
        /// Trim HTML
        /// </summary>
        /// <param name="rawString">HTML</param>
        /// <param name="maxLength">Max Length</param>
        /// <param name="appendString">End with this</param>
        /// <returns></returns>
        public static string Trim(string rawString, int maxLength, string appendString)
        {
            if (string.IsNullOrEmpty(rawString) || (rawString.Length <= maxLength))
            {
                return rawString;
            }
            if (Encoding.Default.GetBytes(rawString).Length <= (maxLength * 2))
            {
                return rawString;
            }
            int length = Encoding.Default.GetBytes(appendString).Length;
            StringBuilder builder = new StringBuilder();
            int num = 0;
            for (int i = 0; i < rawString.Length; i++)
            {
                char ch = rawString[i];
                builder.Append(ch);
                num += Encoding.Default.GetBytes(new char[] { ch }).Length;
                if (num >= ((maxLength * 2) - length))
                {
                    break;
                }
            }
            return (builder.ToString() + appendString);
        }
    }
}
