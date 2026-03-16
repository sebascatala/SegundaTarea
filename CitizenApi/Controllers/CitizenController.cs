using System.Runtime.Versioning;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/citizens")]
public class CitizenController : ControllerBase
{
    // Lógica para obtener todos los ciudadanos
    [HttpGet]
    public void Get()
    {
    }

    // Lógica para obtener un ciudadano específico
    [HttpGet]
    [Route("{id}")]
    public void Get(int id)
    {
    }
    
    // Lógica para crear un nuevo ciudadano
    [HttpPost]
    public void Post()
    {
        
    }
    
    // Lógica para actualizar un ciudadano existente
    [HttpPut]
    public void Put()
    {
    }
    
    // Lógica para eliminar un ciudadano
    [HttpDelete]
    [Route("{id}")]
    public void Delete(int id)
    {
    }

}