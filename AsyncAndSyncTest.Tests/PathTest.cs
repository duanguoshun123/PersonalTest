using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web;
namespace AsyncAndSyncTest.Tests
{
    [TestClass]
    public class PathTest
    {
        [TestMethod]
        public void GetPath()
        {
            var currentApplicationPath = HttpContext.Current.Request.ApplicationPath;
            var filePath = HttpContext.Current.Request.FilePath;
            var path = HttpContext.Current.Request.Path;
            return;
        }
        [TestMethod]
        public void Compare()
        {
            short s1 = 1;
            s1 = (short)(s1 + 1);
            return;
        }
    }
}
