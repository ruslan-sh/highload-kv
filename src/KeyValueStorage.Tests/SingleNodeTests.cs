using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace RuslanSh.KeyValueStorage.Tests
{
	[Collection("Http Server Tests")]
	public class SingleNodeTests : IDisposable
	{
		private readonly KvServerFixture _defaultServer;
		private const string _requestPath = "v0/entity";

		public SingleNodeTests(TestFixture fixture)
		{
			_defaultServer = fixture.GetServer();
			_defaultServer.Start();
		}

		[Fact]
		public async void EmptyKey_ReturnsBadReques()
		{
			await Assert.ThrowsAsync<HttpRequestException>(async () => await GetAsync(""));
			Assert.Equal(HttpStatusCode.BadRequest, (await DeleteAsync("")).StatusCode);
			Assert.Equal(HttpStatusCode.BadRequest, (await UpsertAsync("", new byte[] {0})).StatusCode);
		}

//		[Fact]
//		public void UnexistedRequest_ReturnsNotFound()
//		{
//		    
//		}

		private async Task<HttpResponseMessage> UpsertAsync(string key, byte[] value)
		{
			return await _defaultServer.PostAsync(_requestPath, $"id={key}", value);
		}

		private async Task<HttpResponseMessage> DeleteAsync(string key)
		{
			return await _defaultServer.DeleteAsync(_requestPath, $"id={key}");
		}

		private async Task<byte[]> GetAsync(string key)
		{
			return await _defaultServer.GetAsync(_requestPath, $"id={key}");
		}

		public void Dispose()
		{
			_defaultServer.Stop();
		}
	}
}