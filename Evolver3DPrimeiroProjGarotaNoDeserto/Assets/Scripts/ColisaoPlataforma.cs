using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*********************************************************************************/
/*Existe uma forma mais simples de evitar a collisão do personagem com uma       */
/* plataforma que seria na propria interface unity manter apenas um box collider */
/* o sem trigger selecionar a opção USED BY EFFECTOR e adicionar o componente    */
/* Plataform Effector 2D no source arc colocar de forma que não encoste no       */
/* Player para este caso usaria 110                                              */
/*********************************************************************************/

public class ColisaoPlataforma : MonoBehaviour
{
    //para definir os valores na plataforma unity
    [SerializeField]
    //para o box collider sem trigger, devera ser arrastado para dentro do campo
    private BoxCollider2D plataformaCollider2D;
    //para definir os valores na plataforma unity
    [SerializeField]
    //para o box collider com trigger, devera ser arrastado para dentro do campo
    private BoxCollider2D plataformaTrigger2D;

    // Start is called before the first frame update
    void Start()
    {
        //ignoramos a colisão entre as plataformas
        Physics2D.IgnoreCollision(plataformaCollider2D, plataformaTrigger2D, true);
        
    }

    //Para ignorar a colisão do player com a plataforma
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //quando ele encontra o nome do objeto player(não é a tag é o nome)
        if (collision.gameObject.name == "Player")
        {
            //ele ignora a colisão com o player do box collider que não esta com trigger setado
            Physics2D.IgnoreCollision(plataformaCollider2D, collision, true);
        }
    }

    //Para habilita a colisão do player com a plataforma para o caso de subir encima
    private void OnTriggerExit2D(Collider2D collision)
    {
        //quando ele encontra o nome do objeto player(não é a tag é o nome)
        if (collision.gameObject.name == "Player")
        {
            // ele deixa de ignorar a colisão
            Physics2D.IgnoreCollision(plataformaCollider2D, collision, false);
        }
    }
}
