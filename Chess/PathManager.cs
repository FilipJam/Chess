using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilipsChess
{
    internal class PathManager
    {
        public static string GetMyDirectory() // maintains file management
        {
            string path = Directory.GetCurrentDirectory();
            // path backs from current directory untill it reaches Chess directory
            while (!path.EndsWith("Chess"))
                path = Directory.GetParent(path).FullName;
            return path;
        }
    }
}
