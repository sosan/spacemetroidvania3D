using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;

namespace ModuloEscritura.EscrituraTextMeshPro
{
    public static partial class TextTokenizer
    {
        static readonly Regex SplitPattern = new Regex("(<[^>]*>)");
        static readonly Regex TagNamePattern = new Regex("</*([^/<>=]+)(>|=)");

        static Dictionary<string, ITokenizer> tokenizerTable = new Dictionary<string, ITokenizer>();

        public static void RegisterTokenizer(string tagName, ITokenizer tokenizer)
        {
            tokenizerTable.Add(tagName, tokenizer);
        }

        public static List<IToken> Tokenize(string text)
        {
            var rawToken = SplitPattern.Split(text).Where(t => t != "");

            return rawToken.Select(t => TokenSelector(t)).ToList();
        }

        static IToken TokenSelector(string rawToken)
        {
            var tagMatch = TagNamePattern.Match(rawToken);

            if (tagMatch.Success)
            {
                var tagName = tagMatch.Groups[1].Value;
                var tokenizer = tokenizerTable[tagName];
                return tokenizer.Tokenize(rawToken);
            }
            else
            {
                return new TextToken(rawToken);
            }
        }
    }
}