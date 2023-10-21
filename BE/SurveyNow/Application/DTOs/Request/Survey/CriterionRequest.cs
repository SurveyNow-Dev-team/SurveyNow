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

        public ICollection<Gender>? GenderCriteria { get; set; }

        public ICollection<long>? FieldCriteria { get; set; }

        public ICollection<long>? AreaCriteria { get; set; }

        public ICollection<RelationshipStatus>? RelationshipCriteria { get; set; }
    }
}
