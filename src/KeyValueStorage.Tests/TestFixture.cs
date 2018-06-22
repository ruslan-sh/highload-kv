using System;
using System.Collections.Generic;
using System.IO;

namespace RuslanSh.KeyValueStorage.Tests
{
	// ReSharper disable once ClassNeverInstantiated.Global
	// Used for run tests
	public class TestFixture : IDisposable
	{
		private static readonly Random _rng = new Random();
		private readonly string _host = "localhost";
		private string _path;
		private readonly List<IKvServer> _kvServersPool = new List<IKvServer>();
		public readonly KvServerFixture DefaultKvServer; 

		public TestFixture()
		{
			InitTempPath();
			DefaultKvServer = GetServer();
			DefaultKvServer.Start();
		}

		public void Dispose()
		{
			Directory.Delete(_path);
			DefaultKvServer.Stop();
		}

		public KvServerFixture GetServer()
		{
			var port = GenerateRandomPort();
			var server = new KvServerFixture(_host, port, _path);
			_kvServersPool.Add(server);
			return server;
		}

		private int GenerateRandomPort()
		{
			// Get port from dynamic ports
			return _rng.Next(49152, 65535);
		}

		private void InitTempPath()
		{
			_path = $"{Path.GetTempPath()}kvt_{Guid.NewGuid()}";
			Directory.CreateDirectory(_path);
		}
	}
}