using SchoolEquipmentApp.Helpers;
using SchoolEquipmentApp.Models;
using SchoolEquipmentApp.Services;
using SchoolEquipmentApp.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace SchoolEquipmentApp.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly ApiService _api;
        private readonly AuthService _auth;
        private readonly DialogService _dialog;

        private ObservableCollection<Building> _buildings = new ObservableCollection<Building>();
        private ObservableCollection<Classroom> _classrooms = new ObservableCollection<Classroom>();
        private ObservableCollection<Equipment> _equipment = new ObservableCollection<Equipment>();
        private ObservableCollection<EquipmentType> _equipmentTypes = new ObservableCollection<EquipmentType>();

        private Building _selectedBuilding;
        private Classroom _selectedClassroom;
        private Equipment _selectedEquipment;
        private EquipmentType _selectedEquipmentType;

        private bool _isLoading = false;
        private string _statusText = "Готов к работе";

        public MainViewModel()
        {
            _api = new ApiService();
            _auth = new AuthService(_api);
            _dialog = new DialogService();

            LoadDataCommand = new RelayCommand(async () => await LoadDataAsync());
            AddBuildingCommand = new RelayCommand(async () => await AddBuildingAsync());
            EditBuildingCommand = new RelayCommand(async () => await EditBuildingAsync(), () => SelectedBuilding != null);
            DeleteBuildingCommand = new RelayCommand(async () => await DeleteBuildingAsync(), () => SelectedBuilding != null);

            AddClassroomCommand = new RelayCommand(async () => await AddClassroomAsync(), () => SelectedBuilding != null);
            EditClassroomCommand = new RelayCommand(async () => await EditClassroomAsync(), () => SelectedClassroom != null);
            DeleteClassroomCommand = new RelayCommand(async () => await DeleteClassroomAsync(), () => SelectedClassroom != null);

            AddEquipmentCommand = new RelayCommand(async () => await AddEquipmentAsync(), () => SelectedClassroom != null);
            EditEquipmentCommand = new RelayCommand(async () => await EditEquipmentAsync(), () => SelectedEquipment != null);
            DeleteEquipmentCommand = new RelayCommand(async () => await DeleteEquipmentAsync(), () => SelectedEquipment != null);

            AddEquipmentTypeCommand = new RelayCommand(async () => await AddEquipmentTypeAsync());
            EditEquipmentTypeCommand = new RelayCommand(async () => await EditEquipmentTypeAsync(), () => SelectedEquipmentType != null);
            DeleteEquipmentTypeCommand = new RelayCommand(async () => await DeleteEquipmentTypeAsync(), () => SelectedEquipmentType != null);

            RefreshCommand = new RelayCommand(async () => await LoadDataAsync());
            LogoutCommand = new RelayCommand(Logout);

            _ = LoadDataAsync();
        }

        public ObservableCollection<Building> Buildings
        {
            get => _buildings;
            set => SetProperty(ref _buildings, value);
        }

        public ObservableCollection<Classroom> Classrooms
        {
            get => _classrooms;
            set => SetProperty(ref _classrooms, value);
        }

        public ObservableCollection<Equipment> Equipment
        {
            get => _equipment;
            set => SetProperty(ref _equipment, value);
        }

        public ObservableCollection<EquipmentType> EquipmentTypes
        {
            get => _equipmentTypes;
            set => SetProperty(ref _equipmentTypes, value);
        }

        public Building SelectedBuilding
        {
            get => _selectedBuilding;
            set
            {
                SetProperty(ref _selectedBuilding, value);
                if (value != null)
                    LoadClassroomsAsync(value.Id);
                else
                    Classrooms.Clear();
                OnPropertyChanged(nameof(CanManageClassrooms));
                OnPropertyChanged(nameof(CanManageEquipment));
            }
        }

        public Classroom SelectedClassroom
        {
            get => _selectedClassroom;
            set
            {
                SetProperty(ref _selectedClassroom, value);
                if (value != null)
                    LoadEquipmentAsync(value.Id);
                else
                    Equipment.Clear();
                OnPropertyChanged(nameof(CanManageEquipment));
            }
        }

        public Equipment SelectedEquipment
        {
            get => _selectedEquipment;
            set => SetProperty(ref _selectedEquipment, value);
        }

        public EquipmentType SelectedEquipmentType
        {
            get => _selectedEquipmentType;
            set => SetProperty(ref _selectedEquipmentType, value);
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public string StatusText
        {
            get => _statusText;
            set => SetProperty(ref _statusText, value);
        }

        public bool IsAdmin => _auth.IsAdmin;
        public bool IsOperator => _auth.IsOperator;
        public bool IsAuthenticated => _auth.IsAuthenticated;
        public string UserName => _auth.UserFullName;
        public string UserRole => _auth.UserRole;

        public bool CanManageClassrooms => SelectedBuilding != null;
        public bool CanManageEquipment => SelectedClassroom != null;

        // Commands
        public ICommand LoadDataCommand { get; }
        public ICommand AddBuildingCommand { get; }
        public ICommand EditBuildingCommand { get; }
        public ICommand DeleteBuildingCommand { get; }
        public ICommand AddClassroomCommand { get; }
        public ICommand EditClassroomCommand { get; }
        public ICommand DeleteClassroomCommand { get; }
        public ICommand AddEquipmentCommand { get; }
        public ICommand EditEquipmentCommand { get; }
        public ICommand DeleteEquipmentCommand { get; }
        public ICommand AddEquipmentTypeCommand { get; }
        public ICommand EditEquipmentTypeCommand { get; }
        public ICommand DeleteEquipmentTypeCommand { get; }
        public ICommand RefreshCommand { get; }
        public ICommand LogoutCommand { get; }

        private async System.Threading.Tasks.Task LoadDataAsync()
        {
            if (IsLoading) return;
            IsLoading = true;
            StatusText = "Загрузка данных...";

            try
            {
                await Task.WhenAll(
                    LoadBuildingsAsync(),
                    LoadEquipmentTypesAsync()
                );
                StatusText = "Данные загружены";
            }
            catch (Exception ex)
            {
                StatusText = "Ошибка загрузки";
                _dialog.ShowMessage($"Ошибка загрузки данных: {ex.Message}", "Ошибка");
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async System.Threading.Tasks.Task LoadBuildingsAsync()
        {
            var buildings = await _api.GetBuildingsAsync();
            Buildings.Clear();
            foreach (var b in buildings)
                Buildings.Add(b);
        }

        private async System.Threading.Tasks.Task LoadClassroomsAsync(int buildingId)
        {
            var classrooms = await _api.GetClassroomsAsync(buildingId);
            Classrooms.Clear();
            foreach (var c in classrooms)
                Classrooms.Add(c);
        }

        private async System.Threading.Tasks.Task LoadEquipmentAsync(int classroomId)
        {
            var equipment = await _api.GetEquipmentAsync(classroomId);
            Equipment.Clear();
            foreach (var e in equipment)
                Equipment.Add(e);
        }

        private async System.Threading.Tasks.Task LoadEquipmentTypesAsync()
        {
            var types = await _api.GetEquipmentTypesAsync();
            EquipmentTypes.Clear();
            foreach (var t in types)
                EquipmentTypes.Add(t);
        }

        private async System.Threading.Tasks.Task AddBuildingAsync()
        {
            var vm = new BuildingViewModel(_api, _dialog);
            vm.IsEditMode = false;
            if (_dialog.ShowBuildingDialog(vm) == true)
            {
                await LoadBuildingsAsync();
                StatusText = "Здание добавлено";
            }
        }

        private async System.Threading.Tasks.Task EditBuildingAsync()
        {
            var vm = new BuildingViewModel(_api, _dialog, SelectedBuilding);
            vm.IsEditMode = true;
            if (_dialog.ShowBuildingDialog(vm) == true)
            {
                await LoadBuildingsAsync();
                StatusText = "Здание обновлено";
            }
        }

        private async System.Threading.Tasks.Task DeleteBuildingAsync()
        {
            if (!_dialog.ShowConfirmation($"Удалить здание '{SelectedBuilding.Name}'?", "Подтверждение"))
                return;

            await _api.DeleteBuildingAsync(SelectedBuilding.Id);
            await LoadBuildingsAsync();
            StatusText = "Здание удалено";
        }

        private async System.Threading.Tasks.Task AddClassroomAsync()
        {
            var vm = new ClassroomViewModel(_api, _dialog, SelectedBuilding.Id);
            vm.IsEditMode = false;
            if (_dialog.ShowClassroomDialog(vm) == true)
            {
                await LoadClassroomsAsync(SelectedBuilding.Id);
                StatusText = "Кабинет добавлен";
            }
        }

        private async System.Threading.Tasks.Task EditClassroomAsync()
        {
            var vm = new ClassroomViewModel(_api, _dialog, SelectedClassroom);
            vm.IsEditMode = true;
            if (_dialog.ShowClassroomDialog(vm) == true)
            {
                await LoadClassroomsAsync(SelectedBuilding.Id);
                StatusText = "Кабинет обновлён";
            }
        }

        private async System.Threading.Tasks.Task DeleteClassroomAsync()
        {
            if (!_dialog.ShowConfirmation($"Удалить кабинет {SelectedClassroom.Number}?", "Подтверждение"))
                return;

            await _api.DeleteClassroomAsync(SelectedClassroom.Id);
            await LoadClassroomsAsync(SelectedBuilding.Id);
            StatusText = "Кабинет удалён";
        }

        private async System.Threading.Tasks.Task AddEquipmentAsync()
        {
            var vm = new EquipmentViewModel(_api, _dialog, SelectedClassroom.Id);
            vm.IsEditMode = false;
            if (_dialog.ShowEquipmentDialog(vm) == true)
            {
                await LoadEquipmentAsync(SelectedClassroom.Id);
                StatusText = "Оборудование добавлено";
            }
        }

        private async System.Threading.Tasks.Task EditEquipmentAsync()
        {
            var vm = new EquipmentViewModel(_api, _dialog, SelectedEquipment);
            vm.IsEditMode = true;
            if (_dialog.ShowEquipmentDialog(vm) == true)
            {
                await LoadEquipmentAsync(SelectedClassroom.Id);
                StatusText = "Оборудование обновлено";
            }
        }

        private async System.Threading.Tasks.Task DeleteEquipmentAsync()
        {
            if (!_dialog.ShowConfirmation($"Удалить оборудование '{SelectedEquipment.InventoryNumber}'?", "Подтверждение"))
                return;

            await _api.DeleteEquipmentAsync(SelectedEquipment.Id);
            await LoadEquipmentAsync(SelectedClassroom.Id);
            StatusText = "Оборудование удалено";
        }

        private async System.Threading.Tasks.Task AddEquipmentTypeAsync()
        {
            var vm = new EquipmentTypeViewModel(_api, _dialog);
            vm.IsEditMode = false;
            if (_dialog.ShowEquipmentTypeDialog(vm) == true)
            {
                await LoadEquipmentTypesAsync();
                StatusText = "Тип оборудования добавлен";
            }
        }

        private async System.Threading.Tasks.Task EditEquipmentTypeAsync()
        {
            var vm = new EquipmentTypeViewModel(_api, _dialog, SelectedEquipmentType);
            vm.IsEditMode = true;
            if (_dialog.ShowEquipmentTypeDialog(vm) == true)
            {
                await LoadEquipmentTypesAsync();
                StatusText = "Тип оборудования обновлён";
            }
        }

        private async System.Threading.Tasks.Task DeleteEquipmentTypeAsync()
        {
            if (!_dialog.ShowConfirmation($"Удалить тип '{SelectedEquipmentType.Name}'?", "Подтверждение"))
                return;

            await _api.DeleteEquipmentTypeAsync(SelectedEquipmentType.Id);
            await LoadEquipmentTypesAsync();
            StatusText = "Тип оборудования удалён";
        }

        private void Logout()
        {
            _auth.Logout();
            var loginWindow = new LoginWindow();
            loginWindow.Show();
            Application.Current.Windows[0]?.Close();
        }
    }
}