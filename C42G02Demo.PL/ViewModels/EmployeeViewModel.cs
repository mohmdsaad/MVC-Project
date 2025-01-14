﻿using C42G02Demo.DAL.Model;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;
using Microsoft.AspNetCore.Http;

namespace C42G02Demo.PL.ViewModels
{
    public class EmployeeViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required !")]
        [MaxLength(50, ErrorMessage = "Max length of name is 50 Chars")]
        [MinLength(5, ErrorMessage = "Min length of name is 5 Chars")]
        public string Name { get; set; }


        [Range(22, 35, ErrorMessage = "Age Range is (22 to 35)")]
        [Display(Name = "Age")]
        public int age { get; set; }

        [RegularExpression("^[0-9]{1,3}-[a-zA-Z]{5,10}-[a-zA-Z]{4,10}-[a-zA-Z]{5,10}$",
            ErrorMessage = "Address Must Be Like 123-Street-City-Country")]
        public string Address { get; set; }

        [DataType(DataType.Currency)]
        public decimal Salary { get; set; }

        [Display(Name = "Activity")]
        public bool IsActive { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Hiring Date")]
        public DateTime HireDate { get; set; }
        public IFormFile Image { get; set; }
        public string ImageName { get; set; }

        [ForeignKey("Department")]
        public int? DepartmentId { get; set; }
        [InverseProperty("Employees")]
        public Department Department { get; set; }
    }
}
