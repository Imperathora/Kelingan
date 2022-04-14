using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OOB.UI;
using OOB.Sectors;
using OOB.Player;
using OOB;

public class UIAnimationController : MonoBehaviour
{
    [SerializeField] private Animator m_coinAnimator;
    [SerializeField] private Animator m_starAnimator;
    [SerializeField] private Animator m_fadeAnimator;
    [SerializeField] private Portal m_portal;

    private PlayerCharacter m_player;
    private PlayerAnimationController m_playerAnimation;

    private void Start()
    {
        TryGetPlayer();

        CollectibleDisplay.OnReverseStar += () => CollectableAnimation(m_starAnimator, "flyOut", "StarUIReverse");
        CollectibleDisplay.OnFlyInStar += () => CollectableAnimation(m_starAnimator, "pickedUp", "StarUIFlyIn");
        CollectibleDisplay.OnReverseCoin += () => CollectableAnimation(m_coinAnimator, "flyOut", "CoinUIReverse");
        CollectibleDisplay.OnFlyInCoin += () => CollectableAnimation(m_coinAnimator, "pickedUp", "CoinUIFlyIn");

        m_portal.OnEnterPortal += FadeOutAnimation;
    }

    private void TryGetPlayer()
    {
        m_player = WorldComponent.Instance?.GetPlayerCharacter();

        if (m_player)
        {
            m_playerAnimation = m_player.GetPlayerAnimationController();

            m_player.OnPlayerSpawn += FadeInAnimation;
            m_playerAnimation.OnFadeOutStarted += FadeOutAnimation;
        } else
        {
            WorldComponent.Instance.OnInitialization += TryGetPlayer;
        }
    }

    private void OnDestroy()
    {
        CollectibleDisplay.OnReverseStar -= () => CollectableAnimation(m_starAnimator, "flyOut", "StarUIReverse");
        CollectibleDisplay.OnFlyInStar -= () => CollectableAnimation(m_starAnimator, "pickedUp", "StarUIFlyIn");
        CollectibleDisplay.OnReverseCoin += () => CollectableAnimation(m_coinAnimator, "flyOut", "CoinUIReverse");
        CollectibleDisplay.OnFlyInCoin -= () => CollectableAnimation(m_coinAnimator, "pickedUp", "CoinUIFlyIn");

        m_portal.OnEnterPortal -= FadeOutAnimation;
        m_player.OnPlayerSpawn -= FadeInAnimation;
        m_playerAnimation.OnFadeOutStarted -= FadeOutAnimation;
    }

    private void CollectableAnimation(Animator _animator, string _trigger, string _animation)
    {
        if (_animator == null)
            return;

        _animator.SetTrigger(_trigger);
        _animator.Play(_animation);
        _animator.ResetTrigger(_trigger);
    }

    private void FadeOutAnimation()
    {
        if (m_fadeAnimator == null)
            return;

        m_fadeAnimator.SetTrigger("IsFadingOut");
    }

    private void FadeInAnimation()
    {
        if (m_fadeAnimator == null)
            return;


        m_fadeAnimator.Play("Base Layer.FadeIn", 0, 0.25f);
    }

}
