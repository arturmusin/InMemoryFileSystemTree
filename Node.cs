using System.Collections.Generic;

namespace AM.InMemoryFileSystemTree
{
	/// <summary>
	/// Represents a single Node of a Tree
	/// </summary>
	public class Node
	{
		/// <summary>
		/// Name of the File/Directory
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Full path to the File/Directory
		/// </summary>
		public string Path { get; set; }

		/// <summary>
		/// Type: [File] or [Directory]
		/// </summary>
		public NodeType NodeType { get; set; }

		/// <summary>
		/// Size of the file in bytes
		/// </summary>
		public long FileSize { get; set; }

		/// <summary>
		/// Unix time-stamp when Node was created
		/// </summary>
		public long CreatedAt { get; set; }

		/// <summary>
		/// Descendants of the Node
		/// </summary>
		public IList<Node> Nodes { get; set; }

		/// <summary>
		/// Number of Directories created under certain Directory Node
		/// </summary>
		public int DirectoryCount { get; set; }

		/// <summary>
		/// Number of Files created under certain Directory Node
		/// </summary>
		public int FileCount { get; set; }

		/// <summary>
		/// Node constructor
		/// </summary>
		/// <param name="name">Name of the File/Directory</param>
		/// <param name="path">Full path to the File/Directory</param>
		/// <param name="nodeType">Type of the Node: File or Directory</param>
		public Node(string name, string path, NodeType nodeType)
		{
			Name = name;
			Path = path;
			NodeType = nodeType;
			Nodes = new List<Node>();
		}
	}
}