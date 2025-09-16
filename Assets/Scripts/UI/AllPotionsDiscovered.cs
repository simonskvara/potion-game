using System;
using UnityEngine;
using NaughtyAttributes;

public class AllPotionsDiscovered : MonoBehaviour
{
    [SerializeField] private Animator animator;

    [AnimatorParam("animator")]
    [SerializeField] private string trigger;

    private void Start()
    {
        PotionDiscovery.Instance.AllPotionsDiscovered.AddListener(PlayAnimation);
    }

    private void PlayAnimation()
    {
        animator.SetTrigger(trigger);
    }

    private void OnDestroy()
    {
        PotionDiscovery.Instance.AllPotionsDiscovered.RemoveListener(PlayAnimation);
    }
}
