using System.Diagnostics;
using BenchmarkDotNet.Running;
using DotNetBench;

BenchmarkRunner.Run<Benchmark>();

var bench = new Benchmark();

//prevent removing pathway with removed fields and methods
Debug.Print(bench.GetBoolValue().ToString());
Debug.Print(bench.GetIntValue().ToString());
