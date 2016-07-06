using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using System.ComponentModel;
using System.Windows.Media.Imaging;
using System.Windows.Input;
using System.ComponentModel.Composition;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;

namespace ViewModel
{
    public interface IPromptService
    {
        bool Confirm(string text);
        string GetOpenFileName();
        string GetSaveFileName();
        Point GetMousePosition();
    }

    public class MainViewModel : INotifyPropertyChanged
    {

        private ObservableCollectionThreadInfo thread_collection; // коллекция нитей
        private NumericResultsView _num_res_view;
        private BackgroundWorker[] worker_massive; // массив распараллеливающих вычисления объектов класса BackgroundWorker

        private int all_blocks; // количество блоков на сетке
        private int block_num; // номер блока, который начнут считать
        private string mistake; // элемент для вывода ошибок
        private bool _is_enabled; // блокировка элементов при вычислениях
        private bool _radiobutton1;
        private bool _radiobutton2;
        private string _point_value; // координаты выбранной точки

        public MainViewModel()
        {
            use_mistake = "Поле для вывода ошибок";
            _is_enabled = true;

            image_size_width = 400;
            image_size_height = 400;
            
            point_value = "0";

            thread_collection = new ObservableCollectionThreadInfo();

            _num_res_view = new NumericResultsView(new NumericResults(), false, true);
            radiobutton1 = true;
            radiobutton2 = false;

            _num_res_view.num_res_image_data = new NumericResultsImageData(new NumericResults(), image_size_width, image_size_height);
            image_source = _num_res_view.num_res_image_data.create_image1();
            if (image_source == null) use_mistake = "Невозможно создать изображение.";
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string p)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(p));
        }

        public ObservableCollectionThreadInfo use_thread_collection
        {
            get { return thread_collection; }
            set
            {
                thread_collection = value;
                RaisePropertyChanged("use_thread_collection");
            }
        }

        public string use_mistake
        {
            get { return mistake; }
            set
            {
                mistake = value;
                RaisePropertyChanged("use_mistake");
            }
        }

        public bool is_enabled
        {
            get
            {
                return _is_enabled;
            }
            set
            {
                _is_enabled = value;
                RaisePropertyChanged("is_enabled");
            }
        }

        // ввод данных

        public string title_in
        {
            get { return num_res_view.num_res.title; }
            set
            {
                if (value.Contains(" "))
                {
                    use_mistake = "Название модели не должно содержать пробелов.";
                }
                else
                {
                    num_res_view.num_res.title = value;
                    num_res_view.IsSaved = false;
                }
            }
        }

        public DateTime processing_date_in
        {
            get { return num_res_view.num_res.processing_date; }
            set
            {
                num_res_view.num_res.processing_date = value;
                num_res_view.IsSaved = false;

            }
        }

        public string first_parameter_in
        {
            get { return num_res_view.num_res.model_parameter_1.ToString(); }
            set
            {
                double parameter = 0;
                if (Double.TryParse(value, out parameter) && parameter > 0)
                {
                    num_res_view.num_res.model_parameter_1 = parameter;
                    RaisePropertyChanged("first_parameter_in");
                    num_res_view.IsSaved = false;
                }
                else
                {
                    use_mistake = "Первый параметр модели должен быть положителен.";
                }
            }
        }

        public string second_parameter_in
        {
            get { return num_res_view.num_res.model_parameter_2.ToString(); }
            set
            {
                double parameter = 0;
                if (Double.TryParse(value, out parameter) && parameter > 0)
                {
                    num_res_view.num_res.model_parameter_2 = parameter;
                    RaisePropertyChanged("second_parameter_in");
                    num_res_view.IsSaved = false;
                }
                else
                {
                    use_mistake = "Второй параметр модели должен быть положителен.";
                }
            }
        }

        public string width_count_in
        {
            get { return num_res_view.num_res.width_count.ToString(); }
            set
            {
                int parameter = 0;
                if (Int32.TryParse(value, out parameter) && parameter > 0)
                {
                    num_res_view.num_res.width_count = parameter;
                    RaisePropertyChanged("width_count_in");
                    num_res_view.IsSaved = false;
                }
                else
                {
                    use_mistake = "Должно быть положительное целое число разбиений оси X.";
                }
            }
        }

        public string height_count_in
        {
            get { return num_res_view.num_res.height_count.ToString(); }
            set
            {
                int parameter = 0;
                if (Int32.TryParse(value, out parameter) && parameter > 0)
                {
                    num_res_view.num_res.height_count = parameter;
                    RaisePropertyChanged("height_count_in");
                    num_res_view.IsSaved = false;
                }
                else
                {
                    use_mistake = "Должно быть положительное целое число разбиений оси Y.";
                }
            }
        }

        public string threads_count_in
        {
            get { return num_res_view.num_res.threads_count.ToString(); }
            set
            {
                int parameter = 0;
                if (Int32.TryParse(value, out parameter) && parameter > 0)
                {
                    num_res_view.num_res.threads_count = parameter;
                    RaisePropertyChanged("threads_count_in");
                    num_res_view.IsSaved = false;
                }
                else
                {
                    use_mistake = "Должно быть положительное целое число потоков.";
                }
            }
        }

        // ввод данных

        // размеры элемента image
        public double image_size_width { get; set; }
        public double image_size_height { get; set; }

        public string point_value
        {
            get { return _point_value; }
            set
            {
                _point_value = value;
                RaisePropertyChanged("point_value");
            }
        }

        public NumericResultsView num_res_view
        {
            get { return _num_res_view; }
            set
            {
                _num_res_view = value;
                RaisePropertyChanged("num_res_view");
                num_res_view.IsSaved = false;
            }
        }

        public NumericResults num_res
        {
            get { return _num_res_view.num_res; }
            set
            {
                _num_res_view.num_res = value;
                RaisePropertyChanged("num_res");
            }
        }

        public BitmapSource image_source
        {
            get { return _num_res_view.num_res_image_data.image_source; }
            set
            {
                _num_res_view.num_res_image_data.image_source = value;
                RaisePropertyChanged("image_source");
            }
        }

        public bool radiobutton1
        {
            get { return _radiobutton1; }
            set
            {
                _radiobutton1 = value;
                RaisePropertyChanged("radiobutton1");
                if ((_num_res_view.IsCompleted == true) && (value == true))
                {
                    _num_res_view.num_res_image_data = new NumericResultsImageData(_num_res_view.num_res, image_size_width, image_size_height);
                    image_source = _num_res_view.num_res_image_data.create_image1();
                    if (image_source == null) use_mistake = "Невозможно создать изображение.";
                }
            }
        }

        public bool radiobutton2
        {
            get { return _radiobutton2; }
            set
            {
                _radiobutton2 = value;
                RaisePropertyChanged("radiobutton2");
                if ((_num_res_view.IsCompleted == true) && (value == true))
                {
                    _num_res_view.num_res_image_data = new NumericResultsImageData(_num_res_view.num_res, image_size_width, image_size_height);
                    image_source = _num_res_view.num_res_image_data.create_image2();
                    if (image_source == null) use_mistake = "Невозможно создать изображение.";
                }
            }
        }


        // команды

        [Import(typeof(IPromptService))]
        public IPromptService PromptService { get; set; }

        DelegateCommand _MouseDownEventHandler;
        private void mousedown(object obj)
        {
            double x, y;
            x = PromptService.GetMousePosition().X;
            y = PromptService.GetMousePosition().Y;

            // проверяем координаты на попадание в элемент image
            if ((x >= 0 && x <= image_size_width) && (y >= 0 && y <= image_size_height))
            {
                _num_res_view.num_res_image_data.coord = new Point(x, y);
                point_value = "F(" + (x).ToString("0.##") + ", " + (y).ToString("0.##") + ") = "
                              + num_res.characteristics[_num_res_view.num_res_image_data.char_number[1],
                                                        _num_res_view.num_res_image_data.char_number[0]].ToString("0.##");
            }
        }
        public ICommand MouseDownEventHandler
        {
            get
            {
                return _MouseDownEventHandler ?? (_MouseDownEventHandler = new DelegateCommand(mousedown, o => true));
            }
        }

        DelegateCommand _FormClosedEventHandler;
        private void formclosed(object obj) 
        {
            if (!_num_res_view.IsSaved)
            {
                if (PromptService.Confirm("Сохранить изменения?")) savefile(null);
            }
        }
        public ICommand FormClosedEventHandler
        {
            get
            {
                return _FormClosedEventHandler ?? (_FormClosedEventHandler = new DelegateCommand(formclosed, o => true));
            }
        }

        DelegateCommand _NewFile;
        private void newfile(object obj)
        {
            if (!num_res_view.IsSaved)
            {
                if (PromptService.Confirm("Сохранить изменения?")) savefile(null);
            }

            use_thread_collection.Clear(); // очистили и обновили коллекцию

            // задаем данные по умолчанию
            _num_res_view = new NumericResultsView(new NumericResults(), false, true);
            RaisePropertyChanged("num_res_view");
            RaisePropertyChanged("num_res");
            RaisePropertyChanged("title_in");
            RaisePropertyChanged("processing_date_in");
            RaisePropertyChanged("first_parameter_in");
            RaisePropertyChanged("second_parameter_in");
            RaisePropertyChanged("width_count_in");
            RaisePropertyChanged("height_count_in");
            RaisePropertyChanged("threads_count_in");

            _num_res_view.num_res_image_data = new NumericResultsImageData(new NumericResults(), image_size_width, image_size_height);
            image_source = _num_res_view.num_res_image_data.create_image1();
            if (image_source == null) use_mistake = "Невозможно создать изображение.";

            point_value = "0";
        }
        public ICommand NewFile
        {
            get
            {
                return _NewFile ?? (_NewFile = new DelegateCommand(newfile, o => true));
            }
        }

        DelegateCommand _OpenFile;
        private void openfile(object obj)
        {
            if (!num_res_view.IsSaved)
            {
                if (PromptService.Confirm("Сохранить изменения?")) savefile(null);
            }
            string filename = PromptService.GetOpenFileName();

            if (filename != null)
            {
                Stream fstream = null;
                BinaryFormatter binformat = new BinaryFormatter();
                try
                {
                    fstream = File.OpenRead(filename);
                    num_res = (NumericResults)binformat.Deserialize(fstream); // десериализовали и обновили коллекцию
                    num_res_view.IsSaved = true;
                    num_res_view.IsCompleted = true;
                    RaisePropertyChanged("num_res_view");
                    RaisePropertyChanged("title_in");
                    RaisePropertyChanged("processing_date_in");
                    RaisePropertyChanged("first_parameter_in");
                    RaisePropertyChanged("second_parameter_in");
                    RaisePropertyChanged("width_count_in");
                    RaisePropertyChanged("height_count_in");
                    RaisePropertyChanged("threads_count_in");

                    _num_res_view.num_res_image_data = new NumericResultsImageData(_num_res_view.num_res, image_size_width, image_size_height);
                    if (_radiobutton1) image_source = _num_res_view.num_res_image_data.create_image1();
                    else image_source = _num_res_view.num_res_image_data.create_image2();
                    if (image_source == null) use_mistake = "Невозможно создать изображение.";
                }
                catch (Exception ex)
                {
                    use_mistake = ex.Message.ToString();
                }
                finally
                {
                    if (fstream != null)
                        fstream.Close();
                }
            }
        }
        public ICommand OpenFile
        {
            get
            {
                return _OpenFile ?? (_OpenFile = new DelegateCommand(openfile, o => true));
            }
        }

        DelegateCommand _SaveFile;
        private void savefile(object obj)
        {
            string filename = PromptService.GetSaveFileName();
            if (filename != null)
            {
                Stream fstream = null;
                BinaryFormatter binformat = new BinaryFormatter();
                try
                {
                    fstream = new FileStream(filename, FileMode.OpenOrCreate);
                    binformat.Serialize(fstream, num_res_view.num_res);
                    num_res_view.IsSaved = true;
                }
                catch (Exception ex)
                {
                    use_mistake = ex.Message.ToString();
                }
                finally
                {
                    if (fstream != null)
                        fstream.Close();
                }
            }
        }
        public ICommand SaveFile
        {
            get
            {
                return _SaveFile ?? (_SaveFile = new DelegateCommand(savefile, o => true));
            }
        }

        DelegateCommand _Execute;
        private void execute(object obj)
        {
            if (num_res_view.Error == null)
            {
                is_enabled = false;
                _num_res_view.num_res_image_data = new NumericResultsImageData(new NumericResults(), image_size_width, image_size_height);
                image_source = _num_res_view.num_res_image_data.create_image1();
                if (image_source == null) use_mistake = "Невозможно создать изображение.";

                use_thread_collection.Clear(); // очистили и обновили коллекцию
                num_res_view.IsSaved = false;
                all_blocks = num_res_view.num_res.width_count * num_res_view.num_res.height_count;
                _num_res_view.num_res.characteristics = new double[_num_res_view.num_res.height_count, _num_res_view.num_res.width_count];

                block_num = num_res_view.num_res.threads_count; // вычисление первых блоков сетки запустится в этой команде, остальные - "на подхвате"

                worker_massive = new BackgroundWorker[num_res_view.num_res.threads_count];
                ThreadInfo thread = new ThreadInfo();

                for (int i = 0; i < num_res_view.num_res.threads_count; i++)
                {
                    worker_massive[i] = new BackgroundWorker();
                    worker_massive[i].DoWork += worker_DoWork;
                    worker_massive[i].RunWorkerCompleted += worker_RunWorkerCompleted;

                    thread = new ThreadInfo(i, i, false, 0);
                    use_thread_collection.Add(thread);

                    worker_massive[i].RunWorkerAsync(thread);
                }
            }
            else
            {
                use_mistake = num_res_view.Error;
            }
        }
        public ICommand Execute
        {
            get
            {
                return _Execute ?? (_Execute = new DelegateCommand(execute, o => true));
            }
        }
        
        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            ThreadInfo thread = e.Argument as ThreadInfo;

            thread_collection[thread.thread_number].processing_time = _num_res_view.num_res.calculate_characteristics(thread.block_number);

            e.Result = thread;
        }

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            block_num++; // поток освободился и застолбил следующий блок сетки
            ThreadInfo thread = e.Result as ThreadInfo;
            use_thread_collection[thread.thread_number].is_processing_finished = true;

            if (block_num - 1 < all_blocks)
            {
                worker_massive[thread.thread_number].Dispose();
                worker_massive[thread.thread_number] = new BackgroundWorker();

                worker_massive[thread.thread_number].DoWork += worker_DoWork;
                worker_massive[thread.thread_number].RunWorkerCompleted += worker_RunWorkerCompleted;

                ThreadInfo new_thread = new ThreadInfo(thread.thread_number, block_num - 1, false, 0);
                use_thread_collection[thread.thread_number] = new_thread;

                use_mistake = "Начал считаться блок с номером " + block_num.ToString() + " из " + all_blocks.ToString() + ".";

                worker_massive[thread.thread_number].RunWorkerAsync(new_thread);

            }
            else
            {
                use_mistake = "Все расчеты произведены.";
                _num_res_view.IsCompleted = true;
                RaisePropertyChanged("num_res");

                _num_res_view.num_res_image_data = new NumericResultsImageData(_num_res_view.num_res, image_size_width, image_size_height);
                if (_radiobutton1) image_source = _num_res_view.num_res_image_data.create_image1();
                else image_source = _num_res_view.num_res_image_data.create_image2();
                if (image_source == null) use_mistake = "Невозможно создать изображение.";
                is_enabled = true;
            }


        }
    }
}
