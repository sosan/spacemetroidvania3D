using System.Collections;
using System.Threading;
using UnityEngine;

namespace ModuloEscritura.EscrituraTextMeshPro
{
    public class DelayTagToken : ITagToken
    {
        WaitForSeconds delay;

        public string Text { get; private set; }

        public DelayTagToken(string rawToken)
        {
            Text = rawToken;

            if (this.IsOpenTag())
            {
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
        }

        public void Print(IWriter writer)
        {
            if (this.IsOpenTag())
            {
                writer.Delay = delay;
            }
            else
            {
                writer.Delay = writer.Settings.DefaultDelay;
            }
        }

        public IEnumerator PrintCoroutine(IWriter writer, TypeWriterCancel cancel)
        {
            Print(writer);

            yield break;
        }
    }

    public class DelayTagTokenizer : ITokenizer
    {
        public static void Register()
        {
            TextTokenizer.RegisterTokenizer("delay", new DelayTagTokenizer());
        }

        public IToken Tokenize(string rawToken)
        {
            return new DelayTagToken(rawToken);
        }
    }
}