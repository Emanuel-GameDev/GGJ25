using UnityEngine;
using UnityEngine.Events;

public static class EventManager
{
    public static UnityAction OnPlayerDeath;
    public static UnityAction<int, PlayerLevelManager> OnPlayerLevelUp;
    public static UnityAction<GameObject> OnBubbleGrabbed;
    public static UnityAction<GameObject> OnBubbleThrown;
    public static UnityAction OnBubbleExploded;
}
