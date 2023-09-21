using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Services
{
    public class ApplicationService
    {
        public static void CloseApplication()
        {
            Environment.Exit(0);
        }
    }
}
