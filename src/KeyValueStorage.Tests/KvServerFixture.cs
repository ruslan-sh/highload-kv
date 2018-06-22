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

		public byte[] Get(string requestPath, string queryString = null)
		{
			return SyncRequest(client => client.GetByteArrayAsync(GetUrl(requestPath, queryString)));
		}

		public HttpResponseMessage Delete(string requestPath, string queryString)
		{
			return SyncRequest(client => client.DeleteAsync(GetUrl(requestPath, queryString)));
		}

		public HttpResponseMessage Post(string requestPath, string queryString, byte[] data)
		{
			return SyncRequest(client =>
				client.PostAsync(GetUrl(requestPath, queryString), new ByteArrayContent(data)));
		}

		private static HttpClient GetClient()
		{
			return new HttpClient {Timeout = TimeSpan.FromSeconds(10)};
		}

		public HttpStatusCode Status()
		{
			return SyncRequest(client => client.GetAsync(GetUrl("status"))).StatusCode;
		}

		private string GetUrl(string requestPath, string queryString = null)
		{
			var prefix = $"http://{_host}:{_port}/";
			var statusRequest = $"{prefix}{requestPath}";
			if (!string.IsNullOrWhiteSpace(queryString))
				statusRequest = $"{statusRequest}?{queryString}";
			return statusRequest;
		}

		private T SyncRequest<T>(Func<HttpClient, Task<T>> func)
		{
			try
			{
				using (var client = GetClient())
				{
					return func.Invoke(client).Result;
				}
			}
			catch (AggregateException e)
			{
				throw e.InnerException;
			}
		}
	}
}