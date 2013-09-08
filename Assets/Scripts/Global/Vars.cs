using System;

public enum BehaviorTypes{idle, collecting, moving, docking, attacking};
public enum ObjectsSpawnTypes{bot, collectable}; 

namespace Phobos
{
	public class Vars
	{
		public static string BUILDING_PREVIEW = "BuildingPreview";
		public static string MAIN_CAMERA_TAG = "MainCamera";
		public static string PLAYER_TAG = "Player";
		public static string UNIVERSE_TAG = "Universe";
		public static string PLAYERS_TAG = "Players";
		public static string OBJECTS_TAG = "Objects";
		public static string CAMERA_CONTAINER = "CameraContainer";
		public static string GUI_CONTAINER = "GUIContainer";
		public static int TERRAIN_LAYER = 1 ;//<< 8 ; 
		
		public static string TERRAIN_NAME = "TerrainMain"; 
		public static int COLLECT_DISTANCE = 10; 
		public static int WARP_DISTANCE = 20; 
		public static string MODEL = "Model"; 
		public static string DOCKINGBAY = "DockingBay"; 
		public static string CARGOBAY = "Cargo"; 
		
	}
	public enum dockType {
		station, warp	
	}
	
	public class Commands 
	{
		public const string MOVE_TO = "MoveTo"; 	
		public const string COLLECT = "Collect"; 	
		public const string ATTACK = "Attack"; 	
		public const string DOCK = "Dock"; 	
	}
}

