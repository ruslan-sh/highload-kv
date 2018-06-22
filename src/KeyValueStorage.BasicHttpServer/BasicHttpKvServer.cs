using System;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RuslanSh.KeyValueStorage.BasicHttpServer
{
	public class BasicHttpKvServer : IKvServer
	{
		private readonly HttpListener _listener;
		private readonly CancellationTokenSource _cancellationTokenSource;

		public BasicHttpKvServer(string host, int port, string dataPath)
		{
			_cancellationTokenSource = new CancellationTokenSource();
			_listener = new HttpListener();
			var prefix = BuildPrefix(host, port);
			_listener.Prefixes.Add(prefix);
		}

		public void Start()
		{
			if (_listener.IsListening)
				return;
			_listener.Start();

			Task.Run(
				() =>
				{
					while (true)
					{
						var result = _listener.BeginGetContext(ProcessRequests, _listener);
						result.AsyncWaitHandle.WaitOne();
						if (!_listener.IsListening)
							return;
					}
				},
				_cancellationTokenSource.Token);
		}

		private void ProcessRequests(IAsyncResult ar)
		{
			if (!_listener.IsListening) return;
			var context = _listener.EndGetContext(ar);
			var requestPath = context.Request.Url.AbsolutePath;
			switch (requestPath)
			{
				case "/status":
					SendResponseText(context, HttpStatusCode.OK, "ONLINE");
					break;
				case "/v0/entity":
					SendResponseText(context, HttpStatusCode.BadRequest, "400 Bad Request");
					break;
				default:
					SendResponseText(context, HttpStatusCode.NotFound, "404 Not Found");
					break;
			}
		}

		public void Stop()
		{
			if (!_listener.IsListening)
				return;
			_listener.Stop();
			_cancellationTokenSource.Cancel();
		}

		private string BuildPrefix(string host, int port)
		{
			return $"http://{host}:{port}/";
		}

		private static void SendResponseText(HttpListenerContext context, HttpStatusCode statusCode, string text)
		{
			context.Response.StatusCode = (int) statusCode;
			context.Response.ContentType = "text";
			context.Response.ContentEncoding = Encoding.UTF8;
			var statusResponse = Encoding.UTF8.GetBytes(text);
			context.Response.ContentLength64 = statusResponse.LongLength;
			context.Response.OutputStream.Write(statusResponse, 0, statusResponse.Length);
			context.Response.OutputStream.Close();
		}
	}
}