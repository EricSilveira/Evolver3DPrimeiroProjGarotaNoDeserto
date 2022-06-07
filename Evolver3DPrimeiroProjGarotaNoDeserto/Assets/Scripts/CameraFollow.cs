/***================== Indice do codigo para entendimento ====================***/
/*** 1.0   - Camera seguidora                                                 ***/
/*****                                                                        ***/
/*** 2.0   - Configuracao para unity                                          ***/
/*****                                                                        ***/
/**================== Fim Indice do codigo para entendimento =================***/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    //1.0   - Para o target no caso o personagem
    public Transform player;
    //2.0   - o serialize field serve para liberar no unity para definir valor para private é usado comando abaixo
    [SerializeField]
    //1.0   - Para definição do limite que a camera ira acompanhar
    private float maximoX;
    [SerializeField]
    private float minimoX;
    [SerializeField]
    private float maximoY;
    [SerializeField]
    private float minimoY;

    void Update(){
        //1.0   - O transform da camera é onde é definido a posição da camera pode se observar na unity
        //1.0   - O Mathf.clamp voce define um valor minimo e um maximo no caso utilizado para definir até onde a camera segue
        //1.0   - O Vector 3 esta sendo usado pois são 3 vetores o x, y e z(embora no 2d não mudamos o z geralmente definido em -10)
        transform.position = new Vector3(Mathf.Clamp(player.position.x, minimoX, maximoX), Mathf.Clamp(player.position.y, minimoY, maximoY), transform.position.z); 
    }
}
