namespace WebApplication1.Exceptions;

public class BedNotFoundException : Exception
{
    public BedNotFoundException()
        : base("Bed not found")
    {
    }
}