using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.IsolatedStorage;
using System.IO;

namespace vagunda_hra_zivota_classes
{
    class Grid
    {
        private char[,] TXTArray;
        private int heigh;
        private int width;
        private double treshold=0.5; //definováním tohoto nastavíš jaká je pravděpodobnost, že buňky budou alokované živé/mrtvé
        private Random random;

        private Cell[,] currArray;

        public Grid(int heigh, int width)
        {
            random = new Random();
            this.heigh = heigh + 2;  //zadáno 20, uloženo 22
            this.width = width + 2; //zadáno 20, uloženo 22

            currArray = new Cell[this.heigh, this.width];
            
            //alokování každé buňky tabuly zvlášť
            for (int i = 1; i < (this.heigh-1); i++) //array.GetLength(0)=rows
            {
                for (int j = 1; j < (this.width-1); j++)            //array.GetLength(1)= colums
                {
                    currArray[i, j] = new Cell(false);
                    //currArray[i, j]=new Cell(random.NextDouble() >treshold);
                }
            }
            currArray[1, 3].TmpInitIsAlive(true);
            currArray[2, 1].TmpInitIsAlive(true);
            currArray[3, 1].TmpInitIsAlive(true);
            currArray[4, 2].TmpInitIsAlive(true);
            currArray[2, 4].TmpInitIsAlive(true);
            currArray[3, 4].TmpInitIsAlive(true);        
        }
        
        public Grid()
        {

            //FillTXTArray();
            FillTXTArrayFromIsolatedStorage();

            this.heigh = TXTArray.GetLength(0) ;  
            this.width = TXTArray.GetLength(1); 

            currArray = new Cell[this.heigh, this.width];

            //alokování každé buňky tabuly zvlášť
            for (int i = 1; i < (this.heigh - 1); i++) //array.GetLength(0)=rows
            {
                for (int j = 1; j < (this.width - 1); j++)            //array.GetLength(1)= colums
                {
                    char c = TXTArray[i, j];
                    if (c == '1')
                        currArray[i, j] = new Cell(true);
                    else
                        currArray[i, j] = new Cell(false);
                    //currArray[i, j]=new Cell(random.NextDouble() >treshold);
                }
            }
            
        }

        public void DrawGen()
        {
            for (int i = 0; i < currArray.GetLength(0); i++) //array.GetLength(0)=rows
            {
                for (int j = 0; j < currArray.GetLength(1); j++)            //array.GetLength(1)= colums
                {
                    if(currArray[i, j] != null)
                    {
                        if (currArray[i, j].IsAlive == true)                        
                        {

                            Console.Write("1" + " ");
                            //vratí pocet sousedů
                            //Console.Write(currArray[i, j] + " ");
                        }
                        else
                        {
                            Console.Write("0" + " ");
                            //vrati pocet sousedu
                            //Console.Write(currArray[i, j] + " ");    
                        }
                    }
                    else
                        Console.Write("*" + " ");

                }
                Console.WriteLine();
            }
        }  
        
