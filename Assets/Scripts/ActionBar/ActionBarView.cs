using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ActionBarView : MonoBehaviour, IActionBarView
{
    
    [SerializeField] private SpriteRenderer[] slots;
    [SerializeField] private float moveSpeed = 80;
    [SerializeField] private Color defaultSlotColor;
    [SerializeField] private Color matchSlotColor;
    public int MaxSlots => slots.Length;

    public void HighlightSlots(List<int> indices, bool match)
    {
        foreach (var idx in indices)
            slots[idx].color = match ? matchSlotColor : defaultSlotColor;
    }
    public async Task MoveCardToSlotAsync(CardView card, int slotIndex)
    {
        TaskCompletionSource<bool> tcs = new();
        StartCoroutine(MoveCardCoroutine(card, slotIndex, moveSpeed, tcs));
        await tcs.Task;
    }
    private IEnumerator MoveCardCoroutine(CardView card, int slotIndex, float speed, TaskCompletionSource<bool> tcs)
    {
        if (card == null || card.gameObject == null)
        {
            tcs.SetResult(false);
            yield break;
        }
        Vector2 target = slots[slotIndex].transform.position;
        while (Vector2.Distance(card.transform.position, target) > 0.001f)
        {
            if (card == null || card.gameObject == null)
            {
                tcs.SetResult(false);
                yield break;
            }
            card.transform.position = Vector2.MoveTowards(card.transform.position, target, speed * Time.deltaTime);
            yield return null;
        }
        if (card != null && card.gameObject != null)
        {
            card.transform.position = target;
            card.transform.rotation = Quaternion.identity;
        } 
        tcs.SetResult(true);
    }
}
