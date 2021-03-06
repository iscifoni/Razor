﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using Microsoft.AspNetCore.Razor.Language.Intermediate;
using Xunit;

namespace Microsoft.AspNetCore.Razor.Language.CodeGeneration
{
    public class DefaultDocumentWriterTest
    {
        [Fact]
        public void WriteDocument_Empty_WritesChecksumByDefault()
        {
            // Arrange
            var codeDocument = TestRazorCodeDocument.CreateEmpty();
            var options = RazorCodeGenerationOptions.CreateDefault();

            var target = CodeTarget.CreateDefault(codeDocument, options);
            var context = new CSharpRenderingContext()
            {
                Options = options,
                Writer = new Legacy.CSharpCodeWriter(),
                CodeDocument = codeDocument
            };

            var writer = new DefaultDocumentWriter(target, context);

            var document = new DocumentIRNode();
            var builder = RazorIRBuilder.Create(document);

            // Act
            writer.WriteDocument(document);

            // Assert
            var csharp = context.Writer.Builder.ToString();
            Assert.Equal(
@"#pragma checksum ""test.cshtml"" ""{ff1816ec-aa5e-4d10-87f7-6f4963833460}"" ""da39a3ee5e6b4b0d3255bfef95601890afd80709""
", 
                csharp, 
                ignoreLineEndingDifferences: true);
        }

        [Fact]
        public void WriteDocument_Empty_SuppressChecksumTrue_WritesNothing()
        {
            // Arrange
            var codeDocument = TestRazorCodeDocument.CreateEmpty();
            var optionsBuilder = new DefaultRazorCodeGenerationOptionsBuilder()
            {
                SuppressChecksum = true
            };
            var options = optionsBuilder.Build();

            var target = CodeTarget.CreateDefault(codeDocument, options);
            var context = new CSharpRenderingContext()
            {
                Options = options,
                Writer = new Legacy.CSharpCodeWriter(),
                CodeDocument = codeDocument
            };

            var writer = new DefaultDocumentWriter(target, context);

            var document = new DocumentIRNode();
            var builder = RazorIRBuilder.Create(document);

            // Act
            writer.WriteDocument(document);

            // Assert
            var csharp = context.Writer.Builder.ToString();
            Assert.Empty(csharp);
        }

        [Fact]
        public void WriteDocument_WritesNamespace()
        {
            // Arrange
            var codeDocument = TestRazorCodeDocument.CreateEmpty();
            var options = RazorCodeGenerationOptions.CreateDefault();

            var target = CodeTarget.CreateDefault(codeDocument, options);
            var context = new CSharpRenderingContext()
            {
                Options = options,
                Writer = new Legacy.CSharpCodeWriter(),
                CodeDocument = codeDocument
            };

            var writer = new DefaultDocumentWriter(target, context);

            var document = new DocumentIRNode();
            var builder = RazorIRBuilder.Create(document);
            builder.Add(new NamespaceDeclarationIRNode()
            {
                Content = "TestNamespace",
            });

            // Act
            writer.WriteDocument(document);

            // Assert
            var csharp = context.Writer.Builder.ToString();
            Assert.Equal(
@"#pragma checksum ""test.cshtml"" ""{ff1816ec-aa5e-4d10-87f7-6f4963833460}"" ""da39a3ee5e6b4b0d3255bfef95601890afd80709""
namespace TestNamespace
{
    #line hidden
}
", 
                csharp, 
                ignoreLineEndingDifferences: true);
        }

        [Fact]
        public void WriteDocument_WritesClass()
        {
            // Arrange
            var codeDocument = TestRazorCodeDocument.CreateEmpty();
            var options = RazorCodeGenerationOptions.CreateDefault();

            var target = CodeTarget.CreateDefault(codeDocument, options);
            var context = new CSharpRenderingContext()
            {
                Options = options,
                Writer = new Legacy.CSharpCodeWriter(),
                CodeDocument = codeDocument
            };
            var writer = new DefaultDocumentWriter(target, context);

            var document = new DocumentIRNode();
            var builder = RazorIRBuilder.Create(document);
            builder.Add(new ClassDeclarationIRNode()
            {
                AccessModifier = "internal",
                BaseType = "TestBase",
                Interfaces = new List<string> { "IFoo", "IBar", },
                Name = "TestClass",
            });

            // Act
            writer.WriteDocument(document);

            // Assert
            var csharp = context.Writer.Builder.ToString();
            Assert.Equal(
@"#pragma checksum ""test.cshtml"" ""{ff1816ec-aa5e-4d10-87f7-6f4963833460}"" ""da39a3ee5e6b4b0d3255bfef95601890afd80709""
internal class TestClass : TestBase, IFoo, IBar
{
}
",
                csharp,
                ignoreLineEndingDifferences: true);
        }

