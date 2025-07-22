using StudentBackend.Dtos;
using StudentBackend.Models;
using StudentBackend.Repositories;

namespace StudentBackend.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _repo;

        public StudentService(IStudentRepository repo)
        {
            _repo = repo;
        }

        private static StudentDTO ToDto(Student s) => new()
        {
            Id = s.Id,
            Name = s.Name,
            IDNumber = s.IDNumber,
            Email = s.Email,
            EnrollmentDate = s.EnrollmentDate
        };

        private static Student ToModel(StudentDTO dto) => new()
        {
            Name = dto.Name,
            IDNumber = dto.IDNumber,
            Email = dto.Email,
            EnrollmentDate = dto.EnrollmentDate
        };

        private static void UpdateModel(Student model, StudentDTO dto)
        {
            model.Name = dto.Name;
            model.IDNumber = dto.IDNumber;
            model.Email = dto.Email;
            model.EnrollmentDate = dto.EnrollmentDate;
        }

        public OperationalResultDTO<IEnumerable<StudentDTO>> GetAll()
        {
            try
            {
                var result = _repo.GetAll();
                if (!result.Success)
                {
                    return new OperationalResultDTO<IEnumerable<StudentDTO>>(
                        false,
                        result.Message,
                        null
                    );
                }

                var studentDtos = result.Data?.Select(ToDto) ?? Enumerable.Empty<StudentDTO>();
                return new OperationalResultDTO<IEnumerable<StudentDTO>>(
                    true,
                    "Students retrieved successfully",
                    studentDtos
                );
            }
            catch (Exception ex)
            {
                return new OperationalResultDTO<IEnumerable<StudentDTO>>(
                    false,
                    $"Service error retrieving students: {ex.Message}",
                    null
                );
            }
        }

        public OperationalResultDTO<StudentDTO> GetById(Guid id)
        {
            try
            {
                var result = _repo.GetById(id);
                if (!result.Success)
                {
                    return new OperationalResultDTO<StudentDTO>(
                        false,
                        result.Message,
                        null
                    );
                }

                return new OperationalResultDTO<StudentDTO>(
                    true,
                    "Student retrieved successfully",
                    ToDto(result.Data!)
                );
            }
            catch (Exception ex)
            {
                return new OperationalResultDTO<StudentDTO>(
                    false,
                    $"Service error retrieving student: {ex.Message}",
                    null
                );
            }
        }

        public OperationalResultDTO<StudentDTO> Create(StudentDTO dto)
        {
            try
            {
                if (dto == null)
                {
                    return new OperationalResultDTO<StudentDTO>(
                        false,
                        "Student data cannot be null",
                        null
                    );
                }

                // Check if student with same ID number already exists
                var existsResult = _repo.ExistsByIDNumber(dto.IDNumber);
                if (!existsResult.Success)
                {
                    return new OperationalResultDTO<StudentDTO>(
                        false,
                        $"Error checking student existence: {existsResult.Message}",
                        null
                    );
                }

                if (existsResult.Data)
                {
                    return new OperationalResultDTO<StudentDTO>(
                        false,
                        "Student with this ID number already exists",
                        null
                    );
                }

                var student = ToModel(dto);

                // Ensure unique Id (Guid.NewGuid generated in model constructor but check anyway)
                if (student.Id == Guid.Empty)
                {
                    student.Id = Guid.NewGuid();
                }

                var existsByIdResult = _repo.ExistsById(student.Id);
                if (existsByIdResult.Success && existsByIdResult.Data)
                {
                    student.Id = Guid.NewGuid();
                }

                var addResult = _repo.Add(student);
                if (!addResult.Success)
                {
                    return new OperationalResultDTO<StudentDTO>(
                        false,
                        addResult.Message,
                        null
                    );
                }

                return new OperationalResultDTO<StudentDTO>(
                    true,
                    "Student created successfully",
                    ToDto(addResult.Data!)
                );
            }
            catch (Exception ex)
            {
                return new OperationalResultDTO<StudentDTO>(
                    false,
                    $"Service error creating student: {ex.Message}",
                    null
                );
            }
        }

        public OperationalResultDTO<StudentDTO> Update(Guid id, StudentDTO dto)
        {
            try
            {
                if (dto == null)
                {
                    return new OperationalResultDTO<StudentDTO>(
                        false,
                        "Student data cannot be null",
                        null
                    );
                }

                var existingResult = _repo.GetById(id);
                if (!existingResult.Success)
                {
                    return new OperationalResultDTO<StudentDTO>(
                        false,
                        existingResult.Message,
                        null
                    );
                }

                var existing = existingResult.Data!;

                // Check if another student with same ID number exists (if ID number is being changed)
                if (existing.IDNumber != dto.IDNumber)
                {
                    var existsByIdNumberResult = _repo.ExistsByIDNumber(dto.IDNumber);
                    if (!existsByIdNumberResult.Success)
                    {
                        return new OperationalResultDTO<StudentDTO>(
                            false,
                            $"Error checking ID number existence: {existsByIdNumberResult.Message}",
                            null
                        );
                    }

                    if (existsByIdNumberResult.Data)
                    {
                        return new OperationalResultDTO<StudentDTO>(
                            false,
                            "Another student with this ID number already exists",
                            null
                        );
                    }
                }

                UpdateModel(existing, dto);
                var updateResult = _repo.Update(existing);
                if (!updateResult.Success)
                {
                    return new OperationalResultDTO<StudentDTO>(
                        false,
                        updateResult.Message,
                        null
                    );
                }

                return new OperationalResultDTO<StudentDTO>(
                    true,
                    "Student updated successfully",
                    ToDto(updateResult.Data!)
                );
            }
            catch (Exception ex)
            {
                return new OperationalResultDTO<StudentDTO>(
                    false,
                    $"Service error updating student: {ex.Message}",
                    null
                );
            }
        }

        public OperationalResultDTO<bool> Delete(Guid id)
        {
            try
            {
                var existsResult = _repo.ExistsById(id);
                if (!existsResult.Success)
                {
                    return new OperationalResultDTO<bool>(
                        false,
                        $"Error checking student existence: {existsResult.Message}",
                        false
                    );
                }

                if (!existsResult.Data)
                {
                    return new OperationalResultDTO<bool>(
                        false,
                        "Student not found",
                        false
                    );
                }

                var deleteResult = _repo.Delete(id);
                if (!deleteResult.Success)
                {
                    return new OperationalResultDTO<bool>(
                        false,
                        deleteResult.Message,
                        false
                    );
                }

                return new OperationalResultDTO<bool>(
                    true,
                    "Student deleted successfully",
                    true
                );
            }
            catch (Exception ex)
            {
                return new OperationalResultDTO<bool>(
                    false,
                    $"Service error deleting student: {ex.Message}",
                    false
                );
            }
        }
    }
}