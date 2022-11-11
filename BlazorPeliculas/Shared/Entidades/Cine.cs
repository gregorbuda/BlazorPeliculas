using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorPeliculas.Shared.Entidades
{
    public class Cine
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El campo {0} requerido")]
        public string Nombre { get; set; }
        public List<CinesPeliculas> CinePelicula { get; set; }
    }
}
