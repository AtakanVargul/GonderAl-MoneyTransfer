using Backend.MoneyTransfer.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Backend.MoneyTransfer.Domain.Entities.BaseModels;

public class AuditableEntity : BaseEntity
{
    public DateTimeOffset CreateDate { get; set; }

    public DateTimeOffset? UpdateDate { get; set; }

    [MaxLength(50)]
    [Required]
    public string CreatedBy { get; set; }

    [MaxLength(50)]
    public string LastModifiedBy { get; set; }

    public RecordStatus RecordStatus { get; set; }
}