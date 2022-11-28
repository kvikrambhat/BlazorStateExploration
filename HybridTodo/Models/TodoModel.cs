namespace HybridTodo.Models;

public class TodoModel
{
    //GRL : Step 1 - Define the class to define the todos structure
    public string TodoItem { get; set; } = "";

    public bool IsComplete { get; set; }
}
