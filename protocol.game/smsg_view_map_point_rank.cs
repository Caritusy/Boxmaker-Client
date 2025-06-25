using System;
using System.Collections.Generic;
using ProtoBuf;

namespace protocol.game;

[Serializable]
[ProtoContract(Name = "smsg_view_map_point_rank")]
public class smsg_view_map_point_rank : IExtensible
{
	private readonly List<map_point_rank> _ranks = new List<map_point_rank>();

	private IExtension extensionObject;

	[ProtoMember(1, Name = "ranks", DataFormat = DataFormat.Default)]
	public List<map_point_rank> ranks => _ranks;

	IExtension IExtensible.GetExtensionObject(bool createIfMissing)
	{
		return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
	}
}
