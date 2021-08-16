using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Devop.Models.Content
{
    public class MedicalPlan
    {
        public string PlanId { get; set; }
        public string PlanName { get; set; }
        public virtual ICollection<MedicalAid> GetMedicalAids { get; set; }

    }
}
