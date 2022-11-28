# Blazor State Management 
## Injecting State Using DI
### Code Organization
- **BlazorHybridStateInjected** - Is a Blazor hybrid project that will act as the root app for the demo, Can execute on windows,mac and mobile platforms.
- **BlazorServer** - Blazor server app used here to demonstrate use of Blazor components on web application.
- **HybridTodo** - Razor library that implements a simple todo components that is used for demonstrating state management.

### Steps For Enabling State
- **Step 1** : Define the model class for the todos.
    ~~~
    public class TodoModel
    {
        public string TodoItem { get; set; } = "";
        public bool IsComplete { get; set; }
    }
    ~~~

- **Step 2** : Define the class to store the components state, this will mimic the UI layout/representation of the component and will not necessary be similar to the model class.
    ~~~
    public class TodoState
    {
    	public List<TodoModel> Todos { get; private set; }
    	public TodoModel CurrentTodo { get; private set; }
    	public event EventHandler TodoAddedEventHandler;
    	public void AddTodo(TodoModel todo)
    	{
    		Todos.Add(todo);
    		CurrentTodo = new();
    		TodoHasBeenAdded();
    	}
    	private void TodoHasBeenAdded()
    	{
    		TodoAddedEventHandler?.Invoke(this, EventArgs.Empty);
    	}
    	public TodoState()
    	{
    		Todos = new();
    		CurrentTodo = new();
    	}
    }
    ~~~

- **Step 3** - Inject and use state in the component razor file.
    ~~~
    @using Microsoft.AspNetCore.Components.Web
    @using HybridTodo.Models
    @using HybridTodo.State
    @using Microsoft.AspNetCore.Components.Forms
    @inject TodoState TodoState 
    <EditForm Model="@TodoState.CurrentTodo">
        <div class="mb-3">
            <label for="todoItem" class="form-label">ToDo Item:</label>
            <InputText @bind-Value="TodoState.CurrentTodo.TodoItem" class="form-control" id="todoItem" />
            <button class="btn btn-primary mt-2" @onclick="AddTodo">Add Todo</button>
        </div>
    </EditForm>
    <h3>ToDo List</h3>
    <ul>
        @foreach (var t in TodoState.Todos)
        {
            <li class="mb-2">
                @if (t.IsComplete)
                {
                    <span style="text-decoration: line-through">
                        @t.TodoItem
                    </span>
                }
                else
                {
                    @t.TodoItem
                    <button class="btn btn-warning btn-sm ms-3"
                    @onclick="()=>t.IsComplete=true">
                        Complete
                    </button>
                }
            </li>
        }
    </ul>
    @code {
        private void AddTodo()
        {       
            TodoState.AddTodo(TodoState.CurrentTodo);
        }
    }
    ~~~

- **Step 4** - Add the state object as a scoped service in the root app for the razor library to access during runtime.
    ~~~
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            ...
            builder.Services.AddSingleton<WeatherForecastService>();
            builder.Services.AddScoped<TodoState>();
            ...
        }
    }
    ~~~

#### Sharign State Across Components

- **Step 5** - Inject the state in any the components that needs to use its value, implement the event handling that the state object will fire on update and implement IDisposable to dispose the handler on unmount of component. The sample below shows the state being used in the same component as well as being passed into a child component.
    ~~~
    @using HybridTodo.State
    @inject TodoState todoState
    @implements IDisposable
            ...
    		<div class="nav-item px-3">
                <NavLink class="nav-link">
                    Todo Count = @todoState.Todos.Count 
                </NavLink>
            </div>
            <div class="nav-item px-3">
                <NavLink class="nav-link">
                    <TodoCountSubComponent Todos=@todoState /> 
                </NavLink>
           ...
    @code {
        ...
    	void TodoHasBeenAdded(object sender, EventArgs e) => StateHasChanged();
    	protected override void OnInitialized()
    	{
    		todoState.TodoAddedEventHandler += TodoHasBeenAdded;
    	}
    	void IDisposable.Dispose()
    	{
    		todoState.TodoAddedEventHandler -= TodoHasBeenAdded;
    	}
    	...
    }
    ~~~