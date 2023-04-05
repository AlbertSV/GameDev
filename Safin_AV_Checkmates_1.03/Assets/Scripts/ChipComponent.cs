using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Checks
{
    public class ChipComponent : BaseClickComponent
    {
        private static bool isClicked = false;

        public static bool IsClicked
        {
            get { return isClicked; }
            set { isClicked = value; }
        }

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

            AddAdditionalMaterial(focusMaterial);
            _mesh.material = _meshMaterials[1];
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            CallBackEvent((CellComponent)Pair, false);

            RemoveAdditionalMaterial();

            if (isClicked)
            {
                _mesh.material = clickMaterial;
            }
            isClicked = false;
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
