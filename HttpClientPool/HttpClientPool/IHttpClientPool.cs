using System.Net.Http;

namespace HttpClientPool
{
    public interface IHttpClientPool
    {
        HttpClient Get(string id = null);
        void Register(string id, HttpClient client, bool overrideIfExists = false);
    }
}
