using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace marcoviciTema4
{
    public class StudentRepository : InterfaceStudentRepository 
    {
        private string _connectionString;
        private CloudTableClient _tableClient;
        private CloudTable _studentsTable;
        private List<StudentEntity> students;
        public StudentRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetValue<string>("AzureStorageAccountConnectionString"); 
            Task.Run(async () => { await InitializeTable(); })
            .GetAwaiter()
            .GetResult(); 
        }
        public async Task<string> CreateNewStudent(StudentEntity student)
        {
            await GetAllStudents(); 
            int pos = 0;

            foreach(var obiect in students)
            {
                if(obiect.PartitionKey.Equals(student.PartitionKey) && obiect.RowKey.Equals(student.RowKey))
                {
                    pos = 1; 
                    break;
                }
                else 
                    pos = 0; 
            }

            if(pos == 1)
                return "Already existing";
            else
            {
                var insert = TableOperation.Insert(student); 
                await _studentsTable.ExecuteAsync(insert);

                return "Added";
            }
        }

        public async Task<string> DeleteStudent(StudentEntity student)
        {
            await GetAllStudents();
            int pos = 0;

            foreach(var obiect in students)
            {
                if(obiect.PartitionKey.Equals(student.PartitionKey) && obiect.RowKey.Equals(student.RowKey))
                {
                    pos = 0;
                    var delete = TableOperation.Delete(new TableEntity(student.PartitionKey, student.RowKey) { ETag = "*" }); 
                    await _studentsTable.ExecuteAsync(delete);

                    students.Remove(obiect); 
                    break;
                }
                else 
                    pos = 1;
            }

            if(pos == 1)
                return "Doesn't exist";
            else
                return "Wiped";
        }

        public async Task<string> EditStudent(StudentEntity student)
        {
            await GetAllStudents();
            int pos = 0;

            foreach(var obiect in students)
            {
                if(obiect.PartitionKey.Equals(student.PartitionKey) && obiect.RowKey.Equals(student.RowKey)) 
                {
                    pos = 0;
                    var delete = TableOperation.Delete(new TableEntity(student.PartitionKey, student.RowKey) { ETag = "*" }); 
                    await _studentsTable.ExecuteAsync(delete);
                    students.Remove(obiect);

                    var insert = TableOperation.Insert(student); 
                    await _studentsTable.ExecuteAsync(insert);
                    students.Add(obiect);
                    break;
                }
                else 
                    pos = 1;
            }

            if(pos == 1)
                return "Doesn't exist";
            else
                return "Updated";
        }

        public async Task<List<StudentEntity>> GetAllStudents()
        {
            students = new List<StudentEntity>();

            TableQuery<StudentEntity> query = new TableQuery<StudentEntity>();

            TableContinuationToken token = null; 
            do{
                TableQuerySegment<StudentEntity> resultSegment = await _studentsTable.ExecuteQuerySegmentedAsync(query, token);
                token = resultSegment.ContinuationToken;

                students.AddRange(resultSegment.Results); 

            }while(token != null);

            return students;
        }

        private async Task InitializeTable()
        {
            var account = CloudStorageAccount.Parse(_connectionString);
            _tableClient = account.CreateCloudTableClient();

            _studentsTable = _tableClient.GetTableReference("studenti");

            await _studentsTable.CreateIfNotExistsAsync();

        }
    }
} 