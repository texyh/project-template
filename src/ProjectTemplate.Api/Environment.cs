using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectTemplate.Api
{
    public static class Environment
    {
        public static bool IsDevelopment()
        {
            var value = System.Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            if (value == null) 
            {
                return false;
            };

            return value == "Development";
        }

        public static void SetToDevelopment()
        {
            System.Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Development");
        }
    }
}
