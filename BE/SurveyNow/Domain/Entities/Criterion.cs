using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Enums;

namespace Domain.Entities;

public class Criterion
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    public int? MinAge { get; set; }

    public int? MaxAge { get; set; }

    public bool ExpertParticipant { get; set; }

    public long SurveyId { get; set; }
    [ForeignKey(nameof(SurveyId))] public virtual Survey Survey { get; set; } = null!;

    public virtual ICollection<GenderCriterion> GenderCriteria { get; set; } = new List<GenderCriterion>();

    public virtual ICollection<FieldCriterion> FieldCriteria { get; set; } = new List<FieldCriterion>();

    public virtual ICollection<AreaCriterion> AreaCriteria { get; set; } = new List<AreaCriterion>();

    public virtual ICollection<RelationshipCriterion> RelationshipCriteria { get; set; } =
        new List<RelationshipCriterion>();
}