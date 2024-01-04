using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.DTOs
{
    public class SuccessDataResult<T> : DataResult<T>
    {
        public SuccessDataResult(T data, int count, string message)
          : base(data, count, true, message)
        {
        }

        public SuccessDataResult(T data, string message)
          : base(data, true, message)
        {
        }

        public SuccessDataResult(T data, int count)
          : base(data, count, true)
        {
        }

        public SuccessDataResult(T data)
          : base(data, true)
        {
        }

        public SuccessDataResult(string message)
          : base(default(T), true, message)
        {
        }

        public SuccessDataResult()
          : base(default(T), true)
        {
        }
    }
}
