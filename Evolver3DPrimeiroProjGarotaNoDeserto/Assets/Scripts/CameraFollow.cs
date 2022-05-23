using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    //o serialize field serve para liberar no unity para definir valor para private é usado comando abaixo
    [SerializeField]
    //Para definição do limite que a camera ira acompanhar
    private float maximoX;
    [SerializeField]
    private float minimoX;
    [SerializeField]
    private float maximoY;
    [SerializeField]
    private float minimoY;

    //Para o target no caso o personagem
    public Transform player;

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //o transform da camera é onde é definido a posição da camera pode se observar na unity
        //o Mathf.clamp voce define um valor minimo e um maximo no caso utilizado para definir até onde a camera segue
        //o Vector 3 esta sendo usado pois são 3 vetores o x, y e z(embora no 2d não mudamos o z geralmente definido em -10)
        transform.position = new Vector3(Mathf.Clamp(player.position.x, minimoX, maximoX), Mathf.Clamp(player.position.y, minimoY, maximoY), transform.position.z); 
    }
}
