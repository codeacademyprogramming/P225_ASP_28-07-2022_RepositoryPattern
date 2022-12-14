using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace P225Repository.DTOs.CategoryDTOs
{
    public class CategoryGetDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public bool IsMain { get; set; }
        public Nullable<int> ParentId { get; set; }
    }
}
