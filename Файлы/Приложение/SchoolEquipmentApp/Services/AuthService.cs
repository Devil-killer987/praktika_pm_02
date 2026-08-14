using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SchoolEquipmentApp.Services
{
    public class AuthService
    {
        private readonly ApiService _api;
        private string _token = string.Empty;
        private string _userLogin = string.Empty;
        private string _userRole = string.Empty;
        private string _userFullName = string.Empty;

        public AuthService(ApiService api)
        {
            _api = api;
        }

        public bool IsAuthenticated => !string.IsNullOrEmpty(_token);
        public string Token => _token;
        public string UserLogin => _userLogin;
        public string UserRole => _userRole;
        public string UserFullName => _userFullName;
        public bool IsAdmin => _userRole == "Admin";
        public bool IsOperator => _userRole == "Operator" || _userRole == "Admin";

        public async Task<bool> LoginAsync(string login, string password)
        {
            try
            {
                var result = await _api.LoginAsync(login, password);
                var json = JsonConvert.SerializeObject(result);
                dynamic data = JsonConvert.DeserializeObject(json);

                if (data?.token != null)
                {
                    _token = data.token;
                    _userLogin = data.login ?? login;
                    _userRole = data.role ?? "Viewer";
                    _userFullName = data.fullName ?? login;

                    _api.SetToken(_token);
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка входа: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        public void Logout()
        {
            _token = string.Empty;
            _userLogin = string.Empty;
            _userRole = string.Empty;
            _userFullName = string.Empty;
            _api.SetToken(null);
        }
    }
}