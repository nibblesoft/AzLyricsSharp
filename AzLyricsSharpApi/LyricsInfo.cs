using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AzLyricsSharpApi
{

    abstract class Contribuitor
    {
        public string Name { get; protected set; }
    }

    class Writer : Contribuitor
    {
        public Writer(string name)
        {
            Name = name;
        }
    }

    class Corrector : Contribuitor
    {
        public Corrector(string name)
        {
            Name = name;
        }
    }

    class LyricsInfo
    {
        private List<Writer> _writers;
        public string Artist { get; private set; }
        public string Title { get; private set; }
        public List<Writer> Writers { get { return _writers; } }
        public string Text { get; private set; }

        public LyricsInfo(string artist, string title, string[] writers, string lyric)
        {
            Artist = artist;
            Title = title;
            Text = lyric;

            _writers = new List<Writer>();
            for (int i = 0; i < writers.Length; i++)
                _writers.Add(new Writer(writers[i]));
        }
    }
}
