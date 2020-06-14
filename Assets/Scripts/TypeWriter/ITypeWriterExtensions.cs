using System;

namespace ModuloEscritura.EscrituraTextMeshPro
{
    public static partial class ITypeWriterExtensions
    {
        public static void AddOnPrintedCharacterListener(this ITypeWriter typeWriter, Action<char> onPrintedCharacterListener)
        {
            typeWriter.OnPrintedCharacter += onPrintedCharacterListener;
        }

        public static void AddOnWriteCompletedListener(this ITypeWriter typeWriter, Action onWriteCompleted)
        {
            typeWriter.OnWriteCompleted += onWriteCompleted;
        }

        public static void RemoveOnPrintedCharacterListener(this ITypeWriter typeWriter, Action<char> onPrintedCharacterListener)
        {
            typeWriter.OnPrintedCharacter -= onPrintedCharacterListener;
        }

        public static void RemoveOnWriteCompletedListener(this ITypeWriter typeWriter, Action onWriteCompleted)
        {
            typeWriter.OnWriteCompleted -= onWriteCompleted;
        }
    }
}