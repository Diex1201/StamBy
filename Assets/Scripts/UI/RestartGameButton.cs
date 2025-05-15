using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class RestartGameButton : MonoBehaviour
{
    private Button restartGameButton;


    private void Awake()
    {
        restartGameButton = GetComponent<Button>();
        restartGameButton.onClick.AddListener(() =>
        {
            GameSignals.OnReloadScene.OnNext(Unit.Default);
        });
    }
}
