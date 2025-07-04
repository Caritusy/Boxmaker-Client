using System;
using ProtoBuf.Meta;

namespace ProtoBuf.Serializers;

internal sealed class DoubleSerializer : IProtoSerializer
{
	private static readonly Type expectedType = typeof(double);

	bool IProtoSerializer.RequiresOldValue => false;

	bool IProtoSerializer.ReturnsValue => true;

	public Type ExpectedType => expectedType;

	public DoubleSerializer(TypeModel model)
	{
	}

	public object Read(object value, ProtoReader source)
	{
		return source.ReadDouble();
	}

	public void Write(object value, ProtoWriter dest)
	{
		ProtoWriter.WriteDouble((double)value, dest);
	}
}
