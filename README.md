# Blazor State Management 
## Statement Mangement using [Fluxor](https://github.com/mrpmorris/Fluxor)
### Code Organization
- **BlazorHybridFluxor** - Is a blazor hybrid project that will act as the root app for the demo, Can execute on windows,mac and mobile platforms
- **HybridTodo** - Razor library that implements a simple todo components that is used for demonstrating state management

### Steps For Enabling State
- **Step 1** : Add the Fluxor dependencies to the root app to make state available for the components, Make sure to scan all the required assemblies that have state defined for fluxor to manage when adding the fluxor service.
    ~~~
    using BlazorHybridFluxor.Data;
    using Fluxor; 
    Fluxor.Blazor.Web.ReduxDevTools
    using HybridTodo;
    public static class MauiProgram
    {
            ...
    		builder.Services.AddSingleton<WeatherForecastService>();
            builder.Services.AddFluxor(o =>
            {
                o.ScanAssemblies(typeof(TodoControl).Assembly);
                o.ScanAssemblies(typeof(MauiProgram).Assembly);
                o.UseReduxDevTools(rdt =>
                {
                    rdt.Name = "My application";
                });
            });
            ...
    	}
    }
    ~~~

- **Step 2** : Initialize the fluxor state in the main.razor page
    ~~~
    <Fluxor.Blazor.Web.StoreInitializer />
    <Router AppAssembly="@typeof(Main).Assembly">
    ...
    ~~~

> **_NOTE:_**  
![Core Principles of Redux](https://www.freecodecamp.org/news/content/images/size/w1000/2022/06/2.png)
-**Store** : The Redux store is the main, central bucket which stores all the states of an application. It should be considered and maintained as a single source of truth for the state of the application.
-**Action** : As mentioned above, state in Redux is read-only. This helps you restrict any part of the view or any network calls to write/update the state directly.Instead, if anyone wants to change the state of the application, then they'll need to express their intention of doing so by emitting or dispatching an action.
-**Reducer** : Reducers, as the name suggests, take in two things: previous state and an action. Then they reduce it (read it return) to one entity: the new updated instance of state.

> **_NOTE:_**  
**Fluxor Rules**
-State should always be read-only.
-To alter state our app should dispatch an action.
-Every reducer that processes the dispatched action type will create new state to reflect the old state combined with the changes expected for the action.

- **Step 3** : Define action class to update the todo items as they get added in the UI component
    ~~~
    public class UpdateTodoAction
    {
        public TodoModel todo;
        public UpdateTodoAction(string todo)
        {
            this.todo = new TodoModel(todo);
        }
    }
    ~~~

- **Step 4** : Define the reducer class that takes in the action and current state to create a brand new updated state to be returned.
    ~~~
    public static class TodoReducer
    {
        [ReducerMethod]
        public static TodoState ReduceIncrementCounterAction(TodoState state, UpdateTodoAction action)
        {
            var udpatedTodos = state.Todos.Concat(new[] { action.todo }).ToList(); 
            return new TodoState(udpatedTodos, new TodoModel()); 
        }
    }
    ~~~

- **Step 5** : Define the state class for the todos to mimic the UI componet.Make sure to define constructors that are to be used by the reducer to create new state object.
    ~~~
    public class TodoState
    {
        public List<TodoModel> Todos { get; }
        public TodoModel CurrentTodo { get; }
        public TodoState(List<TodoModel> todos, TodoModel currentTodo) 
        {
            Todos = todos;
            CurrentTodo = currentTodo;
        }
        public TodoState()
        {
            Todos = new();
            CurrentTodo = new();
        }
    }
    ~~~

- **Step 6** : Inject the IState and the IDispatcher objects into the razor file for the component for which state needs to be maintained, use the dispacher to invoke the required action to update state.
    ~~~
    @using HybridTodo.State.Todo
    @inject IDispatcher dispacher
    @inject IState<TodoState> todoState
    
    ...
            <InputText @bind-Value="todoState.Value.CurrentTodo.TodoItem" class="form-control" id="todoItem" />
            <button class="btn btn-primary mt-2" @onclick="AddTodo">Add Todo</button>
        </div>
    </EditForm>
    
    <h3>ToDo List</h3>
    <ul>
        @foreach (var t in todoState.Value.Todos)
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
       ...
        private void AddTodo()
        {
            dispacher.Dispatch(new UpdateTodoAction(todoState.Value.CurrentTodo.TodoItem));
        }
    }
    ~~~

#### Sharign State Across Components
- **Step 7** : To share state variables inject IState generic class and update the component to inherit "FluxorComponent". The state variables can be used in the component or passed onto child componenets.Fluxor will take care of re rendering when state updates.
    ~~~
    @using Fluxor;
    @using HybridTodo.State.Todo;
    @inject IState<TodoState> todoState
    @inherits Fluxor.Blazor.Web.Components.FluxorComponent
    ...
            <div class="nav-item px-3">
                <NavLink class="nav-link">
                    Todo Count = @todoState.Value.Todos.Count
                </NavLink>
            </div>
            <div class="nav-item px-3">
                <NavLink class="nav-link">
                    <TodoCountSubComponent Todos=@todoState.Value />
                </NavLink>
            </div>
        </nav>
    </div>
    ...
    ~~~