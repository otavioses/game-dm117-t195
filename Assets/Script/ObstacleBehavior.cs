using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ObstacleBehavior : MonoBehaviour
{
    public static long points = 0;
    public static long highscore = 0;
    [Tooltip("Quanto tempo antes de reiniciar o jogo")]
    public float tempoEspera = 0.1f;

    [Tooltip("Particle system of explosion")]
    public GameObject explosion;

    MeshRenderer mr = new MeshRenderer();

    BoxCollider bc = new BoxCollider();

    public AudioClip impact;
    AudioSource audioSource;

    public static float velocidadeRolamento = 0.5f;

    /// <summary>
    /// Variavel referencia para o jogador
    /// </summary>
    private GameObject jogador;

    // Start is called before the first frame update
    void Start()
    {
        mr = GetComponent<MeshRenderer>();
        bc = GetComponent<BoxCollider>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (MenuPauseBehavior.paused || GetGameOverMenu().active)
        {
            return;
        }
        // 0 - left - or touch
        // 1 - right
        // 2 - middle
        // Checking click 0 or touch
        if (Input.GetMouseButton(0))
        {
            touchObjects(Input.mousePosition);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<PlayerBehavior>())
        {
            audioSource.PlayOneShot(impact, 0.7F);
            collision.gameObject.SetActive(false);
            jogador = collision.gameObject;

            //Destroy(collision.gameObject);
            Invoke("ResetGame", tempoEspera);
        }
    }

    void ResetGame()
    {
        var gameOverMenu = GetGameOverMenu();
        gameOverMenu.SetActive(true);

        var botoes = gameOverMenu.transform.GetComponentsInChildren<Button>();
        Button botaoContinue = null;

        foreach (var botao in botoes)
        {
            if (botao.gameObject.name.Equals("BotaoContinuar"))
            {
                botaoContinue = botao;
                break;
            }
        }

        if (botaoContinue)
        {

            StartCoroutine(ShowContinue(botaoContinue));
            //botaoContinue.onClick.AddListener(UnityAdControle.ShowRewardAd);
            //UnityAdControle.obstaculo = this;
        }

        //Reinicia o jogo
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public IEnumerator ShowContinue(Button botaoContinue)
    {
        var btnText = botaoContinue.GetComponentInChildren<Text>();
        while(true)
        {
            //if (UnityAdControle.proxTempoReward.HasValue && DateTime.Now < UnityAdControle.proxTempoReward)
            //{
            //    botaoContinue.interactable = false;
            //    TimeSpan restate = UnityAdControle.proxTempoReward.Value - DateTime.Now;
            //    var contagemRegressiva = string.Format("{0:D2}:1:D2", restate.Minutes, restate.Seconds);
            //    btnText.text = contagemRegressiva;
            //    yield return new WaitForSeconds(1f);
            //}
            //else
            //{
            //    botaoContinue.interactable = true;
            //    botaoContinue.onClick.AddListener(UnityAdControle.ShowRewardAd);
            //    UnityAdControle.obstaculo = this;
            //    btnText.text = "Continue (Ad)";
            //    break;
            //}
        }
    }

    /// <summary>
    /// Faz o continue do jogo
    /// </summary>
    public void Continue()
    {
        var go = GetGameOverMenu();
        go.SetActive(false);
        jogador.SetActive(true);
        TouchedObject();
    }

    /// <summary>
    /// Busca o menu game over
    /// </summary>
    /// <returns>O game object MenuGameOver</returns>
    GameObject GetGameOverMenu()
    {
        return GameObject.Find("Canvas").transform.Find("MenuGameOver").gameObject;
    }


    private void touchObjects(Vector2 touch)
    {

        //Convert touch position (screen place) into a ray
        Ray touchRay = Camera.main.ScreenPointToRay(touch);

        //Save info regarding the object that was possibly touched or hit
        RaycastHit hit;

        if (Physics.Raycast(touchRay, out hit))
        {
            hit.transform.SendMessage("TouchedObject", SendMessageOptions.DontRequireReceiver);
            hit.transform.SendMessage("Score", SendMessageOptions.DontRequireReceiver);
        }
    }
    public void Score()
    {
        points += 100;
        if (points > highscore)
        {
            highscore = points;
        }
       
        var textPoints = GameObject.Find("Canvas").transform.Find("Points").GetComponentInChildren<Text>();
        textPoints.text = "Points: " + points;

        var textHighscore = GameObject.Find("Canvas").transform.Find("Highscore").GetComponentInChildren<Text>();
        textHighscore.text = "Higscore: " + highscore;
    }

    /// <summary>
    /// Method called by SendMessage() whichs detect the object was touched
    /// </summary>
    public void TouchedObject()
    {
        velocidadeRolamento += 0.5f;
        if (explosion != null)
        {
            //Creates effect
            audioSource.PlayOneShot(impact, 0.7F);
            var particles = Instantiate(explosion, transform.position, Quaternion.identity);
            audioSource.PlayOneShot(impact, 0.7F);
            Destroy(particles, 1.0f);
        }

        mr.enabled = false;
        bc.enabled = false;
        Destroy(this.gameObject);
    }
}
