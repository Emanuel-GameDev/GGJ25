using System.Collections;
using UnityEngine;

public class RayPistolProjectile : MonoBehaviour
{
    public bool goTrough = false;

    public int _baseDmg = 5;

    [SerializeField]
    private float aliveTime = 1.5f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(CooldownProjectile());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IDamageable damageable))
        {
            damageable.TakeDamage(_baseDmg);

            if (!goTrough)
                gameObject.SetActive(false);
        }
    }

    IEnumerator CooldownProjectile()
    {
        yield return new WaitForSeconds(aliveTime);

        if (gameObject.activeSelf)
            gameObject.SetActive(false);
    }
}
