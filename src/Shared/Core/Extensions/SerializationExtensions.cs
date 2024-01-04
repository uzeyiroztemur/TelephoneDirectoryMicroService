using Newtonsoft.Json;

namespace Core.Extensions
{
    public static class SerializationExtensions
    {
        public static string Serialize(object objectToSerialize)
        {
            if (objectToSerialize == null) return null;
            return JsonConvert.SerializeObject(objectToSerialize);
        }

        public static string Serialize<T>(this T objectToSerialize)
        {
            if(objectToSerialize == null) return null;
            return JsonConvert.SerializeObject(objectToSerialize);
        }

        public static T Deserialize<T>(this string stringToDeserialize)
        {
            return JsonConvert.DeserializeObject<T>(stringToDeserialize);
        }
    }
}