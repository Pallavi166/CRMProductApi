using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
  
        public class CreateProductDto
        {
            [Required]
            [MaxLength(255)]
            public string ProductName { get; set; } = string.Empty;

            [Required]
            public string CreatedBy { get; set; } = string.Empty;
        }
    }

