using System.Runtime.Versioning;
using Microsoft.AspNetCore.Mvc;
using Serilog;

[ApiController]
[Route("api/citizens")]
public class CitizenController : ControllerBase
{

    private List<Citizen> _citizensList;
    private readonly IConfiguration _configuration;
    public CitizenController(IConfiguration configuration)
    {
        _citizensList = new();
        _configuration = configuration;

        List<string[]> data = CSVHelper.ReadCSV(configuration["Data:Location"]);

        for(int i =0; i<data.Count; i++)
        {
            Citizen citizen = new Citizen
            {
                Ci = int.Parse(data[i][0]),
                FirstName = data[i][1],
                LastName = data[i][2],
                BloodType = data[i][3],
                PersonalAssets = data[i][4]
            };
            _citizensList.Add(citizen);

        }
    }

    // Método para generar un tipo de sangre aleatorio
    private string GetRandomBloodType()
    {
        string[] bloodTypes = { "A+", "A-", "B+", "B-", "AB+", "AB-", "O+", "O-" };
        Random random = new Random();
        int index = random.Next(bloodTypes.Length);
        return bloodTypes[index];
    }

    // Método para generar un PersonalAsset aleatorio
    private string GetRandomPersonalAsset(List<PersonalAsset> personalAssets)
    {
        if(personalAssets == null || personalAssets.Count == 0)
        {
            return null;
        }
        else
        {
            Random random = new Random();
            int index = random.Next(personalAssets.Count);
            return personalAssets[index].Name;
        }
    }

    // Lógica para obtener todos los ciudadanos
    [HttpGet]
    public IActionResult Get()
    {
        if (_citizensList.Count == 0)
        {
            Log.Debug("No se encontraron ciudadanos en la lista");
            return Ok("No hay ciudadanos registrados");
        }
        else
        {
            return Ok(_citizensList);
        }
    }

    // Lógica para obtener un ciudadano específico
    [HttpGet]
    [Route("{ci}")]
    public IActionResult Get(int ci)
    {
        Citizen foundCitizen = _citizensList.Find(c => c.Ci == ci);
        if (foundCitizen == null)
        {
            Log.Error("No se encontró el ciudadano con CI: {Ci}", ci);
            return Ok("Ciudadano no encontrado");
        }
        else
        {
            return Ok(foundCitizen);
        }
    }
    
    // Lógica para crear un nuevo ciudadano
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Citizen newCitizen)
    {
        // Validaciones previas para el nuevo ciudadano
        if(newCitizen.Ci == 0 || string.IsNullOrEmpty(newCitizen.FirstName) || string.IsNullOrEmpty(newCitizen.LastName))
        {
            return Ok("Faltan datos obligatorios para crear el ciudadano");
        }
        if(_citizensList.Exists(c => c.Ci == newCitizen.Ci))
        {
            return Ok("Ya existe un ciudadano con el mismo CI");
        }

        //BloodType asignado aleatoriamente
        newCitizen.BloodType = GetRandomBloodType();

        //PersonalAsset asignado aleatoriamente
        try
        {
            PersonalAssetService personalAssetService = new PersonalAssetService(_configuration);
            var personalAssets = personalAssetService.GetPersonalAssets();
            newCitizen.PersonalAssets = GetRandomPersonalAsset(personalAssets.Result);
        }
        catch(Exception ex)
        {
            Log.Error("Error al obtener los personal assets: {Message}", ex.Message);
            return Ok($"Error al obtener los personal assets: {ex.Message}");
        }

        Log.Debug("Creando un nuevo ciudadano con CI: {Ci}, Nombre: {FirstName} {LastName}", newCitizen.Ci, newCitizen.FirstName, newCitizen.LastName);
        Log.Information("Se agrego un nuevo ciudadano");

        _citizensList.Add(newCitizen);
        List<string[]> data = new List<string[]>();
        for (int i = 0; i < _citizensList.Count; i++)
        {
            string[] citizenData = new string[]
            {
                _citizensList[i].Ci.ToString(),
                _citizensList[i].FirstName,
                _citizensList[i].LastName,
                _citizensList[i].BloodType,
                _citizensList[i].PersonalAssets
            };
            data.Add(citizenData);
        }
        CSVHelper.WriteCSV(_configuration["Data:Location"], data);
        return Ok("Ciudadano creado exitosamente");
    }
    
    // Lógica para actualizar un ciudadano existente
    [HttpPut]
    [Route("{ci}")]
    public IActionResult Put([FromRoute] int ci, [FromBody] Citizen updatedCitizen)
    {
        Citizen citizenToUpdate = _citizensList.Find(c => c.Ci == ci);
        
        if (citizenToUpdate == null)
        {
            return Ok("Ciudadano no encontrado");
        }
        else
        {
            citizenToUpdate.Ci = updatedCitizen.Ci;
            citizenToUpdate.FirstName = updatedCitizen.FirstName;
            citizenToUpdate.LastName = updatedCitizen.LastName;

            CSVHelper.WriteCSV(_configuration["Data:Location"], _citizensList.Select(c => new string[]
            {
                c.Ci.ToString(),
                c.FirstName,
                c.LastName,
                c.BloodType,
                c.PersonalAssets
            }).ToList());
            return Ok(_citizensList);
        }
    }
    
    // Lógica para eliminar un ciudadano
    [HttpDelete]
    [Route("{ci}")]
    public IActionResult Delete([FromRoute] int ci)
    {
        Citizen citizenToDelete = _citizensList.Find(c => c.Ci == ci);
        if (citizenToDelete == null)
        {
            return Ok("No se borró el ciudadano, no se encontró");
        }
        else
        {
            _citizensList.Remove(citizenToDelete);
            CSVHelper.WriteCSV(_configuration["Data:Location"], _citizensList.Select(c => new string[]
            {
                c.Ci.ToString(),
                c.FirstName,
                c.LastName,
                c.BloodType,
                c.PersonalAssets
            }).ToList());
        }   


        return Ok("Ciudadano eliminado exitosamente");
    }

}