﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using Newtonsoft.Json.Linq;

namespace Newtonsoft.Json.Tests.Benchmarks
{
    public class JTokenBenchmarks
    {
        private static readonly JObject JObjectSample = JObject.Parse(PerformanceTests.JsonText);
        private static readonly string JsonTextSample;
        private static readonly string NestedJsonText;

        static JTokenBenchmarks()
        {
            JObject o = new JObject();
            for (int i = 0; i < 50; i++)
            {
                o[i.ToString()] = i;
            }
            JsonTextSample = o.ToString();

            NestedJsonText = (new string('[', 100000)) + "1" + (new string(']', 100000));
        }

        [Benchmark]
        public void TokenWriteTo()
        {
            StringWriter sw = new StringWriter();
            JObjectSample.WriteTo(new JsonTextWriter(sw));
        }

        [Benchmark]
        public Task TokenWriteToAsync()
        {
            StringWriter sw = new StringWriter();
            return JObjectSample.WriteToAsync(new JsonTextWriter(sw));
        }

        [Benchmark]
        public JObject JObjectParse()
        {
            return JObject.Parse(JsonTextSample);
        }

        [Benchmark]
        public JArray JArrayNestedParse()
        {
            return JArray.Parse(NestedJsonText);
        }

        [Benchmark]
        public JArray JArrayNestedBuild()
        {
            JArray current = new JArray();
            JArray root = current;
            for (int j = 0; j < 100000; j++)
            {
                JArray temp = new JArray();
                current.Add(temp);
                current = temp;
            }
            current.Add(1);

            return root;
        }
    }
}