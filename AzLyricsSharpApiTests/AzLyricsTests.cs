using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace AzLyricsSharpApi.Tests
{
    [TestClass()]
    public class AzLyricsTests
    {
        [TestMethod()]
        public void AzLyricsTest()
        {
            var artist = "2 Chainz";
            var title = "dresser lil boy";
            var azLyrics = new AzLyrics(artist, title);
            var lyrics = azLyrics.GetLyris();
            var path = "D:\\Downloaded Lyrics\\" + artist + " - " + title + ".txt";
            if (lyrics.Length > 10)
            {
                File.WriteAllText(path, lyrics);
                Assert.IsTrue(true);
            }
            else
                Assert.Fail();
            /*using (FileStream file = File.Create(path))
            {
                file.Write(Encoding.UTF8.GetBytes(lyrics), 0, lyrics.Length);
            }*/
            //TextWriter tr = TextWriter.Null;
            //System.Threading.Thread.Sleep(100000);
        }
    }
}
