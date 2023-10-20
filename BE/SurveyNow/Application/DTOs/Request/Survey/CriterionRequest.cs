using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Request.Survey
{
    public class CriterionRequest
    {
        public int? MinAge { get; set; }

        public int? MaxAge { get; set; }

        public bool ExpertParticipant { get; set; }

        public virtual ICollection<GenderCriterion> GenderCriteria { get; set; } = new List<GenderCriterion>();

        public virtual ICollection<FieldCriterion> FieldCriteria { get; set; } = new List<FieldCriterion>();

        public virtual ICollection<AreaCriterion> AreaCriteria { get; set; } = new List<AreaCriterion>();

        public virtual ICollection<RelationshipCriterion> RelationshipCriteria { get; set; } = new List<RelationshipCriterion>();
    }
}
