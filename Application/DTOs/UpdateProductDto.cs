using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs
{
    
        public class UpdateProductDto
        {
            [Required]
            public int Id { get; set; }

            [Required]
            [MaxLength(255)]
            public string ProductName { get; set; } = string.Empty;

            public string? ModifiedBy { get; set; }
        }
    }
