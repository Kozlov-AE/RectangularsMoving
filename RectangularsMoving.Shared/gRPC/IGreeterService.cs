using System.Runtime.Serialization;
using System.ServiceModel;
using ProtoBuf.Grpc;

namespace RectangularsMoving.Shared.gRPC;
[ServiceContract]
public interface IGreeterService
{
    [OperationContract]
    Task<HelloReply> SayHelloAsync(HelloRequest request,
        CallContext context = default);
}
[DataContract]
public class HelloReply
{
    [DataMember(Order = 1)]
    public string Message { get; set; }
}
[DataContract]
public class HelloRequest
{
    [DataMember(Order = 1)]
    public string Name { get; set; }
}