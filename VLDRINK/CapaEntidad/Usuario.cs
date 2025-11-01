using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CapaEntidad
{
    public  class Usuario
    {
        public int IdUsuario { get; set; }
        public string Nombres { get; set; }

        public string Apellidos { get; set; }

        public string Correo   { get; set; }

        public bool Reestablecer { get; set; }

        public bool Activo { get; set; }

        [MaxLength(64)]
        public byte[] PasswordHash { get; set; }

        [MaxLength(32)]
        public byte[] PasswordSalt { get; set; }
    }
}
