using Fluxor;
using HybridTodo.Models;

namespace HybridTodo.State.Todo;

[FeatureState]
public class TodoState
{
    /*GRL : Step 4 - Define the component state to mimic the UI, also make sure to define 
            1.Constructor with no parameters
            2.Any overrides of constructor to be used by reducer
     */
    public List<TodoModel> Todos { get; }

    public TodoModel CurrentTodo { get; }

    public TodoState(List<TodoModel> todos, TodoModel currentTodo) //Needed by reducer
    {
        Todos = todos;
        CurrentTodo = currentTodo;
    }

    public TodoState()//Needed by fluxor for injecting the state
    {
        Todos = new();
        CurrentTodo = new();
    }

}
