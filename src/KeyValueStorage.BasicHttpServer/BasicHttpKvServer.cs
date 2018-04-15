using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RuslanSh.KeyValueStorage.BasicHttpServer
{
	public class BasicHttpKvServer : IKvServer
	{
		private readonly CancellationTokenSource _cancellationTokenSource;
		private readonly HttpListener _listener;

		public BasicHttpKvServer(string host, int port, string dataPath)
		{
			_listener = new HttpListener();
			var prefix = BuildPrefix(host, port);
			_listener.Prefixes.Add(prefix);
			_cancellationTokenSource = new CancellationTokenSource();
		}

		public void Start()
		{
			var cancellationToken = _cancellationTokenSource.Token;
			_listener.Start();
			Task.Factory.StartNew(async () =>
			{
				while (true)
				{
					var context = await _listener.GetContextAsync();
					await ProcessRequests(context);
					if (cancellationToken.IsCancellationRequested)
						break;
				}
			}, cancellationToken);
		}

		public void Stop()
		{
			_cancellationTokenSource.Cancel();
			_listener.Stop();
		}

		private string BuildPrefix(string host, int port)
		{
			return $"http://{host}:{port}/";
		}

		private async Task ProcessRequests(HttpListenerContext context)
		{
			var requestPath = context.Request.Url.AbsolutePath;

			switch (requestPath)
			{
				case "/status":
					await SendResponseText(context, "ONLINE");
					context.Response.StatusCode = (int) HttpStatusCode.OK;
					break;
				default:
					await SendResponseText(context, "404 Not Found");
					context.Response.StatusCode = (int) HttpStatusCode.NotFound;
					break;
			}

			context.Response.OutputStream.Close();
		}

		private static async Task SendResponseText(HttpListenerContext context, string text)
		{
			var statusResponse = Encoding.UTF8.GetBytes(text);
			context.Response.ContentLength64 = statusResponse.LongLength;
			context.Response.ContentType = "text";
			context.Response.ContentEncoding = Encoding.UTF8;
			await context.Response.OutputStream.WriteAsync(statusResponse, 0, statusResponse.Length);
		}
	}
}