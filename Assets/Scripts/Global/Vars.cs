using System;

public enum BehaviorTypes{idle, collecting, moving};

namespace Phobos
{
	public class Vars
	{
		public static string BUILDING_PREVIEW = "BuildingPreview";
		public static string MAIN_CAMERA_TAG = "MainCamera";
		public static string PLAYER_TAG = "Player";
		public static string UNIVERSE_TAG = "Universe";
		public static string CAMERA_CONTAINER = "CameraContainer";
		public static string GUI_CONTAINER = "GUIContainer";
		public static int TERRAIN_LAYER = 1 ;//<< 8 ; 
		
		public static string TERRAIN_NAME = "TerrainMain"; 
		public static int COLLECT_DISTANCE = 10; 
		public static string MODEL = "Model"; 
		public static string CARGOBAY = "Cargo"; 
		
	}
	
	public class Commands 
	{
		public const string MOVE_TO = "MoveTo"; 	
		public const string COLLECT = "Collect"; 	
		public const string ATTACK = "Attack"; 	
	}
}

