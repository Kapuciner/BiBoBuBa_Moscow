using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    public const string PLAYER_IDLE = "Player_idle";
    public const string PLAYER_ATTACK = "Player_attack";
    public const string PLAYER_WALK = "Player_walk";
    public const string PLAYER_ABILITY = "Player_ability";
    public const string PLAYER_DEATH = "Player_death";

    [SerializeField] private Animator _animator;

    [SerializeField] private PlayerController _controller;
    [SerializeField] private Player _player;

    private bool canAnimationBeChanged = true;

    public void SetAnimation(string animation)
    {
        _animator.Play(animation);
    }
    public float GetCurrentAnimationLength()
    {
        AnimatorStateInfo animationState = _animator.GetCurrentAnimatorStateInfo(0);
        AnimatorClipInfo[] animatorClip = _animator.GetCurrentAnimatorClipInfo(0);
        //float time = animatorClip[0].clip.length * animationState.normalizedTime;
        float time = animatorClip[0].clip.length;
        return time;
    }

    public void SetAnimator(RuntimeAnimatorController animator)
    {
        _animator.StopPlayback();
        _animator.runtimeAnimatorController = animator;
        _animator.StartPlayback();
        _animator.speed = 1;
    }
    private void Awake()
    {
        _controller.PlayerAttacks += () => { StartCoroutine(AttackingRoutine()); };
        _controller.PlayerCasts += () => { StartCoroutine(CastingRoutine()); };
        _player.PlayerDies += () => { StartCoroutine(DeathRoutine());};

        _animator.runtimeAnimatorController = _player.VegetableType.Animator;
    }

    IEnumerator AttackingRoutine()
    {
        SetAnimation(PLAYER_ATTACK);
        canAnimationBeChanged = false;
        _controller.CanCast = false;
        float time = GetCurrentAnimationLength();
        yield return new WaitForSeconds(time);
        _controller.CanCast = true;
        canAnimationBeChanged = true;
    }

    IEnumerator CastingRoutine()
    {
        SetAnimation(PLAYER_ABILITY);
        yield return new WaitForEndOfFrame();
        canAnimationBeChanged = false;
        if (_player.controller.PlayerAbility is GarlicAbility == false)
        {
            _controller.SetCanMove(false);
            _controller.StopImmediate();
        }

        _controller.CanReloadAbility = false;
        _controller.CanAttack = false;
        float time = GetCurrentAnimationLength();
        yield return new WaitForSeconds(time);
        _controller.SetCanMove(true);
        _controller.CanAttack = true;
        canAnimationBeChanged = true; 
        _controller.CanReloadAbility = true;
    }

    public void SetPlayTime(float time)
    {
        _animator.playbackTime = time;
    }

    // public IEnumerator LoopFromTime(float time)
    // {
    //     _animator.playbackTime = time;
    // }
    IEnumerator DeathRoutine()
    {
        SetAnimation(PLAYER_DEATH);
        canAnimationBeChanged = false;
        _controller.SetCanMove(false);
        _controller.StopImmediate();
        _controller.CanAttack = false;
        float time = GetCurrentAnimationLength();
        StartCoroutine(DragDown(time));
        yield return new WaitForSeconds(time);
        _player.gameObject.SetActive(false);
        _player.transform.position = _player.SpawnPoint.transform.position;
        canAnimationBeChanged = true;
    }

    private IEnumerator DragDown(float time)
    {
        _player.GetComponent<SphereCollider>().isTrigger = false;
        float elapsed = 0;
        Vector3 old = _player.transform.position;
        Vector3 newPos = old + new Vector3(0, -0, 0);
        while (elapsed < time)
        {
            // _player.transform.position = Vector3.Lerp(old, newPos, elapsed / time);
            elapsed += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        _player.GetComponent<Collider>().isTrigger = false;
    }
    private void Update()
    {
        if (canAnimationBeChanged == false)
        {   
            return;
        }
        if (_controller.GetVelocity() == 0)
        {
            SetAnimation(PLAYER_IDLE);
        }
        else
        {
            SetAnimation(PLAYER_WALK); 
        }
    }
}
