using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goomba : MonoBehaviour
{
    private Rigidbody2D fisicas;
    private Animator animator;
    public GameObject PuntoA;
    public GameObject PuntoB;
    public float velocidad;
    private Transform posicionActual;
    public MarioScript Mario;

    // Start is called before the first frame update
    void Start()
    {
        fisicas = GetComponent<Rigidbody2D>();
        posicionActual = PuntoB.transform;
        animator = GetComponent<Animator>();
    }

    // Movimiento del Enemigo.................................................................................................................................................................
    void Update()
    {
        Vector2 posicion = posicionActual.position - transform.position;
        if (posicionActual == PuntoB.transform)
        {
            fisicas.velocity = new Vector2(velocidad, 0);
        }
        else
        {
            fisicas.velocity = new Vector2(-velocidad, 0);
        }

        if (Vector2.Distance(transform.position, posicionActual.position) < 0.5f && posicionActual == PuntoB.transform)
        {
            girar();
            posicionActual = PuntoA.transform;
        }
        else if (Vector2.Distance(transform.position, posicionActual.position) < 0.5f && posicionActual == PuntoA.transform)
        {
            girar();
            posicionActual = PuntoB.transform;
        }
    }
    
    private void girar()
    {
        Vector3 ubicar = transform.localScale;
        ubicar.x *= -1;
        transform.localScale = ubicar;
    }

    // Muerte del Enemigo.................................................................................................................................................................
    
    public void Morir()
    {
        animator.SetBool("GoombaMuere", true);
        velocidad = 0;
        Destroy(gameObject, 0.5f);
    }


    // Gizmos para el recorrido.................................................................................................................................................................
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(PuntoA.transform.position, 0.5f);
        Gizmos.DrawWireSphere(PuntoB.transform.position, 0.5f);
        Gizmos.DrawLine(PuntoA.transform.position, PuntoB.transform.position);
    }
}
