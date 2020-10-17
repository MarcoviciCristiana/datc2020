using System.Collections.Generic;
using System.Threading.Tasks;
using marcoviciTema4;
public interface IStudentRepository 
{
    Task<List<StudentEntity>> GetAllStudents();

    Task<string> CreateNewStudent(StudentEntity student);
    Task<string> DeleteStudent(StudentEntity student);
    Task<string> EditStudent(StudentEntity student);
} 