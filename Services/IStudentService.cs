using StudentBackend.Dtos;
using StudentBackend.Models;

namespace StudentBackend.Services
{
    public interface IStudentService
    {
        OperationalResultDTO<IEnumerable<StudentDTO>> GetAll();
        OperationalResultDTO<StudentDTO> GetById(Guid id);
        OperationalResultDTO<StudentDTO> Create(StudentDTO dto);
        OperationalResultDTO<StudentDTO> Update(Guid id, StudentDTO dto);
        OperationalResultDTO<bool> Delete(Guid id);
    }
} 