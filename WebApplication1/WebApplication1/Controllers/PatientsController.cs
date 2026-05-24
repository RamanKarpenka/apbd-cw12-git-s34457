using WebApplication1.Services;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.DTOs;
using WebApplication1.Exceptions;

namespace WebApplication1.Controllers;

[ApiController]
[Route("api/patients")]
public class PatientsController : ControllerBase
{
    private readonly IPatientService _patientService;

    public PatientsController(IPatientService patientService)
    {
        _patientService = patientService;
    }

    [HttpGet]
    public async Task<IActionResult> GetPatients([FromQuery] string? search)
    {
        var patients = await _patientService.GetPatientsAsync(search);

        return Ok(patients);
    }
    
    [HttpPost("{pesel}/bedassignments")]
    public async Task<IActionResult> AssignBed(
        string pesel,
        CreateBedAssignmentDto dto)
    {
        try
        {
            await _patientService.AssignBedAsync(pesel, dto);

            return Ok(new
            {
                message = "Bed assigned successfully"
            });
        }
        catch (PatientNotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (BedNotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (BedOccupiedException e)
        {
            return BadRequest(e.Message);
        }
    }
}