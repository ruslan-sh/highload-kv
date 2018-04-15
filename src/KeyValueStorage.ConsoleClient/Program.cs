using System;
using RuslanSh.KeyValueStorage.Core;

namespace RuslanSh.KeyValueStorage.ConsoleClient
{
	// ReSharper disable once ClassNeverInstantiated.Global
	// Console Application class
	internal class Program
	{
		private static void Main()
		{
			Console.WriteLine("Starting server... ");
			var host = "localhost";
			var port = 8080;
			var dataDirectory = AppDomain.CurrentDomain.BaseDirectory + "data\\";
			var kvServer = KvServerFactory.Create(host, port, dataDirectory);
			kvServer.Start();
			Console.WriteLine("Server started.");
			Console.WriteLine($"Server data directory: {dataDirectory}");
			Console.WriteLine($"Check \"http://{host}:{port}/status\" for server status.");
			while (true)
			{
			}

			// ReSharper disable once FunctionNeverReturns
		}
	}
}