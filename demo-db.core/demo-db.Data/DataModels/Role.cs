using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace demo_db.Data.DataModels
{
    public class Role
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(35)]
        public string Name { get; set; }

        public ICollection<User> Users { get; set; }
    }
}
