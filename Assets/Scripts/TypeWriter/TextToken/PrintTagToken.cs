using System.Collections;
using System.Threading;

namespace ModuloEscritura.EscrituraTextMeshPro
{
    public class PrintTagToken : ITagToken
    {
        public string Text { get; private set; }

        public PrintTagToken(string rawToken)
        {
            Text = rawToken;
        }

        public void Print(IWriter writer)
        {
            writer.AddText(Text);
        }

        public IEnumerator PrintCoroutine(IWriter writer, TypeWriterCancel cancel)
        {
            if (cancel.IsCancellationRequested)
            {
                yield break;
            }

            Print(writer);
            writer.SetText();
            yield return writer.Delay;
        }
    }

    public class PrintTagTokenizer : ITokenizer
    {
        public IToken Tokenize(string rawToken)
        {
            return new PrintTagToken(rawToken);
        }
    }
}