using Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayPistolProjectile : MonoBehaviour
{
    public bool goTrough = false;

    public int _baseDmg = 5;
    private int counter = 0;

    [SerializeField]
    private float aliveTime = 1.5f;
    [SerializeField] private bool isFromMachineGun;
    [Header("Audio")]
    [SerializeField] private AudioClip shootClip;
    [SerializeField] private List<AudioClip> machineGunClips;



    private void OnEnable()
    {
        if (counter == 0) { counter++; }
        else
        {
            //AUDIO
            if (!isFromMachineGun)
                AudioManager.instance.PlayAudioClipWithPosition(shootClip, transform.position);
            else
            {
                AudioClip randomMachineGunSound = machineGunClips[Random.Range(0, machineGunClips.Count)];
                AudioManager.instance.PlayAudioClipWithPosition(randomMachineGunSound, transform.position);
            }
            StartCoroutine(CooldownProjectile());
        }
        
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
