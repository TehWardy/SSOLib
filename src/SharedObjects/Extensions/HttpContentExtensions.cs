namespace SharedObjects.Extensions
{
    public static class HttpContentExtensions
    {
        public static async ValueTask<T> ReadAsAsync<T>(this HttpContent content) => Data.ParseJson<T>(await content.ReadAsStringAsync());
    }
}