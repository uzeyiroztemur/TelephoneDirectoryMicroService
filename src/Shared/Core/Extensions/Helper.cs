using Core.Entities.DTOs;
using System.ComponentModel;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace Core.Extensions
{
    public static class Helper
    {
        private static readonly string[] Punctuations = "0|1|2|3|4|5|6|7|8|9|o|p|m|k|u|z|g|o|!|.|,".Split('|');
        private static readonly CultureInfo _cultureInfo = CultureInfo.InvariantCulture;

        public static double ToDoubleCulture(this string Value, CultureInfo cultureInfo)
        {
            return Value.ToDoubleCulture(double.MinValue, double.MaxValue, cultureInfo);
        }

        public static double ToDoubleCulture(this string Value, double Min, double Max, CultureInfo cultureInfo)
        {
            if (double.TryParse(Value, NumberStyles.Number, cultureInfo, out double i))
                if (i < Min) i = Min;
                else if (i > Max) i = Max;
            return i;
        }

        public static string ToTurkishTitleCase(this string str)
        {
            if (string.IsNullOrEmpty(str)) return str;
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(str.ToLower(CultureInfo.CreateSpecificCulture("tr-TR")));
        }

        public static string ToTitleCase(this string str)
        {
            if (string.IsNullOrEmpty(str)) return str;
            return _cultureInfo.TextInfo.ToTitleCase(str.ToLower());
        }

        public static string ToTitleCaseCulture(this string str)
        {
            if (string.IsNullOrEmpty(str)) return str;
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(str.ToLower());
        }

        public static string FirstCharToUpper(this string input) => input switch
        {
            null => throw new ArgumentNullException(nameof(input)),
            "" => throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input)),
            _ => string.Concat(input[0].ToString().ToUpper(new CultureInfo("en-US", false)), input.AsSpan(1))
        };

        public static void AppendFile(this string path, string message)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            using var fs = new FileStream(path, FileMode.Append, FileAccess.Write);
            using var fw = new StreamWriter(fs);
            fw.WriteLine(message);
            fw.Flush();
        }

        public static void AppendFileWithName(this string path, string fileName, string message)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            using var fs = new FileStream(string.Format(@"{0}\{1}.txt", path, fileName), FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
            using var fw = new StreamWriter(fs);
            fw.WriteLine(message);

            fw.Flush();
            fs.Flush();
        }

        public static string GetDescription<T>(this T enumerationValue) where T : struct
        {
            var type = enumerationValue.GetType();
            if (!type.IsEnum)
            {
                throw new ArgumentException("EnumerationValue must be of Enum type", "enumerationValue");
            }

            //Tries to find a DescriptionAttribute for a potential friendly name
            //for the enum
            var memberInfo = type.GetMember(enumerationValue.ToString());
            if (memberInfo != null && memberInfo.Length > 0)
            {
                var attrs = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attrs != null && attrs.Length > 0)
                {
                    //Pull out the description value
                    return ((DescriptionAttribute)attrs[0]).Description;
                }
            }
            //If we have no description attribute, just return the ToString of the enum
            return enumerationValue.ToString();
        }

        public static string GeneratePassword(int length)
        {
            if (length < 1 || length > 128)
            {
                throw new ArgumentException(nameof(length));
            }

            Random random = new Random();
            StringBuilder stringBuilder = new StringBuilder(length);

            for (int i = 0; i < length; i++)
            {
                int randomIndex = random.Next(Punctuations.Length);
                stringBuilder.Append(Punctuations[randomIndex]);
            }

            return stringBuilder.ToString();
        }

        public static string ConvertByteToString(long byteCount)
        {
            string[] suffixes = { "B", "KB", "MB", "GB", "TB", "PB", "EB" };
            if (byteCount == 0)
            {
                return "0 " + suffixes[0];
            }

            int i = 0;
            double d = (double)byteCount;
            while (d >= 1024 && i < suffixes.Length - 1)
            {
                d /= 1024;
                i++;
            }

            return $"{d:0.##} {suffixes[i]}";
        }

        public static string RandomColor()
        {
            string[] colors = {
                                "#adcc03",
                                "#6bddab",
                                "#f4a609",
                                "#ffde0a",
                                "#00d1ca",
                                "#b159ff",
                                "#7deb00",
                                "#8dbafe",
                                "#ffcc99",
                                "#d4aaff",
                                "#ff6d6d",
                                "#93c655",
                                "#efc851",
                                "#ffa54f",
                                "#f18eba",
                                "#90ee90",
                                "#87cefa",
                                "#8deeee",
                                "#f08080",
                                "#f4a460",
                                "#00e07c",
                                "#fc9336",
                                "#4d97ff",
                                "#fe983e",
                                "#f393f5",
                                "#95bcf5",
                                "#adeb1b",
                                "#64f2de",
                                "#fad4c5",
                                "#f481e9",
                                "#12ce5c"
                            };

            Random random = new Random();
            int randomIndex = random.Next(0, colors.Length);
            return colors[randomIndex];
        }

        public static ExportFileViewModel CreatePath(string templatefile, string basePath)
        {
            string tw = Path.Combine("Resources", "Files");
            var temlateFilePath = Path.Combine(tw, templatefile);
            temlateFilePath = temlateFilePath.Replace("\\", "/");

            string sendReportFilePath = basePath;

            if (templatefile == "Employee_Template.xlsx")
                sendReportFilePath = Path.Combine(sendReportFilePath, "CalisanListesi_" + Guid.NewGuid() + ".xlsx");
            else if (templatefile == "EmployeeLeaveEntitlement_Template.xlsx")
                sendReportFilePath = Path.Combine(sendReportFilePath, "IzinHakedisleri_" + Guid.NewGuid() + ".xlsx");
            else if (templatefile == "LeaveForms_Template.xlsx")
                sendReportFilePath = Path.Combine(sendReportFilePath, "IzinFormlari_" + Guid.NewGuid() + ".xlsx");
            else if (templatefile == "AdvanceForms_Template.xlsx")
                sendReportFilePath = Path.Combine(sendReportFilePath, "AvansFormlari_" + Guid.NewGuid() + ".xlsx");
            else if (templatefile == "ExpenseForms_Template.xlsx")
                sendReportFilePath = Path.Combine(sendReportFilePath, "MasrafFormlari_" + Guid.NewGuid() + ".xlsx");
            else if (templatefile == "OverWorkForms_Template.xlsx")
                sendReportFilePath = Path.Combine(sendReportFilePath, "FazlaMesaiFormlari_" + Guid.NewGuid() + ".xlsx");
            else if (templatefile == "Assets_Template.xlsx")
                sendReportFilePath = Path.Combine(sendReportFilePath, "DemirbasListesi_" + Guid.NewGuid() + ".xlsx");
            else
                sendReportFilePath = Path.Combine(sendReportFilePath, Guid.NewGuid() + ".xlsx");

            ExportFileViewModel result = new() { SendReportFilePath = sendReportFilePath, TemlateFilePath = temlateFilePath };
            return result;
        }
    }
}
