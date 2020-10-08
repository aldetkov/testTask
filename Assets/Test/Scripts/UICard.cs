using AxGrid.Base;
using UnityEngine;
using UnityEngine.UI;

namespace AxGrid.Test
{

    public class UICard : MonoBehaviourExt
    {
        [SerializeField] private UnityEngine.UI.Text nameText = null;
        [SerializeField] private Image image = null;

        public void InitUICard(string name, Sprite image)
        {
            nameText.text = name;
            this.image.sprite = image;
        }
    }
}