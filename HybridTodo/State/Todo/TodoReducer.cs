using Fluxor;
using HybridTodo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HybridTodo.State.Todo
{/*GRL : Step 5 - The reducer gets invoked when the action is dispatched, we use the parameters[if any] pass in with
  *     the action to create a new state object and update it into the store. Note that the current state object is used
  *     but not update i.e a new state object is created every time.
  */
    public static class TodoReducer
    {
        [ReducerMethod]
        public static TodoState ReduceIncrementCounterAction(TodoState state, UpdateTodoAction action)
        {
            var udpatedTodos = state.Todos.Concat(new[] { action.todo }).ToList(); //Create and add the new todo to current state
            return new TodoState(udpatedTodos, new TodoModel()); //Return new todo state object
        }
    }
}
