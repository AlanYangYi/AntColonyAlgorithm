using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntColonyAlgorithm
{
    class Program
    {
        static void Main(string[] args)
        {
           
            Building.HomeX = 0;
            Building.HomeY = 0;
            Building.FoodX = 50;
            Building.FoodY = 50;
            int size = 50; //地图的大小
            Building.block = new int[size+1, size+1];
            Pheromore.pheromorevalue = new double[size+1, size+1];  


            for (int j = 1; j < Pheromore.pheromorevalue.GetLength(0); j++) //行数
            {


                for (int k = 1; k < Pheromore.pheromorevalue.GetLength(1); k++)  //列数
                {
                    if (j == Building.FoodX && k == Building.FoodY)
                        Pheromore.pheromorevalue[j, k] = 1000;

                    else
                    {
                        if (j < Building.FoodX)
                            Pheromore.pheromorevalue[j, k] = 1000 / Math.Abs(j - Building.FoodX);
                        else if(j>Building.FoodX)
                            Pheromore.pheromorevalue[j, k] = 100 / Math.Abs(j - Building.FoodX);
                        //else Pheromore.pheromorevalue[j, k] = 100;
                        else Pheromore.pheromorevalue[j, k] = 1000 / Math.Abs(k - Building.FoodY);



                    }

                 
                }

            }
          

           
            int i = 0;
            int step = 10;
            int J = 0;
           
            
            int N = 30;   //the number of ant
            
            Building.GoodAnts = new List<Ant>();
            BestAnt.bestant = new Ant();
            BestAnt.bestant.Length = 10000000;
        for (i = 0; i < step;i++)
         {
                List<Ant> ants = new List<Ant>();
                int Foundfood = 0;
                Console.WriteLine("======================");
                Console.WriteLine("The step is {0}",i);
                double ave = 0;
                for ( J = 0; J < N; J++)
                {

                
                Ant ant = new Ant();
                ant.ID = J.ToString();
                ant.LoctionX = Building.HomeX;
                ant.LoctionY = Building.HomeY;
                ant.Length = 0;
                ant.Path = new int[size+1, size+1];
                ant.Path[ant.LoctionX, ant.LoctionY] = 1;
                ant.StopMove = false;
                ant.Days = 1;
                ants.Add(ant);
                

                 }


                

                while (true)
                {

                    foreach (var ant in ants)
                    {
                       if(ant.StopMove==false)

                            Move(ant);
                           
                         
                        
                    }

                    foreach (var ant01 in ants)
                    {

                        foreach (var ant02 in ants)
                        {

                            if(ant01.LoctionX != Building.FoodX && ant01.LoctionY != Building.FoodY)
                            Commucation(ant01, ant02);



                        }


                    }


                    foreach (var ant in ants)
                    {
                        
                        if (ant.StopMove==false && ant.LoctionX == Building.FoodX && ant.LoctionY == Building.FoodY)
                        {
                      
                            Foundfood++;
                            ant.StopMove = true;
                          
                           
                        }
                    }

                    if (Foundfood == N)
                    {


                       ave= CalAve(ants);
                        Console.WriteLine("The average is {0}",ave);
                        Ant goodant = new Ant();
                        goodant= UpdatePheromore(ants);
                        if (BestAnt.bestant.Length > goodant.Length)
                            BestAnt.bestant = goodant;
                        Building.GoodAnts.Add(goodant);

                        if (i % 20 == 0 && i!=0)

                            EvaporationPheromore();

                        break;
                    }



                    if (i==step-1)
                        {
                            for (int j = 0; j < Pheromore.pheromorevalue.GetLength(0); j++) //行数
                            {


                                for (int k = 0; k < Pheromore.pheromorevalue.GetLength(1); k++)  //列数
                                {

                                    Console.WriteLine("{0}  {1}   {2} ", j, k, Pheromore.pheromorevalue[j, k]);
                                }

                            }
                        break;

                        }

                      

                 }
               

               
            }

            Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");

            for (int pp = 1; pp <= BestAnt.bestant.Days; pp++)
            {

                for (int j = 0; j < BestAnt.bestant.Path.GetLength(0); j++) //行数
                {


                    for (int k = 0; k < BestAnt.bestant.Path.GetLength(1); k++)  //列数
                    {

                        if (pp == BestAnt.bestant.Path[j, k])

                            Console.WriteLine("{0}    {1}", j, k);

                    }

                }

            }
            Console.WriteLine();







            Ant Move(Ant ant)
            {
                
                Random random = new Random(Guid.NewGuid().GetHashCode());

               int x = ant.LoctionX;
               int y = ant.LoctionY;
                int ii = 0;
                double sum = 0;
                double sum1 = 0;

                Dictionary<int, double> value = new Dictionary<int, double>();
                if (ant.LoctionX + 1 > size || ant.LoctionX +1 < 0|| ant.LoctionY <0|| ant.LoctionY > size
                       || Building.block[ant.LoctionX + 1, ant.LoctionY] == 1 || ant.Path[ant.LoctionX + 1, ant.LoctionY]!=0)
                    value.Add(0, 0);
                else value.Add(0, Pheromore.pheromorevalue[ant.LoctionX + 1, ant.LoctionY]);

                if (ant.LoctionX + 1 > size|| ant.LoctionX +1 <0|| ant.LoctionY +1 <0 || ant.LoctionY + 1 > size
                    || Building.block[ant.LoctionX + 1, ant.LoctionY + 1] == 1 || ant.Path[ant.LoctionX + 1, ant.LoctionY + 1]!=0)
                    value.Add(1, 0);
                else value.Add(1, Pheromore.pheromorevalue[ant.LoctionX + 1, ant.LoctionY + 1]);

                if (ant.LoctionX > size|| ant.LoctionX <0 || ant.LoctionY+1 <0 || ant.LoctionY + 1 > size
                    || Building.block[ant.LoctionX, ant.LoctionY + 1] == 1 || ant.Path[ant.LoctionX, ant.LoctionY + 1]!=0)
                    value.Add(2, 0);
                else value.Add(2, Pheromore.pheromorevalue[ant.LoctionX, ant.LoctionY + 1]);

                if (ant.LoctionX - 1 > size || ant.LoctionX - 1 < 0 || ant.LoctionY + 1 > size || ant.LoctionY + 1 < 0
                    || Building.block[ant.LoctionX - 1, ant.LoctionY + 1] == 1 || ant.Path[ant.LoctionX - 1, ant.LoctionY + 1] !=0)
                    value.Add(3, 0);
                else value.Add(3, Pheromore.pheromorevalue[ant.LoctionX - 1, ant.LoctionY + 1]);

                if (ant.LoctionX - 1 < 0 || ant.LoctionX - 1 > size || ant.LoctionY < 0 || ant.LoctionY > size
                    || Building.block[ant.LoctionX - 1, ant.LoctionY] == 1 || ant.Path[ant.LoctionX - 1, ant.LoctionY]!=0)
                    value.Add(4, 0);
                else value.Add(4, Pheromore.pheromorevalue[ant.LoctionX - 1, ant.LoctionY]);

                if (ant.LoctionX - 1 < 0 || ant.LoctionX - 1 > size || ant.LoctionY - 1 < 0 || ant.LoctionY - 1 > size
                    || Building.block[ant.LoctionX - 1, ant.LoctionY - 1] == 1 || ant.Path[ant.LoctionX - 1, ant.LoctionY - 1] !=0)
                    value.Add(5, 0);
                else value.Add(5, Pheromore.pheromorevalue[ant.LoctionX - 1, ant.LoctionY - 1]);

                if (ant.LoctionX < 0 || ant.LoctionX > size || ant.LoctionY - 1 > size || ant.LoctionY - 1 < 0
                    || Building.block[ant.LoctionX, ant.LoctionY - 1] == 1 || ant.Path[ant.LoctionX, ant.LoctionY - 1] !=0)
                    value.Add(6, 0);
                else value.Add(6, Pheromore.pheromorevalue[ant.LoctionX, ant.LoctionY - 1]);

                if (ant.LoctionX + 1 > size || ant.LoctionX + 1 < 0 || ant.LoctionY - 1 < 0 || ant.LoctionY - 1 > size
                    || Building.block[ant.LoctionX + 1, ant.LoctionY - 1] == 1  || ant.Path[ant.LoctionX + 1, ant.LoctionY - 1] !=0)
                    value.Add(7, 0);
                else value.Add(7, Pheromore.pheromorevalue[ant.LoctionX + 1, ant.LoctionY - 1]);


                foreach (var item in value)
                {
                    sum = sum + item.Value;

                }

                for (ii = 0; ii < value.Count; ii++)
                {

                    value[ii] = value[ii] / sum;

                }

                Dictionary<int, double> value1 = new Dictionary<int, double>();
                value1 = value.OrderByDescending(k => k.Value).ToDictionary(k => k.Key, p => p.Value);


                int[] keys = value1.Keys.ToArray();
                foreach (var item in keys)
                {
                    if (value1[item] == 0)
                        value1.Remove(item);
                    else
                    {
                        sum1 = sum1 + value1[item];
                        value1[item] = sum1;
                    }

                }

             
              
                double j = random.NextDouble();
                foreach (var item in value1)
                {

                    if (j <= item.Value)
                    {

                        if (item.Key == 0)
                        { ant.LoctionX = ant.LoctionX + 1; ant.LoctionY = ant.LoctionY; }
                        if (item.Key == 1)
                        { ant.LoctionX = ant.LoctionX + 1; ant.LoctionY = ant.LoctionY + 1; }
                        if (item.Key == 2)
                        { ant.LoctionX = ant.LoctionX; ant.LoctionY = ant.LoctionY + 1; }
                        if (item.Key == 3)
                        { ant.LoctionX = ant.LoctionX - 1; ant.LoctionY = ant.LoctionY + 1; }
                        if (item.Key == 4)
                        { ant.LoctionX = ant.LoctionX - 1; ant.LoctionY = ant.LoctionY; }
                        if (item.Key == 5)
                        { ant.LoctionX = ant.LoctionX - 1; ant.LoctionY = ant.LoctionY - 1; }
                        if (item.Key == 6)
                        { ant.LoctionX = ant.LoctionX; ant.LoctionY = ant.LoctionY - 1; }
                        if (item.Key == 7)
                        { ant.LoctionX = ant.LoctionX + 1; ant.LoctionY = ant.LoctionY - 1; }

                        break;
                    }


                }
                if (ant.Path[ant.LoctionX,ant.LoctionY] != 0)
                {

                    while (true)
                    {
                        int kk1 = random.Next(1, size );
                        int kk2 = random.Next(1, size );
                        if (ant.Path[kk1, kk2] != 0)
                        {
                            ant.LoctionX = kk1;
                            ant.LoctionY = kk2;


                            for (int jjj = 0; jjj < ant.Path.GetLength(0); jjj++) //行数
                            {


                                for (int kkk = 0; kkk < ant.Path.GetLength(1); kkk++)
                                {
                                    if (ant.Path[jjj,kkk] > ant.Path[kk1, kk2])
                                        ant.Path[jjj,kkk] = 0;


                                }


                            }


                               


                            
                            ant.Days = ant.Path[kk1, kk2];
                            ant.Length = ant.Days;
                            break;
                        }
                    }
                    return ant;
               

                    
                }
                else
                {
                    ant.Days++;
                    ant.Path[ant.LoctionX, ant.LoctionY] = ant.Days;
                    ant.Length = ant.Days;
                    //ant.Length = ant.Length + Math.Sqrt((x - ant.LoctionX) * (x - ant.LoctionX) + (y - ant.LoctionY) * (y - ant.LoctionY));
                    return ant;

                }
            }

            void Commucation(Ant ant1, Ant ant2)
            {

                if (ant1.LoctionX == ant2.LoctionX && ant1.LoctionY == ant2.LoctionY)
                {

                    if (ant1.Length > ant2.Length)
                    {

                        Array.Copy(ant2.Path, ant1.Path,ant2.Path.Length);

                        ant1.Length = ant2.Length;


                    }


                }



            }

            double CalAve(List<Ant> ants)
            {
                double days = 0;
                double result = 0;
                foreach (var ant in ants)
                {

                    days = days + ant.Length;

                    

                }

                result = days / ants.Count;

                return result;


            }





            Ant UpdatePheromore(List<Ant> ants2)
            {

               
                Ant goodant = new Ant();
                double l = ants2[0].Length;

                foreach (var item in ants2)
                {
                    if (item.Length <= l)
                    {
                        goodant = item;
                        l = item.Length;
                    }


                }
                
                if (Building.GoodAnts.Count==5)
                {
                    double l2 = Building.GoodAnts[0].Length;
                    Ant goodant2 = new Ant();
                    goodant2 = Building.GoodAnts[0];
                    foreach (var item in Building.GoodAnts)
                    {

                        if (item.Length <= l2)
                        {
                            goodant2 = item;
                            l2 = item.Length;
                        }


                    }

                    for (int j = 1; j < Pheromore.pheromorevalue.GetLength(0); j++) //行数
                    {

                        for (int k = 1; k < Pheromore.pheromorevalue.GetLength(1); k++)  //列数
                        {

                            if (goodant2.Path[j, k] != 0 && j != Building.HomeX && k != Building.HomeY)
                                Pheromore.pheromorevalue[j, k] = Pheromore.pheromorevalue[j, k] + 8000;
                        }

                    }
                    Building.GoodAnts.Clear();
                    Console.WriteLine("The excellent ant is {0}, the length is {1}, update Pheromore...........",goodant2.ID,goodant2.Length);

                }


                Console.WriteLine("Ant ID {0}  the length of ant : {1} ", goodant.ID, goodant.Length);

                return goodant;


                //for (int pp = 1; pp <= goodant.Days; pp++)
                //{

                //    for (int j = 0; j < goodant.Path.GetLength(0); j++) //行数
                //    {


                //        for (int k = 0; k < goodant.Path.GetLength(1); k++)  //列数
                //        {

                //            if (pp == goodant.Path[j, k])

                //                Console.WriteLine("{0}    {1}", j, k);

                //        }

                //    }

                //}
            }

            void EvaporationPheromore()
            {

                for (int j = 1; j < Pheromore.pheromorevalue.GetLength(0); j++) //行数
                {


                    for (int k = 1; k < Pheromore.pheromorevalue.GetLength(1); k++)  //列数
                    {
                        if (j == Building.FoodX && k == Building.FoodY)
                            Pheromore.pheromorevalue[j, k] = 1000;

                        else
                        {
                            if (j < Building.FoodX)
                                Pheromore.pheromorevalue[j, k] = Pheromore.pheromorevalue[j, k]/1.2+ 1000 / Math.Abs(j - Building.FoodX);
                            else if (j > Building.FoodX)
                                Pheromore.pheromorevalue[j, k] = Pheromore.pheromorevalue[j, k]/1.2+ 100 / Math.Abs(j - Building.FoodX);
                       
                            else Pheromore.pheromorevalue[j, k] = Pheromore.pheromorevalue[j, k]/1.2+ 1000 / Math.Abs(k - Building.FoodY);



                        }


                    }

                }

                Console.WriteLine("Evaporating Pheromore............");

            }

        }
           

        }



       
    
    public class Ant
    {
        public string ID { get; set; }
        public int LoctionX { get; set; }
        public int LoctionY { get; set; }

        public double Length { get; set; }

        public int[,] Path;

        public bool StopMove { get; set; }
        public int Days { get; set; }


    }
    public static class Pheromore
    {

        public static double[,] pheromorevalue;




    }

    public static class Building
    {

        public static int[,] block { get; set; }  //0代表没有block ，1 代表有 block
        public static int HomeX { get ; set; }
        public static  int HomeY { get; set; }

        public static int FoodX { get; set; }
        public static int FoodY { get; set; }


        public static List<Ant> GoodAnts { get; set; }

    }

    public static class BestAnt
    {


        public static Ant bestant { get; set; }


    }

}
