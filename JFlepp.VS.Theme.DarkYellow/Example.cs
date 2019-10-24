using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JFlepp.VS.Theme.DarkYellow
{
    public struct ExampleStruct
    {
        public int ExampleField;

        public ExampleStruct(int exampleParameter)
        {
            this.ExampleField = exampleParameter;
        }
    }
}

namespace JFlepp.VS.Theme.DarkYellow.ExampleNamespace
{
    public enum ExampleEnum 
    {
        Good,
        Bad,
        Unknown,
    } 
}


namespace JFlepp.VS.Theme.DarkYellow.ExampleNamespace
{

    [Example]
    public class Example
    {
        private int exampleField;
        public int ExampleProperty { get; set; }
        public void ExampleMethod()
        {
            return;
        }

        public string ExampleMethod(int exampleParameter)
        {
            const string exampleConstant = nameof(exampleConstant);
            var exampleLocal = exampleParameter.ToString();
            return exampleLocal + exampleConstant;
        }
    }
}
