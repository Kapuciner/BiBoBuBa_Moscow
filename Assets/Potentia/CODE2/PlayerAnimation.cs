using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public string player_idle = "player_idle";
    public string player_walk = "player_walk";
    public string player_die = "player_die";
    [SerializeField] private PlayerManager _playerManager;
    [SerializeField] private Animator _animator;

    public bool blockAnimation = false;
    void Start()
    {
        _animator.SetInteger("playerID", _playerManager.playerID);
    }

    private void Update()
    {
        if(!blockAnimation){
        if (_playerManager.GetVelocity() == 0)
        {
                _animator.SetFloat("velocity", 0);
        }
        else
        {
                _animator.SetFloat("velocity", 1);
            }
        }
    }

    public void SetAnimation(string animation)
    {
        _animator.Play(animation);
    }

    public void DeathAnimation(bool run)
    {
        if (run)
        {
            _animator.SetInteger("death", 1);
        }
        else
        {
            _animator.SetInteger("death", 0);
        }
    }
}
