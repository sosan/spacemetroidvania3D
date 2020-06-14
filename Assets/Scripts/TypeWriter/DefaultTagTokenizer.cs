using System.Collections.Generic;

namespace ModuloEscritura.EscrituraTextMeshPro
{
    public static partial class TextTokenizer
    {
        static readonly ITokenizer tagTokenizer = new TagTokenizer();
        static readonly ITokenizer printTagTokenzier = new PrintTagTokenizer();

        static readonly Dictionary<string, ITokenizer> standardTokenizer = new Dictionary<string, ITokenizer>()
        {
            { "align", tagTokenizer },
            { "alpha", tagTokenizer },
            { "color", tagTokenizer },
            { "b", tagTokenizer },
            { "i", tagTokenizer },
            { "cspace", tagTokenizer },
            { "font", tagTokenizer },
            { "indent", tagTokenizer },
            { "line-height", tagTokenizer },
            { "line-indent", tagTokenizer },
            { "link", tagTokenizer },
            { "lowercase", tagTokenizer },
            { "uppercase", tagTokenizer },
            { "smallcaps", tagTokenizer },
            { "margin", tagTokenizer },
            { "mark", tagTokenizer },
            { "mspace", tagTokenizer },
            // { "noparse", tagTokenizer },
            { "br", tagTokenizer },
            { "nobr", tagTokenizer },
            { "page", tagTokenizer },
            { "pos", tagTokenizer },
            { "size", tagTokenizer },
            { "space", tagTokenizer },
            { "sprite", printTagTokenzier },
            { "sprite index", printTagTokenzier },
            { "sprite name", printTagTokenzier },
            { "s", tagTokenizer },
            { "u", tagTokenizer },
            { "style", tagTokenizer },
            { "sub", tagTokenizer },
            { "sup", tagTokenizer },
            { "voffset", tagTokenizer },
            { "width", tagTokenizer },
        };

        public static void RegisterDefaultTokenizer()
        {
            RegisterStandardTokenizer();
            RegisterCustomTokenizer();
        }

        public static void RegisterStandardTokenizer()
        {
            foreach (var pair in standardTokenizer)
            {
                TextTokenizer.RegisterTokenizer(pair.Key, pair.Value);
            }
        }

        public static void RegisterCustomTokenizer()
        {
            DelayTagTokenizer.Register();
            DelayOnceTagTokenizer.Register();
            BufferTagTokenizer.Register();
        }
    }
}