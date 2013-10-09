using UnityEngine;
using System.Collections;
namespace Phobos
{
	public class MainLib : MonoBehaviour {
		
		public static ObjectsSpawnTypes stringToSpawnType(string entryString) {
			if (entryString == Phobos.KeyNames.BOT) 
				return ObjectsSpawnTypes.bot; 
			else if (entryString == Phobos.KeyNames.COLLECTABLE)
				return ObjectsSpawnTypes.collectable;
			else if (entryString == Phobos.KeyNames.DECORATION)
				return ObjectsSpawnTypes.decoration; 
			else return ObjectsSpawnTypes.decoration ; 
		}
		
	}
}