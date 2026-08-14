using System.Windows;
using SchoolEquipmentApp.Views;

namespace SchoolEquipmentApp.Services
{
    public class DialogService
    {
        public bool? ShowLoginDialog()
        {
            var window = new LoginWindow();
            return window.ShowDialog();
        }

        public bool? ShowBuildingDialog(object viewModel)
        {
            var window = new BuildingDialog();
            window.DataContext = viewModel;
            return window.ShowDialog();
        }

        public bool? ShowClassroomDialog(object viewModel)
        {
            var window = new ClassroomDialog();
            window.DataContext = viewModel;
            return window.ShowDialog();
        }

        public bool? ShowEquipmentDialog(object viewModel)
        {
            var window = new EquipmentDialog();
            window.DataContext = viewModel;
            return window.ShowDialog();
        }

        public bool? ShowEquipmentTypeDialog(object viewModel)
        {
            var window = new EquipmentTypeDialog();
            window.DataContext = viewModel;
            return window.ShowDialog();
        }

        public bool? ShowSpecificationDialog(object viewModel)
        {
            var window = new SpecificationDialog();
            window.DataContext = viewModel;
            return window.ShowDialog();
        }

        public void ShowMessage(string message, string title = "Информация", MessageBoxButton buttons = MessageBoxButton.OK, MessageBoxImage icon = MessageBoxImage.Information)
        {
            MessageBox.Show(message, title, buttons, icon);
        }

        public bool ShowConfirmation(string message, string title = "Подтверждение")
        {
            return MessageBox.Show(message, title, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes;
        }
    }
}