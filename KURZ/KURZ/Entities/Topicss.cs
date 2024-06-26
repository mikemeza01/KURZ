﻿using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace KURZ.Entities
{
    public class Topicss
    {
        [Key]
        [Display(Name = "Id del Tema")]
        public int ID_TOPIC { get; set; }

        [Display(Name = "Nombre del Tema")]
        [StringLength(50, ErrorMessage = "El nombre del tema no puede exceder de 100 carácteres")]
        public string NAME { get; set; }

        [Display(Name = "Descripción")]
        [StringLength(50, ErrorMessage = "El nombre del tema no puede exceder de 1000 carácteres")]
        public string DESCRIPTION { get; set; }

        [Display(Name = "ID de Categoría")]
        public int ID_CATEGORY { get; set; }

        [Display(Name = "ID de la subcategoría")]
        public int ID_SUBCATEGORY { get; set; }
    }
}
