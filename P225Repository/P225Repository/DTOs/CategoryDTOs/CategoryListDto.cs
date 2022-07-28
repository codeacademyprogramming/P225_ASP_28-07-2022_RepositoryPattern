using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace P225Repository.DTOs.CategoryDTOs
{
    public class CategoryListDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsMain { get; set; }

        public CategoryParentDto Parent { get; set; }
        public IEnumerable<CategoryChildrenDto> Children { get; set; }
    }

    public class CategoryParentDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsMain { get; set; }
    }

    public class CategoryChildrenDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsMain { get; set; }
    }
}
