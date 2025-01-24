using UnityEngine;

public class BubbleController : MonoBehaviour
{
    private static BubbleController _instance = null;
    public static BubbleController Instance
    {
        get
        {
            return _instance;
        }
    }

    public bool isGrabbed = false;

    private void Awake()
    {
        _instance = this;
    }
}