        //projde postupně celé hlavní pole a uloží pomocí funkce AddNeighbours do jednotlivých Cells pole s jejich sousedy
        public void PutNeigboursInCell()
        {           
            
            for (int i = 1; i < (heigh - 1); i++) 
            {
                for (int j = 1; j < (width - 1); j++)
                {
                    currArray[i,j].AddNeigbours(ArrOfNeighbours(i, j));                                     
                }
            }
            
        }
        //rozhodování problému, přežije/zemře nastavování property: WillDie, WillResurge
        public void PrepareAnotherGen()
        {
            //přidá do buněk jejich sousedy z aktuální generace
            PutNeigboursInCell();
            for (int x = 1; x < heigh-1; x++) //array.GetLength(0)=rows
            {
                for (int y = 1; y < width-1; y++)            //array.GetLength(1)= colums
                {
                    //zavoláme refresh, prenastaví se se WillDie a WillResurge
                    currArray[x, y].Refresh(currArray[x, y].IsAlive);  
                }
            }
            //z původního Refresh()
            for (int x = 1; x < heigh - 1; x++) //array.GetLength(0)=rows
            {
                for (int y = 1; y < width - 1; y++)            //array.GetLength(1)= colums
                {
                    if (currArray[x, y].WillDie == true && currArray[x, y].WillResurge == false)
                        currArray[x, y].Kill();
                    else
                        currArray[x, y].Resurge();
                }
            }

        }

        
        public Cell[] ArrOfNeighbours(int x, int y)
        {
            
            int counter = 0;
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    if (currArray[x + i, y + j] != null)
                    {
                        counter++;
                    }
                }
            }            
            //musím odečíst, středový bod kolem kterého se počítali sousedi, protože ten je započítán také
            counter--;
            Cell[] ArrayOfNeighbors = new Cell[counter];
            int counterTMP = 0;
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    int row = x + i;
                    int coll = y + j;
                    if (currArray[row, coll] != null)//|| (i!= 0) || (j!= 0)
                    {
                        if (row != x || coll!=y)
                        {
                                ArrayOfNeighbors[counterTMP] = currArray[x + i, y + j];
                                counterTMP++;                           
                        }
                    }                      
                }
            }
            return ArrayOfNeighbors;                     
        }
        public void WriteToIsolateStorage()
        {
            //https://docs.microsoft.com/cs-cz/dotnet/standard/io/how-to-read-and-write-to-files-in-isolated-storage
            IsolatedStorageFile userStore = IsolatedStorageFile.GetUserStoreForAssembly();
            IsolatedStorageFileStream userStream = new IsolatedStorageFileStream("UserSettings.set", FileMode.Create, userStore);
            StreamWriter userWriter = new StreamWriter(userStream);

            for (int i = 0; i < currArray.GetLength(0); i++) //array.GetLength(0)=rows
            {
                for (int j = 0; j < currArray.GetLength(1); j++)            //array.GetLength(1)= colums
                {
                    if (currArray[i, j] != null)
                    {
                        if (currArray[i, j].IsAlive == true)
                        {

                            userWriter.Write("1");
                        }
                        else
                        {
                            userWriter.Write("0");
                        }
                    }
                    else
                        userWriter.Write("*");

                }
                userWriter.WriteLine();
            }




            userWriter.Close();
        }

        public void FillTXTArrayFromIsolatedStorage()
        {

            int wide = 0;
            int rows = 0;

            IsolatedStorageFile userStore = IsolatedStorageFile.GetUserStoreForAssembly();
            IsolatedStorageFileStream userStream = new IsolatedStorageFileStream("UserSettings.set", FileMode.Open, userStore);
            StreamReader userReader = new StreamReader(userStream);

            char ch = ' ';
            string text = userReader.ReadToEnd();
            string[] lines = text.Split('\r');


            rows = lines.Length - 1;//-1 protože je jeden řádek navíc s '\n'
            wide = lines[0].Length;

            //první řádek musíme překopírovat zvlášt, protože neobsahuje znak '\n
            this.TXTArray = new char[rows, wide];
            for (int col = 0; col < wide; col++)
            {
                this.TXTArray[0, col] = lines[0][col];
            }
            //¨z ostatních rádků musíme vyločit znak '\n
            for (int row = 1; row < lines.Length - 1; row++)
            {
                for (int col = 0; col < wide; col++)
                {
                    TXTArray[row, col] = lines[row][col + 1];
                }
            }
            userReader.Close();           

        }

        public void WriteToTXT()
        {
            FileStream theFile = File.Create(@"c:\Users\vojta\source\repos\printGameOfLife.txt");
            StreamWriter writer = new StreamWriter(theFile);                
            
            for (int i = 0; i < currArray.GetLength(0); i++) //array.GetLength(0)=rows
            {
                for (int j = 0; j < currArray.GetLength(1); j++)            //array.GetLength(1)= colums
                {
                    if (currArray[i, j] != null)
                    {
                        if (currArray[i, j].IsAlive == true)
                        {

                            writer.Write("1");                           
                        }
                        else
                        {
                            writer.Write("0");                              
                        }
                    }
                    else
                        writer.Write("*");

                }
                writer.WriteLine();
            }

            writer.Close();
            theFile.Close();
        }
        public void FillTXTArray()
        {
            
            int wide = 0;
            int rows = 0;
            FileStream theFileW = File.Open(@"c:\Users\vojta\source\repos\printGameOfLife.txt", FileMode.Open, FileAccess.Read);
            StreamReader reader = new StreamReader(theFileW);
            char ch = ' ';
            string text = reader.ReadToEnd();
            string[] lines = text.Split('\r');


            rows = lines.Length-1;//-1 protože je jeden řádek navíc s '\n'
            wide = lines[0].Length;

            //první řádek musíme překopírovat zvlášt, protože neobsahuje znak '\n
            this.TXTArray = new char[rows, wide];
            for (int col = 0; col < wide; col++)
            {
                this.TXTArray[0, col] = lines[0][col];
            }
            //¨z ostatních rádků musíme vyločit znak '\n
            for (int row = 1; row < lines.Length-1; row++)
            {
                for (int col = 0; col < wide; col++)
                {
                    TXTArray[row, col] = lines[row][col + 1];
                }
            }

            reader.Close();
            theFileW.Close();

        }
    }
}
