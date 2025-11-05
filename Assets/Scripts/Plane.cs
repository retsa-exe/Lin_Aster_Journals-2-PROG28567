using UnityEngine;

public class Plane : MonoBehaviour
{
    public Rigidbody2D planeRigidbody;

    public Collider2D planeCollider;
    public Collider2D targetCollider;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //apply a force
        //planeRigidbody.AddForce(Vector2.up * 5f, ForceMode2D.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        //planeRigidbody.AddForce(Vector2.down, ForceMode2D.Force);

        Physics2D.IgnoreCollision(planeCollider, targetCollider);

        if (Physics2D.GetIgnoreCollision(planeCollider, targetCollider))
        {
            Debug.Log("The collision has beeing ignored.");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("This object is just collied with another.");
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        Debug.Log("This object is colliding with another.");
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        Debug.Log("This object has stopped colliding with another.");
    }
}
