using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace Model
{
    public class Palette_2Colors : CustomPalette
    {
        private double boundary;

        public Palette_2Colors(double parameter)
        {
            boundary = parameter;
            color_collection = new List<Color>();
            
            color_collection.Add(Colors.Green);
            color_collection.Add(Colors.Red);
        }

        public override int choose_color(double value)
        {
            if (value <= boundary)
                return 0;
            else return 1;
        }
    }
}
