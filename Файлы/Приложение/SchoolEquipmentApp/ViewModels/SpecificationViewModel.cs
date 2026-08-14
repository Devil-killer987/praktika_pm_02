using SchoolEquipmentApp.Helpers;
using SchoolEquipmentApp.Models;
using SchoolEquipmentApp.Services;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace SchoolEquipmentApp.ViewModels
{
    public class SpecificationViewModel : ViewModelBase
    {
        private readonly ApiService _api;
        private readonly DialogService _dialog;

        private int _id;
        private string _name = string.Empty;
        private string _displayName = string.Empty;
        private string _unit = string.Empty;
        private int _equipmentTypeId;
        private bool _isEditMode = false;

        private ObservableCollection<EquipmentType> _equipmentTypes = new ObservableCollection<EquipmentType>();

        public SpecificationViewModel(ApiService api, DialogService dialog)
        {
            _api = api;
            _dialog = dialog;
            SaveCommand = new RelayCommand(async () => await SaveAsync(), () => CanSave);
            CancelCommand = new RelayCommand(() => _dialog.ShowMessage("Отмена", "Информация"));
            LoadEquipmentTypes();
        }

        public SpecificationViewModel(ApiService api, DialogService dialog, SpecificationCategory category)
            : this(api, dialog)
        {
            _id = category.Id;
            _name = category.Name;
            _displayName = category.DisplayName;
            _unit = category.Unit;
            _equipmentTypeId = category.EquipmentTypeId;
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

        public string DisplayName
        {
            get => _displayName;
            set
            {
                SetProperty(ref _displayName, value);
                OnPropertyChanged(nameof(CanSave));
            }
        }

        public string Unit
        {
            get => _unit;
            set => SetProperty(ref _unit, value);
        }

        public int EquipmentTypeId
        {
            get => _equipmentTypeId;
            set
            {
                SetProperty(ref _equipmentTypeId, value);
                OnPropertyChanged(nameof(CanSave));
            }
        }

        public ObservableCollection<EquipmentType> EquipmentTypes
        {
            get => _equipmentTypes;
            set => SetProperty(ref _equipmentTypes, value);
        }

        public bool CanSave => !string.IsNullOrWhiteSpace(Name) &&
                               !string.IsNullOrWhiteSpace(DisplayName) &&
                               EquipmentTypeId > 0;

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        private async void LoadEquipmentTypes()
        {
            var types = await _api.GetEquipmentTypesAsync();
            EquipmentTypes.Clear();
            foreach (var t in types)
                EquipmentTypes.Add(t);
        }

        private async System.Threading.Tasks.Task SaveAsync()
        {
            try
            {
                var data = new
                {
                    Name,
                    DisplayName,
                    Unit,
                    EquipmentTypeId
                };

                if (IsEditMode)
                {
                    // Обновление (PUT) - для простоты используем удаление + создание
                    await _api.DeleteSpecificationCategoryAsync(Id);
                    await _api.CreateSpecificationCategoryAsync(data);
                }
                else
                {
                    await _api.CreateSpecificationCategoryAsync(data);
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