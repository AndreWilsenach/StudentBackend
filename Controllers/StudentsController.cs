using Microsoft.AspNetCore.Mvc;
using StudentBackend.Dtos;
using StudentBackend.Services;

namespace StudentBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentService _service;

        public StudentsController(IStudentService service)
        {
            _service = service;
        }

        /// <summary>
        /// Get all students.
        /// </summary>
        [HttpGet]
        public ActionResult<OperationalResultDTO<IEnumerable<StudentDTO>>> GetAll()
        {
            try
            {
                var result = _service.GetAll();
                
                if (result.Success)
                {
                    return Ok(result);
                }
                
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        /// <summary>
        /// Get a student by unique identifier.
        /// </summary>
        [HttpGet("{id:guid}")]
        public ActionResult<OperationalResultDTO<StudentDTO>> GetById(Guid id)
        {
            try
            {
                var result = _service.GetById(id);
                
                if (result.Success)
                {
                    return Ok(result);
                }
                
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        /// <summary>
        /// Create a new student.
        /// </summary>
        [HttpPost]
        public ActionResult<OperationalResultDTO<StudentDTO>> Create(StudentDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var validationErrors = string.Join("; ", ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage));

                    var validationResult = new OperationalResultDTO<StudentDTO>(
                        false,
                        $"Validation failed: {validationErrors}",
                        null
                    );
                    return BadRequest(validationResult);
                }

                var result = _service.Create(dto);
                
                if (result.Success)
                {
                    return Ok(result);
                }
                
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        /// <summary>
        /// Update an existing student.
        /// </summary>
        [HttpPut("{id:guid}")]
        public ActionResult<OperationalResultDTO<StudentDTO>> Update(Guid id, StudentDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var validationErrors = string.Join("; ", ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage));

                    var validationResult = new OperationalResultDTO<StudentDTO>(
                        false,
                        $"Validation failed: {validationErrors}",
                        null
                    );
                    return BadRequest(validationResult);
                }

                var result = _service.Update(id, dto);
                
                if (result.Success)
                {
                    return Ok(result);
                }
                
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        /// <summary>
        /// Delete a student.
        /// </summary>
        [HttpDelete("{id:guid}")]
        public ActionResult<OperationalResultDTO<bool>> Delete(Guid id)
        {
            try
            {
                var result = _service.Delete(id);
                
                if (result.Success)
                {
                    return Ok(result);
                }
                
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }
    }
} 