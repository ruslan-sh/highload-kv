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
			_defaultServer = _fixture.GetServer();
			_defaultServer.Start();
		}

		public void Dispose()
		{
			_defaultServer.Stop();
		}

		private readonly TestFixture _fixture;
		private KvServerFixture _defaultServer;

		[Fact]
		public async void Status_ServerNotStart_ThrowsAnException()
		{
			await Assert.ThrowsAsync<HttpRequestException>(async () => await _fixture.GetServer().StatusAsync());
		}

		[Fact]
		public async void Status_ServerStarted_ReturnsOk()
		{
			var result = await _defaultServer.StatusAsync();
			Assert.Equal(HttpStatusCode.OK, result);
		}

		[Fact]
		public async void Status_ServerStoped_ThrowsAnException()
		{
			var server = _fixture.GetServer();
			server.Start();
			server.Stop();
			await Assert.ThrowsAsync<HttpRequestException>(() => server.StatusAsync());
		}
	}
}