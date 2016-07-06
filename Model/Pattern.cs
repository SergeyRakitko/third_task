using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    [Serializable]
    public class Pattern
    {
        public string title { get; set; }
        public DateTime processing_date { get; set; }
        public double model_parameter_1 { get; set; }
        public double model_parameter_2 { get; set; }

        public Pattern(DateTime processing_date_p = new DateTime(), string title_p = "new_model", double model_parameter_1_p = 50.0, double model_parameter_2_p = 100.0)
        {
            title = title_p;
            processing_date = processing_date_p;
            model_parameter_1 = model_parameter_1_p;
            model_parameter_2 = model_parameter_2_p;
        }

    }
}
