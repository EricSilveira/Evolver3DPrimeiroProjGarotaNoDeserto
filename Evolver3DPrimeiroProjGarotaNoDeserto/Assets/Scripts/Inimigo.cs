using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inimigo : MonoBehaviour
{
    //definir lado que o personagem esta olhando
    private bool ladoDireito;
    //para definir os valores na plataforma unity
    [SerializeField]
    //para definir a velocidade
    private float velocidade;
    // para trocar animação
    private Animator animator;
    //
    private float tempoIdle;
    private float duracaoIdle = 3;
    //
    private float tempoPatrulhar;
    private float duracaoPatrulhar = 6;
    //
    private float tempoAtacar;
    //para definir os valores na plataforma unity
    [SerializeField]
    private float duracaoAtacar;
    //
    public bool estaPatrulhando;
    //para definir os valores na plataforma unity
    [SerializeField]
    public bool atacar;
    //
    public float playerDistancia;
    public float ataqueDistancia = 6;
    //
    public GameObject player;
    //
    public GameObject prefabProjetil;
    public Transform instanciador;
    //
    public int dano;
    public int vida;
    

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        ladoDireito = true;
        estaPatrulhando = false;
        atacar = false;
    }

    // Update is called once per frame
    void Update()
    {
        MudarEstado();
        //pegando a distancia entre o inimigo e o player
        playerDistancia = transform.position.x - player.transform.position.x;
        //
        if (Mathf.Abs(playerDistancia) < ataqueDistancia)
        {
            atacar = true;
            estaPatrulhando = false;
            Idle();
            Atirar();
        }
        else
        {
            MudarEstado();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Limite")
        {
            MudarDirecao();
        }

        if (collision.tag == "ProjetilPlayer")
        {
            Dano();
        }
    }

    private void Dano()
    {
        vida -= dano;
        if (vida <= 0)
        {
            animator.SetTrigger("Morrendo");
        }
        else
        {
            animator.SetLayerWeight(1, 1);
            animator.SetTrigger("Dano");
        }
    }

    public void DesativarInimigo()
    {
        gameObject.SetActive(false);
    }

    public void SetLayerDano()
    {
        animator.SetLayerWeight(1, 0);
    }

    private void Mover()
    {
        transform.Translate(PegarDirecao() * (velocidade * Time.deltaTime));
    }


    private Vector2 PegarDirecao()
    {
        return ladoDireito ? Vector2.right : Vector2.left;
    }

    private void MudarDirecao()
    {

        //se for verdadeiro vai virar falso se for falso vai virar verdadeiro
        ladoDireito = !ladoDireito;
        //aqui transforma a escala x para o valor positivo ou negativo, pois com o sinal menos colocado ira inverter o valor ja a escala y é mantida assim como o z
        this.transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);

    }

    // metodos para animação e ação
    private void Idle()
    {
        tempoIdle += Time.deltaTime;
        if (tempoIdle <= duracaoIdle)
        {
            animator.SetBool("EstaPatrulhando", estaPatrulhando);
            tempoPatrulhar = 0;
        }
        else
        {
            estaPatrulhando = true;
        }
    }

    private void Patrulhando()
    {
        tempoPatrulhar += Time.deltaTime;
        if (tempoPatrulhar <= duracaoPatrulhar)
        {
            animator.SetBool("EstaPatrulhando", estaPatrulhando);
            Mover();
            tempoIdle = 0;
        }
        else
        {
            estaPatrulhando = false;
        }
    }

    private void MudarEstado()
    {
        if (!atacar)
        {
            if (!estaPatrulhando)
            {
                Idle();
            }
            else
            {
                Patrulhando();
            }
        }
    }

    private void Atirar()
    {
        if (playerDistancia < 0 && !ladoDireito || playerDistancia > 0 && ladoDireito)
        {
            MudarDirecao();
        }

        if (atacar)
        {
            tempoAtacar += Time.deltaTime;
            if (tempoAtacar >= duracaoAtacar)
            {
                animator.SetTrigger("EstaAtirando");
                tempoAtacar = 0;
            }
        }
    }

    private void ResetarAtaque()
    {
        atacar = false;
    }

    public void InstanciarProjetil()
    {

        if (ladoDireito)
        {
            GameObject tmp = Instantiate(prefabProjetil, instanciador.position, instanciador.rotation);
            tmp.GetComponent<Bala>().Inicializacao(Vector2.right);
        }
        else
        {
            GameObject tmp = Instantiate(prefabProjetil, instanciador.position, Quaternion.Euler(new Vector3(0, 0, 180)));
            tmp.GetComponent<Bala>().Inicializacao(Vector2.left);
        }
    }
}
