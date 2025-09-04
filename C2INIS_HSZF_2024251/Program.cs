//using C2INIS_HSZF_2024251.Persistence.MsSql.Models;

using C2INIS_HSZF_2024251.Application;
using C2INIS_HSZF_2024251.Console;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using System.Diagnostics.Metrics;

namespace C2INIS_HSZF_2024251
{
    internal class Program
    {
        static void Main(string[] args)
        {
            StartMessage();
            MenuHandler menuHandler = new MenuHandler();







            /*foreach (var item in dBReacher.AnimalMethods.ListEntities())
            {
                System.Console.WriteLine(item.Id);
            }*/
        }


        static void StartMessage()
        {
            System.Console.ForegroundColor = ConsoleColor.White;
            System.Console.WriteLine("You can navigate by arrows and select a button by enter! Press enter to start!");

            ConsoleKeyInfo consoleKey;
            do
            {
                consoleKey = System.Console.ReadKey(true);
            } while (consoleKey.Key != ConsoleKey.Enter);
        }

    }

}