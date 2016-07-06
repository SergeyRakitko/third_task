using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace Model
{
    public abstract class CustomPalette
    {
        public List<Color> color_collection;

        public abstract int choose_color(double value);
    }
}
