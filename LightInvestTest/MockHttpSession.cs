using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Text;

namespace LightInvestTest
{
	public class MockHttpSession : ISession
	{
		private readonly Dictionary<string, byte[]> _sessionStorage = new Dictionary<string, byte[]>();

		public string Id => throw new NotImplementedException();
		public bool IsAvailable => true;
		public IEnumerable<string> Keys => _sessionStorage.Keys;

		public void Clear() => _sessionStorage.Clear();

		public Task CommitAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;

		public Task LoadAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;

		public void Remove(string key) => _sessionStorage.Remove(key);

		public void Set(string key, byte[] value) => _sessionStorage[key] = value;

		public bool TryGetValue(string key, out byte[] value)
		{
			return _sessionStorage.TryGetValue(key, out value);
		}

		// Métodos auxiliares para facilitar o uso
		public void SetString(string key, string value)
		{
			Set(key, Encoding.UTF8.GetBytes(value));
		}

		public string GetString(string key)
		{
			return TryGetValue(key, out byte[] value) ? Encoding.UTF8.GetString(value) : null;
		}
	}
}