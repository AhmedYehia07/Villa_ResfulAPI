namespace Villa_Utility
{
    public class SD
    {
        public enum ApiType
        {
            Get,
            Post,
            Put,
            Delete
        }
        public static string SessionToken = "JWTToken";
        public static string Version = "v2";
        public const string Admin = "admin";
        public const string Customer = "customer";
    }
}
