using ShieldMyRide.Context;
using ShieldMyRide.Models;
using ShieldMyRide.Repositary.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShieldMyRide.Repositary
{
    public class OfficersAssignmentRepository : IOfficersAssignmentRepository
    {
        private readonly MyDBContext _context;

        public OfficersAssignmentRepository(MyDBContext context)
        {
            _context = context;
        }

        public async Task<OfficersAssignment> CreateAssignmentAsync(OfficersAssignment assignment)
        {
            _context.Officers.Add(assignment);
            await _context.SaveChangesAsync();
            return assignment;
        }

        public async Task<List<OfficersAssignment>> GetAssignmentsByOfficerAsync(string officerId)
        {
            return await _context.Officers
                .Where(a => a.OfficerId == officerId)
                .OrderByDescending(a => a.ActionTime)
                .ToListAsync();
        }

        public async Task<List<OfficersAssignment>> GetAllAssignmentsAsync()
        {
            return await _context.Officers
                .OrderByDescending(a => a.ActionTime)
                .ToListAsync();
        }
    }
}
