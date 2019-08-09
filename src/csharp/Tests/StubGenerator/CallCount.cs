//------------------------------------------------------------------------------
// <copyright file="CallCount.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
// </copyright>
//------------------------------------------------------------------------------
using System.Collections.Generic;

namespace Microsoft.Azure.Kinect.Sensor.Test.StubGenerator
{
    /// <summary>
    /// Access the call counts of functions in the stubbed native assembly.
    /// </summary>
    public class CallCount
    {
        private readonly StubbedModule module;
        private readonly Dictionary<string, int> initialCount;

        /// <summary>
        /// Initializes a new instance of the <see cref="CallCount"/> class.
        /// </summary>
        /// <param name="module">The stubbed module to query for call counts.</param>
        internal CallCount(StubbedModule module)
        {
            this.module = module;
            this.initialCount = new Dictionary<string, int>();

            foreach (FunctionInfo function in module.NativeInterface.Functions)
            {
                this.initialCount.Add(function.Name, module.GetTotalCallCount(function.Name));
            }
        }

        /// <summary>
        /// Gets the number of calls to the specified function since the class was created.
        /// </summary>
        /// <param name="function">The name of the function to get the number of calls for.</param>
        /// <returns>The number of calls to the specified function since the class was created.</returns>
        public int Calls(string function)
        {
            return this.module.GetTotalCallCount(function) - this.initialCount[function];
        }
    }
}
