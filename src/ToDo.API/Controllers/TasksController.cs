using Microsoft.AspNetCore.Mvc;
using ToDo.Application.Dtos;
using ToDo.Application.Services;

namespace ToDo.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        /// <summary>
        /// GET /api/tasks - Obter todas as tarefas
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<TaskResponseDto>>> GetAllTasks()
        {
            try
            {
                var tasks = await _taskService.GetAllAsync();
                return Ok(tasks);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    new { message = "Erro ao buscar tarefas", error = ex.Message });
            }
        }

        /// <summary>
        /// GET /api/tasks/{id} - Obter tarefa por ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<TaskResponseDto>> GetTaskById(int id)
        {
            try
            {
                var task = await _taskService.GetByIdAsync(id);
                return Ok(task);
            }
            catch (FluentValidation.ValidationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    new { message = "Erro ao buscar tarefa", error = ex.Message });
            }
        }

        /// <summary>
        /// POST /api/tasks - Criar nova tarefa
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<TaskResponseDto>> CreateTask([FromBody] CreateTaskDto dto)
        {
            try
            {
                var createdTask = await _taskService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetTaskById), new { id = createdTask.Id }, createdTask);
            }
            catch (FluentValidation.ValidationException ex)
            {
                // Se houver erros estruturados do FluentValidation
                if (ex.Errors.Any())
                {
                    var errors = ex.Errors.Select(e => new { field = e.PropertyName, message = e.ErrorMessage }).ToList();
                    return BadRequest(new { message = "Erro de validação", errors });
                }
                // Se for mensagem de texto (validação customizada)
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    new { message = "Erro ao criar tarefa", error = ex.Message });
            }
        }

        /// <summary>
        /// PUT /api/tasks/{id} - Atualizar tarefa
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<TaskResponseDto>> UpdateTask(int id, [FromBody] UpdateTaskDto dto)
        {
            try
            {
                var updatedTask = await _taskService.UpdateAsync(id, dto);
                return Ok(updatedTask);
            }
            catch (FluentValidation.ValidationException ex)
            {
                // Se houver erros estruturados do FluentValidation
                if (ex.Errors.Any())
                {
                    var errors = ex.Errors.Select(e => new { field = e.PropertyName, message = e.ErrorMessage }).ToList();
                    return BadRequest(new { message = "Erro de validação", errors });
                }
                // Se for mensagem de texto (validação customizada)
                return BadRequest(new { message = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    new { message = "Erro ao atualizar tarefa", error = ex.Message });
            }
        }

        /// <summary>
        /// PUT /api/tasks/{id}/start - Iniciar tarefa
        /// </summary>
        [HttpPut("{id}/start")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<TaskResponseDto>> StartTask(int id)
        {
            try
            {
                var updatedTask = await _taskService.StartAsync(id);
                return Ok(updatedTask);
            }
            catch (FluentValidation.ValidationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    new { message = "Erro ao iniciar tarefa", error = ex.Message });
            }
        }

        /// <summary>
        /// PUT /api/tasks/{id}/complete - Concluir tarefa
        /// </summary>
        [HttpPut("{id}/complete")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<TaskResponseDto>> CompleteTask(int id)
        {
            try
            {
                var updatedTask = await _taskService.CompleteAsync(id);
                return Ok(updatedTask);
            }
            catch (FluentValidation.ValidationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    new { message = "Erro ao concluir tarefa", error = ex.Message });
            }
        }

        /// <summary>
        /// PUT /api/tasks/{id}/reopen - Reabrir tarefa
        /// </summary>
        [HttpPut("{id}/reopen")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<TaskResponseDto>> ReopenTask(int id)
        {
            try
            {
                var updatedTask = await _taskService.ReopenAsync(id);
                return Ok(updatedTask);
            }
            catch (FluentValidation.ValidationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    new { message = "Erro ao reabrir tarefa", error = ex.Message });
            }
        }

        /// <summary>
        /// DELETE /api/tasks/{id} - Deletar tarefa
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteTask(int id)
        {
            try
            {
                await _taskService.DeleteAsync(id);
                return NoContent();
            }
            catch (FluentValidation.ValidationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    new { message = "Erro ao deletar tarefa", error = ex.Message });
            }
        }
    }
}