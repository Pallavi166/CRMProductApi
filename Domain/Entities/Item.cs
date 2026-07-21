using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    using System.ComponentModel.DataAnnotations;

   
        public class Item
        {
            public int Id { get; set; }

            public int ProductId { get; set; }

            [Required]
            public int Quantity { get; set; }

            // Navigation Property
            public Product Product { get; set; } = null!;
        }
    }

