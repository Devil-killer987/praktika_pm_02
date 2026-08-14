using SchoolEquipmentApp.Helpers;
using SchoolEquipmentApp.Models;
using SchoolEquipmentApp.Services;
using System.Collections.Generic;
using System.Windows.Input;

namespace SchoolEquipmentApp.ViewModels
{
    public class EquipmentTypeViewModel : ViewModelBase
    {
        private readonly ApiService _api;
        private readonly DialogService _dialog;

        private int _id;
        private string _name = string.Empty;
        private string _category = string.Empty;
        private bool _isEditMode = false;

        private List<string> _categories = new List<string> { "PC", "Printer", "Projector", "Network", "Other" };

        public EquipmentTypeViewModel(ApiService api, DialogService dialog)
        {
            _api = api;
            _dialog = dialog;
            SaveCommand = new RelayCommand(async () => await SaveAsync(), () => CanSave);
            CancelCommand = new RelayCommand(() => _dialog.ShowMessage("Отмена", "Информация"));
        }

        public EquipmentTypeViewModel(ApiService api, DialogService dialog, EquipmentType type) : this(api, dialog)
        {
            _id = type.Id;
            _name = type.Name;
            _category = type.Category;
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

        public string Category
        {
            get => _category;
            set => SetProperty(ref _category, value);
        }

        public List<string> Categories
        {
            get => _categories;
            set => SetProperty(ref _categories, value);
        }

        public bool CanSave => !string.IsNullOrWhiteSpace(Name) && !string.IsNullOrWhiteSpace(Category);

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        private async System.Threading.Tasks.Task SaveAsync()
        {
            try
            {
                if (IsEditMode)
                {
                    await _api.UpdateEquipmentTypeAsync(Id, new { Name, Category });
                }
                else
                {
                    await _api.CreateEquipmentTypeAsync(new { Name, Category });
                }
                System.Windows.Application.Current.Windows[^1]?.DialogResult = true;
            }
            catch
            {
                // Ошибка уже обработана в ApiService
            }
        }
    }
}