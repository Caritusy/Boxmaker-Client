using System;
using ProtoBuf.Meta;

namespace ProtoBuf.Serializers;

internal sealed class Int16Serializer : IProtoSerializer
{
	private static readonly Type expectedType = typeof(short);

	bool IProtoSerializer.RequiresOldValue => false;

	bool IProtoSerializer.ReturnsValue => true;

	public Type ExpectedType => expectedType;

	public Int16Serializer(TypeModel model)
	{
	}

	public object Read(object value, ProtoReader source)
	{
		return source.ReadInt16();
	}

	public void Write(object value, ProtoWriter dest)
	{
		ProtoWriter.WriteInt16((short)value, dest);
	}
}
