using System.Text.Json;

public class PersonalAssetService
{
    private HttpClient _httpClient;
    public PersonalAssetService(IConfiguration configuration)
    {
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri(configuration["EsternalServices:PersonalAssetsApi:BaseUrl"]);
    }

    public async Task<List<PersonalAsset>> GetPersonalAssets()
    {
        var response = await _httpClient.GetAsync("objects"); 

        if (response.IsSuccessStatusCode)
        {
            string responseContent = await response.Content.ReadAsStringAsync();
            
            // Configuramos para que no importe si es name o Name
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            
            return JsonSerializer.Deserialize<List<PersonalAsset>>(responseContent, options);
        }
        else 
        {
            throw new Exception($"Error API: {response.StatusCode}");
        }
    }
}