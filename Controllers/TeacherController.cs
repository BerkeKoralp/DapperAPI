// Controllers/TeacherController.cs
using DapperApi.Services;
using DapperApı.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

[ApiController]
[Route("api/[controller]")]
public class TeacherController : ControllerBase
{
    private readonly IDatabaseService<Teacher> _databaseService;

    public TeacherController(IDatabaseService<Teacher> teacherService)
    {
        _databaseService = teacherService;
    }

    [HttpPost("CreateTeacherTable")]
    public IActionResult CreateTeacherTable()
    {
        try
        {
            string tableSchema = @"
                TeacherId INT PRIMARY KEY IDENTITY,
                FirstName NVARCHAR(50) NOT NULL,
                LastName NVARCHAR(50) NOT NULL,
                Subject NVARCHAR(50) NOT NULL,
                Email NVARCHAR(50) NOT NULL
            ";

            _databaseService.CreateTable("teacher", tableSchema);

            return Ok("Teacher table created successfully");
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPost("AlterTeacherTable")]
    public IActionResult AlterTeacherTable([FromQuery] string columnName, [FromQuery] string columnType)
    {
        try
        {
            _databaseService.AlterTable("teacher", columnName, columnType);
            return Ok("Teacher table altered successfully");
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPost]
    public ActionResult Post([FromBody] TeacherDto teacherDto)
    {
        try
        {
            var teacher = new Teacher
            {
                FirstName = teacherDto.FirstName,
                LastName = teacherDto.LastName,
                Subject = teacherDto.Subject,
                Email = teacherDto.Email
            };

            var newTeacher = _databaseService.Add(teacher);
            if (newTeacher != null)
            {
                return Ok(newTeacher);
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, "We have an exception: " + ex.Message);
        }

        return BadRequest();
    }

    [HttpGet]
    public ActionResult<IEnumerable<Teacher>> GetAll()
    {
        try
        {
            var teachers = _databaseService.GetAll();
            return Ok(teachers);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "We have an exception: " + ex.Message);
        }
    }

    [HttpGet("{id}")]
    public ActionResult<Teacher> GetById(int id)
    {
        try
        {
            var teacher = _databaseService.GetById(id);
            if (teacher == null)
            {
                return NotFound();
            }
            return Ok(teacher);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "We have an exception: " + ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        try
        {
            bool isDeleted = _databaseService.Delete(id);
            if (isDeleted)
            {
                return Ok($"Teacher with ID {id} deleted successfully");
            }
            return NotFound();
        }
        catch (Exception ex)
        {
            return StatusCode(500, "We have an exception: " + ex.Message);
        }
    }
}
