using System.IO.Compression;
using System.Text;

namespace Core.Extensions
{
    public static class GZipExtensions
    {
        public static void CompressFile(string sDir, string sRelativePath, GZipStream zipStream)
        {
            char[] charArray = sRelativePath.ToCharArray();
            zipStream.Write(BitConverter.GetBytes(charArray.Length), 0, 4);
            foreach (char ch in charArray)
                zipStream.Write(BitConverter.GetBytes(ch), 0, 2);
            byte[] buffer = File.ReadAllBytes(Path.Combine(sDir, sRelativePath));
            zipStream.Write(BitConverter.GetBytes(buffer.Length), 0, 4);
            zipStream.Write(buffer, 0, buffer.Length);
        }

        public static bool DecompressFile(
          string sDir,
          GZipStream zipStream,
          GZipExtensions.ProgressDelegate progress)
        {
            byte[] buffer1 = new byte[4];
            if (zipStream.Read(buffer1, 0, 4) < 4)
                return false;
            int int32_1 = BitConverter.ToInt32(buffer1, 0);
            byte[] buffer2 = new byte[2];
            StringBuilder stringBuilder = new StringBuilder();
            for (int index = 0; index < int32_1; ++index)
            {
                zipStream.Read(buffer2, 0, 2);
                char ch = BitConverter.ToChar(buffer2, 0);
                stringBuilder.Append(ch);
            }
            string str = stringBuilder.ToString();
            if (progress.NotNull<GZipExtensions.ProgressDelegate>())
                progress(str);
            byte[] buffer3 = new byte[4];
            zipStream.Read(buffer3, 0, 4);
            int int32_2 = BitConverter.ToInt32(buffer3, 0);
            byte[] buffer4 = new byte[int32_2];
            zipStream.Read(buffer4, 0, buffer4.Length);
            string path = Path.Combine(sDir, str);
            string directoryName = Path.GetDirectoryName(path);
            if (!Directory.Exists(directoryName))
                Directory.CreateDirectory(directoryName);
            using (FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None))
                fileStream.Write(buffer4, 0, int32_2);
            return true;
        }

        public static void CompressDirectory(
          string sInDir,
          string sOutFile,
          GZipExtensions.ProgressDelegate progress)
        {
            IEnumerable<string> strings = ((IEnumerable<string>)Directory.GetFiles(sInDir, "*.*", SearchOption.AllDirectories)).Where<string>((Func<string, bool>)(w => !Path.GetExtension(w).Equals(".zip")));
            int startIndex = (int)sInDir[sInDir.Length - 1] == (int)Path.DirectorySeparatorChar ? sInDir.Length : sInDir.Length + 1;
            using (FileStream fileStream = new FileStream(sOutFile, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                using (GZipStream zipStream = new GZipStream((Stream)fileStream, CompressionMode.Compress))
                {
                    foreach (string str1 in strings)
                    {
                        string str2 = str1.Substring(startIndex);
                        if (progress != null)
                            progress(str2);
                        GZipExtensions.CompressFile(sInDir, str2, zipStream);
                    }
                }
            }
        }

        public static void DecompressToDirectory(
          string sCompressedFile,
          string sDir,
          GZipExtensions.ProgressDelegate progress)
        {
            using (FileStream fileStream = new FileStream(sCompressedFile, FileMode.Open, FileAccess.Read, FileShare.None))
            {
                using (GZipStream zipStream = new GZipStream((Stream)fileStream, CompressionMode.Decompress, true))
                {
                    do
                        ;
                    while (GZipExtensions.DecompressFile(sDir, zipStream, progress));
                }
            }
        }

        public delegate void ProgressDelegate(string sMessage);
    }
}
