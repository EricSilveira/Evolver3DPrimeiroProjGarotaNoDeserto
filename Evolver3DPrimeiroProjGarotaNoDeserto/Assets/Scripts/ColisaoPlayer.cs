/***================== Indice do codigo para entendimento ====================***/
/*** 1.0   - Colisao com o player nao ocorra nada                             ***/
/*****                                                                        ***/
/*** 2.0   - Configuracao para unity                                          ***/
/*****                                                                        ***/
/**================== Fim Indice do codigo para entendimento =================***/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColisaoPlayer : MonoBehaviour
{
    //2.0   - Para definir os valores na plataforma unity
    [SerializeField]
    //1.0   - Para que a colisão com o player não aconteça
    Collider2D outro;

    private void Awake(){
        //1.0   - Quando o inimigo encoste no player ou vice versa não ira empurrar..
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), outro, true);
    }
}
