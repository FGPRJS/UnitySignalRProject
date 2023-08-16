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

        void Awake()
        {
            if (instance != null)
            {
                return;
            }
            
            instance = this;
            DontDestroyOnLoad(this);
        }

        public void LoadScene(GameSceneName sceneName)
        {
            StartCoroutine(LoadSceneCoroutine(sceneName));
        }
        
        private IEnumerator LoadSceneCoroutine(GameSceneName sceneName)
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
