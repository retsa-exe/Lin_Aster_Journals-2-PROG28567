using UnityEngine;

public class TorqueTest : MonoBehaviour
{
    public Rigidbody2D rigidbody;

    void Start()
    {
        //rigidbody.AddTorque(2, ForceMode2D.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        float turn = Input.GetAxis("Horizontal");
        rigidbody.AddTorque(-turn * 10f);
    }
}
