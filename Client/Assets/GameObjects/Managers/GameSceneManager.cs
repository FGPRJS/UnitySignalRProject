using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameObjects.Managers
{
    public enum GameSceneName
    {
        LoginScene,
        MainScene
    }
    
    public class GameSceneManager : MonoBehaviour
    {
        public static GameSceneManager instance;
        
        private ConnectionManager connectionManager;

        private void OnEnable()
        {
            
        }

        public void Initialize()
        {
            DontDestroyOnLoad(this);
            instance = this;
            
            connectionManager = ConnectionManager.instance;
            
            connectionManager.connectionMessage.AddListener((message) =>
            {
                switch (message.messageName)
                {
                    case Protocol.MessageNameKey.ConnectToServerComplete:
                        StartCoroutine(LoadScene(GameSceneName.MainScene));
                        break;
                }
            });
        }
        
        private IEnumerator LoadScene(GameSceneName sceneName)
        {
            var asyncLoad = SceneManager.LoadSceneAsync(sceneName.ToString(), LoadSceneMode.Single);
            Debug.Log(asyncLoad.progress);

            while (!asyncLoad.isDone)
            {
                yield return null;
            }

            
        }
    }
}
