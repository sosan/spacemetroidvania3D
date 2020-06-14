using System.Collections;
using System.Threading;
using UnityEngine;

namespace ModuloEscritura.EscrituraTextMeshPro
{
    public class DelayOnceTagToken : ITagToken
    {
        WaitForSeconds delay;

        public string Text { get; private set; }

        public DelayOnceTagToken(string rawToken)
        {
            Text = rawToken;

            float delaySeconds;
            if (float.TryParse(this.GetParameter(), out delaySeconds))
            {
                delay = new WaitForSeconds(delaySeconds);
            }
            else
            {
                delay = null;
            }
        }

        public void Print(IWriter writer)
        {
        }

        public IEnumerator PrintCoroutine(IWriter writer, TypeWriterCancel cancel)
        {
            yield return delay;
        }
    }

    public class DelayOnceTagTokenizer : ITokenizer
    {
        public static void Register()
        {
            TextTokenizer.RegisterTokenizer("delayonce", new DelayOnceTagTokenizer());
        }

        public IToken Tokenize(string rawToken)
        {
            return new DelayOnceTagToken(rawToken);
        }
    }
}