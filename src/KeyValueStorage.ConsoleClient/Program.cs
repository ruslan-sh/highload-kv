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
			var kvServer = KvServerFactory.Create("localhost", 8080, AppDomain.CurrentDomain.BaseDirectory + "/data");
			kvServer.Start();
			Console.WriteLine("Server started.");
			while (true)
			{
			}

			// ReSharper disable once FunctionNeverReturns
		}
	}
}