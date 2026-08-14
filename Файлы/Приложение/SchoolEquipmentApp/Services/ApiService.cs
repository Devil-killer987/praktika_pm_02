using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using SchoolEquipmentApp.Models;
using System.Windows.Data;

namespace SchoolEquipmentApp.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private string _token = string.Empty;
        private string _baseUrl = "https://localhost:5001/api/";

        public ApiService()
        {
            _httpClient = new HttpClient();
            _httpClient.Timeout = TimeSpan.FromSeconds(30);
        }

        public void SetBaseUrl(string url)
        {
            _baseUrl = url.EndsWith("/") ? url : url + "/";
        }

        public void SetToken(string token)
        {
            _token = token;
            _httpClient.DefaultRequestHeaders.Clear();
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            }
        }

        private async Task<T> GetAsync<T>(string endpoint)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_baseUrl}{endpoint}");
                var content = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Ошибка API: {response.StatusCode} - {content}");
                }

                return JsonConvert.DeserializeObject<T>(content);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при выполнении запроса: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }
        }

        private async Task<T> PostAsync<T>(string endpoint, object data)
        {
            try
            {
                var json = JsonConvert.SerializeObject(data);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync($"{_baseUrl}{endpoint}", content);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Ошибка API: {response.StatusCode} - {responseContent}");
                }

                return JsonConvert.DeserializeObject<T>(responseContent);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при выполнении запроса: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }
        }

        private async Task<T> PutAsync<T>(string endpoint, object data)
        {
            try
            {
                var json = JsonConvert.SerializeObject(data);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"{_baseUrl}{endpoint}", content);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Ошибка API: {response.StatusCode} - {responseContent}");
                }

                return JsonConvert.DeserializeObject<T>(responseContent);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при выполнении запроса: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }
        }

        private async Task DeleteAsync(string endpoint)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"{_baseUrl}{endpoint}");
                var content = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Ошибка API: {response.StatusCode} - {content}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при выполнении запроса: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }
        }

        // === Auth ===
        public async Task<object> LoginAsync(string login, string password)
        {
            var data = new { login, password };
            return await PostAsync<object>("Auth/login", data);
        }

        // === Buildings ===
        public async Task<List<Building>> GetBuildingsAsync()
        {
            return await GetAsync<List<Building>>("Buildings");
        }

        public async Task<Building> GetBuildingAsync(int id)
        {
            return await GetAsync<Building>($"Buildings/{id}");
        }

        public async Task<Building> CreateBuildingAsync(object data)
        {
            return await PostAsync<Building>("Buildings", data);
        }

        public async Task<Building> UpdateBuildingAsync(int id, object data)
        {
            return await PutAsync<Building>($"Buildings/{id}", data);
        }

        public async Task DeleteBuildingAsync(int id)
        {
            await DeleteAsync($"Buildings/{id}");
        }

        // === Classrooms ===
        public async Task<List<Classroom>> GetClassroomsAsync(int? buildingId = null)
        {
            var url = "Classrooms";
            if (buildingId.HasValue)
                url += $"?buildingId={buildingId.Value}";
            return await GetAsync<List<Classroom>>(url);
        }

        public async Task<Classroom> GetClassroomAsync(int id)
        {
            return await GetAsync<Classroom>($"Classrooms/{id}");
        }

        public async Task<Classroom> CreateClassroomAsync(object data)
        {
            return await PostAsync<Classroom>("Classrooms", data);
        }

        public async Task<Classroom> UpdateClassroomAsync(int id, object data)
        {
            return await PutAsync<Classroom>($"Classrooms/{id}", data);
        }

        public async Task DeleteClassroomAsync(int id)
        {
            await DeleteAsync($"Classrooms/{id}");
        }

        // === Equipment Types ===
        public async Task<List<EquipmentType>> GetEquipmentTypesAsync()
        {
            return await GetAsync<List<EquipmentType>>("EquipmentTypes");
        }

        public async Task<EquipmentType> GetEquipmentTypeAsync(int id)
        {
            return await GetAsync<EquipmentType>($"EquipmentTypes/{id}");
        }

        public async Task<EquipmentType> CreateEquipmentTypeAsync(object data)
        {
            return await PostAsync<EquipmentType>("EquipmentTypes", data);
        }

        public async Task<EquipmentType> UpdateEquipmentTypeAsync(int id, object data)
        {
            return await PutAsync<EquipmentType>($"EquipmentTypes/{id}", data);
        }

        public async Task DeleteEquipmentTypeAsync(int id)
        {
            await DeleteAsync($"EquipmentTypes/{id}");
        }

        // === Equipment ===
        public async Task<List<Equipment>> GetEquipmentAsync(int? classroomId = null, int? typeId = null, string status = null, string search = null)
        {
            var url = "Equipment?";
            if (classroomId.HasValue) url += $"classroomId={classroomId.Value}&";
            if (typeId.HasValue) url += $"equipmentTypeId={typeId.Value}&";
            if (!string.IsNullOrEmpty(status)) url += $"status={status}&";
            if (!string.IsNullOrEmpty(search)) url += $"searchTerm={search}&";
            url = url.TrimEnd('&', '?');
            return await GetAsync<List<Equipment>>(url);
        }

        public async Task<Equipment> GetEquipmentAsync(int id)
        {
            return await GetAsync<Equipment>($"Equipment/{id}");
        }

        public async Task<Equipment> CreateEquipmentAsync(object data)
        {
            return await PostAsync<Equipment>("Equipment", data);
        }

        public async Task<Equipment> UpdateEquipmentAsync(int id, object data)
        {
            return await PutAsync<Equipment>($"Equipment/{id}", data);
        }

        public async Task DeleteEquipmentAsync(int id)
        {
            await DeleteAsync($"Equipment/{id}");
        }

        // === Specifications ===
        public async Task<List<SpecificationCategory>> GetSpecificationCategoriesAsync(int? equipmentTypeId = null)
        {
            var url = "Specifications/categories";
            if (equipmentTypeId.HasValue)
                url += $"?equipmentTypeId={equipmentTypeId.Value}";
            return await GetAsync<List<SpecificationCategory>>(url);
        }

        public async Task<SpecificationCategory> CreateSpecificationCategoryAsync(object data)
        {
            return await PostAsync<SpecificationCategory>("Specifications/categories", data);
        }

        public async Task DeleteSpecificationCategoryAsync(int id)
        {
            await DeleteAsync($"Specifications/categories/{id}");
        }
    }
}