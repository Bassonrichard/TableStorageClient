using BenchmarkDotNet.Running;
using TableStorageClient.Console;

var summary = BenchmarkRunner.Run<MyTableServiceBenchmark>();