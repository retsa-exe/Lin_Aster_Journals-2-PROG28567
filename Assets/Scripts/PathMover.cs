using UnityEngine;

public class PathMover : MonoBehaviour
{
    public Transform startHolder, endHolder;

    public float duration;
    float timeMoving = 0f;

    State currentState;

    enum State
    {
        forward, backward
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timeMoving += Time.deltaTime;

        if (currentState == State.forward)
        {
            transform.position = Vector3.Lerp(startHolder.position, endHolder.position, timeMoving / duration);
        }
        else if (currentState == State.backward)
        {
            transform.position = Vector3.Lerp(endHolder.position, startHolder.position, timeMoving / duration);
        }

        if (timeMoving >= duration) 
        {
            if (currentState == State.forward)
            {
                currentState = State.backward;
                timeMoving = 0f;
            }
            else if (currentState == State.backward)
            {
                currentState = State.forward;
                timeMoving = 0f;
            }
        }
    }
}
