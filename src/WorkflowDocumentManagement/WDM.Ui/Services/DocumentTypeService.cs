using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WDM.Domain.Dtos;
using WDM.Domain.Entities;
using WDM.Domain.Services;

namespace WDM.Ui.Services
{
    public class DocumentTypeService : IDocumentTypeService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<DocumentTypeService> _logger;

        public DocumentTypeService(HttpClient httpClient, ILogger<DocumentTypeService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<IEnumerable<DocumentType>> GetAllDocumentTypeAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("documenttype");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<IEnumerable<DocumentType>>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }) ?? new List<DocumentType>();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all document types");
            }
            return new List<DocumentType>();
        }

        public async Task<DocumentType> GetDocumentTypeByIdAsync(Guid id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"documenttype/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<DocumentType>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting document type by id: {DocumentTypeId}", id);
            }
            return null;
        }

        public async Task<bool> CreateDocumentTypeAsync(CreateDocumentTypeDto documentTypeDto)
        {
            try
            {
                var json = JsonSerializer.Serialize(documentTypeDto);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("documenttype", content);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating document type");
                return false;
            }
        }

        public async Task<bool> UpdateDocumentTypeAsync(Guid id, UpdateDocumentTypeDto documentTypeDto)
        {
            try
            {
                var json = JsonSerializer.Serialize(documentTypeDto);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"documenttype/{id}", content);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating document type: {DocumentTypeId}", id);
                return false;
            }
        }

        public async Task<bool> DeleteDocumentTypeAsync(Guid id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"documenttype/{id}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting document type: {DocumentTypeId}", id);
                return false;
            }
        }
    }
}
