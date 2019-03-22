using System;
using System.Linq;

namespace Prakrishta.Cache.Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            ICache cache = new SimpleMemoryCache();
            cache.AddOrUpdate("Test", "Hello Arul!!!", "Console");

            Console.WriteLine("Hello World!");

            Console.WriteLine(cache.Get("Test", "Console"));
            Console.WriteLine(cache["Test"]?.FirstOrDefault());
            Console.ReadLine();
        }
    }
}
