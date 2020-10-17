using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace marcoviciapi
{
    public class Student
    {
        public int Id {get;set;}
        public string Nume {get;set;}
        public int An {get;set;}
        public string Facultate {get;set;}

    }
}
