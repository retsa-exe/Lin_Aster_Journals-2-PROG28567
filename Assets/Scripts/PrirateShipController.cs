using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class PrirateShipController : MonoBehaviour
{
    public ScoreboardController s;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        CannonballController cannonball = collision.GetComponent<CannonballController>();
        if (cannonball != null)
        {
            s.Score += 1;
        }
    }
}
