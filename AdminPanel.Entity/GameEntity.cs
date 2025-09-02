using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdminPanel.Entity;

public class GameEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(255)]
    public string Name { get; set; }

    [Required]
    [MaxLength(100)]
    public string Key { get; set; }

    public string? Description { get; set; }

    [Required]
    [Range(0, double.MaxValue)]
    public double Price { get; set; }

    [Required]
    [Range(0, int.MaxValue)]
    public int UnitInStock { get; set; }

    [Required]
    [Range(0, 100)]
    public int Discount { get; set; }

    [Required]
    public Guid PublisherId { get; set; }

    public bool IsDeleted { get; set; }
}