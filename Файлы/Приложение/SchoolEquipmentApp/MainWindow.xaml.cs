using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using SchoolEquipmentApp.Models;
using SchoolEquipmentApp.ViewModels;

namespace SchoolEquipmentApp
{
    public partial class MainWindow : Window
    {
        private MainViewModel _viewModel;

        public MainWindow()
        {
            InitializeComponent();
            _viewModel = new MainViewModel();
            DataContext = _viewModel;

            // Подписываемся на изменения выделения в TreeView
            treeView.SelectedItemChanged += TreeView_SelectedItemChanged;
        }

        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var selected = e.NewValue;

            if (selected is Building building)
            {
                _viewModel.SelectedBuilding = building;
            }
            else if (selected is Classroom classroom)
            {
                _viewModel.SelectedClassroom = classroom;
                // Находим родительское здание
                var parent = treeView.SelectedItem as Classroom;
                if (parent != null)
                {
                    // Обновляем здание если нужно
                }
            }
        }
    }
}