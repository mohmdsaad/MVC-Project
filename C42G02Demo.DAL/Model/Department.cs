using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C42G02Demo.DAL.Model
{
    public class Department
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Code Is Required!")] // .Net 5 
        public string Code { get; set; }
        [Required(ErrorMessage = "Name Is Required!")]
        public string Name { get; set; }
        [Display(Name = "Date Of Creation")]
        public DateTime DateOfCreation { get; set; }
        
        [InverseProperty("Department")]
        public ICollection<Employee> Employees { get; set; } = new HashSet<Employee>();
    }
}
