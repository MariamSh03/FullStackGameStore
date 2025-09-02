using System.ComponentModel.DataAnnotations;

namespace AdminPanel.Bll.DTOs;
public class PublisherDto
{
    [Required(ErrorMessage = "Company name is required.")]
    [StringLength(100, ErrorMessage = "Company name cannot exceed 100 characters.")]
    public string CompanyName { get; set; }

    public string HomePage { get; set; }

    [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
    public string Description { get; set; }
}
