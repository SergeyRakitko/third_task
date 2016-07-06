using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Model
{
    public class NumericResultsView : IDataErrorInfo
    {
        public NumericResults num_res { get; set; }
        public NumericResultsImageData num_res_image_data { get; set; }
        public bool IsCompleted;
        public bool IsSaved;

        public NumericResultsView(NumericResults num_res_p, bool IsCompleted_p = false, bool IsSaved_p = true)
        {
            num_res = num_res_p;
            IsCompleted = IsCompleted_p;
            IsSaved = IsSaved_p;
        }

        public string Error
        {
            get
            {
                if (num_res.processing_date.Year < 2000 || num_res.processing_date.Year > 2015)
                    return "Год обработки данных должен быть в диапазоне [2000, 2015].";
                if (num_res.model_parameter_1 < 10 || num_res.model_parameter_1 > 1000)
                    return "Первый параметр модели должен лежать в диапазоне от 10 до 1000.";
                if (num_res.model_parameter_2 < 10 || num_res.model_parameter_2 > 1000)
                    return "Второй параметр модели должен лежать в диапазоне от 10 до 1000.";
                if (num_res.width_count < 10 || num_res.width_count > 1000)
                    return "Число разбиений оси X должно быть от 10 до 1000.";
                if (num_res.height_count < 10 || num_res.height_count > 1000)
                    return "Число разбиений оси Y должно быть от 10 до 1000.";
                if (num_res.threads_count < 1 || num_res.threads_count > 20)
                    return "Число блоков должно быть от 1 до 20.";
                return null;
            }
        }
        
        public string this[string property_name]
        {
            get
            {
                switch (property_name)
                {
                    case "year":
                        if (num_res.processing_date.Year < 2000 || num_res.processing_date.Year > 2015)
                            return "Год обработки данных должен быть в диапазоне [2000, 2015].";
                        break;
                    case "width":
                        if (num_res.model_parameter_1 < 10 || num_res.model_parameter_1 > 1000)
                            return "Первый параметр модели должен лежать в диапазоне от 10 до 1000.";
                        break;
                    case "height":
                        if (num_res.model_parameter_2 < 10 || num_res.model_parameter_2 > 1000)
                            return "Второй параметр модели должен лежать в диапазоне от 10 до 1000.";
                        break;
                    case "width_count":
                        if (num_res.width_count < 10 || num_res.width_count > 1000)
                            return "Число разбиений оси X должно быть от 10 до 1000.";
                        break;
                    case "height_count":
                        if (num_res.height_count < 10 || num_res.height_count > 1000)
                            return "Число разбиений оси Y должно быть от 10 до 1000.";
                        break;
                    case "blocks_count":
                        if (num_res.threads_count < 1 || num_res.threads_count > 20)
                            return "Число блоков должно быть от 1 до 20.";
                        break;
                }
                return null;
            }
        }
    }
}
