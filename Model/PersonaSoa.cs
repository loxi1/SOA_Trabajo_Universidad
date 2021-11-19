using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Transfer;

namespace WebApi.Model
{
    public class PersonaSoa
    {
        public async Task<IActionResult> Guardar(Persona persona)
        {
            SOIContext db = new SOIContext();
            Persona pers = new Persona();
            pers.Nombres = persona.Nombres;
            pers.Apellidos = persona.Apellidos;
            pers.Direccion = persona.Direccion;
            pers.CodigoPostal = persona.Direccion;
            pers.FechaNacimiento = persona.FechaNacimiento;
            db.Entry(pers).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            await db.SaveChangesAsync();
            return (IActionResult)pers;
        }
    }
}
