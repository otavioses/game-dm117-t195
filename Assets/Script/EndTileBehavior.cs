using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTileBehavior : MonoBehaviour
{

    [Tooltip("Tempo esperado antes de destruir o tile basico")]
    public float tempoDestruir = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        //vamos ver se foi o jogador que passou pelo fim do tile basico
        if (other.GetComponent<PlayerBehavior>())
        {
            //como foi o jogador que passou, vamos criar um tile basico no proximo ponto
            //mas esse proximo ponto esta depois do ultimo tile basico presente na cena
            GameObject.FindObjectOfType<GameController>().SpawProxTile();

            Destroy(transform.parent.gameObject, tempoDestruir);
        }
    }
}
