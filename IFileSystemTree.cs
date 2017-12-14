using System.Collections.Generic;

namespace AM.InMemoryFileSystemTree
{
	/// <summary>
	/// Defines generic functionality of any file-system
	/// </summary>
	public interface IFileSystemTree
	{
		Node Root { get; }
		IEnumerable<Node> GetFilesAtPath(string path);
		void Add(string path, long fileSize, long fileCreatedAt);
	}
}