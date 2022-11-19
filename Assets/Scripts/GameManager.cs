using System.Linq;
using UnityEngine;

public class GameManager : SingletonWithInitialization<GameManager>, IAwakeInitializationSubject
{
    public GameObject giraffe;
    public GameObject head;
    public GameObject ball;

    [HideInInspector]
    public Players currentPlayer = Players.Ball;

    [HideInInspector]
    public bool isHeadActive = false;

    public void OnSingletonCreated()
    {
        SetCurrentPlayerMovement();
    }

    public void SetCurrentPlayerMovement()
    {
        if (currentPlayer == Players.Ball)
        {
            ball.GetComponent<BallMovement>().enabled = true;
            giraffe.GetComponent<GiraffeMovement>().enabled = false;
            head.GetComponent<HeadMovement>().enabled = false;
        }
        else if (currentPlayer == Players.Giraffe)
        {
            ball.GetComponent<BallMovement>().enabled = false;
            giraffe.GetComponent<GiraffeMovement>().enabled = true;
            head.GetComponent<HeadMovement>().enabled = false;
        }
    }

    public void SwitchPlayers()
    {
        currentPlayer = currentPlayer.Next();

        SetCurrentPlayerMovement();
    }

    public void ChangeHeadStatus()
    {
        if (currentPlayer != Players.Giraffe)
        {
            return;
        }

        isHeadActive = !isHeadActive;

        if (isHeadActive)
        {
            giraffe.GetComponent<GiraffeMovement>().enabled = false;
            head.GetComponent<HeadMovement>().enabled = true;
        }
        else
        {
            giraffe.GetComponent<GiraffeMovement>().enabled = true;
            head.GetComponent<HeadMovement>().enabled = false;
        }
    }
    public void DisableHead()
    {
        isHeadActive = false;
        head.GetComponent<Rigidbody2D>().isKinematic = true;
    }

    public void ChangeHeadPhysics()
    {
        var headRb = head.GetComponent<Rigidbody2D>();
        var isHeadKinematic = headRb.isKinematic;
        headRb.isKinematic = !isHeadKinematic;
    }

    public void EndLevel()
    {
        Debug.Log("End level");
    }
}

