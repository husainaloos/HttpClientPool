using HttpClientPool;
using System;
using System.Net.Http;
using Xunit;

namespace HttpClientPoolTest
{
    public class HttpClientPoolTest
    {
        [Fact]
        public void GetShouldGetNullClient()
        {
            var pool = new HttpClientPool.HttpClientPool();
            var client = pool.Get();
            Assert.NotNull(client);
            Assert.IsType<HttpClient>(client);
        }

        [Fact]
        public void GetShouldThrowExceptionIfClientDoesNotExist()
        {
            var pool = new HttpClientPool.HttpClientPool();
            Assert.Throws<ArgumentException>(() =>  pool.Get("non_existing_id"));
        }

        [Fact]
        public void RegisterShouldRejectNullId()
        {
            var pool = new HttpClientPool.HttpClientPool();
            Assert.Throws<ArgumentNullException>(
                () =>  pool.Register(null, new HttpClient()));
        }
        
        [Fact]
        public void RegisterShouldRegisterClientIfTheyAreNotAlreadyThere()
        {
            var client = new HttpClient();
            var pool = new HttpClientPool.HttpClientPool();
            pool.Register("my_id", client);

            var resultClient = pool.Get("my_id");

            Assert.Equal(client, resultClient);
        }

        [Fact]
        public void RegisterShouldNotRegisterIfKeyAlreadyExistsWhenOverrideIsDisabled()
        {
            var client = new HttpClient();
            var pool = new HttpClientPool.HttpClientPool();
            pool.Register("my_id", client);
            Assert.Throws<ArgumentException>(() => pool.Register("my_id", client));
        }

        [Fact]
        public void RegisterShouldThrowExceptionIfIdIsReserved()
        {
            var client = new HttpClient();
            var pool = new HttpClientPool.HttpClientPool();
            Assert.Throws<ArgumentException>(() => pool.Register("__default__", client));
        }

        [Fact]
        public void RegisterShouldRegisterIfKeyAlreadyExistsWhenOverrideIsEnabled()
        {
            var client1 = new HttpClient();
            var client2 = new HttpClient();

            var pool = new HttpClientPool.HttpClientPool();
            pool.Register("my_id", client1, false);
            pool.Register("my_id", client2, true);

            var resultClient = pool.Get("my_id");

            Assert.Equal(client2, resultClient);
        }
    }
}
