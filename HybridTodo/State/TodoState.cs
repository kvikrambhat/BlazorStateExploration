using HybridTodo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HybridTodo.State;

public class TodoState
{
	/*
     *GRL : Step 2 - Define state object to mimic the UI defined in the razor file. Make sure to 
     *define default values for the state variables so that UI has some value to work with
     *on initializing [make sure not to initialize to null values as they can cause exceptions].    
     *
     *Might be better to keep this in the root app instead of in each of the libs to facilitate reuse of state variables
     */

	//GRL : Step 2 - Make the setters private as methods defined below need to be used to set state and raise events
	public List<TodoModel> Todos { get; private set; }

	public TodoModel CurrentTodo { get; private set; }

	//GRL : Step 2 - Event to be fired when todos get added
	public event EventHandler TodoAddedEventHandler;

	public void AddTodo(TodoModel todo)
	{
		//GRL : Step 2 - Method that the component needs to use to update the state
		Todos.Add(todo);
		CurrentTodo = new();
		TodoHasBeenAdded();//Fire event once state has been changed
	}
	private void TodoHasBeenAdded()
	{
		//GRL : Step 2 - Fire the event to all the subscribers 
		TodoAddedEventHandler?.Invoke(this, EventArgs.Empty);
	}
	public TodoState()
	{
		Todos = new();
		CurrentTodo = new();
	}
}
