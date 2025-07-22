using StudentBackend.Models;
using StudentBackend.Dtos;

namespace StudentBackend.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        // Thread-safe in-memory storage
        private static readonly List<Student> _students = new();
        private static readonly object _lock = new();

        public OperationalResultDTO<IEnumerable<Student>> GetAll()
        {
            try
            {
                lock (_lock)
                {
                    var students = _students.ToList();
                    return new OperationalResultDTO<IEnumerable<Student>>(
                        true, 
                        "Students retrieved successfully", 
                        students
                    );
                }
            }
            catch (Exception ex)
            {
                return new OperationalResultDTO<IEnumerable<Student>>(
                    false, 
                    $"Error retrieving students: {ex.Message}", 
                    null
                );
            }
        }

        public OperationalResultDTO<Student> GetById(Guid id)
        {
            try
            {
                lock (_lock)
                {
                    var student = _students.FirstOrDefault(s => s.Id == id);
                    if (student != null)
                    {
                        return new OperationalResultDTO<Student>(
                            true, 
                            "Student found successfully", 
                            student
                        );
                    }
                    return new OperationalResultDTO<Student>(
                        false, 
                        "Student not found", 
                        null
                    );
                }
            }
            catch (Exception ex)
            {
                return new OperationalResultDTO<Student>(
                    false, 
                    $"Error retrieving student: {ex.Message}", 
                    null
                );
            }
        }

        public OperationalResultDTO<Student> Add(Student student)
        {
            try
            {
                if (student == null)
                {
                    return new OperationalResultDTO<Student>(
                        false, 
                        "Student cannot be null", 
                        null
                    );
                }

                lock (_lock)
                {
                    // Check if student with same ID number already exists
                    if (_students.Any(s => s.IDNumber.Equals(student.IDNumber, StringComparison.OrdinalIgnoreCase)))
                    {
                        return new OperationalResultDTO<Student>(
                            false, 
                            "Student with this ID number already exists", 
                            null
                        );
                    }

                    _students.Add(student);
                    return new OperationalResultDTO<Student>(
                        true, 
                        "Student added successfully", 
                        student
                    );
                }
            }
            catch (Exception ex)
            {
                return new OperationalResultDTO<Student>(
                    false, 
                    $"Error adding student: {ex.Message}", 
                    null
                );
            }
        }

        public OperationalResultDTO<Student> Update(Student student)
        {
            try
            {
                if (student == null)
                {
                    return new OperationalResultDTO<Student>(
                        false, 
                        "Student cannot be null", 
                        null
                    );
                }

                lock (_lock)
                {
                    var index = _students.FindIndex(s => s.Id == student.Id);
                    if (index >= 0)
                    {
                        // Check if another student with same ID number exists
                        var existingWithSameIdNumber = _students.FirstOrDefault(s => 
                            s.Id != student.Id && 
                            s.IDNumber.Equals(student.IDNumber, StringComparison.OrdinalIgnoreCase));
                        
                        if (existingWithSameIdNumber != null)
                        {
                            return new OperationalResultDTO<Student>(
                                false, 
                                "Another student with this ID number already exists", 
                                null
                            );
                        }

                        _students[index] = student;
                        return new OperationalResultDTO<Student>(
                            true, 
                            "Student updated successfully", 
                            student
                        );
                    }
                    return new OperationalResultDTO<Student>(
                        false, 
                        "Student not found", 
                        null
                    );
                }
            }
            catch (Exception ex)
            {
                return new OperationalResultDTO<Student>(
                    false, 
                    $"Error updating student: {ex.Message}", 
                    null
                );
            }
        }

        public OperationalResultDTO<bool> Delete(Guid id)
        {
            try
            {
                lock (_lock)
                {
                    var toRemove = _students.FirstOrDefault(s => s.Id == id);
                    if (toRemove != null)
                    {
                        _students.Remove(toRemove);
                        return new OperationalResultDTO<bool>(
                            true, 
                            "Student deleted successfully", 
                            true
                        );
                    }
                    return new OperationalResultDTO<bool>(
                        false, 
                        "Student not found", 
                        false
                    );
                }
            }
            catch (Exception ex)
            {
                return new OperationalResultDTO<bool>(
                    false, 
                    $"Error deleting student: {ex.Message}", 
                    false
                );
            }
        }

        public OperationalResultDTO<bool> ExistsById(Guid id)
        {
            try
            {
                lock (_lock)
                {
                    var exists = _students.Any(s => s.Id == id);
                    return new OperationalResultDTO<bool>(
                        true, 
                        exists ? "Student exists" : "Student does not exist", 
                        exists
                    );
                }
            }
            catch (Exception ex)
            {
                return new OperationalResultDTO<bool>(
                    false, 
                    $"Error checking student existence: {ex.Message}", 
                    false
                );
            }
        }

        public OperationalResultDTO<bool> ExistsByIDNumber(string idNumber)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(idNumber))
                {
                    return new OperationalResultDTO<bool>(
                        false, 
                        "ID number cannot be null or empty", 
                        false
                    );
                }

                lock (_lock)
                {
                    var exists = _students.Any(s => s.IDNumber.Equals(idNumber, StringComparison.OrdinalIgnoreCase));
                    return new OperationalResultDTO<bool>(
                        true, 
                        exists ? "Student with ID number exists" : "Student with ID number does not exist", 
                        exists
                    );
                }
            }
            catch (Exception ex)
            {
                return new OperationalResultDTO<bool>(
                    false, 
                    $"Error checking student existence by ID number: {ex.Message}", 
                    false
                );
            }
        }
    }
} 