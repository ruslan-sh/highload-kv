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
		}

		public void Dispose()
		{
			// _fixture.Server.Stop(); Cause tests freezing
		}

		private readonly TestFixture _fixture;

		[Fact]
		public async void Status_ServerNotStart_ThrowsAnException()
		{
			await Assert.ThrowsAsync<HttpRequestException>(() => _fixture.CheckServerStatus());
		}

		[Fact]
		public async void Status_ServerStarted_ReturnsOk()
		{
			_fixture.Server.Start();
			var result = await _fixture.CheckServerStatus();
			Assert.Equal(HttpStatusCode.OK, result);
		}

		[Fact]
		public async void Status_ServerStoped_ThrowsAnException()
		{
			_fixture.Server.Start();
			_fixture.Server.Stop();
			await Assert.ThrowsAsync<HttpRequestException>(() => _fixture.CheckServerStatus());
		}
	}
}