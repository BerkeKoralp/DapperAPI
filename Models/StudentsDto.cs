//DATA TRANSFER OBJECT MODEL FOR STUDENTS
//IT SHOULD ONLY CONTAIN THE REQUESTED FIELDS FROM THE USER

using System.ComponentModel.DataAnnotations;

namespace DapperApı.Models;

public class StudentsDto {

    [Required]
    public string? name{set;get;}

    [Required]
    public string? email {set;get;}

    [Required]
    public string? password {set;get;}
}