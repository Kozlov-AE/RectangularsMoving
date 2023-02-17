using ProtoBuf.Grpc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace RectangularsMoving.Shared.gRPC {
    [ServiceContract]
    public interface ISettingsService {
        [OperationContract]
        Task<RectReply> SetBoardSettingsAsync(BoardSettingsRequest request, CallContext context = default);
    }
    [DataContract]
    public class BoardSettingsRequest {
        [DataMember(Order = 1)] public int Width { get; set; }
        [DataMember(Order = 2)] public int Height { get; set; }
        [DataMember(Order = 3)] public int RectsCount { get; set; }
        [DataMember(Order = 4)] public int TreadsCount { get; set; }
    }
    [DataContract]
    public class RectReply {
        [DataMember(Order = 0)] public int Width { get; set; }
        [DataMember(Order = 1)] public int Height { get; set; }
        [DataMember(Order = 2)] public int X { get; set; }
        [DataMember(Order = 3)] public int Y { get; set; }
        [DataMember(Order = 4)] public int Id { get; set; }
    }
}
