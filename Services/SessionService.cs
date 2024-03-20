using Newtonsoft.Json;

namespace ShoppingWebsite.Services
{
    public static class SessionService
    {
        public static T GetSessionValue<T>(this HttpContext context, string sessionKey)
        {
            string value = context.Session.GetString(sessionKey);
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }

        public static void AddToSession(this HttpContext context, string sessionKey, object data)
        {
            context.Session.SetString(sessionKey, JsonConvert.SerializeObject(data));
        }

        public static void DeleteSession(this HttpContext context, string sessionKey)
        {
            context.Session.Remove(sessionKey);
        }
    }
}
