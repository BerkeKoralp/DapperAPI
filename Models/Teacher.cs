using System.ComponentModel.DataAnnotations;

public class Teacher
{

    [Key]
    public int TeacherId { get; set; }
    [Required]
    public string? FirstName { get; set; }
    [Required]
    public string? LastName { get; set; }
    [Required]
    public string? Subject { get; set; }
    [Required]
    public string? Email { get; set; }
    [Required]
    public string? PhoneNumber { get; set; } // New field
}