using System.Collections.Generic;
using Cinemachine;
using GameObjects.Characters;
using GameObjects.Locations;
using UnityEngine;
using Utilities;

namespace GameObjects.Managers
{
    public class MainSceneManager : MonoBehaviour
    {
        [Header("[Spawn]")]
        public List<SpawnZone> spawnZones;
        public List<string> spawnCharacterPaths;

        [Header("[Camera]")] public CinemachineVirtualCamera playerCamera;
        
        // Start is called before the first frame update
        void Start()
        {
            var gameManager = GameManager.instance;

            var spawnZoneTargetIndex = gameManager.currentUser.spawnToken % this.spawnZones.Count;
            var spawnZoneTarget = this.spawnZones[spawnZoneTargetIndex];

            var spawnCharacterPathIndex = gameManager.currentUser.spawnToken % this.spawnCharacterPaths.Count;
            var spawnCharacterPath = this.spawnCharacterPaths[spawnCharacterPathIndex];
            
            var currentUserPosition = spawnZoneTarget.gameObject.transform.position;
            
            var tankResource = Resources.Load<Character>(spawnCharacterPath);

            var playerInstance = Instantiate(tankResource,
                    currentUserPosition,
                    Quaternion.identity);

            playerCamera.Follow = playerInstance.head.transform;
        }
    }
}
