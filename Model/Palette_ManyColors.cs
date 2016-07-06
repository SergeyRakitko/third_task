using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace Model
{
    public class Palette_ManyColors : CustomPalette
    {
        private double lower_boundary;
        private double upper_boundary;

        public Palette_ManyColors(double parameter1, double parameter2)
        {
            lower_boundary = parameter1;
            upper_boundary = parameter2;

            color_collection = new List<Color>();
            color_collection.Add(Colors.Aqua);
            color_collection.Add(Colors.Azure);
            color_collection.Add(Colors.Blue);
            color_collection.Add(Colors.Brown);
            color_collection.Add(Colors.Black);
        }

        public override int choose_color(double value)
        {
            double h = (upper_boundary - lower_boundary) / color_collection.Count; // шаг в диапазоне значений
            return (int)Math.Floor( (value - lower_boundary) / h ); // округление вверх
        }
    }
}
