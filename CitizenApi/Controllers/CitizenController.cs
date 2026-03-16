using System.Runtime.Versioning;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/citizens")]
public class CitizenController : ControllerBase
{
    private List<Citizen> _citizensList;

    public CitizenController()
    {
        _citizensList = new();
        _citizensList.Add(new Citizen()
        {
            FirstName = "Juan",
            LastName = "Perez",
            Ci = 12345678,
            BloodType = "A+",
            PersonalAssets = "Casa, Auto"
        });   
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
        _citizensList.Add(newCitizen);
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

        _citizensList.Remove(citizenToDelete);
        return Ok("Ciudadano eliminado exitosamente");
    }

}