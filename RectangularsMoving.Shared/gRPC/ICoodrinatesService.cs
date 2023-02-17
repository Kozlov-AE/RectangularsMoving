using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace RectangularsMoving.Shared.gRPC {
    [ServiceContract]
    public interface ICoodrinatesService {
    []
    }
    [DataContract]
    public class HelloReply {
        [DataMember(Order = 1)]
        public string Message { get; set; }
    }
}
