using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        // Используем HTTP клиент для работы с Qdrant через HTTPS прокси
        var httpClient = new HttpClient();
        var apiKey = Environment.GetEnvironmentVariable("QDRANT_KEY") ?? "your-api-key";
        httpClient.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", apiKey);

        var baseUrl = "https://qdrant.steel-designer-engineer.com";

        try
        {
            // Проверка существующих коллекций
            var response = await httpClient.GetAsync($"{baseUrl}/collections");
            var content = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Collections response: {content}");
            
            // Создание коллекции через REST API
            var collectionData = new
            {
                vectors = new
                {
                    size = 4,
                    distance = "Cosine"
                }
            };
            
            var json = JsonSerializer.Serialize(collectionData);
            var contentData = new StringContent(json, Encoding.UTF8, "application/json");
            
            var createResponse = await httpClient.PutAsync($"{baseUrl}/collections/my_collection_rest", contentData);
            Console.WriteLine($"Create collection status: {createResponse.StatusCode}");
            Console.WriteLine($"Create collection response: {await createResponse.Content.ReadAsStringAsync()}");
            
            // Подготовка точек для вставки
            var pointsData = new
            {
                points = new[] {
                    new {
                        id = 1,
                        vector = new float[] { 0.1f, 0.2f, 0.3f, 0.4f },
                        payload = new { 
                            city = "Москва",
                            country = "Россия", 
                            population = 12600000 
                        }
                    }
                }
            };
            
            // Добавление точек
            var pointsJson = JsonSerializer.Serialize(pointsData);
            var pointsContent = new StringContent(pointsJson, Encoding.UTF8, "application/json");
            
            // Используем PostAsync вместо несуществующего putAsync
            var upsertResponse = await httpClient.PostAsync($"{baseUrl}/collections/my_collection_rest/points", pointsContent);
            Console.WriteLine($"Upsert points status: {upsertResponse.StatusCode}");
            Console.WriteLine($"Upsert response: {await upsertResponse.Content.ReadAsStringAsync()}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            if (ex.InnerException != null)
                Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
        }
    }
}