using WebApplication1.DTOs;

namespace WebApplication1.Services;

public interface IPatientService
{
    Task<IEnumerable<PatientDto>> GetPatientsAsync(string? search);

    Task AssignBedAsync(string pesel, CreateBedAssignmentDto dto);
}