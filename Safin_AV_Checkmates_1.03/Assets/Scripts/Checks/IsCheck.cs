using UnityEngine;

namespace Checks
{
    public class IsCheck : MonoBehaviour
    {
        [SerializeField] private bool isBlackCheck = true;

        public bool IsBlackCheck
        {
            get
            {
                return isBlackCheck;
            }
        }
    }
}

