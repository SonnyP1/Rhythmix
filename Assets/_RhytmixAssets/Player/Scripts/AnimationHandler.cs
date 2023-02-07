using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHandler : MonoBehaviour
{
    [Header("Generic")]
    [SerializeField] bool hasMultipleAttackAnimations;
    [SerializeField][Range(1, 4)] int attackAnimationCount;
    private Animator _playerAnimator;

    [Header("EffectsForAttacks")]
    [SerializeField] Transform _attackEffect1SpawnPoint;
    [SerializeField] GameObject _attackEffect;

    [SerializeField] Transform _attackEffect2SpawnPoint;
    private void Start()
    {
        _playerAnimator= GetComponent<Animator>();
    }
    public void PlayAttackAnimation(AttackType attackType)
    {
        if (_playerAnimator != null)
        {
            if (!_playerAnimator.GetBool("DeathBool"))
            {
                if (attackType == AttackType.Tap)
                {
                    if (hasMultipleAttackAnimations)
                    {
                        System.Random rand = new System.Random();
                        int randomNum = rand.Next(1, attackAnimationCount + 1);
                        //print(randomNum);
                        switch (randomNum)
                        {
                            case 1:
                                _playerAnimator.SetTrigger("AttackTrigger1");
                                if(_attackEffect != null)
                                    Instantiate(_attackEffect,_attackEffect1SpawnPoint);
                                break;
                            case 2:
                                _playerAnimator.SetTrigger("AttackTrigger2");
                                if(_attackEffect != null)
                                    Instantiate(_attackEffect,_attackEffect2SpawnPoint);
                                break;
                            case 3:
                                _playerAnimator.SetTrigger("AttackTrigger3");
                                break;
                            case 4:
                                _playerAnimator.SetTrigger("AttackTrigger4");
                                break;
                            default:
                                _playerAnimator.SetTrigger("AttackTrigger1");
                                break;
                        }
                    }
                    else
                    {
                        _playerAnimator.SetTrigger("AttackTrigger1");
                    }
                }
                else if (attackType == AttackType.SwipeUp)
                {
                    _playerAnimator.SetTrigger("JumpAttack");
                }
            }
        }
    }

    public void PlayHitAnimation()
    {
        if (_playerAnimator != null)
        {
            _playerAnimator.SetTrigger("HitTrigger");
        }
    }
}
