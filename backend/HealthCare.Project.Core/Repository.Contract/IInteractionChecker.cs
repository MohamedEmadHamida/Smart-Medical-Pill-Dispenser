using HealthCare.Project.Core.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Project.Core.Repository.Contract
{
    public interface IInteractionChecker
    {

        Task<InteractionResult> CheckInteractionAsync(string medicine1, string medicine2);


    }
}
