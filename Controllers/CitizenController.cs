using System.Runtime.Versioning;
using Microsoft.AspNetCore.Mvc;

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

    private string GetRandomBloodType()
    {
        string[] bloodTypes = { "A+", "A-", "B+", "B-", "AB+", "AB-", "O+", "O-" };
        Random random = new Random();
        int index = random.Next(bloodTypes.Length);
        return bloodTypes[index];
    }

    // Lógica para obtener todos los ciudadanos
    [HttpGet]
    public IActionResult Get()
    {

        return Ok(_citizensList);
    }

    // Lógica para obtener un ciudadano específico
    [HttpGet]
    [Route("{ci}")]
    public IActionResult Get(int ci)
    {
        Citizen foundCitizen = _citizensList.Find(c => c.Ci == ci);
        if (foundCitizen == null)
        {
            return Ok("Ciudadano no encontrado");
        }
        else
        {
        return Ok(foundCitizen);
        }
    }
    
    // Lógica para crear un nuevo ciudadano
    [HttpPost]
    public IActionResult Post([FromBody] Citizen newCitizen)
    {
        newCitizen.BloodType = GetRandomBloodType();
        //poner validacion de si faltan datos o si el ci ya existe
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

        citizenToUpdate.Ci = updatedCitizen.Ci;
        citizenToUpdate.FirstName = updatedCitizen.FirstName;
        citizenToUpdate.LastName = updatedCitizen.LastName;
        citizenToUpdate.BloodType = updatedCitizen.BloodType;
        citizenToUpdate.PersonalAssets = updatedCitizen.PersonalAssets;

        return Ok(_citizensList);
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