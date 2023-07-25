namespace API.Utilities.Handlers;

public class GenerateHandler
{
    public static string GenerateNIK(string nik)
    {
        if(nik is null)
        {
            return "111111";
        }
        var newNIKGenerate = Convert.ToInt32(nik) + 1;
        return newNIKGenerate.ToString();
    }
}
