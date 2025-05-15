using UnityEngine;
using UniRx;

public class GameResultView : MonoBehaviour
{
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject losePanel;

    private void Start()
    {
        GameSignals.OnWin.Subscribe(_ =>
        {
            winPanel.SetActive(true);
            losePanel.SetActive(false);
        }).AddTo(this);
        GameSignals.OnLose.Subscribe(_ =>
        {
            winPanel.SetActive(false);
            losePanel.SetActive(true);
        }).AddTo(this);
    }
}
