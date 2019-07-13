using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Void.Selenium
{
    public interface IRoboBrowser
    {
        byte[] GetScreenshot();
        bool IsContentLoaded();
        bool CloseAlert();
        IAlert GetAlert();
        void Scroll(int offset);
        Task SoftScrollAsync(int offset);
        Task BackAsync();
    }
}
