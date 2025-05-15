using System.Collections.Generic;
using System.Threading.Tasks;

public interface IActionBarView
{
    void HighlightSlots(List<int> indices, bool match);
    Task MoveCardToSlotAsync(CardView card, int slotIndex);
    int MaxSlots { get; }
}