        [Fact]
        public void WriteDocument_WritesMethod()
        {
            // Arrange
            var codeDocument = TestRazorCodeDocument.CreateEmpty();
            var options = RazorCodeGenerationOptions.CreateDefault();

            var target = CodeTarget.CreateDefault(codeDocument, options);
            var context = new CSharpRenderingContext()
            {
                Options = options,
                Writer = new Legacy.CSharpCodeWriter(),
                CodeDocument = codeDocument
            };
            var writer = new DefaultDocumentWriter(target, context);

            var document = new DocumentIRNode();
            var builder = RazorIRBuilder.Create(document);
            builder.Add(new MethodDeclarationIRNode()
            {
                AccessModifier = "internal",
                Modifiers = new List<string> { "virtual", "async", },
                Name = "TestMethod",
                ReturnType = "string",
            });

            // Act
            writer.WriteDocument(document);

            // Assert
            var csharp = context.Writer.Builder.ToString();
            Assert.Equal(
@"#pragma checksum ""test.cshtml"" ""{ff1816ec-aa5e-4d10-87f7-6f4963833460}"" ""da39a3ee5e6b4b0d3255bfef95601890afd80709""
#pragma warning disable 1998
internal virtual async string TestMethod()
{
}
#pragma warning restore 1998
",
                csharp,
                ignoreLineEndingDifferences: true);
        }

        [Fact]
        public void WriteDocument_WritesField()
        {
            // Arrange
            var codeDocument = TestRazorCodeDocument.CreateEmpty();
            var options = RazorCodeGenerationOptions.CreateDefault();

            var target = CodeTarget.CreateDefault(codeDocument, options);
            var context = new CSharpRenderingContext()
            {
                Options = options,
                Writer = new Legacy.CSharpCodeWriter(),
                CodeDocument = codeDocument
            };
            var writer = new DefaultDocumentWriter(target, context);

            var document = new DocumentIRNode();
            var builder = RazorIRBuilder.Create(document);
            builder.Add(new FieldDeclarationIRNode()
            {
                AccessModifier = "internal",
                Modifiers = new List<string> { "readonly", },
                Name = "_foo",
                Type = "string",
            });

            // Act
            writer.WriteDocument(document);

            // Assert
            var csharp = context.Writer.Builder.ToString();
            Assert.Equal(
@"#pragma checksum ""test.cshtml"" ""{ff1816ec-aa5e-4d10-87f7-6f4963833460}"" ""da39a3ee5e6b4b0d3255bfef95601890afd80709""
internal readonly string _foo;
",
                csharp,
                ignoreLineEndingDifferences: true);
        }

        [Fact]
        public void WriteDocument_WritesProperty()
        {
            // Arrange
            var codeDocument = TestRazorCodeDocument.CreateEmpty();
            var options = RazorCodeGenerationOptions.CreateDefault();

            var target = CodeTarget.CreateDefault(codeDocument, options);
            var context = new CSharpRenderingContext()
            {
                Options = options,
                Writer = new Legacy.CSharpCodeWriter(),
                CodeDocument = codeDocument
            };
            var writer = new DefaultDocumentWriter(target, context);

            var document = new DocumentIRNode();
            var builder = RazorIRBuilder.Create(document);
            builder.Add(new PropertyDeclarationIRNode()
            {
                AccessModifier = "internal",
                Modifiers = new List<string> { "virtual", },
                Name = "Foo",
                Type = "string",
            });

            // Act
            writer.WriteDocument(document);

            // Assert
            var csharp = context.Writer.Builder.ToString();
            Assert.Equal(
@"#pragma checksum ""test.cshtml"" ""{ff1816ec-aa5e-4d10-87f7-6f4963833460}"" ""da39a3ee5e6b4b0d3255bfef95601890afd80709""
internal virtual string Foo { get; set; }
",
                csharp,
                ignoreLineEndingDifferences: true);
        }
    }
}
