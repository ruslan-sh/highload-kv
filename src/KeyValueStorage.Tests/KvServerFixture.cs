using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using RuslanSh.KeyValueStorage.Core;

namespace RuslanSh.KeyValueStorage.Tests
{
	public class KvServerFixture : IKvServer
	{
		private readonly string _host;
		private readonly IKvServer _kvServer;
		private readonly string _path;
		private readonly int _port;

		public KvServerFixture(string host, int port, string path)
		{
			_host = host;
			_port = port;
			_path = path;
			_kvServer = KvServerFactory.Create(host, port, path);
		}

		public void Start()
		{
			_kvServer.Start();
		}

		public void Stop()
		{
			_kvServer.Stop();
		}

		public async Task<byte[]> GetAsync(string requestPath, string queryString = null)
		{
			using (var client = GetClient())
			{
				return await client.GetByteArrayAsync(GetUrl(requestPath, queryString));
			}
		}

		public async Task<HttpResponseMessage> DeleteAsync(string requestPath, string queryString)
		{
			using (var client = GetClient())
			{
				return await client.DeleteAsync(GetUrl(requestPath, queryString));
			}
		}

		public async Task<HttpResponseMessage> PostAsync(string requestPath, string queryString,
			byte[] data)
		{
			using (var client = GetClient())
			{
				return await client.PostAsync(GetUrl(requestPath, queryString), new ByteArrayContent(data));
			}
		}

		private static HttpClient GetClient()
		{
			return new HttpClient {Timeout = TimeSpan.FromSeconds(10)};
		}

		public async Task<HttpStatusCode> StatusAsync()
		{
			using (var client = GetClient())
			{
				return (await client.GetAsync(GetUrl("status"))).StatusCode;
			}
		}

		private string GetUrl(string requestPath, string queryString = null)
		{
			var prefix = $"http://{_host}:{_port}/";
			var statusRequest = $"{prefix}{requestPath}";
			if (!string.IsNullOrWhiteSpace(queryString))
				statusRequest = $"{statusRequest}?{queryString}";
			return statusRequest;
		}
	}
}