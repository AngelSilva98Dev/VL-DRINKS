using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace CapaEntidad
{
    public  class Cliente
    {
        public int IdCliente { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Correo { get; set; }
        public bool Reestablecer { get; set; }
        public DateTime FechaRegistro { get; set; } 
        public string Telefono { get; set; }
        public string Direccion { get; set; }
        public bool EsMayorDeEdad { get; set; }


        [MaxLength(20)]
        public byte[] PasswordHash { get; set; }

        [MaxLength(32)]
        public byte[] PasswordSalt { get; set; }
    }
}
