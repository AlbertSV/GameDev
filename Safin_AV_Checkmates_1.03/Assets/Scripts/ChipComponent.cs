using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Checks
{
    public class ChipComponent : BaseClickComponent
    {
        protected bool isClicked = false;

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                OnClickEventHandler += ClickController;
            }
            if (Input.GetMouseButtonUp(0))
            {
                OnClickEventHandler -= ClickController;
            }
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            CallBackEvent((CellComponent)Pair, true);
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            CallBackEvent((CellComponent)Pair, false);
        }

        private void ClickController()
        {
            AddAdditionalMaterial(clickMaterial);
            _mesh.material = _meshMaterials[2];
            _mesh.gameObject.AddComponent<Selected>();

            isClicked = true;
        }
    }
}
