using UnityEngine;
using UnityEngine.Events;

public static class EventManager
{
    public static UnityAction OnPlayerDeath;
    public static UnityAction<int> OnPlayerLevelUp;
    public static UnityAction<GameObject> OnBubbleGrabbed;
}
