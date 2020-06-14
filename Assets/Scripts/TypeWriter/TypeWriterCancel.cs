using System;

namespace ModuloEscritura.EscrituraTextMeshPro
{
    public class TypeWriterCancel : IDisposable
    {
        public bool IsCancellationRequested { get; private set; }

        public TypeWriterCancel()
        {
            IsCancellationRequested = false;
        }

        public void Dispose()
        {
            IsCancellationRequested = true;
        }
    }
}