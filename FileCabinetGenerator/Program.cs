
namespace FileCabinetGenerator
{
    public static class Program
    {
        private static readonly Generator generator = new ();
        public static void Main()
        {
            var isCorrectSettings = false;
            while (!isCorrectSettings)
            {
                try
                {
                    Console.Write("$ FileCabinetGenerator.exe ");
                    var settings = Console.ReadLine()?.Split(' ');
                    generator.SetSettings(settings);
                    generator.Generate();
                    isCorrectSettings = true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }
    }
}