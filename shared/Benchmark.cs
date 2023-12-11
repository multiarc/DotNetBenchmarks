using BenchmarkDotNet.Attributes;

namespace DotNetBench
{
    public class DummyClass {
        private readonly object? _dummyObj;

        public DummyClass(object dummyObj) {
            _dummyObj = dummyObj;
        }

        public DummyClass() {
            _dummyObj = null;
        }

        //trick optimizer to not cut off unused field
        public bool SomeOp() {
            return _dummyObj == null;
        }
    }
    
    public class Benchmark
    {
        private int _intDummy;
        private bool _boolDummy;
        
        [Benchmark]
        public void CreateTwoInstances()
        {
            var obj = new object();
            var dummy = new DummyClass(obj);
            
            //we need this to skip possible optimizations
            _boolDummy &= dummy.SomeOp();
        }

        [Benchmark]
        public void CreateObjectHashCode()
        {
            var obj = new object();
            //we need this to skip possible optimizations
            _intDummy ^= obj.GetHashCode();
        }

        [Benchmark]
        public void CreateObjectToString()
        {
            var obj = new object();
            //we need this to skip possible optimizations
            _intDummy ^= obj.ToString()!.Length;
        }

        [Benchmark]
        public void CreateObjectToGetType()
        {
            var obj = new object();
            //we need this to skip possible optimizations
            _boolDummy &= obj.GetType().IsValueType;
        }

        [Benchmark]
        public void CreateObjectPinUnpinArray()
        {
            var arr = new byte[1];
            Memory<byte> mem = arr;
            using var pin = mem.Pin();
            _intDummy ^= pin.GetHashCode();
        }

        [Benchmark]
        public void CreateObjectPinUnpinEmptyArray()
        {
            var arr = new byte[0];
            Memory<byte> mem = arr;
            using var pin = mem.Pin();
            _intDummy ^= pin.GetHashCode();
        }

        public bool GetBoolValue() {
            return _boolDummy;
        }

        public int GetIntValue() {
            return _intDummy;
        }
    }
}
