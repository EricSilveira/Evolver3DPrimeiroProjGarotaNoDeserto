using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb2d;
    //para receber a tecla pressionada esquerda ou direita 
    float horizontal;
    //para liberar no unity para definir valor para private é usado comando abaixo
    [SerializeField]
    //somente o player pode acessar por ser private
    private float velocidade = 0;
    //para liberar no unity para definir valor para private é usado comando abaixo
    [SerializeField]
    //a posição que sera instanciada a bala definida nessa variavel
    private GameObject posicaoBala;
    //para liberar no unity para definir valor para private é usado comando abaixo
    [SerializeField]
    //a bala sera definida aqui
    private GameObject bala;
    //definir lado que o personagem esta olhando
    private bool ladoDireito;
    //
    private bool acao;
    //Usado public para debugar o correto e deixar tudo privateas variaveis abaixo
    //para identificar a plataforma
    public LayerMask plataforma;
    //para identificar se esta pisando na plataforma
    public Vector2 pontoColisaoPiso = Vector2.zero;
    //verificar se realmente esta no chão
    public bool estaNoPiso;
    //para mostrar que esta colidindo um collider no outro
    public float raio;
    //para realizar debug
    public Color debugColisao = Color.red;
    // 
    public float forcaPulo = 300f;
    //
    public int vida = 3;

    // Start is called before the first frame update
    void Start()
    {
        //pegamos a referencia do objeto
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        //toda imagem ja começa com a escala positiva idependente de qual lado esta virada como esta imagem do personagem esta virada para direita(pode mudar na ferramenta)
        //então caso seja maior que 0 sera verdadeiro, aqui iniciamos com verdadeiro a variavel booleana
        ladoDireito = transform.localScale.x > 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // este texto Horizontal esta definido no project setings input na unity (referindo-se ao eixo(escala) X)
        horizontal = Input.GetAxis("Horizontal");
        //para se movimentar
        Movimentar(horizontal);
        //para virar a imagem para a direção que esta indo
        MudarDirecao(horizontal);
        //para confirmar se esta no chão
        EstaNoPiso();

    }

    private void Update()
    {
        //para controle do animator
        ControlarLayers();
        //para verificar o que foi pressionado
        ControlarEntradas();
        //para efetuar a ação de atirar
        Acao();
        //desabilitar o atirar
        Resetar();
    }

    private void EstaNoPiso()
    {
        //para acompanhar o personagem
        var pontoPosicao = pontoColisaoPiso;
        pontoPosicao.x += transform.position.x;
        pontoPosicao.y += transform.position.y;

        estaNoPiso = Physics2D.OverlapCircle(pontoPosicao, raio, plataforma);
        //valida se esta caindo de um pulo ou plataforma
        Cair();
    }

    private void Pular()
    {
        //para colocar a força da gravidade no pulo
        rb2d.gravityScale = 1.6f;
        //valida se esta no chao e se o eixo y esta menor igual a zero
        if(estaNoPiso && rb2d.velocity.y <= 0)
        {
            //aqui é adicionado o valor definido no eixo y 
            rb2d.AddForce(new Vector2(0, forcaPulo));
            animator.SetTrigger("Pular");
        }
    }

    private void Cair()
    {
        if(!estaNoPiso && rb2d.velocity.y <= 0)
        {
            animator.SetBool("Cair", true);
            animator.ResetTrigger("Pular");
        }
        if (estaNoPiso)
        {
            animator.SetBool("Cair", false);
        }
    }

    private void ControlarEntradas()
    {
        if (Input.GetButtonDown("Jump"))
        {
            Pular();
        }
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            acao = true;
        }
    }

    private void Resetar()
    {
        acao = false;
    }

    private void Movimentar(float h)
    {
        //se não esta atirando pode se movimentar
        if (!animator.GetCurrentAnimatorStateInfo(0).IsTag("Atirar"))
        { 
             //Movimentação do personagem
            rb2d.velocity = new Vector2(h * velocidade, rb2d.velocity.y);
        }
        //seleciona a transição da animação no caso Velocidade e usa o valor oabsoluto com o Mathf.Abs pois para direita temos positivos e para esquerda negativo
        animator.SetFloat("Velocidade", Mathf.Abs(h));

        if (rb2d.velocity.y == 0)
        {
            rb2d.gravityScale = 1.0f;
        }
    }

    private void MudarDirecao(float horizontal)
    {
        //se o botão apertado é maior que zero e não esta virado para direita
        //se o botão apertado é menor que zero e esta virado para direita
        if (horizontal > 0 && !ladoDireito || horizontal < 0 && ladoDireito)
        {
            //se for verdadeiro vai virar falso se for falso vai virar verdadeiro
            ladoDireito = !ladoDireito;
            //aqui transforma a escala x para o valor positivo ou negativo, pois com o sinal menos colocado ira inverter o valor ja a escala y é mantida assim como o z
            transform.localScale = new Vector3(- transform.localScale.x, transform.localScale.y, transform.localScale.z);

        }
    }

    private void ControlarLayers()
    {
        if (!estaNoPiso)
        {
            //esta verificando a camada que seria a 1 (no caso aqui camada 0 é piso e a camada 1 pular)
            animator.SetLayerWeight(1, 1);
        }
        else
        {
            animator.SetLayerWeight(1, 0);
        }
    }

    private void Acao()
    {
        if (acao && !animator.GetCurrentAnimatorStateInfo(0).IsTag("Atirar"))
        {
            animator.SetTrigger("Atirar");
            rb2d.velocity = Vector2.zero;
            //instancia a munição fazendo com que apareça na animação
            AcaoAtirar();
        }
    }

    private void AcaoAtirar()
    {
        if (ladoDireito)
        {
            GameObject tempBala = (GameObject)(Instantiate(bala, posicaoBala.transform.position, Quaternion.identity));
            tempBala.GetComponent<Bala>().Inicializacao(Vector2.right);
        }
        else
        {
            GameObject tempBala = (GameObject)(Instantiate(bala, posicaoBala.transform.position, Quaternion.Euler(new Vector3(0,0,180))));
            tempBala.GetComponent<Bala>().Inicializacao(Vector2.left);
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("ProjetilInimigo"))
        {
            vida--;
            Destroy(collision.gameObject);

            if (vida > 0)
            {
                animator.SetLayerWeight(2, 1);
                animator.SetTrigger("Dano");
            }
            else
            {
                animator.SetTrigger("Morrer");
            }
        }
    }

    public void SetLayerDano()
    {
        animator.SetLayerWeight(1, 0);    
    }

    //Para desenhar na tela nesse caso o esta no chão umma especie de debug
    private void OnDrawGizmos()
    {
        Gizmos.color = debugColisao;
        var pontoPosicao = pontoColisaoPiso;
        pontoPosicao.x += transform.position.x;
        pontoPosicao.y += transform.position.y;
        Gizmos.DrawWireSphere(pontoPosicao, raio);

    }
}
