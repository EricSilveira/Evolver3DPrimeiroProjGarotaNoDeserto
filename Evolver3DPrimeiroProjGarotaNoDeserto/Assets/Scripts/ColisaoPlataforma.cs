/***==================== Indice do codigo para entendimento ======================***/
/*** 1.0   - Plataforma para colider deixe passar por ela ou subir por baixo      ***/
/*****                                                                            ***/
/*** 2.0   - Configuracao para unity                                              ***/
/*****                                                                            ***/
/***------------------------------------------------------------------------------***/
/***---------> OBS:Existe uma forma mais simples de evitar a          <-----------***/
/***--------->     collis�o do personagem com uma  plataforma que     <-----------***/
/***--------->     seria na propria interface unity manter apenas um  <-----------***/
/***--------->     box collider o sem trigger selecionar a op��o USED <-----------***/
/***--------->     BY EFFECTOR e adicionar o componente Plataform     <-----------***/
/***--------->     Effector 2D no source arc colocar de forma que n�o <-----------***/
/***--------->     encoste noPlayer para este caso usaria 110         <-----------***/
/***------------------------------------------------------------------------------***/
/**==================== Fim Indice do codigo para entendimento ===================***/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ColisaoPlataforma : MonoBehaviour
{
    //2.0   - Para definir os valores na plataforma unity
    [SerializeField]
    //1.0   - Para o box collider sem trigger, devera ser arrastado para dentro do campo
    private BoxCollider2D plataformaCollider2D;
    //2.0   - Para definir os valores na plataforma unity
    [SerializeField]
    //1.0   - Para o box collider com trigger, devera ser arrastado para dentro do campo
    private BoxCollider2D plataformaTrigger2D;

    void Start(){
        //1.0   - Ignoramos a colis�o entre as plataformas
        Physics2D.IgnoreCollision(plataformaCollider2D, plataformaTrigger2D, true);
    }

    //1.0   - Para ignorar a colis�o do player com a plataforma
    private void OnTriggerEnter2D(Collider2D collision){
        //1.0   - Quando ele encontra o nome do objeto player(n�o � a tag � o nome)
        if (collision.gameObject.name == "Player"){
            //1.0   - Ele ignora a colis�o com o player do box collider que n�o esta com trigger setado
            Physics2D.IgnoreCollision(plataformaCollider2D, collision, true);
        }
    }

    //1.0   - Para habilita a colis�o do player com a plataforma para o caso de subir encima
    private void OnTriggerExit2D(Collider2D collision){
        //1.0   - Quando ele encontra o nome do objeto player(n�o � a tag � o nome)
        if (collision.gameObject.name == "Player"){
            //1.0   - Ele deixa de ignorar a colis�o
            Physics2D.IgnoreCollision(plataformaCollider2D, collision, false);
        }
    }
}
