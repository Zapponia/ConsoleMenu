using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleMenu
{
    class Program
    {
        static void Main(string[] args)
        {
            bool isRunning = true;
            DnDCharacterCreator characterCreator = new DnDCharacterCreator();

            while (isRunning)
            {
                Console.WriteLine("What do you wish to do? \n" +
                    "1. DnDCharacterCreator \n" +
                    "2. Exit");
                string input = Console.ReadLine();

                switch(input)
                {
                    case "1":
                        characterCreator.Menu();
                        break;
                    case "2":
                        isRunning = false;
                        break;
                    default:
                        Console.WriteLine("I'm sorry I didn't understand that");
                        break;
                }
                
            }
        }
    }
}
