using System;
using System.Diagnostics;
using System.IO.Pipes;
using System.Threading;

namespace NamedPipeClient
{
    class Client
    {
        static void Main(string[] args)
        {
            string pipeName = args[0];
            int bufferSize = int.Parse(args[1]);

            Console.WriteLine("Connecting to pipe {0} [bufferSize: {1}]", pipeName, bufferSize);

            Thread.Sleep(TimeSpan.FromSeconds(1));

            var client = new NamedPipeClientStream(pipeName);
            
            while (!client.IsConnected)
            {
                Console.WriteLine("Attempting to connect to {0}", pipeName);

                client.Connect((int) TimeSpan.FromSeconds(1).TotalMilliseconds);
            }

            Console.WriteLine("Connected");
            
            var buffer = new byte[bufferSize];

            var startTime = DateTime.Now;
            long totalReads = 0;
            while (client.IsConnected)
            {
                client.Read(buffer, 0, buffer.Length);
                totalReads++;

                if (totalReads%1000 == 0)
                {
                    double totalBytes = totalReads*bufferSize;
                    double totalMBps = (totalBytes / (1024*1024) ) / (DateTime.Now - startTime).TotalSeconds ;
                    Console.WriteLine("Rate = {0:F1} mb/s", totalMBps);
                }
            }

            Console.WriteLine("Disconnected.");
        }
    }
}
