using SchoolEquipmentApp.Helpers;
using SchoolEquipmentApp.Models;
using SchoolEquipmentApp.Services;
using System.Windows.Data;
using System.Windows.Input;

namespace SchoolEquipmentApp.ViewModels
{
    public class BuildingViewModel : ViewModelBase
    {
        private readonly ApiService _api;
        private readonly DialogService _dialog;

        private int _id;
        private string _name = string.Empty;
        private string _address = string.Empty;
        private bool _isEditMode = false;

        public BuildingViewModel(ApiService api, DialogService dialog)
        {
            _api = api;
            _dialog = dialog;
            SaveCommand = new RelayCommand(async () => await SaveAsync(), () => CanSave);
            CancelCommand = new RelayCommand(() => _dialog.ShowMessage("Отмена", "Информация"));
        }

        public BuildingViewModel(ApiService api, DialogService dialog, Building building) : this(api, dialog)
        {
            _id = building.Id;
            _name = building.Name;
            _address = building.Address;
            _isEditMode = true;
        }

        public int Id => _id;
        public bool IsEditMode
        {
            get => _isEditMode;
            set => SetProperty(ref _isEditMode, value);
        }

        public string Name
        {
            get => _name;
            set
            {
                SetProperty(ref _name, value);
                OnPropertyChanged(nameof(CanSave));
            }
        }

        public string Address
        {
            get => _address;
            set => SetProperty(ref _address, value);
        }

        public bool CanSave => !string.IsNullOrWhiteSpace(Name);

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        private async System.Threading.Tasks.Task SaveAsync()
        {
            try
            {
                if (IsEditMode)
                {
                    await _api.UpdateBuildingAsync(Id, new { Name, Address });
                }
                else
                {
                    await _api.CreateBuildingAsync(new { Name, Address });
                }
                // Закрываем диалог
                System.Windows.Application.Current.Windows[^1]?.DialogResult = true;
            }
            catch
            {
                // Ошибка уже обработана в ApiService
            }
        }
    }
}