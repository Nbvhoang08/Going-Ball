using UnityEngine;
using System;
using System.Collections.Generic;

namespace Hapiga.Core.Runtime.FSM
{
	///<summary>
	/// This is the main engine of our FSM, without this, you won't be
	/// able to use FSM States and FSM Actions.
	///</summary>
	public class FSM
	{
		private readonly string name;
		public FSMState currentState;
		public FSMState previousState;
		private readonly Dictionary<int, FSMState> stateMap;

		public string Name
		{
			get
			{
				return name;
			}
		}

		private delegate void StateActionProcessor(FSMAction action);

		/// <summary>
		/// This gets all the actions that is inside the state and loop them.
		/// </summary>
		/// <param name="state">State.</param>
		/// <param name="actionProcessor">Action processor.</param>
		private void ProcessStateAction(FSMState state, StateActionType actionType, float dt = 0)//, Action<FSMAction> actionProcessor)
		{
			FSMState currentStateOnInvoke = this.currentState;
			List<FSMAction> actions = state.GetActions();

			//foreach (FSMAction action in actions)
			for (int i = 0; i < actions.Count; i++)
			{
				if (this.currentState != currentStateOnInvoke)
				{
					break;
				}
				switch (actionType)
				{
					case StateActionType.OnEnter:
						actions[i].OnEnter();
						break;
					case StateActionType.OnExit:
						if (this.currentState != currentStateOnInvoke)
							Debug.LogError("State cannont be changed on exit of the specified state");

						actions[i].OnExit();
						break;
					case StateActionType.OnUpdate:
						actions[i].OnUpdate(dt);
						break;
					case StateActionType.OnFixUpdate :
						actions[i].OnFixUpdate(dt);
						break;
					case StateActionType.OnDestroy:
						actions[i].OnDestroy();
						break;
				}
			}
		}

		public FSMState AddState(int name)
		{
			if (stateMap.ContainsKey(name))
			{
				Debug.LogWarning("The FSM already contains: " + name);
				return null;
			}

			FSMState newState = new FSMState(name, this);
			stateMap[name] = newState;
			return newState;
		}

		///<summary>
		/// This is the constructor that will initialize the FSM and give it
		/// a unique name or id.
		///</summary>
		public FSM(string name)
		{
			this.name = name;
			this.currentState = null;
			this.previousState = null;
			stateMap = new Dictionary<int, FSMState>();
		}

		///<summary>
		/// This initializes the FSM. We can indicate the starting State of
		/// the Object that has an FSM.
		///</summary>
		public void Start(int stateId)
		{
			if (!stateMap.ContainsKey(stateId))
			{
				Debug.LogWarning("The FSM doesn't contain: " + stateId);
				return;
			}

			ChangeToState(stateMap[stateId]);
		}

		///<summary>
		/// This changes the state of the Object. This also calls the exit
		/// state before doing the next state.
		///</summary>
		public void ChangeToState(FSMState state)
		{
			if (this.currentState != null)
			{
				ExitState(this.currentState);
			}
			this.previousState = currentState;
			this.currentState = state;
			EnterState(this.currentState);
		}

		///<summary>
		/// This changes the state of the Object. It is not advisable to
		/// call this to change state.
		///</summary>
		public void EnterState(FSMState state)
		{
#if UNITY_EDITOR
			//Debug.Log("Enter to State: " + state.GetId());
#endif
			ProcessStateAction(state, StateActionType.OnEnter);
			//, (FSMAction action) =>
			//{
			//	action.OnEnter();
			//});
		}

		private void ExitState(FSMState state)
		{
			FSMState currentStateOnInvoke = this.currentState;

			ProcessStateAction(state, StateActionType.OnExit);
			//, (FSMAction action) =>
			//{

			//	if (this.currentState != currentStateOnInvoke)
			//		Debug.LogError("State cannont be changed on exit of the specified state");

			//	action.OnExit();
			//});
		}

		///<summary>
		/// Call this under a MonoBehaviour's Update.
		///</summary>
		public void Update(float dt)
		{
			if (this.currentState == null)
				return;

			ProcessStateAction(this.currentState, StateActionType.OnUpdate, dt);
			//, (FSMAction action) =>
			//{
			//	action.OnUpdate();
			//});
		}

		public void FixUpdate(float dt)
		{
			if (this.currentState == null)
				return;

			ProcessStateAction(this.currentState, StateActionType.OnFixUpdate, dt);
		}
		public void Destroy()
		{
			if (this.currentState == null)
				return;

			ProcessStateAction(this.currentState, StateActionType.OnDestroy);
			//, (FSMAction action) =>
			//{
			//	action.OnDestroy();
			//});
		}
		///<summary>
		/// This handles the events that is bound to a state and changes
		/// the state.
		///</summary>
		public void SendEvent(int eventId)
		{
			FSMState transitonState = ResolveTransition(eventId);

			if (transitonState == null)
				Debug.LogWarning("The current state has no transition for event " + eventId);
			else
				ChangeToState(transitonState);

		}

		public int GetCurrentState()
		{
			if (currentState == null)
			{
				return 0;
			}
			else
			{
				return currentState.GetId();
			}
		}

		public int GetPreviousState()
		{
			if (previousState == null)
			{
				return 0;
			}

			return previousState.GetId();
		}
		/// <summary>
		/// This gets the next state from the current state.
		/// </summary>
		/// <returns>The transition.</returns>
		/// <param name="eventId">Event identifier.</param>
		private FSMState ResolveTransition(int eventId)
		{
			FSMState transitionState = this.currentState.GetTransition(eventId);

			if (transitionState == null)
				return null;
			else
				return transitionState;
		}
	}

	enum StateActionType : byte
	{
		OnEnter = 1,
		OnExit = 2,
		OnUpdate = 3,
		OnFixUpdate = 4,
		OnDestroy = 5
	}

}
