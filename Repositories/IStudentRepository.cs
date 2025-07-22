using StudentBackend.Models;
using StudentBackend.Dtos;

namespace StudentBackend.Repositories
{
    public interface IStudentRepository
    {
        OperationalResultDTO<IEnumerable<Student>> GetAll();
        OperationalResultDTO<Student> GetById(Guid id);
        OperationalResultDTO<Student> Add(Student student);
        OperationalResultDTO<Student> Update(Student student);
        OperationalResultDTO<bool> Delete(Guid id);
        OperationalResultDTO<bool> ExistsById(Guid id);
        OperationalResultDTO<bool> ExistsByIDNumber(string idNumber);
    }
} 