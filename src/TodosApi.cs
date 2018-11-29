using System;
using System.Collections.Generic;
#if (UseAzureStorage)
using System.IO;
#endif
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Company.WebApplication1
{
    [Route("[controller]")]
    [ApiController]
    public class TodosApi : ControllerBase
    {
        private readonly ITodoRepository _todoRepository;

        public TodosApi(ITodoRepository todoRepository)
        {
            _todoRepository = todoRepository;
        }

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<Todo>> Get()
        {
            return Ok(_todoRepository.GetTodos());
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<Todo> Get(int id)
        {
            var todo = _todoRepository.GetTodo(id);
            if(todo != null) 
                return Ok(_todoRepository.GetTodo(id));
            
            return NotFound();
        }

        // POST api/values
        [HttpPost]
        public ActionResult Post([FromBody] Todo todo)
        {
            _todoRepository.Create(todo);
            return Ok();
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Todo todo)
        {
            _todoRepository.Update(todo);
            return Ok();
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            _todoRepository.Delete(id);
            return Ok();
        }
    }

    public interface ITodoRepository
    {
        IEnumerable<Todo> GetTodos();
        Todo GetTodo(int id);
        void Create(Todo todo);
        void Update(Todo todo);
        void Delete(int id);
    }

    public class Todo
    {
        public int Id { get; set; }
        public string Description { get; set; }
    }
    
    public class InMemoryTodoRepository : ITodoRepository
    {
        private List<Todo> _todos;

        public InMemoryTodoRepository()
        {
            _todos = new List<Todo>();
            _todos.Add(new Todo { Id = 1, Description = "Go to work" });
            _todos.Add(new Todo { Id = 2, Description = "Merge the pull requests" });
            _todos.Add(new Todo { Id = 3, Description = "Resolve the issues" });
        }

        public void Create(Todo todo)
        {
            _todos.Add(todo);
        }

        public void Delete(int id)
        {
            _todos.RemoveAll(x => x.Id == id);
        }

        public Todo GetTodo(int id)
        {
            return _todos.FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<Todo> GetTodos()
        {
            return _todos.AsEnumerable();
        }

        public void Update(Todo todo)
        {
            if(_todos.Any(x => x.Id == todo.Id))
                _todos.First(x => x.Id == todo.Id).Description = todo.Description;
        }
    }
}