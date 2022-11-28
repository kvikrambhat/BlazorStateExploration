using HybridTodo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HybridTodo.State.Todo
{
    /*GRL : Step 4 - Action is the method that gets called when state needs to be changed/updated 
        the class structure and constructor signature is as per requirement to pass any parameter, it can be left 
        empty if no data needs to be passed from the component to the state
     */
    public class UpdateTodoAction
    {
        public TodoModel todo;

        public UpdateTodoAction(string todo)//In our case we need to pass the todo string to the store
        {
            this.todo = new TodoModel(todo);
        }
    }
}
