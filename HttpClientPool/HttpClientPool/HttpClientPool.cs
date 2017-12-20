using System;
using System.Collections;
using System.Net.Http;

namespace HttpClientPool
{
    public class HttpClientPool : IHttpClientPool
    {
        private const string NullKey = "__default__";
        private readonly Hashtable _pool;

        public HttpClientPool()
        {
            _pool = new Hashtable {{ NullKey, new HttpClient()}};
        }

        public HttpClient Get(string id = null)
        {
            if (id == null)
            {
                return _pool[NullKey] as HttpClient;
            }
            else
            {
                if (!_pool.Contains(id))
                {
                    throw new ArgumentException($"HttpClient with id {id} does not exist.", nameof(id));
                }

                return _pool[id] as HttpClient;
            }
        }

        public void Register(string id, HttpClient client, bool overrideIfExists = false)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            if (id == NullKey)
            {
                throw new ArgumentException($"{id} is a reserved id.", nameof(id));
            }

            if (!overrideIfExists && _pool.Contains(id))
            {
                throw new ArgumentException($"HttpClient {id} already exists.", nameof(id));
            }

            _pool[id] = client;
        }
    }
}