/***================== Indice do codigo para entendimento ====================***/
/*** 1.0   - Movimentacao                                                     ***/
/*****                                                                        ***/
/*** 2.0   - Pulo                                                             ***/
/*****                                                                        ***/
/*** 3.0   - Ataque                                                           ***/
/*****                                                                        ***/
/*** 4.0   - Dano                                                             ***/
/*****                                                                        ***/
/*** 5.0   - Configuracao para unity                                          ***/
/*****                                                                        ***/
/*** 6.0   - Controle da plataforma                                           ***/
/*****                                                                        ***/
/**================== Fim Indice do codigo para entendimento =================***/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb2d;
    //1.0   - Para receber a tecla pressionada esquerda ou direita 
    float horizontal;
    //5.0   - Para liberar no unity para definir valor para private é usado comando abaixo
    [SerializeField]
    //1.0   - Para a velocidade do movimento (somente o player pode acessar por ser private) 
    private float velocidade = 0;
    //5.0   - Para liberar no unity para definir valor para private é usado comando abaixo
    [SerializeField]
    //3.0   - A posição que sera instanciada a bala definida nessa variavel
    private GameObject posicaoBala;
    //5.0   - Para liberar no unity para definir valor para private é usado comando abaixo
    [SerializeField]
    //3.0   - A bala sera definida aqui
    private GameObject bala;
    //1.0   - Definir lado que o personagem esta olhando
    private bool ladoDireito;
    //3.0   - Para setar quando pode atirar
    private bool acao;
    //6.0   - Usado public para debugar o correto e deixar tudo privateas variaveis abaixo
    //6.0   - para identificar a plataforma
    public LayerMask plataforma;
    //6.0   - para identificar se esta pisando na plataforma
    public Vector2 pontoColisaoPiso = Vector2.zero;
    //2.0   - Verificar se realmente esta no chão
    public bool estaNoPiso;
    //2.0   - Para mostrar que esta colidindo um collider no outro
    public float raio;
    //2.0   - Para realizar debug
    public Color debugColisao = Color.red;
    //2.0   - Para a foca usada no pulo
    public float forcaPulo = 300f;
    //4.0   - Para guardar a vida do personagem
    public int vida = 3;

    void Start(){
        //1.0   - pegamos a referencia do objeto
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        //1.0   - toda imagem ja começa com a escala positiva idependente de qual lado esta virada como esta imagem do personagem esta virada para direita(pode mudar na ferramenta)
        //1.0   - então caso seja maior que 0 sera verdadeiro, aqui iniciamos com verdadeiro a variavel booleana
        ladoDireito = transform.localScale.x > 0;
    }

    void FixedUpdate(){
        //1.0   - Este texto Horizontal esta definido no project setings input na unity (referindo-se ao eixo(escala) X)
        horizontal = Input.GetAxis("Horizontal");
        //1.0   - Para se movimentar
        Movimentar(horizontal);
        //1.0   - Para virar a imagem para a direção que esta indo
        MudarDirecao(horizontal);
        //2.0   - para confirmar se esta no chão
        EstaNoPiso();
    }

    private void Update(){
        //2.0   - Para controle do animator
        ControlarLayers();
        //5.0   - para verificar o que foi pressionado
        ControlarEntradas();
        //3.0   - Para efetuar a ação de atirar
        Acao();
        //3.0   - Desabilitar o atirar
        Resetar();
    }

    private void EstaNoPiso(){
        //2.0   - para acompanhar o personagem
        var pontoPosicao = pontoColisaoPiso;
        pontoPosicao.x += transform.position.x;
        pontoPosicao.y += transform.position.y;

        estaNoPiso = Physics2D.OverlapCircle(pontoPosicao, raio, plataforma);
        //2.0   - valida se esta caindo de um pulo ou plataforma
        Cair();
    }

    private void Pular(){
        //2.0   - para colocar a força da gravidade no pulo
        rb2d.gravityScale = 1.6f;
        //2.0   - valida se esta no chao e se o eixo y esta menor igual a zero
        if (estaNoPiso && rb2d.velocity.y <= 0){
            //2.0   - aqui é adicionado o valor definido no eixo y 
            rb2d.AddForce(new Vector2(0, forcaPulo));
            animator.SetTrigger("Pular");
        }
    }

    private void Cair(){
        if(!estaNoPiso && rb2d.velocity.y <= 0){
            animator.SetBool("Cair", true);
            animator.ResetTrigger("Pular");
        }
        if (estaNoPiso){
            animator.SetBool("Cair", false);
        }
    }

    private void ControlarEntradas(){
        //2.0   - Quando pressionado a tecla de jump realiza a funcao pular
        if (Input.GetButtonDown("Jump")){
            Pular();
        }

        //3.0   - Quando pressionado a tecla alt esquerdo coloca verdadeira na acao 
        if (Input.GetKeyDown(KeyCode.LeftAlt)){
            acao = true;
        }
    }

    private void Resetar(){
        acao = false;
    }

    private void Movimentar(float h){
        //3.0   - Se não esta atirando pode se movimentar
        if (!animator.GetCurrentAnimatorStateInfo(0).IsTag("Atirar")){
            //1.0   - Movimentação do personagem
            rb2d.velocity = new Vector2(h * velocidade, rb2d.velocity.y);
        }
        //1.0   - seleciona a transição da animação no caso Velocidade e usa o valor oabsoluto com o Mathf.Abs pois para direita temos positivos e para esquerda negativo
        animator.SetFloat("Velocidade", Mathf.Abs(h));

        if (rb2d.velocity.y == 0){
            rb2d.gravityScale = 1.0f;
        }
    }

    private void MudarDirecao(float horizontal){
        //1.0   - se o botão apertado é maior que zero e não esta virado para direita
        //1.0   - se o botão apertado é menor que zero e esta virado para direita
        if (horizontal > 0 && !ladoDireito || horizontal < 0 && ladoDireito){
            //1.0   - se for verdadeiro vai virar falso se for falso vai virar verdadeiro
            ladoDireito = !ladoDireito;
            //1.0   - aqui transforma a escala x para o valor positivo ou negativo, pois com o sinal menos colocado ira inverter o valor ja a escala y é mantida assim como o z
            transform.localScale = new Vector3(- transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
    }

    private void ControlarLayers(){
        if (!estaNoPiso){
            //2.0   - Esta verificando a camada que seria a 1 (no caso aqui camada 0 é piso e a camada 1 pular)
            animator.SetLayerWeight(1, 1);
        } else{
            animator.SetLayerWeight(1, 0);
        }
    }

    private void Acao(){
        if (acao && !animator.GetCurrentAnimatorStateInfo(0).IsTag("Atirar")){
            animator.SetTrigger("Atirar");
            rb2d.velocity = Vector2.zero;
            //3.0   - Instancia a munição fazendo com que apareça na animação
            AcaoAtirar();
        }
    }

    private void AcaoAtirar(){
        if (ladoDireito){
            GameObject tempBala = (GameObject)(Instantiate(bala, posicaoBala.transform.position, Quaternion.identity));
            tempBala.GetComponent<Bala>().Inicializacao(Vector2.right);
        } else{
            GameObject tempBala = (GameObject)(Instantiate(bala, posicaoBala.transform.position, Quaternion.Euler(new Vector3(0,0,180))));
            tempBala.GetComponent<Bala>().Inicializacao(Vector2.left);
        }
    }

    //4.0   - Quando o projetil acerta o player retirado a vida ateh que acabe chegar o zero acontecendo isso ira dar game over
    public void OnTriggerEnter2D(Collider2D collision){
        if (collision.gameObject.CompareTag("ProjetilInimigo")){
            vida--;
            Destroy(collision.gameObject);

            if (vida > 0){
                animator.SetLayerWeight(2, 1);
                animator.SetTrigger("Dano");
            } else{
                animator.SetTrigger("Morrer");
            }
        }
    }

    public void SetLayerDano(){
        animator.SetLayerWeight(1, 0);    
    }

    //2.0   - Para desenhar na tela nesse caso o esta no chão umma especie de debug
    private void OnDrawGizmos(){
        Gizmos.color = debugColisao;
        var pontoPosicao = pontoColisaoPiso;
        pontoPosicao.x += transform.position.x;
        pontoPosicao.y += transform.position.y;
        Gizmos.DrawWireSphere(pontoPosicao, raio);
    }
}
