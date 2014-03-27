using System;
using System.Diagnostics;
using System.IO.Pipes;
using System.Linq;
using System.Threading;

namespace NamedPipeServer
{
    class Server
    {
        static void Main(string[] args)
        {
            const string pipeName = "testnp";

            if (args.Length == 0)
            {
                args = new []{"1024"};
            }

            int bufferSize = int.Parse(args[0]);

            var buffer = Enumerable.Range(0, bufferSize).Select(i => (byte)i).ToArray();

            //Console.WriteLine(String.Join(", ", Enumerable.Range(0, bufferSize)));

            Console.WriteLine("Listening..");
            //var server = new NamedPipeServerStream(pipeName, PipeDirection.InOut, 1, PipeTransmissionMode.Message);
            var server = new NamedPipeServerStream(pipeName);
            

            Process.Start("NamedPipeClient.exe", string.Format("{0} {1}", pipeName, bufferSize));
            
            server.WaitForConnection();

            while (!Console.KeyAvailable)
            {
                server.Write(buffer, 0, buffer.Length);
                //Thread.Sleep(TimeSpan.FromMilliseconds(5));
            }

            Console.WriteLine("Done");
        }
    }
}
