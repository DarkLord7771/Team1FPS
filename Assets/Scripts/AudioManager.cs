using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource aud;
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource sfxSource;
    [SerializeField] AudioSource menuSource;

    [Header("-----BackGround Music-----")]
    [SerializeField] AudioClip[] musicAud;
    bool playingSong;

    [Header("-----Player Sounds-----")]
    [SerializeField] AudioClip[] audJump;
    [SerializeField] AudioClip[] audHurt;
    [SerializeField] AudioClip[] audSteps;
    [SerializeField] AudioClip audShoot;
    [SerializeField] AudioClip audPowerUp;
    [SerializeField] AudioClip audHeal;

   
    [Header("-----Shop Sounds-----")]
    [SerializeField] AudioClip shopGoodAud;
    [SerializeField] AudioClip shopBadAud;

    [Header("-----Menu Sounds-----")]
    [SerializeField] AudioClip menuAud;


    [Header("-----Volume Controls-----")]
    [Range(0, 1)] [SerializeField] float musicVol;
    [Range(0, 1)] [SerializeField] float menuSFXVol;
    [Range(0, 1)] [SerializeField] float gameSFXVol;


    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (!playingSong)
        {
            StartCoroutine(PlayBackgroundMusic());
        }
    }

    public IEnumerator PlayBackgroundMusic() //Background Music
    {
        playingSong = true;
        int songChoice = Random.Range(0, musicAud.Length);
        musicSource.PlayOneShot(musicAud[songChoice]);
        yield return new WaitForSeconds(musicAud[songChoice].length);
        playingSong = false;
    }

    public void PlayFootSteps() //Player Footsteps
    {
        sfxSource.PlayOneShot(audSteps[Random.Range(0, audSteps.Length)], gameSFXVol);
    }

    public void PlayHurtSound() //Player Hurt Sounds
    {
        sfxSource.PlayOneShot(audHurt[Random.Range(0, audHurt.Length)], gameSFXVol);
    }

    public void PlayJumpSound() //Player Jump Sounds
    {
        sfxSource.PlayOneShot(audJump[Random.Range(0, audJump.Length)], gameSFXVol);
    }

    public void PlayShootSound()
    {
        sfxSource.PlayOneShot(audShoot, gameSFXVol);
    }

    public void PlayPowerUpSound()
    {
        sfxSource.PlayOneShot(audPowerUp, gameSFXVol);
    }

    public void PlayHealSound()
    {
        sfxSource.PlayOneShot(audHeal, gameSFXVol);
    }

    public void PlayShopGoodSound() //Plays successful buy sound
    {
        sfxSource.PlayOneShot(shopGoodAud, menuSFXVol);
    }

    public void PlayShopBadSound() //Plays unsuccessful buy sound
    {
        sfxSource.PlayOneShot(shopBadAud, menuSFXVol);
    }

    public void PlayMenuSound() //Plays menu sound
    {
        sfxSource.PlayOneShot(menuAud, menuSFXVol);
    }
}
