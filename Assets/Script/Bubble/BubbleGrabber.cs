using UnityEngine;

public class BubbleGrabber : MonoBehaviour
{
    [SerializeField] private BubbleController _bubbleGrabbed;
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.TryGetComponent(out BubbleController bubble))
        {
            if(bubble.isGrabbed) 
                return;

            _bubbleGrabbed = bubble;

            _bubbleGrabbed.isGrabbed = true;

            bubble.gameObject.transform.SetParent(transform.parent, false);

            bubble.gameObject.transform.localPosition = Vector3.zero;

            EventManager.OnBubbleGrabbed?.Invoke(transform.parent.gameObject);
        }
    }
}
