using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Accessibility;
using HtmlAgilityPack;
using Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT;
using Microsoft.Toolkit.Wpf.UI.Controls;

namespace KSPUpdater.Extensions
{
    public class MyWebView
    {
        private Dispatcher Dispatcher { get => Application.Current.Dispatcher; }
        private WebView _wb;
        private Semaphore _sem;
        private Semaphore _semContentLoaded;

        private WebViewRequestType _requestType;
        private object _requestResult;

        public MyWebView(WebView wb)
        {
            _wb = wb;
            _wb.DOMContentLoaded += OnDOMContentLoaded;
            _wb.NavigationCompleted += OnNavigationCompleted;

            _sem = new Semaphore(1,1);
            _semContentLoaded = new Semaphore(0,1);

            _requestResult = null;
        }

        ~MyWebView()
        {
            _wb.DOMContentLoaded -= OnDOMContentLoaded;
            _wb.NavigationCompleted -= OnNavigationCompleted;
            _wb.Dispose();
        }

        public string LoadAndGetRedirectionURLOf(string url)
        {
            string toRet = null;

            _sem.WaitOne();
            _requestType = WebViewRequestType.GetRedirection;
            Dispatcher.Invoke(() => _wb.Navigate(url));

            _semContentLoaded.WaitOne();

            toRet = (string) this._requestResult;
            _sem.Release();

            return toRet;
        }

        public HtmlDocument LoadAndGetContentOf(string url)
        {
            HtmlDocument toRet = null;

            _sem.WaitOne();
            _requestType = WebViewRequestType.GetContent;
            Dispatcher.Invoke(() => _wb.Navigate(url));

            _semContentLoaded.WaitOne();

            toRet = (HtmlDocument) this._requestResult;
            _sem.Release();

            return toRet;
        }

        public HtmlDocument GetContentOfCurrentURL()
        {
            var toRet = new HtmlDocument();

            Dispatcher.Invoke(() =>
            {
                var DocumentString = _wb.InvokeScript("eval", new string[] {"document.documentElement.outerHTML;"});

                toRet.LoadHtml(DocumentString);
            });

            return toRet;
        }


        #region EventHandler
        // When the HTML and JS are loaded, but JS not executed
        private void OnDOMContentLoaded(object sender, WebViewControlDOMContentLoadedEventArgs e)
        {
        }

        // When the HTML and JS are loaded and executed (some content can be loaded by the JS so when this event is triggered, this is the final content)
        private void OnNavigationCompleted(object sender, WebViewControlNavigationCompletedEventArgs e)
        {
            if(_requestType == WebViewRequestType.GetRedirection)
            {
                _requestType = WebViewRequestType.GetURL;
                return;
            }
            else if (_requestType == WebViewRequestType.GetURL)
            {
                _requestResult = e.Uri.ToString();
            }
            else if (_requestType == WebViewRequestType.GetContent)
            {
                _requestResult = GetContentOfCurrentURL();
            }
            _semContentLoaded.Release();
        }

        #endregion


    }
}
