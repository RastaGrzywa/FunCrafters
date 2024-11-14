using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ItemsDisplay
{
    public class ItemObject : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI noText;
        [SerializeField] private TextMeshProUGUI descriptionText;
        [SerializeField] private Image badgeImage;
        [SerializeField] private GameObject highlightObject;
        
        public void UpdateVisuals(int itemIndex, string description, Sprite badgeSprite, bool isSpecial)
        {
            noText.text = $"{itemIndex}";
            descriptionText.text = description;
            if (badgeSprite != null)
            {
                badgeImage.sprite = badgeSprite;
            }
            highlightObject.SetActive(isSpecial);
        }
    }
}