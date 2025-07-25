using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Music")]
    public AudioClip backgroundMusic;
    private AudioSource musicSource;

    [Header("Sound Effects")]
    public AudioClip EmptyClip;
    public AudioClip coinSound;
    public AudioClip shotSound;          
    public AudioClip explosionSound;     
    public AudioClip cricketsSound;      
    public AudioClip waterSound;         
    public AudioClip pickupSound;
    public AudioClip pickupFailSound;
    public AudioClip pickupSuccessSound;
    public AudioClip enemyDeathSound;
    public AudioClip enemyDefeatedSound;
    public AudioClip playerDeathSound;
    public AudioClip playerDefeatedSound;

    [Header("Collections (randomized)")]
    public AudioClip[] otherSoundEffects;
    public AudioClip[] environmentSounds;
    public AudioClip[] enemySounds;
    public AudioClip[] playerSounds;

    private AudioSource cricketsSource;
    private AudioSource waterSource;

    void Awake()
    {
        // --- Singleton setup ---
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

        // --- Music Source ---
        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.clip = backgroundMusic;
        musicSource.loop = true;
        musicSource.volume = 0.5f;

        // --- Environment Sources ---
        cricketsSource = gameObject.AddComponent<AudioSource>();
        cricketsSource.clip = cricketsSound;
        cricketsSource.loop = true;
        cricketsSource.spatialBlend = 0f; // 2D
        cricketsSource.volume = 0.3f;

        waterSource = gameObject.AddComponent<AudioSource>();
        waterSource.clip = waterSound;
        waterSource.loop = true;
        waterSource.spatialBlend = 0f; // 2D
        waterSource.volume = 0.4f;
    }

    void Start()
    {
        // Auto-play background music
        if (backgroundMusic != null)
            PlayMusic();
    }

    // ===== MUSIC CONTROLS =====
    public void PlayMusic()  => musicSource.Play();
    public void StopMusic()  => musicSource.Stop();
    public void PauseMusic() => musicSource.Pause();
    public void ResumeMusic() => musicSource.UnPause();

    // ===== GENERIC ONE-SHOT SFX =====
    public void PlaySFX(AudioClip clip, float volume = 1f)
    {
        if (clip == null) return;
        GameObject tempGO = new GameObject("TempAudio");
        tempGO.transform.position = Camera.main.transform.position;
        AudioSource src = tempGO.AddComponent<AudioSource>();
        src.spatialBlend = 0f; // one-shot SFX are 2D by default
        src.PlayOneShot(clip, volume);
        Destroy(tempGO, clip.length + 0.1f);
    }

    // ===== ENVIRONMENT SOUND CONTROLS =====
    public void PlayCrickets() => cricketsSource.Play();
    public void StopCrickets() => cricketsSource.Stop();
    public void PlayWater()    => waterSource.Play();
    public void StopWater()    => waterSource.Stop();

    // ===== SPECIFIC SFX METHODS =====
    public void PlayEmptyClipSound()         => PlaySFX(EmptyClip);
    public void PlayCoinSound()         => PlaySFX(coinSound);
    public void PlayShotSound()         => PlaySFX(shotSound);
    public void PlayExplosionSound()    => PlaySFX(explosionSound);
    public void PlayPickupSound()       => PlaySFX(pickupSound);
    public void PlayPickupFailSound()   => PlaySFX(pickupFailSound);
    public void PlayPickupSuccessSound()=> PlaySFX(pickupSuccessSound);
    public void PlayEnemyDeathSound()   => PlaySFX(enemyDeathSound);
    public void PlayEnemyDefeatedSound(float volume = 1f)=> PlaySFX(enemyDefeatedSound, volume);
    public void PlayPlayerDeathSound()  => PlaySFX(playerDeathSound);
    public void PlayPlayerDefeatedSound()=> PlaySFX(playerDefeatedSound);

    // ===== RANDOMIZED COLLECTION PLAYERS =====
    public void PlayRandomOther()
    {
        if (otherSoundEffects.Length == 0) return;
        var clip = otherSoundEffects[Random.Range(0, otherSoundEffects.Length)];
        PlaySFX(clip);
    }

    public void PlayRandomEnvironment()
    {
        if (environmentSounds.Length == 0) return;
        var clip = environmentSounds[Random.Range(0, environmentSounds.Length)];
        PlaySFX(clip);
    }

    public void PlayRandomEnemySFX()
    {
        if (enemySounds.Length == 0) return;
        var clip = enemySounds[Random.Range(0, enemySounds.Length)];
        PlaySFX(clip);
    }

    public void PlayRandomPlayerSFX()
    {
        if (playerSounds.Length == 0) return;
        var clip = playerSounds[Random.Range(0, playerSounds.Length)];
        PlaySFX(clip);
    }
}