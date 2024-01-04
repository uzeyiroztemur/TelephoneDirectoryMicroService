using System;
using System.IO;

namespace Core.Extensions
{
  public static class SeoExtensions
  {
    private static string GenerateSeoText(this string value, bool isFile = false)
    {
      string path = value.ToLowerEnglish().Replace("!", string.Empty).Replace("'", string.Empty).Replace("^", string.Empty).Replace("+", string.Empty).Replace("$", string.Empty).Replace("\"", string.Empty).Replace("%", string.Empty).Replace("&", string.Empty).Replace("/", string.Empty).Replace("(", string.Empty).Replace("[", string.Empty).Replace(")", string.Empty).Replace("]", string.Empty).Replace("{", string.Empty).Replace("}", string.Empty).Replace("=", string.Empty).Replace("\\", string.Empty).Replace("?", string.Empty).Replace("*", string.Empty).Replace("_", "-").Replace(" ", "-").Replace("~", string.Empty).Replace("@", string.Empty).Replace("₺", string.Empty).Replace("|", string.Empty).Replace("<", string.Empty).Replace(">", string.Empty).Replace(".", isFile ? "." : string.Empty).Replace(",", string.Empty).Replace(";", string.Empty);
      if (isFile)
      {
        string withoutExtension = Path.GetFileNameWithoutExtension(path);
        string extension = Path.GetExtension(path);
        path = string.Format("{0}-{1}", (object) withoutExtension, (object) Guid.NewGuid().ToString().Substring(0, 4)) + extension;
      }
      return path;
    }

    public static string GenerateUrl(this string value) => value.GenerateSeoText();

    public static string GenerateFileName(this string value) => value.GenerateSeoText(true);
  }
}
