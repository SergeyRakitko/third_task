using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Model
{
    public class ThreadInfo : INotifyPropertyChanged
    {
        private int _thread_number; // номер потока (нити)
        private int _block_number; // номер блока сетки, который считается
        private bool _is_processing_finished; // статус вычислений
        private int _processing_time; // время вычислений

        public event PropertyChangedEventHandler PropertyChanged;

        public ThreadInfo(int thread_number_p = 0, int block_number_p = 0, bool is_processing_finished_p = false,  int processing_time_p = 0)
        {
            _thread_number = thread_number_p;
            _block_number = block_number_p;
            _is_processing_finished = is_processing_finished_p;
            _processing_time = processing_time_p;
        }

        public int thread_number
        {
            get { return _thread_number; }
            set
            {
                _thread_number = value;
                OnPropertyChanged("thread_number");
            }
        }

        public int block_number
        {
            get { return _block_number; }
            set
            {
                _block_number = value;
                OnPropertyChanged("block_number");

            }
        } 
        
        public bool is_processing_finished
        {
            get { return _is_processing_finished; }
            set
            {
                _is_processing_finished = value;
                OnPropertyChanged("is_processing_finished");
                
            }
        } 

        public int processing_time
        {
            get { return _processing_time; }
            set
            {
                _processing_time = value;
                OnPropertyChanged("processing_time");
            }
                
        } 

        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(name));
        }
        
    }
}
