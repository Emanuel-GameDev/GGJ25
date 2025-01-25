using System.Collections;
using UnityEngine;

public class Granade : MonoBehaviour
{
    public float _dmg;

    [SerializeField]
    private float _timeBeforeStop = 2f;


    private Rigidbody2D rb;
    private Animator anim;
    private CircleCollider2D circleCollider;
    private Sprite originalSprite;

    private void OnEnable()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        originalSprite = GetComponent<SpriteRenderer>().sprite;

        circleCollider = GetComponent<CircleCollider2D>();
        circleCollider.enabled = false;

        StartCoroutine(CooldownProjectile());
    }

    IEnumerator CooldownProjectile()
    {
        yield return new WaitForSeconds(_timeBeforeStop);

        rb.linearVelocity = Vector3.zero;

        circleCollider.enabled = true;
        anim.SetTrigger("Explosion");
    }

    public void ResetProjectile()
    {
        GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
        GetComponent<SpriteRenderer>().sprite = originalSprite;
        GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 1);

        if (gameObject.activeSelf)
            gameObject.SetActive(false);

        circleCollider.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IDamageable damageable))
        {
            damageable.TakeDamage(_dmg);
        }
    }

    public void SetColliderRadius(float amount)
    {
        circleCollider.radius = amount;
    }
}
