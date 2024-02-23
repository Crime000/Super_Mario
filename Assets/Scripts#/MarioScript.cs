using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MarioScript : MonoBehaviour
{

    public AnimatorOverrideController MarioGrande;
    public RuntimeAnimatorController MarioChikito;
    private Rigidbody2D movFisicas;
    private Animator animator;
    public AudioSource Audio;
    public AudioClip powerUp, audioSalto;
    public Goomba goomba;

    public float movX, fuerzaSalto, vidas = 4;
    private bool mirandoDerecha = true, PuedeSaltar = false, EsGrande = false, Vivo = true;

    

    // Start is called before the first frame update
    void Start()
    {
        movFisicas = GetComponent<Rigidbody2D>();

        animator = GetComponent<Animator>();

        MarioChikito = animator.runtimeAnimatorController;
    }

    // Update is called once per frame
    void Update()
    {
        // Declarar movimiento del jugador...
        movX = Input.GetAxis("Horizontal");

        if (Input.GetButtonDown("Jump") && PuedeSaltar == true)                //---Activar Salto
        {
            animator.SetBool("TocaSuelo", false);
            animator.SetBool("Jump", true);
            movFisicas.AddForce(new Vector2(movFisicas.velocity.x, fuerzaSalto));
            Audio.clip = audioSalto;
            Audio.Play();
            PuedeSaltar = false;
        }
    }

    void FixedUpdate()
    {
        // Mover al jugador................................................................................................................................................................................
        

        if (Vivo)
        { 
            Vector2 movimiento = new Vector2(movX * 3, movFisicas.velocity.y);
            movFisicas.velocity = movimiento;

        }
        else if(Vivo == false)
        {
            movFisicas.velocity = new Vector2(0, movFisicas.velocity.y);
        }
        //Animaciones de movimiento........................................................................................................................................................................
        if (movX == 0 && vidas > 0)
        {
           animator.SetBool("Walking", false);
        }
        else if (movX != 0 && vidas > 0)
        {
           animator.SetBool("Walking", true);
        }

        Orientacion();
    }

    // Cambiar la orientacion del personaje.................................................................................................................................................................
    void Orientacion()
    {
        if ((mirandoDerecha == true && movX < 0 && vidas > 0) || (mirandoDerecha == false && movX > 0 && vidas > 0))
        {
            mirandoDerecha = !mirandoDerecha;
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
    }

    // Muerte del personaje.................................................................................................................................................................
    public void Muerte()
    {
        Vivo = false;
        animator.SetBool("MarioMuere", true);
        movFisicas.AddForce(new Vector2(movFisicas.velocity.x, fuerzaSalto));
        Destroy(gameObject, 1f);
    }

    // Colisiones del personaje.................................................................................................................................................................
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Terrain"))
        {
            animator.SetBool("Jump", false);
            animator.SetBool("TocaSuelo", true);
            PuedeSaltar = true;
        }
        if (collision.gameObject.CompareTag("Goomba"))
        {
            if(EsGrande == true)
            {
                animator.runtimeAnimatorController = MarioChikito as RuntimeAnimatorController;
                movFisicas.AddForce(new Vector2(-movFisicas.velocity.x * fuerzaSalto, fuerzaSalto));
                EsGrande = false;
            }
            else
            {
                Muerte();
            }
        }
    }

    // Triggers del personaje.................................................................................................................................................................
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("champi"))
        {
            animator.runtimeAnimatorController = MarioGrande as RuntimeAnimatorController;
            EsGrande = true;
            Audio.clip = powerUp;
            Audio.Play();
        }
        if (collision.gameObject.CompareTag("Goomba"))
        {
            movFisicas.AddForce(new Vector2(movFisicas.velocity.x, fuerzaSalto));
            goomba.Morir();
        }
    }
}
