using ShieldMyRide.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShieldMyRide.Repositary.Interfaces
{
    public interface IOfficersAssignmentRepository
    {
        Task<OfficersAssignment> CreateAssignmentAsync(OfficersAssignment assignment);
        Task<List<OfficersAssignment>> GetAssignmentsByOfficerAsync(string officerId);
        Task<List<OfficersAssignment>> GetAllAssignmentsAsync();
    }
}
