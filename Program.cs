using System;

namespace AM.InMemoryFileSystemTree
{
	class Program
	{
		static void Main()
		{
			string[] paths = 
			{
				"dir1/subdir1/file1.txt",
				"dir1/subdir1/file2.txt",
				"dir2/subdir2/subsubdir2/file3.txt",
				"file0.txt"
				//"dir2/file2.txt"
			};

			var fsTree = new InMemoryFileSystemTree("root");
			foreach (var path in paths)
			{
				fsTree.Add(path, 0, 0);
			}

			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine($"Directory structure:");
			Console.ForegroundColor = ConsoleColor.Gray;
			var json = fsTree.ToJson();
			foreach (var node in json)
			{
				Console.WriteLine($"{{ \"path\": \"" + node.Path + "\", \"dirs\": \"" + node.DirectoryCount + "\", \"files\": \"" + node.FileCount + "\" },");
			}

			Console.WriteLine();

			string p = "/dir1/subdir1/";
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine($"Files at Path: {p}");
			Console.ForegroundColor = ConsoleColor.Gray;

			var files = fsTree.GetFilesAtPath(p);
			foreach(var file in files)
			{
				Console.WriteLine(file.Path);
			}
		}
	}
}