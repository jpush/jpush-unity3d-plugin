using UnityEngine;
using System.Collections;

namespace JPush {
	
	// internal event listener model
	internal class EventListener {
		public string name;
		public GameObject listener;
		public string function;
	}
	
	// Custom event class, extend when creating custom events
	public class CustomEvent {
	
		private string _type;
		private Hashtable _arguments = new Hashtable();
		
		// constructor
		public CustomEvent(string eventType = "") {
			_type = eventType;
		}
	
		// the type of event
		public string type {
			get { return _type; }
			set { _type = value; }
		}
		
		// the arguments to pass with the event
		public Hashtable arguments {
			get { return _arguments; }
			set { _arguments = value; }
		}
	}
	
	public class JPushEventManager : MonoBehaviour {
	
		// singleton instance
		public static JPushEventManager instance;
		
		// settings
		public bool allowSingleton = true; // JPushEventManager class will transfer between scene changes.
		public bool allowWarningOutputs = true;
		public bool allowDebugOutputs = true;
		
		private static bool _created = false;
		private Hashtable _listeners = new Hashtable();
		
		static JPushEventManager(){
			instance = new JPushEventManager() ;
		}
		// setup singleton if allowed
		public void Awake() {
			if (!_created && allowSingleton) {
				DontDestroyOnLoad(this);
				instance = this;
				_created = true;
				Setup();
			} else {
				if (allowSingleton) {
					if (JPushEventManager.instance.allowWarningOutputs) {
						Debug.LogWarning("Only a single instance of " + this.name + " should exists!");
					}
					Destroy(gameObject);
				} else {
					instance = this;
					Setup();
				}
			}
		}
		
		// clear events on quit
		public void OnApplicationQuit() {
			_listeners.Clear();
		}
		
		// PUBLIC *******************************
		
		// Add event listener
		public bool addEventListener(string eventType, GameObject listener, string function) {
			if (listener == null || eventType == null) {
				if (allowWarningOutputs) {
					Debug.LogWarning("Event Manager: AddListener failed due to no listener or event name specified.");
				}
				return false;
			}
			recordEvent(eventType);
			return recordListener(eventType, listener, function);
		}
		
		// Remove event listener
		public bool removeEventListener(string eventType, GameObject listener) {
			if (!checkForEvent(eventType)) return false;
			
			ArrayList listenerList = _listeners[eventType] as ArrayList;
			foreach (EventListener callback in listenerList) {
				if (callback.name == listener.GetInstanceID().ToString()) {
					listenerList.Remove(callback);
					return true;
				}
			}
			return false;
		}
		
		// Remove all event listeners
		public void removeAllEventListeners(GameObject listener) {
			//print ("listener.name------" + listener.name + " ---" + listener.GetInstanceID().ToString()) ;
			_listeners.Clear() ;
			
			/*foreach (EventListener callback in _listeners) { 
				if (callback.listener.GetInstanceID().ToString() == listener.GetInstanceID().ToString()) {
					_listeners.Remove(callback);
				}
			}*/
		}
		
		// Dispatch an event
		public bool dispatchEvent(CustomEvent evt) {
			string eventType = evt.type;
			if (!checkForEvent(eventType)) {
				if (allowWarningOutputs) {
					Debug.LogWarning("Event Manager: Event \"" + eventType + "\" triggered has no listeners!");
				}
				return false;
			}
		
			ArrayList listenerList = _listeners[eventType] as ArrayList;
			if (allowDebugOutputs) {
				Debug.Log("Event Manager: Event " + eventType + " dispatched to " + listenerList.Count + ((listenerList.Count == 1) ? " listener." : " listeners."));
			}
			
			foreach (EventListener callback in listenerList) {
				//print ("function---------" + callback.function) ;
				if (callback.listener && callback.listener.activeSelf) {//callback.listener.active
					callback.listener.SendMessage(callback.function, evt, SendMessageOptions.DontRequireReceiver);			
				}
			}
			return false;
		}
		
		// PRIVATE *******************************
		
		private void Setup() {
		// TO DO: Self create GameObject if not already created
		}
		
		// see if event already exists
		private bool checkForEvent(string eventType) {
			if (_listeners.ContainsKey(eventType)) return true;
			return false;
		}
		
		// record event, if it doesn't already exists
		private bool recordEvent(string eventType) {
			if (!checkForEvent(eventType)) {
				_listeners.Add(eventType, new ArrayList());
			}
			return true;
		}
		
		// delete event, if not already removed
		private bool deleteEvent(string eventType) { 
			if (!checkForEvent(eventType)) return false;
				_listeners.Remove(eventType);
			return true;
		}
		
		// check if listener exists
		private bool checkForListener(string eventType, GameObject listener) {
			if (!checkForEvent(eventType)) {
				recordEvent(eventType);
			}
		
			ArrayList listenerList = _listeners[eventType] as ArrayList;
			foreach (EventListener callback in listenerList) {
				if (callback.name == listener.GetInstanceID().ToString()) return true;
			}
			return false;
		}
		
		// record listener, if not already recorded
		private bool recordListener(string eventType, GameObject listener, string function) {
			if (!checkForListener(eventType, listener)) {
				ArrayList listenerList = _listeners[eventType] as ArrayList;
				EventListener callback = new EventListener();
				callback.name = listener.GetInstanceID().ToString();
				callback.listener = listener;
				callback.function = function;
				listenerList.Add(callback);
				return true;
			} else {
				if (allowWarningOutputs) {
					Debug.LogWarning("Event Manager: Listener: " + listener.name + " is already in list for event: " + eventType);
				}
				return false;
			}
		}
	}
	
	
}

