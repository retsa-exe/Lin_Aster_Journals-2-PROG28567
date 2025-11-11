using UnityEngine;

public class TorqueTest : MonoBehaviour
{
    public Rigidbody2D rb;

    void Start()
    {
        rb.AddTorque(2, ForceMode2D.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        float turn = Input.GetAxis("Horizontal");
        rb.AddTorque(-turn * 10f);
    }
}
