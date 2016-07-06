using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    [Serializable]
    public class NumericResults : Pattern
    {
        public int width_count { get; set; }
        public int height_count { get; set; }
        public int threads_count { get; set; }
        public double[,] characteristics { get; set; }

        public NumericResults(int width_count_p = 50, int height_count_p = 50, int threads_count_p = 10) : base(new DateTime(2015, 3, 1))
        {
            width_count = width_count_p;
            height_count = height_count_p;
            threads_count = threads_count_p;
            characteristics = new double[height_count,width_count];
        }

        public int calculate_characteristics(int number)
        {
            int start = Environment.TickCount, duration;
            
            Random rand = new Random();
            int parameter1 = rand.Next(300, 500), parameter2 = rand.Next(500, 1000);

            for (int i = 0; i < parameter1; i++)
            {
                for (int j = 0; j < parameter2; j++)
                {
                    characteristics[number / width_count, number % width_count] += Math.Cos((Math.Atan(number / width_count + i))) * Math.Exp(-i) + Math.Sin(j + number / height_count);
                }
            }
            characteristics[number / width_count, number % width_count] *= model_parameter_1 / model_parameter_2;
            duration = Environment.TickCount - start;
            return duration;
        }

        public double minimum_value
        {
            get
            {
                double min = characteristics[0, 0];
                for (int i = 0; i < height_count; i++)
                {
                    for (int j = 0; j < width_count; j++)
                    {
                        if (characteristics[i, j] < min)
                            min = characteristics[i, j];
                    }
                }
                return min;
            }
        }

        public double maximum_value
        {
            get
            {
                double max = characteristics[0, 0];
                for (int i = 0; i < height_count; i++)
                {
                    for (int j = 0; j < width_count; j++)
                    {
                        if (characteristics[i, j] > max)
                            max = characteristics[i, j];
                    }
                }
                return max;
            }
        }

        public double average_value
        {
            get
            {
                double sum = 0;
                for (int i = 0; i < height_count; i++)
                {
                    for (int j = 0; j < width_count; j++)
                    {
                        sum += characteristics[i, j];
                    }
                }
                return sum / (width_count * height_count);
            }
        }
    }
}
