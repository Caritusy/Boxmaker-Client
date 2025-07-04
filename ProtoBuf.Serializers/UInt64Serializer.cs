using System;
using ProtoBuf.Meta;

namespace ProtoBuf.Serializers;

internal sealed class UInt64Serializer : IProtoSerializer
{
	private static readonly Type expectedType = typeof(ulong);

	bool IProtoSerializer.RequiresOldValue => false;

	bool IProtoSerializer.ReturnsValue => true;

	public Type ExpectedType => expectedType;

	public UInt64Serializer(TypeModel model)
	{
	}

	public object Read(object value, ProtoReader source)
	{
		return source.ReadUInt64();
	}

	public void Write(object value, ProtoWriter dest)
	{
		ProtoWriter.WriteUInt64((ulong)value, dest);
	}
}
