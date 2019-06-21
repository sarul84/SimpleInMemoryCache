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
            cache.AddOrUpdate("NullSession", "Null Session Orginal data", string.Empty);
            
            Console.WriteLine("Hello World!");

            Console.WriteLine(cache.Get("Test", "Console"));
            Console.WriteLine(cache["Test"]?.FirstOrDefault());
            Console.WriteLine(cache.Get("NullSession", string.Empty));

            cache.AddOrUpdate("NullSession", "Null Session Data Updated", string.Empty);

            Console.WriteLine(cache.Get("NullSession", string.Empty));

            Console.ReadLine();
        }
    }
}
