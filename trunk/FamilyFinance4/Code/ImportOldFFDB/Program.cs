using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImportOldFFDB
{
    class Program
    {
        static void Main(string[] args)
        {
            Importer import = new Importer();
            import.import();
        }
    }
}
