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
            // List of the categories. Now they are loaded in memory, might be better with a static class.
            List<Category> categories = new List<Category>();
            categories.Add(new Category(1, "Beverages"));
            categories.Add(new Category(2, "Condiments"));
            categories.Add(new Category(3, "Confections"));

            // Server setup
            int port = 5000;
            IPAddress localAddr = IPAddress.Parse("127.0.0.1");

            var server = new TcpListener(localAddr, port);

            server.Start();

            Console.WriteLine("Started");

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

            var strm = client.GetStream();

            byte[] buffer = new byte[client.ReceiveBufferSize];
            int bytesRead = strm.Read(buffer, 0, buffer.Length);

            var request = Encoding.UTF8.GetString(buffer);

            
            Console.WriteLine(request.Trim('\0'));

            var response = Encoding.UTF8.GetBytes(request.ToUpper());

            strm.Write(response, 0, bytesRead);

            strm.Close();

            client.Dispose();
        }
    }
}
