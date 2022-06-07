/***================== Indice do codigo para entendimento ====================***/
/*** 1.0   - Movimentacao                                                     ***/
/*****                                                                        ***/
/*** 2.0   - Ataque                                                           ***/
/*****                                                                        ***/
/*** 3.0   - Dano                                                             ***/
/*****                                                                        ***/
/*** 4.0   - Controle animacao                                                ***/
/*****                                                                        ***/
/*** 5.0   - Configuracao para unity                                          ***/
/*****                                                                        ***/
/**================== Fim Indice do codigo para entendimento =================***/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inimigo : MonoBehaviour
{
    //1.0   - definir lado que o personagem esta olhando
    private bool ladoDireito;
    //5.0   - para definir os valores na plataforma unity
    [SerializeField]
    //1.0   - para definir a velocidade
    private float velocidade;
    //4.0   -  para trocar animação
    private Animator animator;
    //4.0   - Para que fique parado
    private float tempoIdle;
    //4.0   - Para duracao do tempo Parado
    private float duracaoIdle = 3;
    //4.0   - Para que comece a caminhar
    private float tempoPatrulhar;
    //4.0   - Para o tempo que fique andando
    private float duracaoPatrulhar = 6;
    //2.0   - Para o tempo do ataque
    private float tempoAtacar;
    //5.0   - Para definir os valores na plataforma unity
    [SerializeField]
    //2.0   - Para o tempo que entre os ataques
    private float duracaoAtacar;
    //4.0   - Para saber se esta caminhando
    public bool estaPatrulhando;
    //5.0   - para definir os valores na plataforma unity
    [SerializeField]
    //2.0   - Para iniciar o ataque
    public bool atacar;
    //2.0   - Para saber a distancia entre o inimigo e o player
    public float playerDistancia;
    //2.0   - Para a distancia que comeca a atacar
    public float ataqueDistancia = 6;
    //2.0   - Para definir o que ira atacar
    public GameObject player;
    //2.0   - Para instanciar o projetil de ataque do inimigo
    public GameObject prefabProjetil;
    //2.0   - Para a posicao inicial da bala ira apresentar
    public Transform instanciador;
    //3.0   - Para o dano tomado
    public int dano;
    //3.0   - Para a morte
    public int vida;
    
    void Start(){
        animator = GetComponent<Animator>();
        ladoDireito = true;
        estaPatrulhando = false;
        atacar = false;
    }

    void Update() {
        MudarEstado();
        //2.0   - pegando a distancia entre o inimigo e o player
        playerDistancia = transform.position.x - player.transform.position.x;
        //2.0   - Verifica a distancia entre o inimigo e o player usando valor absoluto para que sempre veja como positivo
        if (Mathf.Abs(playerDistancia) < ataqueDistancia){
            atacar = true;
            estaPatrulhando = false;
            Idle();
            Atirar();
        } else{
            MudarEstado();
        }
    }

    //1.0   - Quando encontra o objeto limite muda de direcao da esquerda para direita vice e versa
    private void OnTriggerEnter2D(Collider2D collision){
        if (collision.tag == "Limite"){
            MudarDirecao();
        }

        //3.0   - Quando a bala do player acerta o inimigo ele recebe o dano
        if (collision.tag == "ProjetilPlayer"){
            Dano();
        }
    }

    //3.0   - Recebe o dano se a vida for menor ou igual a zero ele morre
    private void Dano(){
        vida -= dano;
        if (vida <= 0){
            animator.SetTrigger("Morrendo");
        } else{
            animator.SetLayerWeight(1, 1);
            animator.SetTrigger("Dano");
        }
    }

    //3.0   - Metodo usado quando setado morrendo, eh chamada esta funcao atraves de evento na animacao
    public void DesativarInimigo() {
        gameObject.SetActive(false);
    }

    //3.0   - Metodo usado quando setado dano, eh chamada esta funcao atraves de evento na animacao
    public void SetLayerDano() {
        animator.SetLayerWeight(1, 0);
    }

    private void Mover() {
        //1.0   - Ele pega a direcao se eh direita ou esqueda multiplica pela velocidade e pelo tempo de do delta time  
        transform.Translate(PegarDirecao() * (velocidade * Time.deltaTime));
    }

    private Vector2 PegarDirecao(){
        return ladoDireito ? Vector2.right : Vector2.left;
    }

    private void MudarDirecao() {
        //1.0   - se for verdadeiro vai virar falso se for falso vai virar verdadeiro
        ladoDireito = !ladoDireito;
        //1.0   - aqui transforma a escala x para o valor positivo ou negativo, pois com o sinal menos colocado ira inverter o valor ja a escala y é mantida assim como o z
        this.transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }

    //4.0   -   metodos para animação e ação
    private void MudarEstado(){
        if (!atacar){
            if (!estaPatrulhando){
                Idle();
            } else{
                Patrulhando();
            }
        }
    }

    //4.0   -  Ficar parado ira contar o tempo parado para mudar para caminhando
    private void Idle(){ 
        tempoIdle += Time.deltaTime;
        if (tempoIdle <= duracaoIdle) {
            animator.SetBool("EstaPatrulhando", estaPatrulhando);
            tempoPatrulhar = 0;
        } else{
            estaPatrulhando = true;
        }
    }

    //4.0   - Ficar caminhando ira contar o tempo caminhand para mudar para parado
    private void Patrulhando() {
        tempoPatrulhar += Time.deltaTime;
        if (tempoPatrulhar <= duracaoPatrulhar){
            animator.SetBool("EstaPatrulhando", estaPatrulhando);
            Mover();
            tempoIdle = 0;
        } else{
            estaPatrulhando = false;
        }
    }

    //2.0   - Metodo para atque verifica a distancia do player e o lado que o player esta
    private void Atirar() {
        if (playerDistancia < 0 && !ladoDireito || playerDistancia > 0 && ladoDireito) {
            MudarDirecao();
        }
        if (atacar) {
            tempoAtacar += Time.deltaTime;
            if (tempoAtacar >= duracaoAtacar) {
                animator.SetTrigger("EstaAtirando");
                tempoAtacar = 0;
            }
        }
    }

    //2.0   - Metodo usado no final da animacao de ataque, eh chamada esta funcao atraves de evento na animacao
    private void ResetarAtaque(){
        atacar = false;
    }

    //2.0   - Metodo usado no inicio da animacao de ataque, eh chamada esta funcao atraves de evento na animacao
    public void InstanciarProjetil(){
        if (ladoDireito){
            GameObject tmp = Instantiate(prefabProjetil, instanciador.position, instanciador.rotation);
            tmp.GetComponent<Bala>().Inicializacao(Vector2.right);
        } else{
            GameObject tmp = Instantiate(prefabProjetil, instanciador.position, Quaternion.Euler(new Vector3(0, 0, 180)));
            tmp.GetComponent<Bala>().Inicializacao(Vector2.left);
        }
    }
}
