//------------------------------------------------------------------------------
// <copyright file="BaseTestClass.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
// </copyright>
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Azure.Kinect.Sensor.Test.StubGenerator;
using NUnit.Framework;

namespace Microsoft.Azure.Kinect.Sensor.UnitTests
{
    [TestFixture]
    public class BaseTestClass : IDisposable
    {
        private bool disposed;
        private readonly StubbedModule nativeK4a;

        public BaseTestClass()
        {
            this.nativeK4a = StubbedModule.Get("k4a");
            if (this.nativeK4a == null)
            {
                NativeInterface k4ainterface = NativeInterface.Create(
                    EnvironmentInfo.CalculateFileLocation(@"k4a\k4a.dll"),
                    EnvironmentInfo.CalculateFileLocation(@"k4a\k4a.h"));

                this.nativeK4a = StubbedModule.Create("k4a", k4ainterface);
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>True</c> to release both managed and unmanaged resources; <c>False</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            // Prevent double dispose.
            if (!this.disposed)
            {
                if (disposing)
                {
                    // Release managed resources here.
                }

                // Release native unmanaged resources here.

                this.disposed = true;
            }
        }

        protected void ImplementRequiredFunctions()
        {
            this.nativeK4a.SetImplementation(@"

k4a_logging_message_cb_t *g_message_cb;

k4a_result_t k4a_set_debug_message_handler(
    k4a_logging_message_cb_t *message_cb,
    void *message_cb_context,
    k4a_log_level_t min_level)
{
    STUB_ASSERT(message_cb != NULL);
    STUB_ASSERT(message_cb_context == NULL);
    STUB_ASSERT(min_level >= K4A_LOG_LEVEL_WARNING);

    g_message_cb = message_cb;

    return K4A_RESULT_SUCCEEDED;
}");
        }
    }
}
