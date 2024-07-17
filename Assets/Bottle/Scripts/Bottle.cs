using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bottle : MonoBehaviour
{
    [SerializeField] GameObject brokenBottlePrefab;

    public AudioClip bottleBreakSound;

    public AudioSource audioSource;

    void Start()
    {
        // audioSource = GetComponent<AudioSource>();
    }

    void Update() // just for testing
    {
        // if(Input.GetKeyDown(KeyCode.K))
        // {
        //     Explode();
        // }

        //si la botella cae al vació se romperá o si choca en el eje x con otra botella

        if (transform.position.y < -2 || transform.rotation.x < -2 || transform.rotation.x > 2)
        {
            Explode();
        }

    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Explode();
        }
    }

    public void Explode()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(bottleBreakSound, 1f);
        }
        //add a taptic feedback  here
        // Handheld.Vibrate();
        if (!GameManager.instance.isGamePaused && !GameManager.instance.isLevelFinished)
        {
            GameManager.instance.UpdateScore(1.0f);
        }

        GameObject brokenBottle = Instantiate(brokenBottlePrefab, this.transform.position, Quaternion.identity);
        brokenBottle.GetComponent<BrokenBottle>().RandomVelocities();
        Destroy(gameObject);
    }
}
