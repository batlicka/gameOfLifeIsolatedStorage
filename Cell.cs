using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vagunda_hra_zivota_classes
{
    class Cell
    {
        private bool isAlive;//backing field ?
        public bool IsAlive
        {
            get
            {
                return isAlive;
            }
            private set
            {
                isAlive = value;
            }
        }
        Nullable<bool> willDie = null;

        //Nullable<bool> willResurge = null;

        public bool WillDie
        {
            get
            {
                return willDie.HasValue ? willDie.Value: false;
            }
            set
            {
                willDie = value;
            }
        }
        //public bool WillDie { get; set; }
        public bool WillResurge { get; set; }

        private Cell[] ArrayOfNeighbors;

        public Cell(bool all)
        {
            IsAlive = all;
            WillDie = true;
            WillResurge = false;
        }        
        

        public void TmpInitIsAlive(bool live)
        {
            isAlive = live;
            WillDie = false;
            WillResurge = true;
        }
        //List<Cell> obj = new List<Cell>();//
        public void AddNeigbours(Cell[] arrNeigh)
        {
            ArrayOfNeighbors = arrNeigh; 
            for(int i=0; i<arrNeigh.Length; i++)
            {
                
                //Console.WriteLine("saved cell index:" + i +"," + "state of cell: "+ ArrayOfNeighbors[i].IsAlive + "|");
            }
            //Console.WriteLine("-------------------------");
        }

        public int SumOfLiveNeighbours()
        {
            int liveNeighbours = 0;
            for (int i = 0; i < ArrayOfNeighbors.Length; i++)
            {
               /* if (ArrayOfNeighbors[i].IsAlive == true)
                    liveNeighbours++;*/
                liveNeighbours= liveNeighbours+ this.ArrayOfNeighbors[i];
            }
            return liveNeighbours;
        }

        public void Kill()
        {
            IsAlive = false;
        }

        public void Resurge()
        {
            IsAlive = true;
        }

        public void Refresh(bool IsAlive_middleP)
        {
            bool state = IsAlive_middleP;
            int neighbors =SumOfLiveNeighbours();
            if (state == false && neighbors == 3)
            {
                WillResurge = true; //born to 
                WillDie = false; //born to live
            }
            else if (state == true && (neighbors < 2 || neighbors > 3))
            {
                WillResurge = false; //born to 
                WillDie = true; //born to live
            }
            else //cases sum==2 || sum==3 ...can live
            {
                if (state == true)
                {
                    WillResurge = true;
                    WillDie = false;
                }
                else
                {
                    WillResurge = false;
                    WillDie = true;
                }

            }
        }
        //When you create a custom class or struct, you should override the ToString method in order to provide information about your type to client code.
        public override string ToString()
        {
            return Convert.ToString(SumOfLiveNeighbours());
            //return "5";
        }

        public static int operator +(int arg1, Cell arg2)
        {
            if (arg2.IsAlive == true)
                return arg1 + 1;
            else
                return arg1;
        }
        public bool Retype()
        {
            return IsAlive;
        }

    }
}
