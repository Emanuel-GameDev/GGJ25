using System.Collections;
using UnityEngine;

public class RayPistolProjectile : MonoBehaviour
{

    [SerializeField]
    private float aliveTime = 1.5f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(CooldownProjectile());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if ()
        //{
        //    //Collisione con nemico, distruzione
        //}
    }

    IEnumerator CooldownProjectile()
    {
        yield return new WaitForSeconds(aliveTime);
        gameObject.SetActive(false);
    }
}
