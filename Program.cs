using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vagunda_hra_zivota_classes
{
    class Program
    {
        static void Main(string[] args)
        {
            int height = 20;
            int widht = 20;
            int gen = 1;

            Console.WriteLine("Set large of field:");
            Console.WriteLine("If you want to load saved game press L=load if you want to create new game press whenewer KEY:");
            /*if (Console.ReadKey(true).Key == ConsoleKey.L)
            {
                int a = 7;
                Grid FieldOfLife = new Grid();
            }
            else
            {
                Console.Clear();
                Console.SetCursorPosition(0, 0);
                Console.Write("Create new Game of Life ");
                Console.Write("height: ");
                while (Int32.TryParse(Console.ReadLine(), out height) == false)
                {
                    Console.Write("height was bad, you must write INT number of heitht. Please set height: ");
                }
                Console.WriteLine("----------------------------------");
                Console.Write("widht: ");
                while (Int32.TryParse(Console.ReadLine(), out widht) == false)
                {
                    Console.Write("height was bad, you must write INT number of width. Please set width: ");
                }
                Grid FieldOfLife = new Grid(height, widht);
            */
            //----------------------------------------------
            Grid FieldOfLife = new Grid();
            //----------------------------------------------


            //Console.Clear();            
            //FieldOfLife.DrawGen();

            while (Console.ReadKey(true).Key == ConsoleKey.Enter)//
            {
                //Console.SetCursorPosition(0, 0);
                FieldOfLife.PrepareAnotherGen();
                FieldOfLife.DrawGen();
                Console.WriteLine("generation: " + gen);
                gen++;
                Console.WriteLine("--------------------------------");
                //ukol od Varachy
                //FieldOfLife.ToString();

                if (Console.ReadKey(true).Key == ConsoleKey.S)
                {
                    FieldOfLife.WriteToTXT();
                    //FieldOfLife.WriteToIsolateStorage();
                }
            }
        }
    }
}
