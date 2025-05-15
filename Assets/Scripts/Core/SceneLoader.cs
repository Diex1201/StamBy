using UnityEngine;
using UniRx;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{


    private void Start()
    {
        GameSignals.OnReloadScene.Subscribe(_ =>
        {
            ReloadCurrentScene();
        }).AddTo(this);
    }
    private void ReloadCurrentScene()
    {
        GameSignals.ResetAll();
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
}
