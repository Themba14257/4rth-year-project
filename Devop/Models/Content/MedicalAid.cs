using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Devop.Models.Content
{
    public class MedicalAid
    {
        public string MedId { get; set; }
        public string MedicalName { get; set; }

        public virtual MedicalPlan MedicalPlan { get; set; }


    }
}
