using System.Collections;
using System.Threading;

namespace ModuloEscritura.EscrituraTextMeshPro
{
    public class TagToken : ITagToken
    {
        public string Text { get; private set; }

        public TagToken(string rawToken)
        {
            Text = rawToken;
        }

        public void Print(IWriter writer)
        {
            writer.AddText(Text);
        }

        public IEnumerator PrintCoroutine(IWriter writer, TypeWriterCancel cancel)
        {
            Print(writer);
            yield break;
        }
    }

    public class TagTokenizer : ITokenizer
    {
        public IToken Tokenize(string rawToken)
        {
            return new TagToken(rawToken);
        }
    }
}