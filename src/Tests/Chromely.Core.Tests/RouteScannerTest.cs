﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RouteScannerTest.cs" company="Chromely Projects">
//   Copyright (c) 2017-2018 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------

namespace Chromely.Core.Tests
{
    using System.Reflection;
    using Chromely.Core.RestfulService;
    using Xunit;
    using Xunit.Abstractions;

    /// <summary>
    /// The route scanner test.
    /// </summary>
    public class RouteScannerTest
    {
        /// <summary>
        /// The m_test output.
        /// </summary>
        // ReSharper disable once NotAccessedField.Local
        private readonly ITestOutputHelper mTestOutput;

        /// <summary>
        /// Initializes a new instance of the <see cref="RouteScannerTest"/> class.
        /// </summary>
        /// <param name="testOutput">
        /// The test output.
        /// </param>
        public RouteScannerTest(ITestOutputHelper testOutput)
        {
            this.mTestOutput = testOutput;
        }

        /// <summary>
        /// The scan test.
        /// </summary>
        [Fact]
        public void ScanTest()
        {
            // Note that the current assembly scan will include this file
            // And all other routes defined in other files in the assembly
            var scanner = new RouteScanner(Assembly.GetExecutingAssembly());
            Assert.NotNull(scanner);

            var result = scanner.Scan();
            Assert.NotNull(result);

            Assert.Equal(9, result.Count);
        }

        /// <summary>
        /// The scanner controller.
        /// </summary>
        [ControllerProperty(Name = "ScannerController", Route = "scannercontroller")]
        public class ScannerController : ChromelyController
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="ScannerController"/> class.
            /// </summary>
            public ScannerController()
            {
                this.RegisterGetRequest("/scannercontroller/get1", this.Get1);
                this.RegisterGetRequest("/scannercontroller/get2", this.Get2);
                this.RegisterPostRequest("/scannercontroller/save", this.Save);
            }

            /// <summary>
            /// The get 1.
            /// </summary>
            /// <param name="request">
            /// The request.
            /// </param>
            /// <returns>
            /// The <see cref="ChromelyResponse"/>.
            /// </returns>
            private ChromelyResponse Get1(ChromelyRequest request)
            {
                var response = new ChromelyResponse { Data = 1000 };
                return response;
            }

            /// <summary>
            /// The get 2.
            /// </summary>
            /// <param name="request">
            /// The request.
            /// </param>
            /// <returns>
            /// The <see cref="ChromelyResponse"/>.
            /// </returns>
            private ChromelyResponse Get2(ChromelyRequest request)
            {
                var response = new ChromelyResponse { Data = "Test Get 2" };
                return response;
            }

            /// <summary>
            /// The save.
            /// </summary>
            /// <param name="request">
            /// The request.
            /// </param>
            /// <returns>
            /// The <see cref="ChromelyResponse"/>.
            /// </returns>
            private ChromelyResponse Save(ChromelyRequest request)
            {
                var response = new ChromelyResponse { Data = request.PostData };
                return response;
            }
        }
    }
}
