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
    void Start()
    {
        
    }

    private void Update()
    {
        if (_playerManager.GetVelocity() == 0)
        {
            SetAnimation(player_idle);
        }
        else
        {
            SetAnimation(player_walk);
        }
    }

    public void SetAnimation(string animation)
    {
        _animator.Play(animation);
    }
}
