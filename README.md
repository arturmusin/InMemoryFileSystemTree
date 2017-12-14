# InMemoryFileSystemTree
[![License: Unlicense](https://img.shields.io/badge/license-Unlicense-blue.svg)](http://unlicense.org/)

InMemoryFileSystemTree is a in-memory representation of the file-system consisting of nested directories and files.

## Motivation

I was trying to create a simple CMS system in C# .NET and use CKEditor along with its plug-in RoxyFileman.
I needed to generate a JSON representation of the directory and files structure in certain format.
Basically I had to convert this:

[
	"root_dir/file0.pdf",
	"root_dir/dir1/subdir1/file1.txt",
	"root_dir/dir1/subdir1/file2.jpg",
	"root_dir/dir2/subdir2/subsubdir3/file3.doc"
]

into this:

[
   {"p":"root_dir", "f":"3", "d":"2"},
   {"p":"root_dir/dir1", "f":"2", "d":"0"},
   {"p":"root_dir/dir2", "f":"5", "d":"1"},
   {"p":"root_dir/dir2/subdir1", "f":"9", "d":"0"}
]

Since all the files were stored as the binaries in the database, I needed have some mechanism to pretend that we HAVE such directories structure.
I was looking for a way to store binary files in the database where every file had its absolute path which physically did not exist in the file system.
I had to provide feature that any file system has out of the box like:
 
* having unique file names under the same directory
* ability to create nested directories and files in them
* ability to retrieve all the files under specific directory at certain path.

I couldn't find any complete solution for this problem, so I decided to implement In-Memory File-System myself.

## Goals

* Being able to create virtual directory structure out of the list of paths to files
* Provide ability to retrieve a list of files located at path provided.
* Not allowing to have same file name at the same directory level

## Features

At the moment the following features are implemented:

* Ability to create in-memory file-system tree with file names.
* Retrieving list of file names at certain path.
* Always having a pointer to the topmost directory (the root).


## Implementation

At the heart of the system there is the `IFileSystemTree` interface. This describes the operations that should be provided by every file-system:

public interface IFileSystemTree
{
	Node Root { get; }
	IEnumerable<Node> GetFilesAtPath(string path);
	void Add(string path, long fileSize, long fileCreatedAt);
}

Constraints:

* Independent of the OS, the directory-separator is always `/`.
* All paths always start with `/`. This means: there are no relative paths.
* All paths that refer to a directory end with `/`.
* All paths that refer to a file do not end with `/`.

Some examples of using `InMemoryFileSystemTree`:

Operation | Result
--- | ---
`Node rootDir = fileSystemTreeObject.Root` | `{Root: "root_dir"}`
`fileSystemTreeObject.Add("dir1/subdir1/file1.txt", 512, 1513222570)` | `void`
`IEnumerable<Node> files = fsTree.GetFilesAtPath("dir1/subdir1/")` | `["file1.txt", "file2.txt"]`