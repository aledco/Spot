using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spot.Business.Models.Result
{
    public class ErrorResult
    {
        public IList<string> Errors { get; set; } = [];
        public bool IsFatal { get; set; }
    }
}
