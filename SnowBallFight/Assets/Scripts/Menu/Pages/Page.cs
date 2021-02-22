
using UnityEngine;

namespace GameplayCore
{
    namespace Menu
    {
        public class Page : MonoBehaviour
        {
            public PageType pageType = PageType.none;
            public PageType previousPageType = PageType.none;


            #region publicFuntions
            public void OpenPage()
            {
                gameObject.SetActive(true);
            }

            public void ClosePage()
            {
                gameObject.SetActive(false);
            }
            #endregion

            #region privateFuntions
            #endregion
        }
    }
}
