﻿using System.Threading.Tasks;
using CryptographyLib.Interfaces;
namespace CryptographyLib.FeistelNetwork
{
	public class FeistelNetwork : ISymmetricEncryptor
	{
		private byte[]? _key;

		public byte[]? Key
		{
			get => _key;
			set => _key = value;
		}

		public Task<byte[]>[] Expand(byte[] key)
		{
			throw new System.NotImplementedException();
		}

		public Task<byte[]> Encrypt(byte[] value, byte[] key)
		{
			throw new System.NotImplementedException();
		}

		public async Task<byte[]> Decrypt(byte[] value, byte[] key)
		{
			throw new System.NotImplementedException();
		}
	}
}
