using System;
using System.Net;
using System.Net.Http;
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
			_defaultServer = fixture.DefaultKvServer;
		}

		[Fact]
		public void EmptyKey_ReturnsBadReques()
		{
			Assert.Throws<HttpRequestException>(() => Get(""));
			Assert.Equal(HttpStatusCode.BadRequest, Delete("").StatusCode);
			Assert.Equal(HttpStatusCode.BadRequest, Upsert("", new byte[] {0}).StatusCode);
		}

//		[Fact]
//		public void UnexistedRequest_ReturnsNotFound()
//		{
//		    
//		}

		private HttpResponseMessage Upsert(string key, byte[] value)
		{
			return _defaultServer.Post(_requestPath, $"id={key}", value);
		}

		private HttpResponseMessage Delete(string key)
		{
			return _defaultServer.Delete(_requestPath, $"id={key}");
		}

		private byte[] Get(string key)
		{
			return _defaultServer.Get(_requestPath, $"id={key}");
		}

		public void Dispose()
		{ }
	}
}