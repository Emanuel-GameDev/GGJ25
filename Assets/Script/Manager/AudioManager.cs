using UnityEngine;
using UnityEngine.Audio;

namespace Managers
{
    public enum AudioType
    {
        Music,
        Effect
    }

    public class AudioManager : MonoBehaviour
    {
        public static AudioManager instance { get; private set; }
        public GameObject prefabEmpty;



        [Header("AudioMixers")]
        [SerializeField] AudioMixerGroup audioMixerMusic;
        [SerializeField] AudioMixerGroup audioMixerEffect;

        [Header("LevelMusic")]
        public AudioClip clipOSTLevel_1;



        private void Awake()
        {
            if (instance != null)
            {
                Debug.Log("trovato gia un AudioManager nella scena");
            }
            else
            {
                instance = this;

            }
        }

        private void Start()
        {
            PlayOSTLevel_1();
        }


        public void PlayAudioClip(AudioClip clipToPLay)
        {
            GameObject temp = Instantiate(prefabEmpty, Camera.main.transform.position, Quaternion.identity);
            temp.AddComponent<AudioSource>();
            temp.GetComponent<AudioSource>().clip = clipToPLay;

            //switch (audioType)            //forse ci serve piu in la,vediamo visto che qua passano solo effetti
            //{
            //    case AudioType.Music:
            //        temp.GetComponent<AudioSource>().outputAudioMixerGroup = audioMixerMusic;
            //        break;
            //    case AudioType.Effect:
            //        temp.GetComponent<AudioSource>().outputAudioMixerGroup = audioMixerEffect;
            //        break;
            //    default:
            //        break;
            //}

            temp.GetComponent<AudioSource>().outputAudioMixerGroup = audioMixerEffect;


            temp.GetComponent<AudioSource>().Play();

            Destroy(temp, clipToPLay.length);
        }
        public void PlayAudioClipWithPosition(AudioClip clipToPLay, Vector3 position)
        {
            GameObject temp = Instantiate(prefabEmpty, position, Quaternion.identity);
            AudioSource source = temp.AddComponent<AudioSource>();
            source.clip = clipToPLay;

            //switch (audioType)            //forse ci serve piu in la,vediamo visto che qua passano solo effetti
            //{
            //    case AudioType.Music:
            //        temp.GetComponent<AudioSource>().outputAudioMixerGroup = audioMixerMusic;
            //        break;
            //    case AudioType.Effect:
            //        temp.GetComponent<AudioSource>().outputAudioMixerGroup = audioMixerEffect;
            //        break;
            //    default:
            //        break;
            //}

            source.outputAudioMixerGroup = audioMixerEffect;


            source.Play();

            Destroy(temp, clipToPLay.length);
        }

        public void PlayOSTLevel_1()
        {
            GameObject temp = Instantiate(prefabEmpty);
            temp.gameObject.name = "aaaaaaaaaaaaaaaaaaaaaaaaaaaa";
            temp.AddComponent<AudioSource>().clip = clipOSTLevel_1;
            temp.GetComponent<AudioSource>().loop = true;

            temp.GetComponent<AudioSource>().outputAudioMixerGroup = audioMixerMusic;

            temp.GetComponent<AudioSource>().Play();


        }

    }
}