using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndTileBehavior : MonoBehaviour
{
    public static long up = 1;
    public static long score = 0;
    public static long highscore = 0;

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

            Score();
        }
    }

    public void Score()
    {
        score += 100;

        if (score > highscore)
        {
            highscore = score;
        }

        var textScore = GameObject.Find("Canvas").transform.Find("Score").GetComponentInChildren<Text>();
        textScore.text = "Score: " + score;

        var textHighscore = GameObject.Find("Canvas").transform.Find("HighScore").GetComponentInChildren<Text>();
        textHighscore.text = "High Score: " + highscore;

        if (score % 300 == 0)
        {
            var textUp = GameObject.Find("Canvas").transform.Find("Up").GetComponentInChildren<Text>();
            textUp.text = "Up: " + ++up;
        }
    }
}
