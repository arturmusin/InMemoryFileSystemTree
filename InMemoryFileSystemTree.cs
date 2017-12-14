using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AM.InMemoryFileSystemTree
{
	/// <summary>
	/// Represents in-memory file system tree
	/// </summary>
	public class InMemoryFileSystemTree : IFileSystemTree, IEnumerable<Node>
	{
		/// <summary>
		/// The topmost "root" node of the tree
		/// </summary>
		public Node Root { get; private set; }

		/// <summary>
		/// Default ctor
		/// </summary>
		public InMemoryFileSystemTree(string rootDir = "root_dir")
		{
			Root = new Node(rootDir, rootDir, NodeType.Directory);
		}

		/// <summary>
		/// Gets all the files at the specified path
		/// </summary>
		/// <param name="path">e.g. "/Root/dir1/sudir2/"</param>
		/// <returns>List of file records</returns>
		public IEnumerable<Node> GetFilesAtPath(string path)
		{
			string[] pathChunks = path.Split(new[] { '/' });
			string startingChunk = pathChunks.First();
			var endingChunks = pathChunks.Skip(1).ToList();
			var prefixPath = string.Empty;

			string postfixPath = string.Join("/", endingChunks);
			prefixPath += startingChunk + "/";

			var current = Root;

			while (current != null)
			{
				if (postfixPath.Contains("/")) // DIR
				{
					// check if DIR exists
					var nextDirName = postfixPath.Split(new[] { '/' })?[0];

					var nextDir = current.Nodes.FirstOrDefault(x => x.Name == nextDirName);
					if (nextDir == null) // DIR doesn't exist - non-existing path was specified
					{
						throw new Exception($"Path: [{prefixPath}] does not exist");
					}
					else // DIR exist - passing back recursively
					{
						endingChunks = postfixPath.Split(new[] { '/' }).ToList();
						postfixPath = string.Join("/", endingChunks.Skip(1).ToList());

						current = nextDir;
					}
				}
				else // FILE
				{
					var result = current.Nodes
						.Where(n => n.NodeType == NodeType.File);

					return result;
				}
			}

			return null;
		}

		/// <summary>
		/// Creates new Node under path specified
		/// </summary>
		public void Add(string path, long fileSize = 0, long fileCreatedAt = 0)
		{
			InsertNode(Root, path);
		}

		/// <summary>
		/// Recursively adds Node to the Tree
		/// </summary>
		private void InsertNode(Node root, string path)
		{
			string[] pathChunks;
			string newNodeName;
			IList<string> endingChunks;
			string currPath = path;
			Node rootPtr = root;			

			string postfixPath = "/";
			string prefixPath = string.Empty;

			while (true)
			{
				pathChunks = currPath.Split(new[] { '/' });
				newNodeName = pathChunks[0];
				endingChunks = pathChunks.Skip(1).ToList();

				prefixPath += newNodeName + "/";
				postfixPath = string.Join("/", endingChunks);

				var dir = root.Nodes.FirstOrDefault(x => x.Name == newNodeName);
				if (dir == null)
				{
					// FILE: break loop
					if (newNodeName.Contains("."))
					{
						string pathToFile = rootPtr.Name + "/" + path;
						var fileToCreate = new Node(newNodeName, pathToFile, NodeType.File);
						root.Nodes.Add(fileToCreate);
						root.FileCount++;

						break;
					}

					// DIRECTORY: new
					string pathToDir = rootPtr.Name + "/" + prefixPath.TrimEnd(new [] { '/' });
					var dirToCreate = new Node(newNodeName, pathToDir, NodeType.Directory);
					root.Nodes.Add(dirToCreate);
					root.DirectoryCount++;

					root = dirToCreate; // re-pointing Node
				}
				else
				{
					// DIRECTORY: existing
					root = dir;
				}

				currPath = postfixPath;
			}
		}

		#region Iteration

		/// <summary>
		/// Fetches elements in breadth-first traversal manner
		/// </summary>
		public IEnumerator<Node> GetEnumerator()
		{
			var traverseOrder = new Queue<Node>();

			var q = new Queue<Node>();
			q.Enqueue(Root);

			var set = new HashSet<Node>();
			set.Add(Root);

			while (q.Count > 0)
			{
				Node e = q.Dequeue();
				traverseOrder.Enqueue(e);

				foreach (Node n in e.Nodes)
				{
					if (!set.Contains(n))
					{
						q.Enqueue(n);
						set.Add(n);
					}
				}
			}

			while (traverseOrder.Count > 0)
			{
				Node e = traverseOrder.Dequeue();
				yield return e;
			}
		}

		/// <summary>
		/// Fetches elements in breadth-first traversal manner
		/// </summary>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
		#endregion

		/// <summary>
		/// Returns JSON serialized string of the whole Tree
		/// </summary>
		public IEnumerable<Node> ToJson()
		{
			foreach (Node node in this)
				if (node.NodeType == NodeType.Directory)
					yield return node;
		}
	}
}