using UnityEngine;
using TMPro;
using DG.Tweening;
using Unity.Android.Gradle.Manifest;
using JetBrains.Annotations;

public class Player : MonoBehaviour
{
    [Header("Variable Initialization")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float movementSpeed = 43.0f;
    [SerializeField] private float rotationalSpeed = 45.0f;
    [SerializeField] private float gameSpeed = 2.0f;
    [SerializeField] private float speedIncreaseRate = 0.1f; // Speed increase per second
    [SerializeField] private float maxGameSpeed = 10.0f;     // Maximum forward speed

    [Header("Constraint Border")]
    private float minX;
    private float maxX;

    [Header("Coins Score Added")]
    public TextMeshProUGUI coins;
    int coinsToBeAdded;

    [Header("Game Over Text")]
    public TextMeshProUGUI gameOver;

    [Header("GameOverPannel")]
    [SerializeField] private GameObject gameOverPanel; // Drag your Game Over Panel here
    [SerializeField] private float panelAnimationDelay = 2.5f; // Delay before panel animation
    [SerializeField] private float panelFadeDuration = 1f; // Duration for panel fade-in animation

    [SerializeField] private bool gameOverFlag = false;
    [SerializeField] public bool GameStarted = false;
    [SerializeField] public bool Acceloro = false;
    [SerializeField] public bool TouchSystem = false;

    [SerializeField] public ParticleSystem gameWins;
    public SpawnManager spawnManager;


    [Header("ModeSelection")]
    [SerializeField] public GameObject ModeSelection;


    private void Awake()
    {
        ModeSelection.SetActive(true);
    }

    void Start()
    {
        spawnManager=FindObjectOfType<SpawnManager>();
        gameOver.gameObject.SetActive(false);
        gameOver.alpha = 0;
        coinsToBeAdded = 0;
        rb = GetComponent<Rigidbody>();

        // Set the initial allowed range for X-axis
        minX = rb.position.x - 3f;
        maxX = rb.position.x + 3f;
    }

    void Update()
    {
        if (GameStarted)
        {
            if (!gameOverFlag)
            {


                // Gradually increase the gameSpeed over time
                gameSpeed += speedIncreaseRate * Time.deltaTime;

                // Clamp gameSpeed to avoid exceeding the maximum speed
                gameSpeed = Mathf.Clamp(gameSpeed, 0, maxGameSpeed);
            }
        }
    }

    void FixedUpdate()
    {
        if (GameStarted)
        {
            //if (!gameOverFlag)
            //{


            //    Vector3 forwardMovement = Vector3.forward * gameSpeed;

            //    // Horizontal movement based on player input
            //    float horizontalInput = Input.GetAxis("Horizontal"); // -1 for left, +1 for right
            //    Vector3 horizontalMovement = Vector3.right * horizontalInput * movementSpeed;

            //    // Apply movement to the ball
            //    Vector3 combinedVelocity = forwardMovement + horizontalMovement;
            //    rb.linearVelocity = new Vector3(combinedVelocity.x, rb.linearVelocity.y, combinedVelocity.z);

            //    // Clamp the ball's X position within the allowed range
            //    Vector3 clampedPosition = rb.position;
            //    clampedPosition.x = Mathf.Clamp(rb.position.x, minX, maxX);
            //    rb.position = clampedPosition;

            //    // Apply rotation for the rolling effect
            //    float rotation = gameSpeed * Time.fixedDeltaTime * rotationalSpeed;
            //    rb.AddTorque(Vector3.right * rotation);
            //}
            if(!gameOverFlag && Acceloro == true)
            {
                Debug.Log(" Accelero");
                Vector3 forwardMovement = Vector3.forward * gameSpeed;

                float tiltInput = Input.acceleration.x;
                float horizontalInput;
                if (tiltInput < 0)  // Tilted to the left
                {
                    horizontalInput = -1f; // Move left
                }
                else if (tiltInput > 0) // Tilted to the right
                {
                    horizontalInput = 1f;  // Move right
                }
                else
                {
                    horizontalInput = 0f; // No tilt or stationary
                }

                Vector3 horizontalMovement = Vector3.right * horizontalInput * movementSpeed;

                // Apply movement to the ball
                Vector3 combinedVelocity = forwardMovement + horizontalMovement;
                rb.linearVelocity = new Vector3(combinedVelocity.x, rb.linearVelocity.y, combinedVelocity.z);

                // Clamp the ball's X position within the allowed range
                Vector3 clampedPosition = rb.position;
                clampedPosition.x = Mathf.Clamp(rb.position.x, minX, maxX);
                rb.position = clampedPosition;

                // Apply rotation for the rolling effect
                float rotation = gameSpeed * Time.fixedDeltaTime * rotationalSpeed;
                rb.AddTorque(Vector3.right * rotation);



            }
            else if(!gameOverFlag && TouchSystem == true)
            {
                Debug.Log(" TouchSystem");
                Vector3 forwardMovement = Vector3.forward * gameSpeed;

                float touchInput = 0f;
                Vector2 touchPosition = Input.mousePosition; // Or Input.touch.position for mobile
                float screenWidth =UnityEngine.Screen.width;

                if (touchPosition.x < screenWidth / 2) // Left side touch
                {
                    touchInput = -1f; // Move left
                }
                else if (touchPosition.x > screenWidth / 2) // Right side touch
                {
                    touchInput = 1f; // Move right
                }

                Vector3 horizontalMovement = Vector3.right * touchInput * movementSpeed;

                // Apply movement to the ball
                Vector3 combinedVelocity = forwardMovement + horizontalMovement;
                rb.linearVelocity = new Vector3(combinedVelocity.x, rb.linearVelocity.y, combinedVelocity.z);

                // Clamp the ball's X position within the allowed range
                Vector3 clampedPosition = rb.position;
                clampedPosition.x = Mathf.Clamp(rb.position.x, minX, maxX);
                rb.position = clampedPosition;

                // Apply rotation for the rolling effect
                float rotation = gameSpeed * Time.fixedDeltaTime * rotationalSpeed;
                rb.AddTorque(Vector3.right * rotation);
            }

        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Coins"))
        {
            coinsToBeAdded++;
            coins.text = "Coins " + coinsToBeAdded;
            Debug.Log(other.gameObject.name);
            Destroy(other.gameObject);
        }
        if (other.gameObject.CompareTag("Finish"))
        {
            spawnManager.CancelInvoke();
            Debug.Log("Win");
            gameOverFlag = true;
            gameWins.Play();
            ShowWinText();
            ShowGameOverPanel();
            InstantStopBall();
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            spawnManager.CancelInvoke();
            TriggerGameOver();
            gameOverFlag = true;
        }
    }
    [ContextMenu("GameOver")]

    private void TriggerGameOver()
    {
        if (gameOver != null)
        {
            gameOver.gameObject.SetActive(true); // Activate the Game Over text

            // Reset initial state for animation
            gameOver.alpha = 0; // Ensure it's transparent initially
            gameOver.transform.localScale = Vector3.zero; // Start with no scale

            // Fade-in and scale animation
            Sequence gameOverSequence = DOTween.Sequence();
            gameOverSequence
                .Append(gameOver.DOFade(4, 4f)) // Fade in the text
                .Join(gameOver.transform.DOScale(Vector3.one, 4f).SetEase(Ease.OutElastic)).AppendInterval(panelAnimationDelay).OnComplete(ShowGameOverPanel); // Scale with bounce effect
        }

    }
    private void ShowGameOverPanel()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true); // Activate the Game Over panel

            // Animate the panel (fade-in effect)
            CanvasGroup canvasGroup = gameOverPanel.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = gameOverPanel.AddComponent<CanvasGroup>(); // Add CanvasGroup if not present
            }
            canvasGroup.alpha = 0; // Ensure the panel starts transparent
            canvasGroup.DOFade(1, panelFadeDuration); // Fade to full opacity
        }
    }
    private void ShowWinText()
    {
        // Step 1: Fade in and scale up the text with bounce
        gameOver.DOFade(1, 1f); // Fade to full opacity in 1 second
        gameOver.transform.DOScale(1.5f, 1f).SetEase(Ease.OutBounce); // Scale up with bounce effect

        // Step 2: Add a pulsating effect
        gameOver.transform.DOScale(1.3f, 0.5f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo); // Pulsate infinitely
    }
    private void InstantStopBall()
    {
        if (rb != null)
        {
            rb.velocity = Vector3.zero; // Completely stop linear motion
            rb.angularVelocity = Vector3.zero; // Completely stop angular motion
            rb.isKinematic = true; // Disable physics-based interactions instantly
        }
    }
}
