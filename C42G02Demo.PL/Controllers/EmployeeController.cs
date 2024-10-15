using AutoMapper;
using C42G02Demo.BLL.Interfacies;
using C42G02Demo.DAL.Model;
using C42G02Demo.PL.Helpers;
using C42G02Demo.PL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace C42G02Demo.PL.Controllers
{
	[Authorize]
	public class EmployeeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _env;
        private readonly IMapper _mapper;

        public EmployeeController(IUnitOfWork unitOfWork,
                                  IWebHostEnvironment env,
                                  IMapper mapper) //Ask CLR To Create Object Of EmployeeRepository
        {
            _unitOfWork = unitOfWork;
            _env = env;
            _mapper = mapper;
        }
        public  async Task<IActionResult> Index(string SearchValue)
        {
            IEnumerable<Employee> employees;
            
            if (string.IsNullOrEmpty(SearchValue))
                 employees =  await _unitOfWork.EmployeeRepository.GetAllAsync();
            else
                employees = _unitOfWork.EmployeeRepository.GetEmployeesByName(SearchValue);

            var mappedEmployees = _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeViewModel>>(employees);
            return View(mappedEmployees);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Departments = await _unitOfWork.DepartmentRepository.GetAllAsync(); // Populate the ViewBag
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EmployeeViewModel employeeVM)
        {
            if (ModelState.IsValid)
            {
                #region Manual Mapping
                /// Manual Mapping
                //var mappedEmployee = new Employee
                //{
                //    Name = employeeVM.Name,
                //    age = employeeVM.age,
                //    Address = employeeVM.Address,
                //    IsActive = employeeVM.IsActive,
                //    Email = employeeVM.Email,
                //    HireDate = employeeVM.HireDate,
                //    PhoneNumber = employeeVM.PhoneNumber,
                //    Salary = employeeVM.Salary,
                //    Id = employeeVM.Id,
                //    Department = employeeVM.Department,
                //    DepartmentId = employeeVM.DepartmentId
                //}; 
                #endregion

                employeeVM.ImageName = DocumentSettings.UploadFile(employeeVM.Image, "Images");

                var mappedEmployee = _mapper.Map<EmployeeViewModel,Employee>(employeeVM);

                await _unitOfWork.EmployeeRepository.AddAsync(mappedEmployee);

                await _unitOfWork.CompleteAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(employeeVM);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id, string viewName = "Details")
        {
            if (!id.HasValue)
                return BadRequest();

            var employees = await _unitOfWork.EmployeeRepository.GetByIdAsync(id.Value);

            if (employees == null)
                return NotFound();

            var mappedEmployee = _mapper.Map<Employee, EmployeeViewModel>(employees);

            return View(viewName, mappedEmployee);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (!id.HasValue)
                return BadRequest();

            var employees = await _unitOfWork.EmployeeRepository.GetByIdAsync(id.Value);
            if (employees== null)
                return NotFound();
            
            var mappedEmployee = _mapper.Map<Employee, EmployeeViewModel>(employees);
            
            ViewBag.Departments = await _unitOfWork.DepartmentRepository.GetAllAsync(); // Populate the ViewBag
            return View(mappedEmployee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int? id, EmployeeViewModel employeeVM)
        {
            if (id != employeeVM.Id)
                return BadRequest();

            if (!ModelState.IsValid)
            {
                return View(employeeVM);
            }
                
            try
            {
                if(employeeVM.Image is not null)
                    employeeVM.ImageName = DocumentSettings.UploadFile(employeeVM.Image, "Images");

                var mappedEmployees = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);
                
                _unitOfWork.EmployeeRepository.Update(mappedEmployees);
                await _unitOfWork.CompleteAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // 1. Log Exception
                // 2. Friendly Msg

                if (_env.IsDevelopment())
                    ModelState.AddModelError(string.Empty, ex.Message);
                else
                    ModelState.AddModelError(string.Empty, "An Error Occured During Update Employee Operation");

                return View(employeeVM);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            return await Details(id, "Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(EmployeeViewModel employeeVM)
        {
            try
            {
                var mappedEmployee = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);
                
                _unitOfWork.EmployeeRepository.Delete(mappedEmployee);
                
                var res = await _unitOfWork.CompleteAsync();
                
                if (res > 0)
                    DocumentSettings.DeleteFile(employeeVM.ImageName, "Images");
                
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // 1. Log Exception
                // 2. Friendly Msg

                if (_env.IsDevelopment())
                    ModelState.AddModelError(string.Empty, ex.Message);
                else
                    ModelState.AddModelError(string.Empty, "An Error Occured During Update Employee Operation");

                return View(employeeVM);
            }
        }
    }
}
