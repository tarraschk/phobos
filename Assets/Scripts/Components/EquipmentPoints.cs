using UnityEngine;
using System.Collections;
namespace Phobos 
{
	public class EquipmentPoints {
		
		/**
		 * Depending on the ship's model, we stock the different equipment attachment points !
		 * 
		 **/
		public class playerShip1 {
			public static int pointCount = 2; 
			public static Vector3[] points = new Vector3[2]{new Vector3(-2.5f, -1f, 0.5f),  new Vector3(2.5f, -1f, 0.5f)};  
		}
	}
}
