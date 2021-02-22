using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameplayCore
{
    namespace Menu
    {
        public class PageController : MonoBehaviour
        {
            public static PageController Instance;
            public Page[] pages;

            private Hashtable m_Pages;
            private List<Page> m_OnList;
            private List<Page> m_OffList;

            private PageType activePageType = PageType.none;

            private void Awake()
            {
                Instance = this;

                m_Pages = new Hashtable();

                RegistryAllPage();

                
            }

            #region publicFunctions
            public void TurnPageOn(PageType _pageType)
            {
                if (_pageType == PageType.none) return;
                if (!PageExists(_pageType)) return;
                

                Page _page = GetPage(_pageType);
                activePageType = _page.pageType;
                _page.OpenPage();

            }
            public void TurnPageOff(PageType _off, PageType _on = PageType.none)
            {
                if (_off == PageType.none) return;
                if(!PageExists(_off)) return;

                Page _offPage = GetPage(_off);
                _offPage.ClosePage();
                activePageType = PageType.none;

                TurnPageOn(_on);
                

            }
            public bool StepBackPage()
            {
                if (activePageType == PageType.none) return false;
                if (!PageExists(activePageType)) return false;

                Page activePage = GetPage(activePageType);

                if (activePage.previousPageType == PageType.none) return false;

                TurnPageOff(activePage.pageType, activePage.previousPageType);

                return true;
            }

            #endregion

            #region privateFunctions
            private void RegistryAllPage()
            {
                foreach(Page _page in pages)
                {
                    RegisterPage(_page);
                }
            }

            private void RegisterPage(Page _page)
            {
                if (PageExists(_page.pageType)) return;
                

                m_Pages.Add(_page.pageType, _page);

            }

            private Page GetPage(PageType _pageType)
            {
                if(!PageExists(_pageType)) return null;
                

                return (Page)m_Pages[_pageType];
            }

            private bool PageExists(PageType _pageType)
            {
                return m_Pages.ContainsKey(_pageType);
            }
            #endregion
        }

    }
}

