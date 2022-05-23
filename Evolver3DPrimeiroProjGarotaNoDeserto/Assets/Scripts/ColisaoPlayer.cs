using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColisaoPlayer : MonoBehaviour
{
    //para definir os valores na plataforma unity
    [SerializeField]
    //para que a colisão com o player não aconteça
    Collider2D outro;

    private void Awake()
    {
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), outro, true);
    }

}
