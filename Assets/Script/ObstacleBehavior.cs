using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ObstacleBehavior : MonoBehaviour
{
    [Tooltip("Wating time before restart the game")]
    public float waitingTime = 0.1f;

    [Tooltip("Particle system of explosion")]
    public GameObject explosion;

    MeshRenderer meshRenderer = new MeshRenderer();
    BoxCollider boxCollider = new BoxCollider();
    SphereCollider sphereCollider = new SphereCollider();

    [Tooltip("Sound to play when impact")]
    public AudioClip impact;

    public static float velocidadeRolamento = 2f;

    private GameObject player;

    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        boxCollider = GetComponent<BoxCollider>();
        sphereCollider = GetComponent<SphereCollider>();
    }

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
            GetComponent<AudioSource>().PlayOneShot(impact, 0.7F);
            collision.gameObject.SetActive(false);
            player = collision.gameObject;

            Invoke("ResetGame", waitingTime);
        }
    }

    /// <summary>
    /// Metodo para resetar o jogo
    /// </summary>
    void ResetGame()
    {
        var gameOverMenu = GetGameOverMenu();
        gameOverMenu.SetActive(true);

        var botoes = gameOverMenu.transform.GetComponentsInChildren<Button>();
        Button botaoContinue = null;

        foreach (var botao in botoes)
        {
            if (botao.gameObject.name.Equals("Resume"))
            {
                botaoContinue = botao;
                break;
            }
        }

        if (botaoContinue)
        {
            StartCoroutine(ShowContinue(botaoContinue));
        }
    }

    /// <summary>
    /// Método para continuar o game
    /// </summary>
    /// <param name="buttonResume">botao resume</param>
    /// <returns></returns>
    public IEnumerator ShowContinue(Button buttonResume)
    {
        var buttonTxt = buttonResume.GetComponentInChildren<Text>();

        while (true)
        {
            if (UnityAdController.nextTimeReward.HasValue && (DateTime.Now < UnityAdController.nextTimeReward.Value))
            {
                buttonResume.interactable = false;

                TimeSpan remaing = UnityAdController.nextTimeReward.Value - DateTime.Now;

                var countDown = String.Format("{0:D2}: {1:D2}", remaing.Minutes, remaing.Seconds);

                buttonTxt.text = countDown;

                yield return new WaitForSeconds(1f);
            }
            else
            {
                buttonResume.interactable = true;
                buttonResume.onClick.AddListener(UnityAdController.ShowRewardAd);
                UnityAdController.obstacle = this;
                buttonTxt.text = "Resume Ad";
                break;
            }
        }
    }

    /// <summary>
    /// Continuar o game depois de pausado
    /// </summary>
    public void Continue()
    {
        var go = GetGameOverMenu();
        go.SetActive(false);
        player.SetActive(true);
        TouchedObject();
    }

    /// <summary>
    /// retorna o game over menu
    /// </summary>
    /// <returns>game over menu</returns>
    GameObject GetGameOverMenu()
    {
        return GameObject.Find("Canvas").transform.Find("MenuGameOver").gameObject;
    }

    /// <summary>
    /// Método para detectar toques
    /// </summary>
    /// <param name="touch">posisao tocada</param>
    private void touchObjects(Vector2 touch)
    {
        Ray touchRay = Camera.main.ScreenPointToRay(touch);

        RaycastHit hit;

        if (Physics.Raycast(touchRay, out hit))
        {
            hit.transform.SendMessage("TouchedObject", SendMessageOptions.DontRequireReceiver);
        }
    }

    /// <summary>
    /// Objeto tocado
    /// </summary>
    public void TouchedObject()
    {
        velocidadeRolamento += 0.5f;

        if (explosion != null)
        {
            GetComponent<AudioSource>().PlayOneShot(impact, 0.7F);

            var particles = Instantiate(explosion, transform.position, Quaternion.identity);
            Destroy(particles, 1.0f);
        }

        meshRenderer.enabled = false;
        if (boxCollider != null)
        {
            boxCollider.enabled = false;
        }
        else if (sphereCollider != null)
        {
            sphereCollider.enabled = false;
        }
        

        Destroy(this.gameObject);
    }
}
