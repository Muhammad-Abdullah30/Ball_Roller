using UnityEngine;
using UnityEngine.PlayerLoop;


public class Player : MonoBehaviour
{
    [Header("Variable Intialization")]
   [ SerializeField ] private Rigidbody rb;
    [SerializeField]  private float movementSpeed = 43.0f;
    [SerializeField] private float rotationalSpeed = 45.0f;
    [SerializeField] private float gameSpeed = 2.0f;

        //ap kr rhy chlyga e na
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Debug.Log("hELLO aB");
        
        /* Done Brother ye vector 3 k KIA THA?*/
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    rb.AddForce(Vector3.up*speed*Time.deltaTime,ForceMode.Impulse);
        //}
    }

    void FixedUpdate()
    {
        Vector3 forwardMovement = Vector3.forward * gameSpeed;

        // Horizontal movement based on player input
        float horizontalInput = Input.GetAxis("Horizontal"); // -1 for left, +1 for right
        Vector3 horizontalMovement = Vector3.right * horizontalInput * movementSpeed;

        // Apply movement to the ball
        Vector3 combinedVelocity = forwardMovement + horizontalMovement;
        rb.linearVelocity = new Vector3(combinedVelocity.x, rb.linearVelocity.y, combinedVelocity.z);

        // Apply rotation for the rolling effect
        float rotation = gameSpeed * Time.fixedDeltaTime * rotationalSpeed;
        rb.AddTorque(Vector3.right * rotation);

        //

    }
}
