using SchoolEquipmentApp.Helpers;
using SchoolEquipmentApp.Models;
using SchoolEquipmentApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace SchoolEquipmentApp.ViewModels
{
    public class EquipmentViewModel : ViewModelBase
    {
        private readonly ApiService _api;
        private readonly DialogService _dialog;

        private int _id;
        private int _classroomId;
        private int _equipmentTypeId;
        private string _inventoryNumber = string.Empty;
        private string _manufacturer = string.Empty;
        private string _model = string.Empty;
        private string _serialNumber = string.Empty;
        private DateTime? _purchaseDate;
        private DateTime? _warrantyEnd;
        private string _status = "Working";
        private string _notes = string.Empty;
        private bool _isEditMode = false;

        private ObservableCollection<EquipmentType> _equipmentTypes = new ObservableCollection<EquipmentType>();
        private ObservableCollection<EquipmentSpecification> _specifications = new ObservableCollection<EquipmentSpecification>();

        private List<string> _statuses = new List<string> { "Working", "Repair", "Decommissioned" };

        public EquipmentViewModel(ApiService api, DialogService dialog, int classroomId)
        {
            _api = api;
            _dialog = dialog;
            _classroomId = classroomId;
            _purchaseDate = DateTime.Now;

            SaveCommand = new RelayCommand(async () => await SaveAsync(), () => CanSave);
            CancelCommand = new RelayCommand(() => _dialog.ShowMessage("Отмена", "Информация"));
            AddSpecificationCommand = new RelayCommand(AddSpecification);
            RemoveSpecificationCommand = new RelayCommand<EquipmentSpecification>(RemoveSpecification);

            LoadEquipmentTypes();
        }

        public EquipmentViewModel(ApiService api, DialogService dialog, Equipment equipment)
            : this(api, dialog, equipment.ClassroomId)
        {
            _id = equipment.Id;
            _equipmentTypeId = equipment.EquipmentTypeId;
            _inventoryNumber = equipment.InventoryNumber;
            _manufacturer = equipment.Manufacturer;
            _model = equipment.Model;
            _serialNumber = equipment.SerialNumber;
            _purchaseDate = equipment.PurchaseDate;
            _warrantyEnd = equipment.WarrantyEnd;
            _status = equipment.Status;
            _notes = equipment.Notes;
            _isEditMode = true;

            foreach (var spec in equipment.Specifications)
                _specifications.Add(spec);
        }

        public int Id => _id;
        public bool IsEditMode
        {
            get => _isEditMode;
            set => SetProperty(ref _isEditMode, value);
        }

        public int ClassroomId
        {
            get => _classroomId;
            set => SetProperty(ref _classroomId, value);
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

        public string InventoryNumber
        {
            get => _inventoryNumber;
            set
            {
                SetProperty(ref _inventoryNumber, value);
                OnPropertyChanged(nameof(CanSave));
            }
        }

        public string Manufacturer
        {
            get => _manufacturer;
            set => SetProperty(ref _manufacturer, value);
        }

        public string Model
        {
            get => _model;
            set => SetProperty(ref _model, value);
        }

        public string SerialNumber
        {
            get => _serialNumber;
            set => SetProperty(ref _serialNumber, value);
        }

        public DateTime? PurchaseDate
        {
            get => _purchaseDate;
            set => SetProperty(ref _purchaseDate, value);
        }

        public DateTime? WarrantyEnd
        {
            get => _warrantyEnd;
            set => SetProperty(ref _warrantyEnd, value);
        }

        public string Status
        {
            get => _status;
            set => SetProperty(ref _status, value);
        }

        public string Notes
        {
            get => _notes;
            set => SetProperty(ref _notes, value);
        }

        public ObservableCollection<EquipmentType> EquipmentTypes
        {
            get => _equipmentTypes;
            set => SetProperty(ref _equipmentTypes, value);
        }

        public ObservableCollection<EquipmentSpecification> Specifications
        {
            get => _specifications;
            set => SetProperty(ref _specifications, value);
        }

        public List<string> Statuses
        {
            get => _statuses;
            set => SetProperty(ref _statuses, value);
        }

        public bool CanSave => !string.IsNullOrWhiteSpace(InventoryNumber) && EquipmentTypeId > 0;

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand AddSpecificationCommand { get; }
        public ICommand RemoveSpecificationCommand { get; }

        private async void LoadEquipmentTypes()
        {
            var types = await _api.GetEquipmentTypesAsync();
            EquipmentTypes.Clear();
            foreach (var t in types)
                EquipmentTypes.Add(t);
        }

        private void AddSpecification()
        {
            Specifications.Add(new EquipmentSpecification
            {
                Value = string.Empty,
                CustomName = "Новая характеристика"
            });
        }

        private void RemoveSpecification(EquipmentSpecification spec)
        {
            if (spec != null && _dialog.ShowConfirmation("Удалить характеристику?", "Подтверждение"))
                Specifications.Remove(spec);
        }

        private async System.Threading.Tasks.Task SaveAsync()
        {
            try
            {
                var data = new
                {
                    ClassroomId,
                    EquipmentTypeId,
                    InventoryNumber,
                    Manufacturer,
                    Model,
                    SerialNumber,
                    PurchaseDate,
                    WarrantyEnd,
                    Status,
                    Notes,
                    Specifications = Specifications.Select(s => new
                    {
                        CategoryId = s.CategoryId,
                        Value = s.Value,
                        CustomName = s.CustomName
                    }).ToList()
                };

                if (IsEditMode)
                {
                    await _api.UpdateEquipmentAsync(Id, data);
                }
                else
                {
                    await _api.CreateEquipmentAsync(data);
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