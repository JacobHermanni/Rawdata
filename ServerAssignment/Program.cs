using System;

namespace ServerAssignment
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting server");

            var echo = new EchoServer();
        }
    }
}
