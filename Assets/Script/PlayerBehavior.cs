using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerBehavior : MonoBehaviour
{
    public enum HorizontalMoveType {
        Acceleration,
        Touch
    }

    public HorizontalMoveType horizontalMove = HorizontalMoveType.Acceleration;

    // Start is called before the first frame update
    /// <summary>
    /// Uma referencia para o componenete RigidBody
    /// </summary>
    private Rigidbody rb;

    [Tooltip("A velocidade que o jogador irá se esquivar para os lados")]
    [Range(0, 10)]
    public float velocidadeEsquiva = 5.0f;

    [Tooltip("A velocidade que o jogador irá se deslocar para frente")]
    [Range(0, 10)]
    public float velocidadeRolamento = 5.0f;

    [Header("Attributes responsible for swipe")]
    [Tooltip("Determines how far the player's finger must slide to be considered a swipe")]
    public float minDisSwipe = 2.0f;

    [Tooltip("distancia que o jogador ira percorrer atras do swipe")]
    public float swipeMove = 2.0f;

    private Vector2 touchInitial;

    [Tooltip("O som de impacto na parede")]
    public AudioClip impact;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {
        if (MenuPauseBehavior.paused) {
            return;
        }

        var velocidadeHorizontal = Input.GetAxis("Horizontal") * velocidadeEsquiva;
        var velocidadeVertical = Input.GetAxis("Vertical");

#if UNITY_STANDALONE || UNIT_EDITOR || UNIT_WEBPLAYER

        // 0 - left - or touch
        // 1 - right
        // 2 - middle
        // Checking click 0 or touch
        if (Input.GetMouseButton(0)) {
            velocidadeHorizontal = CalcMoves(Input.mousePosition);
        }

#elif UNIT_IOS || UNIT_ANDROID

        if (horizontalMove == HorizontalMoveType.Acceleration)
        {
            velocidadeHorizontal = Input.acceleration.x * velocidadeRolamento;
        } else
        {
            // Checking only touch 
            if (Input.touchCount > 0)
            {
                //Get first touch inside the frame
                Touch touch = Input.touches[0];
                velocidadeHorizontal = CalcMoves(touch.position);
                SwipeTeleport(touch);
            }
        }

#endif
        var moveForce = new Vector3(velocidadeHorizontal, 0, ObstacleBehavior.velocidadeRolamento * calculateAcceleration(velocidadeVertical));

        //Time.deltaTime returns the time spent in the previous frame

        moveForce *= (Time.deltaTime * 60);
        rb.AddForce(moveForce);
    }

    private float calculateAcceleration(float value)
    {
        if (value == 0)
        {
            return 1f;
        } else if (value > 0){
            return 5f;
        } else
        {
            return 0.01f;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<WallController>())
        {
            GetComponent<AudioSource>().PlayOneShot(impact, 0.7F);
        }
    }

    /// <summary>
    /// Calc player's moves
    /// </summary>
    /// <param name="screenSpaceCoordinate">Screen space coordinates</param>
    /// <returns></returns>
    private float CalcMoves(Vector2 screenSpaceCoordinate)
    {
        var position = Camera.main.ScreenToViewportPoint(screenSpaceCoordinate);
        float directionX = 0;

        if (position.x < 0.5)
        {
            directionX = -1;
        }
        else
        {
            directionX = 1;
        }

        return directionX * velocidadeEsquiva;
    }
    
    private void SwipeTeleport(Touch touch)
    {
        // Checks if is the point where the swipe began
        if (touch.phase == TouchPhase.Began)
        {
            touchInitial = touch.position;
        }
        // Checks if is the point where the swipe ended
        else if (touch.phase == TouchPhase.Ended)
        {
            Vector2 touchEnd = touch.position;
            Vector3 directionMove;

            //Diff betwen touchInitial and touchEnd
            float diff = touchEnd.x - touchInitial.x;

            //Checks if the swipe was far enough to be considered a swipe
            if (diff >= Math.Abs(minDisSwipe))
            {
                if (diff < 0)
                {
                    directionMove = Vector3.left;
                } else
                {
                    directionMove = Vector3.right;
                }
            } else
            {
                return;
            }

            //Raycast is another way to detect a collision
            RaycastHit hit;

            //Checks if the swipe won't cause a collision
            if (!rb.SweepTest(directionMove, out hit, swipeMove))
            {
                rb.MovePosition(rb.position + (directionMove * swipeMove));
            }
        }
    }
}
