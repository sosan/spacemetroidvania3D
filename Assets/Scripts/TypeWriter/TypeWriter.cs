using UnityEngine;
using TMPro;
using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;

namespace ModuloEscritura.EscrituraTextMeshPro
{
    [DisallowMultipleComponent]
    public class TypeWriter : MonoBehaviour, ITypeWriter
    {
        int lastCharacterIndex = -1;
        char[] currentText;
        public bool IsSkipRequested { get; set; }
        TypeWriterCancel writeCoroutineCancel;

        [SerializeField] int capacity = 128;
        [SerializeField] TypeWriterSettings settings;
        [SerializeField] TextMeshProUGUI textComponent;
        [SerializeField] TextMeshPro text3DComponent;

        public TypeWriterSettings Settings => settings;
        public WaitForSeconds Delay { get; set; }
        public bool IsPaused { get; set; }
        public bool IsWriting => writeCoroutineCancel != null && !writeCoroutineCancel.IsCancellationRequested;

        public event Action<char> OnPrintedCharacter;
        public event Action OnWriteCompleted;

        void Awake()
        {
            Alloc(capacity);
            Delay = Settings.DefaultDelay;
        }

        public void Clear()
        {
            IsPaused = false;
            IsSkipRequested = false;
            Delay = Settings.DefaultDelay;

            for (int i = 0; i < currentText.Length; ++i)
            {
                currentText[i] = '\0';
            }

            lastCharacterIndex = -1;
        }

        public IDisposable Write(string text)
        {
            if (IsWriting)
            {
                return writeCoroutineCancel;
            }

            if (text.Length > capacity)
            {
                Alloc(text.Length);
            }

            Clear();

            var token = TextTokenizer.Tokenize(text);

            writeCoroutineCancel = new TypeWriterCancel();

            StartCoroutine(WriteCoroutineEnumerator(writeCoroutineCancel, token));

            return writeCoroutineCancel;
        }

        public void Skip()
        {
            if (writeCoroutineCancel != null)
            {
                IsSkipRequested = true;
            }
        }

        public void Dispose()
        {
            if (writeCoroutineCancel != null)
            {
                writeCoroutineCancel.Dispose();
                writeCoroutineCancel = null;
            }
        }

        public void SetText()
        {
            textComponent.SetCharArray(currentText);

            if (OnPrintedCharacter != null)
            {
                OnPrintedCharacter(currentText[lastCharacterIndex]);
            }
        }

        public void AddText(char addCharacter)
        {
            ++lastCharacterIndex;
            currentText[lastCharacterIndex] = addCharacter;
        }

        public void AddText(string addString)
        {
            for (int i = 0; i < addString.Length; ++i)
            {
                AddText(addString[i]);
            }
        }

        IEnumerator WriteCoroutineEnumerator(TypeWriterCancel cancel, List<IToken> token)
        {
            for (int i = 0; i < token.Count; ++i)
            {
                if (cancel.IsCancellationRequested)
                {
                    yield break;
                }

                if (IsPaused)
                {
                    --i;
                    yield return null;
                }
                else
                {
                    if (IsSkipRequested)
                    {
                        token[i].Print(this);
                    }
                    else
                    {
                        yield return token[i].PrintCoroutine(this, cancel);
                    }
                }
            }

            if (IsSkipRequested)
            {
                SetText();
            }

            WriteCompleted();
        }

        void WriteCompleted()
        {
            if (OnWriteCompleted != null)
            {
                OnWriteCompleted();
            }

            writeCoroutineCancel = null;
            IsSkipRequested = false;
            IsPaused = false;
        }

        void Alloc(int capacity)
        {
            this.capacity = capacity;
            currentText = new char[capacity];
        }
    }
}