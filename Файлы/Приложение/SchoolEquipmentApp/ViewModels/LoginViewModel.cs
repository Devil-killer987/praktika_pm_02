using SchoolEquipmentApp.Helpers;
using SchoolEquipmentApp.Services;
using System.Windows;
using System.Windows.Input;

namespace SchoolEquipmentApp.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private readonly AuthService _authService;
        private readonly DialogService _dialogService;

        private string _login = string.Empty;
        private string _password = string.Empty;
        private bool _isLoading = false;

        public LoginViewModel(AuthService authService, DialogService dialogService)
        {
            _authService = authService;
            _dialogService = dialogService;
            LoginCommand = new RelayCommand(async () => await LoginAsync(), () => !IsLoading);
        }

        public string Login
        {
            get => _login;
            set => SetProperty(ref _login, value);
        }

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                SetProperty(ref _isLoading, value);
                (LoginCommand as RelayCommand)?.RaiseCanExecuteChanged();
            }
        }

        public ICommand LoginCommand { get; }

        private async System.Threading.Tasks.Task LoginAsync()
        {
            if (string.IsNullOrWhiteSpace(Login))
            {
                _dialogService.ShowMessage("Введите логин", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(Password))
            {
                _dialogService.ShowMessage("Введите пароль", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            IsLoading = true;

            try
            {
                var success = await _authService.LoginAsync(Login, Password);
                if (success)
                {
                    var mainWindow = new MainWindow();
                    mainWindow.Show();
                    Application.Current.Windows[0]?.Close();
                }
                else
                {
                    _dialogService.ShowMessage("Неверный логин или пароль", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}