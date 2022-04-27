using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System.Windows.Input;
using MVVMDemo.DAL;
using MVVMDemo.Model;
using System.Collections.ObjectModel;
using System;
using System.Collections.Generic;
using System.Windows;
using MVVMDemo.View;
using System.Linq;

namespace MVVMDemo.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        #region 属性及构造函数
        private LocalDb localDb;

        private ObservableCollection<Student> gridModelList;

        public ObservableCollection<Student> GridModelList
        {
            get { return gridModelList; }
            set 
            {
                gridModelList = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// 查询条件
        /// </summary>
        private string search;

        public string Search
        {
            get { return search; }
            set 
            { 
                search = value;
                RaisePropertyChanged();
            }
        }

        public MainViewModel()
        {
            localDb = new LocalDb();
            QueryCommand = new RelayCommand(this.Query);
            ResetCommand = new RelayCommand(this.Reset);
            EditCommand = new RelayCommand<int>(this.Edit);
            AddCommand = new RelayCommand(this.Add);
            DeleteCommand = new RelayCommand<int>(this.Delete);
        }

        #endregion

        private void Reset()
        {
            this.Search = string.Empty;
            this.Query();
        }
        public void Query()
        {
            List<Student> students;
            if (string.IsNullOrEmpty(search))
            {
                students = localDb.Query();
            }
            else
            {
                students = localDb.QueryByName(search);
            }
            GridModelList = new ObservableCollection<Student>();
            if (students != null)
            {
                students.ForEach((t) =>
                 {
                     GridModelList.Add(t);
                 });
            }
        }
        private void Edit(int id)
        {
            var model = localDb.QueryById(id);
            if (model != null)
            {
                StudentWindow view = new StudentWindow(model);
                var r = view.ShowDialog();
                if (r.Value)
                {
                    var newModel = gridModelList.FirstOrDefault(t => t.Id == model.Id);
                    if (newModel != null)
                    {
                        newModel.Name = model.Name;
                        newModel.Age = model.Age;
                        newModel.Classes = model.Classes;
                    }
                    this.Query();
                }
            }
        }
        private void Add()
        {
            Student model = new Student();
            StudentWindow view = new StudentWindow(model);
            var r = view.ShowDialog();
            if (r.Value)
            {
                model.Id = GridModelList.Max(t => t.Id) + 1;
                localDb.AddStudent(model);
                this.Query();
            }
        }
        private void Delete(int Id)
        {
            var model = localDb.QueryById(Id);
            if (model != null)
            {
                var r = MessageBox.Show($"确定要删除吗【{model.Name}】？", "提示", MessageBoxButton.YesNo);
                if (r == MessageBoxResult.Yes)
                {
                    localDb.DelStudent(Id);
                    this.Query();
                }
            }
        }
        #region Command
        /// <summary>
        /// 查询命令
        /// </summary>
        public RelayCommand QueryCommand { get; set; }

        /// <summary>
        /// 重置命令
        /// </summary>
        public RelayCommand ResetCommand { get; set; }

        /// <summary>
        /// 编辑命令
        /// </summary>
        public RelayCommand<int> EditCommand { get; set; }

        /// <summary>
        /// 删除命令
        /// </summary>
        public RelayCommand<int> DeleteCommand { get; set; }

        /// <summary>
        /// 新增命令
        /// </summary>
        public RelayCommand AddCommand { get; set; }

        #endregion

    }
}