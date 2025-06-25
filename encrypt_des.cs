using System.Text;
using Renci.SshNet.Security.Cryptography.Ciphers;
using Renci.SshNet.Security.Cryptography.Ciphers.Modes;
using Renci.SshNet.Security.Cryptography.Ciphers.Paddings;

public class encrypt_des
{
	private static string ekey = "tsjhtsjh";

	private static string eiv = "51478543";

	public static byte[] encode(byte[] inputByteArray)
	{
		byte[] bytes = Encoding.UTF8.GetBytes(ekey);
		byte[] bytes2 = Encoding.UTF8.GetBytes(eiv);
		PKCS5Padding pKCS5Padding = new PKCS5Padding();
		byte[] data = pKCS5Padding.Pad(8, inputByteArray);
		DesCipher desCipher = new DesCipher(bytes, new CbcCipherMode(bytes2), null);
		return desCipher.Encrypt(data);
	}
}
