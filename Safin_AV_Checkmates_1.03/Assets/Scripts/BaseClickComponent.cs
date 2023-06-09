﻿using System.Linq;

using UnityEngine;
using UnityEngine.EventSystems;

namespace Checks
{
    public abstract class BaseClickComponent : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        //Меш игрового объекта
        protected MeshRenderer _mesh;
        //Список материалов на меше объекта
        protected Material[] _meshMaterials = new Material[3];

        protected Material focusMaterial;
        protected Material clickMaterial;
        protected Material blackChipMaterial;
        protected Material whiteChipMaterial;
        private Material blackCellMaterial;

        [Tooltip("Цветовая сторона игрового объекта"), SerializeField]
        private ColorType _color;

        /// <summary>
        /// Возвращает цветовую сторону игрового объекта
        /// </summary>
        public ColorType GetColor => _color;

        /// <summary>
        /// Возвращает или устанавливает пару игровому объекту
        /// </summary>
        /// <remarks>У клеток пара - фишка, у фишек - клетка</remarks>
        public BaseClickComponent Pair { get; set; }

        /// <summary>
        /// Добавляет дополнительный материал
        /// </summary>
        public void AddAdditionalMaterial(Material material, int index = 1)
        {
            if (index < 1 || index > 2)
            {
                Debug.LogError("Попытка добавить лишний материал. Индекс может быть равен только 1 или 2");
                return;
            }
           
            _meshMaterials[index] = material;
            _mesh.materials = _meshMaterials.Where(t => t != null).ToArray();
        }

        public void AddAdditionalMaterial(MeshRenderer _mesh, Material[] _meshMaterials, Material material, int index = 1)
        {
            if (index < 1 || index > 2)
            {
                Debug.LogError("Попытка добавить лишний материал. Индекс может быть равен только 1 или 2");
                return;
            }
            _meshMaterials[0] = blackCellMaterial;
            _meshMaterials[index] = material;
            _mesh.materials = _meshMaterials.Where(t => t != null).ToArray();
        }

        /// <summary>
        /// Удаляет дополнительный материал
        /// </summary>
        public void RemoveAdditionalMaterial(int index = 1)
        {
            if (index < 1 || index > 2)
            {
                Debug.LogError("Попытка удалить несуществующий материал. Индекс может быть равен только 1 или 2");
                return;
            }
            _meshMaterials[index] = null;
            _mesh.materials = _meshMaterials.Where(t => t != null).ToArray();
        }

        public void RemoveAdditionalMaterial( MeshRenderer _mesh, Material[] _meshMaterials, ColorType checkColor, int index = 1)
        {
            if (index < 1 || index > 2)
            {
                Debug.LogError("Попытка удалить несуществующий материал. Индекс может быть равен только 1 или 2");
                return;
            }

            _meshMaterials[index] = null;
            _meshMaterials[0] = checkColor == ColorType.Black ? blackChipMaterial : whiteChipMaterial;
            _mesh.materials = _meshMaterials.Where(t => t != null).ToArray();
        }

        /// <summary>
        /// Событие клика на игровом объекте
        /// </summary>
        public event ClickEventHandler OnClickEventHandler;

        /// <summary>
        /// Событие наведения и сброса наведения на объект
        /// </summary>
        public event FocusEventHandler OnFocusEventHandler;


        //При навадении на объект мышки, вызывается данный метод
        //При наведении на фишку, должна подсвечиваться клетка под ней
        //При наведении на клетку - подсвечиваться сама клетка
        public abstract void OnPointerEnter(PointerEventData eventData);

        //Аналогично методу OnPointerEnter(), но срабатывает когда мышка перестает
        //указывать на объект, соответственно нужно снимать подсветку с клетки
        public abstract void OnPointerExit(PointerEventData eventData);

        //При нажатии мышкой по объекту, вызывается данный метод
        public void OnPointerClick(PointerEventData eventData)
		{
            OnClickEventHandler?.Invoke(-1,-1);
        }

        //Этот метод можно вызвать в дочерних классах (если они есть) и тем самым пробросить вызов
        //события из дочернего класса в родительский
        protected void CallBackEvent(CellComponent target, bool isSelect)
        {
            OnFocusEventHandler?.Invoke(target, isSelect);
		}

        private void Awake()
        {
            focusMaterial = Resources.Load<Material>("Materials/FocusMaterial");
            clickMaterial = Resources.Load<Material>("Materials/ClickMaterial");
            blackCellMaterial = Resources.Load<Material>("Materials/BlackCell");
            blackChipMaterial = Resources.Load<Material>("Materials/BlackCheck");
            whiteChipMaterial = Resources.Load<Material>("Materials/WhiteCheck");
        }

        protected virtual void Start()
        {
            _mesh = GetComponent<MeshRenderer>();
            //Этот список будет использоваться для набора материалов у меша,
            //в данном ДЗ достаточно массива из 3 элементов
            //1 элемент - родной материал меша, он не меняется
            //2 элемент - материал при наведении курсора на клетку/выборе фишки
            //3 элемент - материал клетки, на которую можно передвинуть фишку
             _meshMaterials[0] = _mesh.material;
        }
	}

    public enum ColorType
    {
        White,
        Black
    }

    public delegate void ClickEventHandler(int x, int y);
    public delegate void FocusEventHandler(CellComponent component, bool isSelect);
}