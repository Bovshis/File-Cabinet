using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Dynamic;
using System.Linq;

namespace FileCabinetApp
{
    public class AdvancedSearch
    {
        public static readonly string[] Commands = {
            "help", "exit", "stat", "create", "update", "export", "import", "delete", "purge", "select",
        };

        private string command;

        public AdvancedSearch(string command)
        {
            this.command = command;
        }

        public IList<string> GetSimilarCommand()
        {
            var distances = new int[Commands.Length];
            for (var i = 0; i < Commands.Length; i++)
            {
                distances[i] = LevenshteinDistance(this.command, Commands[i]);
            }

            var min = GetMinimums(distances);
            return min.Select(i => Commands[i]).ToList();
        }

        private static int LevenshteinDistance(string firstWord, string secondWord)
        {
            var n = firstWord.Length + 1;
            var m = secondWord.Length + 1;
            var matrixD = new int[n, m];

            const int deletionCost = 1;
            const int insertionCost = 1;

            for (var i = 0; i < n; i++)
            {
                matrixD[i, 0] = i;
            }

            for (var j = 0; j < m; j++)
            {
                matrixD[0, j] = j;
            }

            for (var i = 1; i < n; i++)
            {
                for (var j = 1; j < m; j++)
                {
                    var substitutionCost = firstWord[i - 1] == secondWord[j - 1] ? 0 : 1;

                    matrixD[i, j] = Minimum(matrixD[i - 1, j] + deletionCost,
                        matrixD[i, j - 1] + insertionCost,
                        matrixD[i - 1, j - 1] + substitutionCost);
                }
            }

            return matrixD[n - 1, m - 1];
        }

        private static IList<int> GetMinimums(params int[] values)
        {
            var min = values.Min();
            var list = new List<int>();
            for (var i = 0; i < values.Length; i++)
            {
                if (values[i] == min)
                {
                    list.Add(i);
                }
            }

            return list;
        }

        private static int Minimum(int a, int b, int c) => (a = a < b ? a : b) < c ? a : c;
    }
}
