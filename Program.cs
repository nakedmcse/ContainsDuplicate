using System.Linq;

//C# timing of various contains duplicate methods

//Create large int list with far spaced duplicates
static List<int> CreateLargeList() {
    var retval = new List<int>();
    for(int i=0; i<100000; i++) retval.Add(i);
    retval.Add(99999); //add one duplicate at the end
    return retval; 
}

//Check for duplicates by scanning all elements against one another
static bool CheckDuplicateTrivial(List<int> items) {
    bool retval = false;
    for(int idx = 0; idx<items.Count; idx++) {
        for(int inner = 0; inner<items.Count; inner++) {
            if(inner == idx) continue; //do not compare to itself
            if(items[inner] == items[idx]) retval = true;
        }
    }
    return retval;
}

//Check for duplicates by scanning all elements and keep track of what we have seen
static bool CheckDuplicateImproved(List<int> items) {
    var alreadySeen = new List<int>();
    for(int idx = 0; idx<items.Count; idx++) {
        if(alreadySeen.Contains(items[idx])) return true;
        alreadySeen.Add(items[idx]);
    }
    return false;
}

//Check for duplicates using a dictionary
static bool CheckDuplicateDictionary(List<int> items) {
    var alreadySeen = new Dictionary<int,int> ();
    for(int idx = 0; idx<items.Count; idx++) {
        try { alreadySeen.Add(items[idx],0); } catch { return true; }  //Dictionary throws exception on dup key
    }
    return false;
}

//Check for duplicates by sort and scan
static bool CheckDuplicateSortScan(List<int> items) {
    var sortedItems = items.OrderBy(x => x).AsParallel().ToList();
    for(int idx = 0; idx<items.Count-1; idx++) {
       if(sortedItems[idx] == sortedItems[idx+1]) return true; 
    }
    return false;
}

//Check for duplicates using distinct
static bool CheckDuplicateDistinct(List<int> items) {
    return items.Count != items.Distinct().Count();
}

//Check for duplicates using a hash set
static bool CheckDuplicateHashset(List<int> items) {
    var hashBucket = new HashSet<int>();
    foreach(var item in items) {
        if(!hashBucket.Add(item)) return true;  //False means we have a duplicate
    }
    return false;
}

//Main
var trivialSW = new System.Diagnostics.Stopwatch();
var improvedSW = new System.Diagnostics.Stopwatch();
var dictionarySW = new System.Diagnostics.Stopwatch();
var sortscanSW = new System.Diagnostics.Stopwatch();
var distinctSW = new System.Diagnostics.Stopwatch();
var hashsetSW = new System.Diagnostics.Stopwatch();
Console.WriteLine("");
Console.WriteLine("Starting Duplicate Methods Comparison");
Console.WriteLine("Creating list...");
var listItems = CreateLargeList();
var sortedList = listItems.OrderBy(x => x).ToList();
Console.WriteLine("");

Console.WriteLine("Starting trivial duplicate check...");
trivialSW.Start();
var trivret = CheckDuplicateTrivial(listItems);
trivialSW.Stop();
Console.WriteLine($"Trivial returned {trivret.ToString()} in {trivialSW.ElapsedMilliseconds.ToString()} ms");
Console.WriteLine("");

Console.WriteLine("Starting improved duplicate check...");
improvedSW.Start();
var impret = CheckDuplicateImproved(listItems);
improvedSW.Stop();
Console.WriteLine($"Improved returned {impret.ToString()} in {improvedSW.ElapsedMilliseconds.ToString()} ms");
Console.WriteLine("");

Console.WriteLine("Starting dictionary duplicate check...");
dictionarySW.Start();
var dicret = CheckDuplicateDictionary(listItems);
dictionarySW.Stop();
Console.WriteLine($"Dictionary returned {dicret.ToString()} in {dictionarySW.ElapsedMilliseconds.ToString()} ms");
Console.WriteLine("");

Console.WriteLine("Starting sort/scan duplicate check...");
sortscanSW.Start();
var sortret = CheckDuplicateSortScan(listItems);
sortscanSW.Stop();
Console.WriteLine($"Sort/Scan returned {sortret.ToString()} in {sortscanSW.ElapsedMilliseconds.ToString()} ms");
Console.WriteLine("");

Console.WriteLine("Starting distinct duplicate check...");
distinctSW.Start();
var distret = CheckDuplicateDistinct(listItems);
distinctSW.Stop();
Console.WriteLine($"Distinct returned {distret.ToString()} in {distinctSW.ElapsedMilliseconds.ToString()} ms");
Console.WriteLine("");

Console.WriteLine("Starting hashset duplicate check...");
hashsetSW.Start();
var hashret = CheckDuplicateHashset(listItems);
hashsetSW.Stop();
Console.WriteLine($"Hashset returned {hashret.ToString()} in {hashsetSW.ElapsedMilliseconds.ToString()} ms");
Console.WriteLine("");
