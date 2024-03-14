using Newtonsoft.Json;

namespace ExcelToDatabase.Session
{
    public static class SessionExtensions
    {

        public static void MyNewSetObject(this ISession session, string key, object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T MyNewGetObject<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }
    }
}
