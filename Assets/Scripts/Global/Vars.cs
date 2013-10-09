using System;
using UnityEngine;
using System.Collections;

public enum BehaviorTypes{idle, collecting, moving, docking, attacking};
public enum ObjectsSpawnTypes{bot, collectable, decoration}; 

namespace Phobos
{
	
	public class Faction {
		public string name = "Lunatix Corporation" ; 
	}
	
	public class DataPaths {

		public static string LOOTS = "Assets/Resources/GameData/Loot/botloots.json"; 
		
	}
	
	public class Misc 
	{
		public static string NULL = "Null"; 	
	}
	
	public class KeyNames {
		public static string BOT = "bot"; 
		public static string COLLECTABLE = "collectable"; 
		public static string DECORATION = "decoration"; 
	}
	
	public class Vars
	{
		public static Quaternion ROTATION_DEFAULT = Quaternion.Euler(0, 0, 0); 
		public static string BUILDING_PREVIEW = "BuildingPreview";
		public static string TOOLTIP = "Tooltip";
		public static string MAIN_CAMERA_TAG = "MainCamera";
		public static string PLAYER_TAG = "Player";
		public static string UNIVERSE_TAG = "Universe";
		public static string PLAYERS_TAG = "Players";
		public static string OBJECTS_TAG = "Objects";
		public static string CAMERA_CONTAINER = "CameraContainer";
		public static string GUI_MODEL = "GUIModel";
		public static string GUI_CONTAINER = "GUIContainer";
		public static int TERRAIN_LAYER = 1 ;//<< 8 ; 
		
		public static string TERRAIN_NAME = "TerrainMain"; 
		public static int COLLECT_DISTANCE = 10; 
		public static int WARP_DISTANCE = 20; 
		public static string MODEL = "Model"; 
		public static string PORT = "port"; 
		public static string DOCKINGBAY = "DockingBay"; 
		public static string EQUIPMENT_CONTAINER = "Equipment"; 
		public static string CARGOBAY = "Cargo"; 
		public static string RECIPES = "recipes"; 
		
	}
	
	public class Gameplay {
		public const int EQUIPMENT_MAX = 10; 
	}
	
	public enum AITypes{
		idleAggressive,
		idlePassive,
		attack, 
		returnToPos
	};	
	
	public class recType {
		public const string BUILDING = "buildings"; 	
		public const string EQUIPMENT = "equipment"; 	
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
		public const string EQUIPMENT_REMOVE = "EquipmentRemove"; 	
		public const string EQUIPMENT_ADD = "EquipmentAdd"; 	
		public const string EQUIPMENT_CHANGE = "EquipmentChange"; 	
	}
}

