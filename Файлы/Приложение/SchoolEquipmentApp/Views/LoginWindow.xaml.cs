using System.Windows;
using System.Windows.Controls;
using SchoolEquipmentApp.Services;
using SchoolEquipmentApp.ViewModels;

namespace SchoolEquipmentApp.Views
{
    public partial class LoginWindow : Window
    {
        private LoginViewModel _viewModel;

        public LoginWindow()
        {
            InitializeComponent();

            var apiService = new ApiService();
            var authService = new AuthService(apiService);
            var dialogService = new DialogService();

            _viewModel = new LoginViewModel(authService, dialogService);
            DataContext = _viewModel;
        }

        private void txtPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is LoginViewModel vm)
            {
                vm.Password = ((PasswordBox)sender).Password;
            }
        }
    }
}