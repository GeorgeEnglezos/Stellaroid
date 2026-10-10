using System;
using System.Collections.Generic;

public static class EventBroadcaster
{

	//Main Stuff
	public static event EventHandler OnGameStart;
	public static event EventHandler OnGameOver;

	
	//Asteroids
	public static event EventHandler<ListEventArgs> onHitAsteroid;
	public static event EventHandler<ListEventArgs> onDestroyAsteroid;
	public static event EventHandler<ListEventArgs> onAsteroidCrush;

	
	//To be able to pass Data to the Events
	public class ListEventArgs : EventArgs
	{
		public List<string> Data { get; set; }
		public ListEventArgs(List<string> data)
		{
			Data = data;
		}
	}
	
	//ASteroids

	#region Asteroids

	public static void call_Event_Hit_asteroid(List<string> data)
	{
		if (onHitAsteroid != null) {
			onHitAsteroid(null, new ListEventArgs(data));
		}
	}
	public static void call_Event_Destroy_asteroid(List<string> data)
	{
		if (onDestroyAsteroid != null) {
			onDestroyAsteroid(null, new ListEventArgs(data));
		}
	}	public static void call_Event_Crushed_On_Asteroid(List<string> data)
	{
		if (onAsteroidCrush != null) {
			onAsteroidCrush(null, new ListEventArgs(data));
		}
	}
	#endregion
	
	//Level 

	#region Level_Related_REGION
    	public static void call_Event_Game_Over()
    	{
    		if (OnGameOver != null) {
    			OnGameOver(null, EventArgs.Empty);
    		}
    	}
    	public static void call_Event_Game_Start()
    	{
    		if (OnGameStart != null) {
	            OnGameStart(null, EventArgs.Empty);
    		}
    	}


	#endregion

}
