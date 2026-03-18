using System.Text.Json.Serialization;
public class PersonalAsset
{
    [JsonPropertyName("id")]
    public string Id { get; set; }
    [JsonPropertyName("name")]
    public string Name { get; set; }
    [JsonPropertyName("attributes")]
    public PersonalAssetsAtributes Atributes { get; set; }
}

public class PersonalAssetsAtributes
{
    public string Color { get; set; }
    public string Capacity { get; set; }
}