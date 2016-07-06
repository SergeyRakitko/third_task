using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ViewModel;
using System.ComponentModel.Composition;
using System.Windows;
using System.ComponentModel.Composition.Hosting;

namespace ViewModelTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void threads_count_in_Test()
        {
            var mvm = new MainViewModel();
            string str = "-12";
            mvm.threads_count_in = str;

            // строковое поле для положительных чисел меняется, если аргумент можно преобразовать к положительному числу

            int parameter = 0;
            if (Int32.TryParse(str, out parameter) && parameter > 0)
                Assert.AreEqual(mvm.threads_count_in, str);
            else 
                Assert.AreNotEqual(mvm.threads_count_in, str);
        }

        [TestMethod]
        public void NewFileTest()
        {
            var catalog = new AssemblyCatalog(typeof(TestPromptService).Assembly);
            var mvm = new MainViewModel();
            var cc = new CompositionContainer(catalog);
            cc.ComposeParts(mvm);

            string str = "Model1";
            mvm.title_in = str;

            mvm.NewFile.Execute(null);
            // модель задалась по умолчанию, измененные данные стали данными по умолчанию
            Assert.AreNotEqual(mvm.title_in, str);
        }

    }

    [Export(typeof(IPromptService))]
    public class TestPromptService : IPromptService
    {
        public bool Confirm(string text)
        {
            return true;
        }

        public string GetOpenFileName()
        {
            return null;
        }

        public string GetSaveFileName()
        {
            return null;
        }

        public Point GetMousePosition()
        {
            return new Point(0,0);
        }
    }
}
