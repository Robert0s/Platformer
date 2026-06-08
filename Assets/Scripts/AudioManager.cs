using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Music")]
    [SerializeField] private AudioClip gameMusic;

    [Header("Player SFX")]
    [SerializeField] public AudioClip jumpSFX;
    [SerializeField] public AudioClip landSFX;
    [SerializeField] public AudioClip hitSFX;
    [SerializeField] public AudioClip attackSFX;

    [Header("Buff SFX")]
    [SerializeField] public AudioClip shieldSFX;
    [SerializeField] public AudioClip speedUpSFX;

    [Header("Item SFX")]
    [SerializeField] public AudioClip equipSFX;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        if (gameMusic != null)
        {
            musicSource.clip = gameMusic;
            musicSource.loop = true;
            musicSource.Play();
        }
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip == null) return;
        sfxSource.PlayOneShot(clip);
    }
}