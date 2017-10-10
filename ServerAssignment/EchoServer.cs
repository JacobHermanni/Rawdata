using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace ServerAssignment
{
    class EchoServer
    {
        TcpListener server = null;
        public EchoServer()
        {
            // Set the TcpListener on port 13000.
            Int32 port = 5000;
            IPAddress localAddr = IPAddress.Parse("127.0.0.1");

            // TcpListener server = new TcpListener(port);
            server = new TcpListener(localAddr, port);

            // Start listening for client requests.
            server.Start();

            while (true)
            {
                var client = server.AcceptTcpClient();

                Console.WriteLine("Client connected");

                var thread = new Thread(HandleClient);
                thread.Start(client);

            }
        }

        void HandleClient(object clientObj)
        {
            var client = clientObj as TcpClient;
            if (client == null) return;

            var stream = client.GetStream();

            byte[] buffer = new byte[client.ReceiveBufferSize];
            var bytesRead = stream.Read(buffer, 0, buffer.Length);

            string request = Encoding.UTF8.GetString(buffer);

            Console.WriteLine(request);

            var response = Encoding.UTF8.GetBytes(request);

            stream.Write(buffer, 0, bytesRead);


        }
    }
}
