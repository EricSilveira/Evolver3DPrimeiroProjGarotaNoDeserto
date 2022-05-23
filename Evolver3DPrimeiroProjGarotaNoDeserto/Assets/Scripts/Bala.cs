using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//para fazer com que o rigidbody apareca no sprite ou pode fazer direto pela unity mesmo lembrando que tem que tirar a gravidade colocando em 0 para o objeto não cair
[RequireComponent (typeof(Rigidbody2D))]

public class Bala : MonoBehaviour
{
    //para definir os valores na plataforma unity
    [SerializeField]
    //para a velocidade que a munição ira ir
    private float velocidade;
    //para a direção da bala caso esteja virado para esquerda a bala vai para esquerda e vice versa
    private Vector2 direcao;
    //
    private Rigidbody2D rb;
    //para definir os valores na plataforma unity
    [SerializeField]
    //
    private GameObject explosao;
    //
    private float tempo = 0.8f;



    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //inicializado para a direita pois o personagem esta virado para a direita.
        //direcao = Vector2.right;   // retirado pois esta sendo colocado no metodo inicializar
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.velocity = direcao * velocidade;
    }

    //public para que a outra classe possa enxergar o metodo
    public void Inicializacao(Vector2 _direcao)
    {
        direcao = _direcao;

    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D outro)
    {
        if (outro.gameObject.tag == "Inimigo")
        {
            if (explosao != null)
            explosao.SetActive(true);
            StartCoroutine("Destruir");
        }

        if (outro.gameObject.tag == "Player")
        {
            StartCoroutine("Destruir");
        }
    }

    IEnumerator Destruir()
    {
        yield return new WaitForSeconds(tempo);
        Destroy(gameObject);
    }
}
