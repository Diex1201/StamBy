using UnityEngine;
using UniRx;
using UnityEngine.UI;

public class RespawnButton : MonoBehaviour
{
    private Button btn;

    private void Awake()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(() =>
        {
            GameSignals.OnReshuffle.OnNext(Unit.Default);
        });
    }
}
