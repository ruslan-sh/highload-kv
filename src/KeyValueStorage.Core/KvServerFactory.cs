using RuslanSh.KeyValueStorage.BasicHttpServer;

namespace RuslanSh.KeyValueStorage.Core
{
	public static class KvServerFactory
	{
		public static IKvServer Create(string host, int port, string path)
		{
			return new BasicHttpKvServer(host, port, path);
		}
	}
}