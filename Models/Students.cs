using System.ComponentModel.DataAnnotations;

namespace DapperApı;

public class Students
{
    [Key]
    public int studentId{set;get;}

    [Required]
    public string? name{set;get;}

    [Required]
    public string? email {set;get;}


}
