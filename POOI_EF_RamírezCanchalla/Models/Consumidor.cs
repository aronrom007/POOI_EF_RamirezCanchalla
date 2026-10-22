using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace POOI_EF_RamírezCanchalla.Models
{
    public class Consumidor
    {
        [Display(Name = "DNI")]
        [Required(ErrorMessage = "El DNI es obligatorio")]
        [StringLength(8, MinimumLength = 8, ErrorMessage = "El DNI debe tener 8 digitos")]
        public string dnicont { get; set; }

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string nomcont { get; set; }

        [Display(Name = "Apellido")]
        [Required(ErrorMessage = "El apellido es obligatorio")]
        public string apecont { get; set; }

        [Display(Name = "Email")]
        public string emailcont { get; set; }

        [Display(Name = "Pais")]
        public string idpais { get; set; }
        public Consumidor() { }
        public Consumidor (string dnicont, string nomcont, string apecont, string emailcont, string idpais)
        {
            this.dnicont = dnicont;
            this.nomcont = nomcont;
            this.apecont = apecont;
            this.emailcont = emailcont;
            this.idpais = idpais;
        }
    }
}