// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Microsoft.AspNetCore.Razor.Language.CodeGeneration
{
    public struct CSharpCodeWritingScope : IDisposable
    {
        private CSharpCodeWriter _writer;
        private bool _autoSpace;
        private int _tabSize;
        private int _startIndent;

        public CSharpCodeWritingScope(CSharpCodeWriter writer) : this(writer, true) { }
        public CSharpCodeWritingScope(CSharpCodeWriter writer, int tabSize) : this(writer, tabSize, true) { }
        // TODO: Make indents (tabs) environment specific
        public CSharpCodeWritingScope(CSharpCodeWriter writer, bool autoSpace) : this(writer, 4, autoSpace) { }
        public CSharpCodeWritingScope(CSharpCodeWriter writer, int tabSize, bool autoSpace)
        {
            _writer = writer;
            _autoSpace = true;
            _tabSize = tabSize;
            _startIndent = -1; // Set in WriteStartScope

            OnClose = () => { };

            WriteStartScope();
        }

        public Action OnClose;

        public void Dispose()
        {
            WriteEndScope();
            OnClose();
        }

        private void WriteStartScope()
        {
            TryAutoSpace(" ");

            _writer.WriteLine("{");
            _writer.CurrentIndent += _tabSize;
            _startIndent = _writer.CurrentIndent;
        }

        private void WriteEndScope()
        {
            TryAutoSpace(_writer.NewLine);

            // Ensure the scope hasn't been modified
            if (_writer.CurrentIndent == _startIndent)
            {
                _writer.CurrentIndent -= _tabSize;
            }

            _writer.WriteLine("}");
        }

        private void TryAutoSpace(string spaceCharacter)
        {
            if (_autoSpace && 
                _writer.Builder.Length > 0 && 
                !char.IsWhiteSpace(_writer.Builder[_writer.Builder.Length - 1]))
            {
                _writer.Write(spaceCharacter);
            }
        }
    }
}
