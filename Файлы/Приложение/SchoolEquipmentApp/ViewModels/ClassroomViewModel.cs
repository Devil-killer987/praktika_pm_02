using SchoolEquipmentApp.Helpers;
using SchoolEquipmentApp.Models;
using SchoolEquipmentApp.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;

namespace SchoolEquipmentApp.ViewModels
{
    public class ClassroomViewModel : ViewModelBase
    {
        private readonly ApiService _api;
        private readonly DialogService _dialog;

        private int _id;
        private int _buildingId;
        private string _number = string.Empty;
        private string _floor = string.Empty;
        private string _description = string.Empty;
        private bool _isEditMode = false;
        private ObservableCollection<Building> _buildings = new ObservableCollection<Building>();

        public ClassroomViewModel(ApiService api, DialogService dialog, int buildingId)
        {
            _api = api;
            _dialog = dialog;
            _buildingId = buildingId;
            SaveCommand = new RelayCommand(async () => await SaveAsync(), () => CanSave);
            CancelCommand = new RelayCommand(() => _dialog.ShowMessage("Отмена", "Информация"));
            LoadBuildings();
        }

        public ClassroomViewModel(ApiService api, DialogService dialog, Classroom classroom) : this(api, dialog, classroom.BuildingId)
        {
            _id = classroom.Id;
            _number = classroom.Number;
            _floor = classroom.Floor;
            _description = classroom.Description;
            _buildingId = classroom.BuildingId;
            _isEditMode = true;
        }

        public int Id => _id;
        public bool IsEditMode
        {
            get => _isEditMode;
            set => SetProperty(ref _isEditMode, value);
        }

        public string Number
        {
            get => _number;
            set
            {
                SetProperty(ref _number, value);
                OnPropertyChanged(nameof(CanSave));
            }
        }

        public string Floor
        {
            get => _floor;
            set => SetProperty(ref _floor, value);
        }

        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        public int BuildingId
        {
            get => _buildingId;
            set => SetProperty(ref _buildingId, value);
        }

        public ObservableCollection<Building> Buildings
        {
            get => _buildings;
            set => SetProperty(ref _buildings, value);
        }

        public bool CanSave => !string.IsNullOrWhiteSpace(Number) && BuildingId > 0;

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        private async void LoadBuildings()
        {
            var buildings = await _api.GetBuildingsAsync();
            Buildings.Clear();
            foreach (var b in buildings)
                Buildings.Add(b);
        }

        private async System.Threading.Tasks.Task SaveAsync()
        {
            try
            {
                if (IsEditMode)
                {
                    await _api.UpdateClassroomAsync(Id, new { Number, Floor, Description });
                }
                else
                {
                    await _api.CreateClassroomAsync(new { BuildingId, Number, Floor, Description });
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