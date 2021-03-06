// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;

using BSOA.Test.Components;

using Xunit;

namespace BSOA.Test.IO
{
    public class TreeReaderWriterTests
    {
        [Fact]
        public void TreeReaderWriter_ExtensionMethods_Tests()
        {
            // DateTime and Guid serialization covered in TreeSerializable.Basics

            Random r = new Random();
            Sample sample = new Sample(r);
            Sample sample2 = new Sample(r);

            // List and Dictionary serialization built-ins
            CollectionContainer<Sample> samples = new CollectionContainer<Sample>();
            samples.Add(sample);
            samples.Add(sample2);

            samples.AssertEqual(TreeSerializer.RoundTrip(samples, TreeFormat.Json));
            samples.AssertEqual(TreeSerializer.RoundTrip(samples, TreeFormat.Binary));


            // Null List / Dictionary handling
            samples.SetCollectionsNull();
            samples.AssertEqual(TreeSerializer.RoundTrip(samples, TreeFormat.Json));
            samples.AssertEqual(TreeSerializer.RoundTrip(samples, TreeFormat.Binary));
        }
    }
}
