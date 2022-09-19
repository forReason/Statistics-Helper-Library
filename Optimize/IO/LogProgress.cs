using Optimize.AverageNS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optimize.IO
{
    internal class LogProgress
    {
        internal LogProgress(string path)
        {
            Path = path;
            Table = new CSV_Helper_Project.Table();
            Table.SetColumnNames(new string[] {"Low Bound", "Low Bound Loss", "Current Optimum","Current Optimum Loss", "Upper Bound","Upper Bound Loss" });
        }
        private string Path { get; set; }
        private CSV_Helper_Project.Table Table { get; set; }
        public void AddEntry(BenchmarkResult[] restrainBracket)
        {
            int index = Table.AppendEmptyRecord();
            Table.SetCell( "Low Bound", index, restrainBracket[0].Value.ToString());
            Table.SetCell("Low Bound Loss", index, restrainBracket[0].Score.ToString());
            Table.SetCell("Current Optimum", index, restrainBracket[1].Value.ToString());
            Table.SetCell("Current Optimum Loss", index, restrainBracket[1].Score.ToString());
            Table.SetCell("Upper Bound", index, restrainBracket[2].Value.ToString());
            Table.SetCell("Upper Bound Loss", index, restrainBracket[2].Score.ToString());
            Table.WriteTableToFile(Path);
        }
    }
}
