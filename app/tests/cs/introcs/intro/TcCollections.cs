namespace Introcs.Intro.Tests {

using System;
using System.Linq;
using NUnit.Framework;
using System.Collections.Generic;

using Util = Introcs.Util.Library;

[TestFixture]
public class TcCollections : Base.ClsBase {
  private float epsilon = 0.001f; //1.20e-7f;

  [OneTimeSetUp]
  public override void SetUpClass()
  {
      base.SetUpClass();
      Console.Error.WriteLine("SetUpClass({0})", GetType());
  }

  [OneTimeTearDown]
  public override void TearDownClass()
  {
      Console.Error.WriteLine("TearDownClass({0})", GetType());
      base.TearDownClass();
  }

  [SetUp]
  public override void SetUp()
  {
      base.SetUp();
      Console.Error.WriteLine("SetUp");
  }

  [TearDown]
  public override void TearDown()
  {
      Console.Error.WriteLine("TearDown");
      base.TearDown();
  }

  public override void Dispose()
  {
      Console.Error.WriteLine("Derived Dispose({0})", GetType());
      base.Dispose();
  }

	[Test] [Category("Tag4")]
	public void QueueTest()
	{
	  float[] floatArr = {25.7f, 0.1f, 78.5f, 52.3f};
	  Queue<float> queue1 = new Queue<float>();

	  Assert.AreEqual(0, queue1.Count, "isEmpty");
	  foreach (float elem in floatArr) {
      queue1.Enqueue(elem);
    }

	  Assert.AreEqual(floatArr.Length, queue1.Count, "length");
	  Assert.AreEqual(floatArr[0], queue1.Peek(), "peek");
	  queue1.Enqueue(-0.5f);
	  Assert.AreEqual(true, queue1.Contains(-0.5f), "offer");
	  Assert.AreEqual(floatArr[0], queue1.Dequeue(), epsilon * floatArr[0],
		"poll");
		Assert.AreEqual("[0.1, 78.5, 52.3, -0.5]",
			Util.MkString<float>(queue1.ToArray()), "toString");
	}

	[Test] [Category("Tag4")]
	public void ListTest()
	{
	  long[] numArr = {16L, 2L, 77L, 29L};
	  long[] nines = {9L, 9L, 9L, 9L};
	  List<long> lst1 = new List<long>();

	  Assert.AreEqual(0, lst1.Count, "isEmpty");
	  //foreach (long elem in numArr) {
    //    lst1.Add(elem);
    //}
    lst1.AddRange(numArr);

	  Assert.AreEqual(numArr.Length, lst1.Count, "length");
	  Assert.AreEqual(numArr[0], lst1[0], "first");
	  Assert.AreEqual(numArr[2], lst1[2], "nth");
	  Assert.AreEqual(1, lst1.IndexOf(numArr[1]), "indexOf");
	  lst1.AddRange(nines);
	  Assert.AreEqual(numArr.Length + nines.Length, lst1.Count, "append");
	  lst1.Sort();
	  Assert.AreEqual("[2, 9, 9, 9, 9, 16, 29, 77]",
		Util.MkString<long>(lst1), "toString");
	}

	[Test] [Category("Tag4")]
	public void DictionaryTest()
	{
	  char[] charArr = {'a', 'e', 'k', 'p', 'u', 'k', 'a'};
	  Dictionary<string, char> dict1 = new Dictionary<string, char>();
	  string key1;

	  Assert.AreEqual(0, dict1.Count, "isEmpty");
	  for (int i = charArr.Length - 1; -1 < i; --i) {
      key1 = string.Format("letter {0}", i % 5);

      if (!dict1.ContainsKey(key1))
        dict1.Add(key1, charArr[i]);
    }

	  Assert.AreEqual(true, dict1.ContainsKey("letter 1"), "contains");
	  Assert.AreEqual('k', dict1["letter 2"], "get");
	  dict1.Remove("letter 2");
	  Assert.AreEqual(false, dict1.ContainsKey("letter 2"), "remove");
	}

	[Test] [Category("Tag4")]
	public void SetTest()
	{
	  char[] charArr = {'a', 'e', 'k', 'p', 'u', 'k', 'a'};
	  HashSet<char> set1 = new HashSet<char>();

	  Assert.AreEqual(0, set1.Count, "isEmpty");
	  for (int i = charArr.Length - 1; -1 < i; --i) {
      set1.Add(charArr[i]);
    }

    HashSet<char> setOld = new HashSet<char>(set1);
	  set1.UnionWith(new char[] {'q', 'p', 'z', 'u'});
	  Assert.True(set1.SetEquals(
			new char[] {'a', 'e', 'k', 'p', 'u', 'q', 'z'}), "union");

		set1 = new HashSet<char>(setOld);
	  set1.IntersectWith(new char[] {'q', 'p', 'z', 'u'});
	  Assert.True(set1.SetEquals(new char[] {'p', 'u'}), "intersect");

		set1 = new HashSet<char>(setOld);
	  set1.ExceptWith(new char[] {'q', 'p', 'z', 'u'});
	  Assert.True(set1.SetEquals(new char[] {'a', 'e', 'k'}),
			"difference");

		set1 = new HashSet<char>(setOld);
	  set1.SymmetricExceptWith(new char[] {'q', 'p', 'z', 'u'});
	  Assert.True(set1.SetEquals(new char[] {'a', 'e', 'k', 'q', 'z'}),
			"xor");
	}
}

}
