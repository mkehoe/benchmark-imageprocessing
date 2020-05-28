using System;
using System.Collections.Generic;

namespace ImageProcessingApiPerfTest
{
    public class ResultsDTO
    {
        
        public IList<TimestampDTO> resizeTimestamps { get; set; }
        public IList<TimestampDTO> tileTimestamps { get; set; }
    }
}
