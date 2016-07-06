using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace Model
{
    public class NumericResultsImageData
    {
        private NumericResults num_res;
        private Point _coord;
        private double image_size_width;
        private double image_size_height;

        public int[] char_number; // трансформированные координаты или координаты на сетке

        public NumericResultsImageData(NumericResults num_res_p, double p1, double p2)
        {
            num_res = num_res_p;
            _coord = new Point(0, 0);
            image_size_width = p1;
            image_size_height = p2;
        }

        public Point coord
        {
            get { return _coord; }
            set
            {
                _coord = value;
                coord_transform();
            }
        }

        public BitmapSource image_source { get; set; }

        public BitmapSource create_image1() // два цвета
        {
            Palette_2Colors pal_2 = new Palette_2Colors(num_res.average_value);
            BitmapPalette bit_pal = new BitmapPalette(pal_2.color_collection);
            
            PixelFormat pf = PixelFormats.Indexed8;
            int width = num_res.width_count;
            int height = num_res.height_count;
            int stride = (width * pf.BitsPerPixel + 7) / 8;

            int dpiX = 96;
            int dpiY = 96;

            byte[] color_index = new byte[stride * height];
            
            try
            {
                for (int i = 0; i < width; i++)
                {
                    for (int j = 0; j < height; j++)
                    {
                        color_index[j * stride + i] = (byte)pal_2.choose_color(num_res.characteristics[j, i]);
                    }
                }
                
            }
            catch
            {
                return null;
            }
            return BitmapSource.Create(width, height, dpiX, dpiY, pf, bit_pal, color_index, stride);
        }

        public BitmapSource create_image2() // больше двух цветов
        {
            Palette_ManyColors pal_many = new Palette_ManyColors(num_res.minimum_value, num_res.maximum_value);
            BitmapPalette bit_pal = new BitmapPalette(pal_many.color_collection);

            PixelFormat pf = PixelFormats.Indexed8;
            int width = num_res.width_count;
            int height = num_res.height_count;
            int stride = (width * pf.BitsPerPixel + 7) / 8;
            int dpiX = 96;
            int dpiY = 96;

            byte[] color_index = new byte[stride * height];
            
            try
            {
                for (int i = 0; i < width; i++)
                {
                    for (int j = 0; j < height; j++)
                    {
                        color_index[j * stride + i] = (byte)pal_many.choose_color(num_res.characteristics[j, i]);
                    }
                }

            }
            catch
            {
                return null;
            }
            return BitmapSource.Create(width, height, dpiX, dpiY, pf, bit_pal, color_index, stride);
        }

        public void coord_transform()
        {
            char_number = new int[2];
            char_number[0] = (int)Math.Floor(num_res.width_count * _coord.X / image_size_width);
            char_number[1] = (int)Math.Floor(num_res.height_count * _coord.Y / image_size_height);
        }
    }
}
