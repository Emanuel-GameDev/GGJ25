using Cysharp.Threading.Tasks;
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
    public bool isGrabbable = true;

    [SerializeField] private float speed = 5f;
    [SerializeField] private int time = 2;

    private void Awake()
    {
        _instance = this;
    }

    // void Update()
    // {
    //     // if(isGrabbed && transform.parent != null)
    //     // {
    //     //     // transform.position = transform.parent.position;
    //     // }
    // }

    public async UniTask ThrowTask(Vector2 direction, GameObject player)
    {
        EventManager.OnBubbleThrown?.Invoke(player);
        // Debug.Log("Direction: " + direction);
        if(direction == Vector2.zero)
        {
            direction = Vector2.up;
        }

        var rb = GetComponent<Rigidbody2D>();
        
        isGrabbable = false;

        rb.linearVelocity = direction * 5f;
        
        await UniTask.Delay(time * 1000);
        
        isGrabbable = true;

        rb.linearVelocity = Vector2.zero;
    }
}
