using System.Collections;
using System.Collections.Generic;
using DG.Tweening; 
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuInicio : MonoBehaviour
{

    public AudioClip sonidoClick;
    public AudioSource audioSource;

    public Camera camaraPrincipal;
    public GameObject bola;

    
    // Start is called before the first frame update
    void Start()
    {
        
        //hacer que la bola sea empujada hacia a partir de z -1.42 hacia adelante z hasta 0.5
        bola.transform.DOMoveZ(0.5f, 1f);


        //hacer una animacion en la camara en rotacion
        camaraPrincipal.transform.DOMove(new Vector3(1.92f, 1.17f, 2.3f), 1f).SetEase(Ease.InOutBack);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void IniciarJuego()
    {
        audioSource.PlayOneShot(sonidoClick, 1f);
        StartCoroutine(LoadScene(2f));
       
    }
 
    private IEnumerator LoadScene(float time)
    {
        //Wait for random amount of time
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene("nivel-1");
    }

    public void ejecutarSonido(AudioClip sonido, float volumen = 1f)
    {
        StartCoroutine(ExecSound(sonido, 1f, volumen));
    }

    IEnumerator ExecSound(AudioClip sonido, float delay, float volumen)
    {
        //esperar el delay despues de ejecutar el sonido
        audioSource.PlayOneShot(sonido, volumen);
        yield return new WaitForSeconds(delay); 

    }
}
