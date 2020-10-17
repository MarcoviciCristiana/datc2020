using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Linq;


namespace marcoviciapi.Controllers{
    [ApiController]
    [Route("[controller]")]

    public class StudentController:ControllerBase
    {
        [HttpGet]
        public List<Student> Get()
        {
            return StudentRepo.Students;
        }
          [HttpGet("details/{id}")]
        public Student Get(int id)
        {
            return StudentRepo.Students.FirstOrDefault(s => s.Id==id);
        }

        [HttpPost]
        public string Post([FromBody] Student student){
            try
            {
                 StudentRepo.Students.Add(student);
                 return "succes";
            }
            catch (System.Exception e)
            {
                return "eroare"+ e.Message;
                throw;
            }
           ;
        }
    }

}