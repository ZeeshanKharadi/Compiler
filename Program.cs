using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler
{
    class Program
    {
        static void Main(string[] args)
        {
            LexicalAnalyser la = new LexicalAnalyser();
            la.splitwords();
            List.display();
           // List.writeIntoFile();

            Console.ReadKey();
        }
    }
}
