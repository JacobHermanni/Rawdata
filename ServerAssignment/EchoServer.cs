using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace ServerAssignment
{
    class EchoServer
    {
        private List<Category> categories = null;
            TcpListener server = null;
        public EchoServer()
        {
            // List of the categories. Now they are loaded in memory, might be better with a static class.
            categories = new List<Category>
            {
                new Category(1, "Beverages"),
                new Category(2,"Condiments"),
                new Category(3,"Confections")
            };


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

            try
            {
                var strm = client.GetStream();

                byte[] buffer = new byte[client.ReceiveBufferSize];
                var bytesRead = strm.Read(buffer, 0, buffer.Length);

                var request = Encoding.UTF8.GetString(buffer);
                request = request.Trim('\0');
                Console.WriteLine(request);

                //var responseObject = new
                //{
                //    test = "test"
                //};

                //var response = Encoding.UTF8.GetBytes(responseObject.ToJson());

                //Console.WriteLine(response.ToJson());

                //strm.Write(response, 0, response.Length);

                /*var jsonObject = JsonConvert.DeserializeObject<Request>(request);
                Console.WriteLine(jsonObject.date);
                var json = jsonObject.ToJson();
                var jsonBytes = Encoding.UTF8.GetBytes(json);
                strm.Write(jsonBytes, 0, jsonBytes.Length);*/

                var categories = new List<object>
                {
                    new {cid = 1, name = "Beverages"},
                    new {cid = 2, name = "Condiments"},
                    new {cid = 3, name = "Confections"}
                };

                var response = new
                {
                    Status = "1 Ok",
                    Body = categories.ToJson()
                };

                var responseJson = response.ToJson();

                var responseBytes = Encoding.UTF8.GetBytes(responseJson);
                strm.Write(responseBytes, 0, responseBytes.Length);

                //Console.WriteLine(jsonObject.method);

                //var response = categories.ToJson();

                //var msg = Encoding.UTF8.GetBytes(response);
                //strm.Write(msg, 0, msg.Length);

                //var response = Encoding.UTF8.GetBytes(request.ToUpper());

                //strm.Write(response, 0, bytesRead);

                strm.Close();

                client.Dispose();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            
        }
    }

    public class Request
    {
        public string method { get; set; }
        //public string Body;
        public string path { get; set; }

        public string date { get; set; }
    }

    public static class Util
    {
        public static string ToJson(this object data)
        {
            return JsonConvert.SerializeObject(data,
                new JsonSerializerSettings {ContractResolver = new CamelCasePropertyNamesContractResolver()});
        }
    }
}
