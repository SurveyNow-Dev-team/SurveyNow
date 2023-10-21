using Domain.Entities;
using Domain.Enums;
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

        public virtual ICollection<Gender> GenderCriteria { get; set; } = new List<Gender>();

        public virtual ICollection<long> FieldCriteria { get; set; } = new List<long>();

        public virtual ICollection<long> AreaCriteria { get; set; } = new List<long>();

        public virtual ICollection<RelationshipStatus> RelationshipCriteria { get; set; } = new List<RelationshipStatus>();
    }
}
