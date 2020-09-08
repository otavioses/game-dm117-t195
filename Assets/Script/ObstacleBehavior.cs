using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ObstacleBehavior : MonoBehaviour
{
    [Tooltip("Wating time before restart the gaame")]
    public float watingTime = 0.1f;

    [Tooltip("Particle system of explosion")]
    public GameObject explosion;

    MeshRenderer meshRenderer = new MeshRenderer();
    BoxCollider boxCollider = new BoxCollider();
    SphereCollider sphereCollider = new SphereCollider();

    public AudioClip impact;

    [Tooltip("Explosion sound")]
    AudioSource audioSource;

    public static float velocidadeRolamento = 2f;

    private GameObject player;

    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        boxCollider = GetComponent<BoxCollider>();
        sphereCollider = GetComponent<SphereCollider>();
        audioSource = GetComponent<AudioSource>();
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
            audioSource.PlayOneShot(impact, 0.7F);
            collision.gameObject.SetActive(false);
            player = collision.gameObject;

            Invoke("ResetGame", watingTime);
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

    public void Continue()
    {
        var go = GetGameOverMenu();
        go.SetActive(false);
        player.SetActive(true);
        TouchedObject();
    }

    GameObject GetGameOverMenu()
    {
        return GameObject.Find("Canvas").transform.Find("MenuGameOver").gameObject;
    }


    private void touchObjects(Vector2 touch)
    {
        Ray touchRay = Camera.main.ScreenPointToRay(touch);

        RaycastHit hit;

        if (Physics.Raycast(touchRay, out hit))
        {
            hit.transform.SendMessage("TouchedObject", SendMessageOptions.DontRequireReceiver);
        }
    }

    public void TouchedObject()
    {
        velocidadeRolamento += 0.5f;

        if (explosion != null)
        {

            audioSource.PlayOneShot(impact, 0.7F);
            var particles = Instantiate(explosion, transform.position, Quaternion.identity);
            audioSource.PlayOneShot(impact, 0.7F);
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

    public static void SetInitialVelocity()
    {
        velocidadeRolamento = 2.0f;
    }
}
