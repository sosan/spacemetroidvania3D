using System.Collections;
using System.Threading;
using System.Collections.Generic;

namespace ModuloEscritura.EscrituraTextMeshPro
{
    
    public class BufferTagToken : ITagToken
    {
        readonly List<IToken> token;

        public string Text { get; private set; }

        public BufferTagToken(string rawToken)
        {
            Text = rawToken.Replace("&lt;", "<").Replace("&gt;", ">");
            token = TextTokenizer.Tokenize(this.GetParameter());
        }

        public void Print(IWriter writer)
        {
            foreach (var t in token)
            {
                t.Print(writer);
            }
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

    public class BufferTagTokenizer : ITokenizer
    {
        public static void Register()
        {
            TextTokenizer.RegisterTokenizer("buffer", new BufferTagTokenizer());
        }

        public IToken Tokenize(string rawToken)
        {
            return new BufferTagToken(rawToken);
        }
    }
}