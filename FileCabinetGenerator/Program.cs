﻿
namespace FileCabinetGenerator
{
    public static class Program
    {
        private static Generator _generator = new ();
        public static void Main()
        {
            var isCorrectSettings = false;
            while (!isCorrectSettings)
            {
                try
                {
                    Console.Write("$ FileCabinetGenerator.exe ");
                    var settings = Console.ReadLine()?.Split(' ');
                    _generator.SetSettings(settings);
                    _generator.Generate();
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