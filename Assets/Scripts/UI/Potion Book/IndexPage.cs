using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class IndexPage : MonoBehaviour
{
    [BoxGroup("References")]
    [SerializeField]
    private RectTransform leftPageButtons;
    [BoxGroup("References")]
    [SerializeField]
    private RectTransform rightPageButtons;
    [BoxGroup("References")]
    [SerializeField]
    private IndexButton indexButtonPrefab;
    [BoxGroup("References")]
    [SerializeField]
    private GameObject rightHalfFillerImage;

    [BoxGroup("Layout")]
    [SerializeField]
    private int buttonsPerPage;

    public int SpreadCount { get; private set; }

    private readonly List<IndexButton> leftPool = new List<IndexButton>();
    private readonly List<IndexButton> rightPool = new List<IndexButton>();

    private IReadOnlyList<PotionEffect> potions;
    private Action<int> onSelectPotion;

    /// <summary>
    /// Builds the button pool and computes how many index spreads are needed. Called once.
    /// </summary>
    public void Build(IReadOnlyList<PotionEffect> potions, Action<int> onSelectPotion)
    {
        this.potions = potions;
        this.onSelectPotion = onSelectPotion;

        int totalHalves = Mathf.CeilToInt((float)potions.Count / buttonsPerPage);
        SpreadCount = Mathf.CeilToInt(totalHalves / 2f);

        for (int i = 0; i < buttonsPerPage; i++)
        {
            leftPool.Add(Instantiate(indexButtonPrefab, leftPageButtons));
            rightPool.Add(Instantiate(indexButtonPrefab, rightPageButtons));
        }
    }

    /// <summary>
    /// Displays the given index spread (left half + right half). If the right half falls past the
    /// last populated half-page, a filler image is shown there instead of buttons.
    /// </summary>
    public void ShowSpread(int spreadIndex)
    {
        int leftHalf = spreadIndex * 2;
        int rightHalf = leftHalf + 1;
        int totalHalves = Mathf.CeilToInt((float)potions.Count / buttonsPerPage);

        FillHalf(leftPool, leftHalf);

        bool rightIsImage = rightHalf >= totalHalves;
        rightHalfFillerImage.SetActive(rightIsImage);
        if (rightIsImage)
        {
            foreach (var b in rightPool)
                b.gameObject.SetActive(false);
        }
        else
        {
            FillHalf(rightPool, rightHalf);
        }
    }

    private void FillHalf(List<IndexButton> pool, int halfIndex)
    {
        int start = halfIndex * buttonsPerPage;
        for (int i = 0; i < pool.Count; i++)
        {
            int potionIndex = start + i;
            bool active = potionIndex < potions.Count;
            pool[i].gameObject.SetActive(active);
            if (active)
            {
                int captured = potionIndex;
                pool[i].Setup(potions[captured].DisplayName, () => onSelectPotion(captured));
            }
        }
    }
}
