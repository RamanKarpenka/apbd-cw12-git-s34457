using WebApplication1.Data;
using WebApplication1.DTOs;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;
using WebApplication1.Exceptions;

namespace WebApplication1.Services;

public class PatientService : IPatientService
{
    private readonly _2019sbdContext _context;

    public PatientService(_2019sbdContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<PatientDto>> GetPatientsAsync(string? search)
    {
        var query = _context.Patients.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(p =>
                EF.Functions.Like(p.FirstName, $"%{search}%") ||
                EF.Functions.Like(p.LastName, $"%{search}%"));
        }

        return await query
            .Select(p => new PatientDto
            {
                Pesel = p.Pesel,
                FirstName = p.FirstName,
                LastName = p.LastName,
                Age = p.Age,
                Sex = p.Sex
            })
            .ToListAsync();
    }
    
    public async Task AssignBedAsync(string pesel, CreateBedAssignmentDto dto)
    {
        var patient = await _context.Patients
            .FirstOrDefaultAsync(p => p.Pesel == pesel);

        if (patient == null)
            throw new PatientNotFoundException();

        var bed = await _context.Beds
            .FirstOrDefaultAsync(b => b.Id == dto.BedId);

        if (bed == null)
            throw new BedNotFoundException();

        var occupied = await _context.BedAssignments
            .AnyAsync(b =>
                b.BedId == dto.BedId &&
                b.To == null);

        if (occupied)
            throw new BedOccupiedException();

        var assignment = new BedAssignment
        {
            PatientPesel = pesel,
            BedId = dto.BedId,
            From = dto.From,
            To = dto.To
        };

        _context.BedAssignments.Add(assignment);

        await _context.SaveChangesAsync();
    }
}