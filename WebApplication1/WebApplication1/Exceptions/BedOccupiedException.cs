namespace WebApplication1.Exceptions;

public class BedOccupiedException : Exception
{
    public BedOccupiedException()
        : base("Bed is occupied")
    {
    }
}