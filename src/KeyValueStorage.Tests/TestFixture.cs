using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using RuslanSh.KeyValueStorage.Core;

namespace RuslanSh.KeyValueStorage.Tests
{
	// ReSharper disable once ClassNeverInstantiated.Global
	// Used for run tests
	public class TestFixture : IDisposable
	{
		private static readonly Random _rng = new Random();
		private readonly string _host;
		private readonly int _port;
		private string _path;

		public TestFixture()
		{
			InitTempPath();
			_host = "localhost";
			_port = GenerateRandomPort();
			Server = KvServerFactory.Create(_host, _port, _path);
		}

		public IKvServer Server { get; }

		public void Dispose()
		{
			Directory.Delete(_path);
		}

		private string GetTestUri()
		{
			return $"http://{_host}:{_port}/";
		}

		private int GenerateRandomPort()
		{
			return _rng.Next(49152, 65535);
		}

		private void InitTempPath()
		{
			_path = $"{Path.GetTempPath()}kvt_{Guid.NewGuid()}";
			Directory.CreateDirectory(_path);
		}

		public async Task<HttpStatusCode> CheckServerStatus()
		{
			var client = new HttpClient {Timeout = TimeSpan.FromSeconds(10)};
			var prefix = GetTestUri();
			var statusRequest = $"{prefix}status";
			var response = await client.GetAsync(statusRequest);
			return response.StatusCode;
		}
	}
}