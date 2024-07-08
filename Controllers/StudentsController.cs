using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using DapperApi.Services;
using DapperApı.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace DapperApı.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly string connectionString;
        //IConfiguration is given to to the constructor so that we can read the ConnectionString in appsettings.json which we needed.
        private readonly IConfiguration _configuration;

        private readonly IDatabaseService<Students> _databaseService;

        public StudentsController(IConfiguration configuration, IDatabaseService<Students> databaseService)
        {
            _configuration = configuration;
            //In here I add ! to state that our connection String will not return null
            _databaseService = databaseService!;
        }
        //CREATING STUDENT TABLE

        [HttpPost("CreateStudentTable")]
        public IActionResult CreateStudentTable()
        {
            try
            {
                string tableSchema = @"
                studentId INT PRIMARY KEY IDENTITY,
                name NVARCHAR(50) NOT NULL,
                email NVARCHAR(50) NOT NULL
            ";

                _databaseService.CreateTable("students", tableSchema);

                return Ok("Students table created successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("AlterStudentsTable")]
        public IActionResult AlterTeacherTable([FromQuery] string columnName, [FromQuery] string columnType)
          {
        try
        {
            _databaseService.AlterTable("students", columnName, columnType);
            return Ok("Students table altered successfully");
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
           }
        //CREATING STUDENT

        [HttpPost]
        public ActionResult Post([FromBody] StudentsDto studentsDto)
        {

            try
            {
                var student = new Students
                {
                    name = studentsDto.name,
                    email = studentsDto.email
                };

                var newStudent = _databaseService.Add(student);
                if (newStudent != null)
                {
                    return Ok(newStudent);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "We have an exception: " + ex.Message);
            }

            return BadRequest();
        }

        //DELETE STUDENT BY ID

        [HttpDelete("{id}")]
        public IActionResult DeleteStudent(int id)
        {
            try
            {
                bool isDeleted = _databaseService.Delete(id);
                if (isDeleted)
                {
                    return Ok($"Student with ID {id} deleted successfully");
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "We have an exception: " + ex.Message);

            }

        }
    }
}