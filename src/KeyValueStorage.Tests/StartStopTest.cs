using System;
using System.Net;
using System.Net.Http;
using Xunit;

namespace RuslanSh.KeyValueStorage.Tests
{
	[Collection("Http Server Tests")]
	public class StartStopTest : IDisposable
	{
		public StartStopTest(TestFixture fixture)
		{
			_fixture = fixture;
			_defaultServer = _fixture.DefaultKvServer;
		}

		public void Dispose()
		{ }

		private readonly TestFixture _fixture;
		private readonly KvServerFixture _defaultServer;

		[Fact]
		public void Status_ServerNotStart_ThrowsAnException()
		{
			Assert.Throws<HttpRequestException>(() => _fixture.GetServer().Status());
		}

		[Fact]
		public void Status_ServerStarted_ReturnsOk()
		{
			var result = _defaultServer.Status();
			Assert.Equal(HttpStatusCode.OK, result);
		}

		[Fact]
		public void Status_ServerStoped_ThrowsAnException()
		{
			var server = _fixture.GetServer();
			server.Start();
			server.Stop();
			Assert.Throws<HttpRequestException>(() => server.Status());
		}
	}
}