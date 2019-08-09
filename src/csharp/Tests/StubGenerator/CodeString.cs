//------------------------------------------------------------------------------
// <copyright file="CodeString.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
// </copyright>
//------------------------------------------------------------------------------
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using System.Text;

namespace Microsoft.Azure.Kinect.Sensor.Test.StubGenerator
{
    /// <summary>
    /// A string representation of C code.
    /// </summary>
    /// <remarks>
    /// This class captures source and line meta-data for the location where the string was assigned.
    /// This information is used when compiling the code, or in debugging the code, to reference back to the
    /// source location where the string was assigned.
    /// </remarks>
    [DataContract]
    public class CodeString
    {
        [DataMember]
        private string code;

        /// <summary>
        /// Initializes a new instance of the <see cref="CodeString"/> class with the specified C code.
        /// </summary>
        /// <param name="code">The C code string to represent.</param>
        public CodeString(string code)
        {
            this.AssignCode(code);
        }

        /// <summary>
        /// Gets or sets the code block.
        /// </summary>
        public string Code
        {
            get => this.code;
            set => this.AssignCode(this.code);
        }

        /// <summary>
        /// Gets a version of the code block with embedded #line data.
        /// </summary>
        public string EmbdededLineData
        {
            get
            {
                StringBuilder source = new StringBuilder();

                _ = source.AppendLine($"#line {this.SourceLineNumber} \"{this.SourceFileName.Replace("\\", "\\\\")}\"");
                _ = source.AppendLine(this.Code);
                return source.ToString();
            }
        }

        /// <summary>
        /// Gets the source file that defined the code block.
        /// </summary>
        public string SourceFileName { get; private set; } = string.Empty;

        /// <summary>
        /// Gets the line number that defined the code block.
        /// </summary>
        public int SourceLineNumber { get; private set; } = 0;

        /// <summary>
        /// Provides an implicit conversion from <see cref="CodeString"/> to a <see cref="string"/>.
        /// </summary>
        /// <param name="s">The <see cref="CodeString"/> to convert.</param>
        public static implicit operator string(CodeString s)
        {
            return s == null ? string.Empty : s.Code;
        }

        /// <summary>
        /// Provides an implicit conversion from <see cref="string"/> to a <see cref="CodeString"/>.
        /// </summary>
        /// <param name="s">The <see cref="string"/> to convert.</param>
        [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "The alternate for this is just the constructor.")]
        public static implicit operator CodeString(string s)
        {
            return new CodeString(s ?? string.Empty);
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return this.Code;
        }

        /// <summary>
        /// Performs the assignment of the code block and looks through the stack to find the origin information, filename and line number.
        /// </summary>
        /// <param name="code">The C code string to represent.</param>
        private void AssignCode(string code)
        {
            int frameDepth = 0;
            StackTrace trace = new StackTrace(true);
            StackFrame frame = trace.GetFrame(frameDepth);

            while (frame.GetMethod().DeclaringType == typeof(CodeString))
            {
                frame = trace.GetFrame(++frameDepth);
            }

            this.SourceLineNumber = frame.GetFileLineNumber();
            this.SourceFileName = frame.GetFileName();

            this.code = code;
        }
    }
}
