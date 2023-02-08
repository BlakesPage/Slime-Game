using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNarationSystem : MonoBehaviour, IObserver
{
    [SerializeField] Subject _playerSubject;
    int _jumpThreshHold = 5;

    public string[] JumpNaration = { "Im Tired", "Just a little further", "Keep bouncing away"};

    public void OnNotify(PlayerActions action)
    {
        switch(action)
        {
            case PlayerActions.Jump:
                if(PlayerStats.jumpCount % _jumpThreshHold == 0)
                {
                    // do something
                    Debug.Log(JumpNaration[Random.Range(0, JumpNaration.Length - 1)]);
                }
                break;
            case PlayerActions.Fall:

                break;

            case PlayerActions.Checkpoint:

                break;
        }
    }

    private void OnEnable()
    {
        _playerSubject.AddObserver(this);
    }

    private void OnDisable()
    {
        _playerSubject.RemoveObserver(this);
    }
}
