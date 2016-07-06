using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model;

namespace ModelTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void NumericResultsViewTest()
        {
            NumericResultsView num_res_view = new NumericResultsView(new NumericResults(), false, false);
            
            //"Год обработки данных должен быть в диапазоне [2000, 2015].";
            num_res_view.num_res.processing_date = new DateTime(2012, 4, 15);
            //"Первый параметр модели должен лежать в диапазоне от 10 до 1000.";
            num_res_view.num_res.model_parameter_1 = 12;
            //"Второй параметр модели должен лежать в диапазоне от 10 до 1000.";
            num_res_view.num_res.model_parameter_2 = 1000;
            //"Число разбиений оси X должно быть от 10 до 1000.";
            num_res_view.num_res.width_count = 11;
            //"Число разбиений оси Y должно быть от 10 до 1000.";
            num_res_view.num_res.height_count = 100;
            //"Число блоков должно быть от 1 до 20.";
            num_res_view.num_res.threads_count = 2;

            Assert.IsNull(num_res_view.Error);
        }

        [TestMethod]
        public void NumericResultsTest()
        {
            NumericResults num_res = new NumericResults();

            // проверяем положительность времени (в мс) подсчета одного блока сетки 
            Assert.IsTrue(num_res.calculate_characteristics(5) > 0);
        }
    }
}
