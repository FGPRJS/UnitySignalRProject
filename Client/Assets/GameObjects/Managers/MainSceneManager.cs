using GameObjects.Characters;
using UnityEngine;
using Utilities;

namespace GameObjects.Managers
{
    public class MainSceneManager : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            var gameManager = GameManager.instance;
            var currentUserPosition = VectorConverter.ToUnityVector3(
                gameManager.currentUser.positionString);
            
            var tankInstance = Resources.Load<Character>("Characters/TankFree_Blue");

            Instantiate(tankInstance,
                    currentUserPosition,
                    Quaternion.identity);
        }
    }
}
