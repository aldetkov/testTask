using AxGrid.Base;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AxGrid.Test
{
    public class CardView : MonoBehaviourExt, IPointerClickHandler
    {

        [SerializeField] private UnityEngine.UI.Text nameText = null;
        [SerializeField] private Image image = null;

        public int Value { get; private set; }
        public RectTransform RectTransf { get; private set; }

        public void InitUICard(string name, Sprite image, int id)
        {
            nameText.text = name;
            this.image.sprite = image;
            Value = id;
            RectTransf = GetComponent<RectTransform>();
        }


        public void OnPointerClick(PointerEventData eventData)
        {
            //s Debug.Log("click" + name);
            GetComponentInParent<CardCollecView>().OnDelete(this);
            Settings.Model.EventManager.Invoke("OnSelectCard", Value);
        }
    }
}