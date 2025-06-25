using System;
using System.Collections.Generic;
using ProtoBuf;

namespace protocol.game;

[Serializable]
[ProtoContract(Name = "smsg_play_map")]
public class smsg_play_map : IExtensible
{
	private byte[] _map_data;

	private readonly List<int> _x = new List<int>();

	private readonly List<int> _y = new List<int>();

	private IExtension extensionObject;

	[ProtoMember(1, IsRequired = true, Name = "map_data", DataFormat = DataFormat.Default)]
	public byte[] map_data
	{
		get
		{
			return _map_data;
		}
		set
		{
			_map_data = value;
		}
	}

	[ProtoMember(2, Name = "x", DataFormat = DataFormat.TwosComplement)]
	public List<int> x => _x;

	[ProtoMember(3, Name = "y", DataFormat = DataFormat.TwosComplement)]
	public List<int> y => _y;

	IExtension IExtensible.GetExtensionObject(bool createIfMissing)
	{
		return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
	}
}
