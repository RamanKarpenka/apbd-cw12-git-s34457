namespace WebApplication1.Exceptions;

public class PatientNotFoundException : Exception
{
    public PatientNotFoundException()
        : base("Patient not found")
    {
    }
}