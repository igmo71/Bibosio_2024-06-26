namespace Bibosio.WebApi.Modules.Todos
{
    public interface ITodoService
    {
        Task<IResult> GetAll();
        Task<IResult> GetComplete();
        Task<IResult> Get(int id);
        Task<IResult> Create(Todo todo);
        Task<IResult> Update(int id, Todo inputTodo);
        Task<IResult> Delete(int id);
    }
}
