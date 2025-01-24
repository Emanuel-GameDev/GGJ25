using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

public class BubbleGrabber : MonoBehaviour
{
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private BubbleController _bubbleGrabbed;

    void Awake()
    {
        _playerController = GetComponentInParent<PlayerController>(); 
        _playerController.ThrowAction.performed += ThrowBubble;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.TryGetComponent(out BubbleController bubble))
        {
            if(bubble.isGrabbed
                || !bubble.isGrabbable) 
                return;

            _bubbleGrabbed = bubble;

            _bubbleGrabbed.isGrabbed = true;
            _bubbleGrabbed.isGrabbable = false;

            bubble.gameObject.transform.SetParent(transform.parent, false);

            bubble.gameObject.transform.localPosition = Vector3.zero;

            EventManager.OnBubbleGrabbed?.Invoke(transform.parent.gameObject);
        }
    }

    public void ThrowBubble(InputAction.CallbackContext context)
    {
        _bubbleGrabbed.ThrowTask(_playerController.MoveValue).Forget();
        _bubbleGrabbed.isGrabbed = false;
        _bubbleGrabbed.transform.parent = null;
        _bubbleGrabbed = null;
    }
}
