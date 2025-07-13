using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Project.Core.Dtos
{
    public class InteractionResult
    {
        public bool InteractionFound { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}
