using UnityEngine;

namespace Checks
{
    public class IsCheck : MonoBehaviour
    {
        [SerializeField] bool isBlackcheck = true;

        public bool IsBlackcheck
        {
            get
            {
                return isBlackcheck;
            }
        }
    }
}

