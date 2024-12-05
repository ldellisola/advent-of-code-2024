using static Day05.Parser;

using var file = File.OpenRead("input.txt");
using var streamReader = new StreamReader(file);

var rules = LoadOrderRules(streamReader).ToDictionaryWithRepeated();
var manuals = LoadSafetyManuals(streamReader).ToArray();


// ----------------------------------

List<int[]> validManuals = [];
foreach(var manual in manuals)
{
    var localRules = rules
                     .Where(t=> manual.Contains(t.Key))
                     .ToDictionary(t=> t.Key, t => t.Value.Where(r=> manual.Contains(r)).ToHashSet());
    HashSet<int> pagesFound = [];
    var isValid = true;
    foreach(var page in manual)
    {
        if(!localRules.TryGetValue(page, out var previousPages))
        {
            pagesFound.Add(page);
            continue;
        }

        if(previousPages.IsSubsetOf(pagesFound))
        {
            pagesFound.Add(page);
            continue;
        }
        
        isValid = false;
        break;
    }
    
    if (isValid)
        validManuals.Add(manual);
}

var result = validManuals.Sum(t=> t[ t.Length /2 + (t.Length % 2 == 0 ? 1 : 0) ] );
Console.WriteLine("Challenge 1:");
Console.WriteLine(result);
// ---------------------------------------------------------------------------------------

List<List<int>> invalidManuals = [];
// Finding invalid manuals
foreach(var manual in manuals)
{
    var localRules = rules
                     .Where(t=> manual.Contains(t.Key))
                     .ToDictionary(t=> t.Key, t => t.Value.Where(r=> manual.Contains(r)).ToHashSet());
    HashSet<int> pagesFound = [];
    var isValid = true;
    foreach(var page in manual)
    {
        if(!localRules.TryGetValue(page, out var previousPages))
        {
            pagesFound.Add(page);
            continue;
        }

        if(previousPages.IsSubsetOf(pagesFound))
        {
            pagesFound.Add(page);
            continue;
        }
        
        isValid = false;
    }
    
    if (!isValid)
        invalidManuals.Add(manual.ToList());
}

// fix invalid manuals

List<List<int>> fixedManuals = [];
foreach(var manual in invalidManuals)
{
    var localRules = rules
                     .Where(t=> manual.Contains(t.Key))
                     .SelectMany(t=>t.Value.Where(r=> manual.Contains(r)).Select(r=> (t.Key, r)))
                     .ToHashSet();

    var fixedManual = manual.ToList();
    bool hasChanged = false;
    do
    {
        hasChanged = false;
        foreach(var (after, before) in localRules)
        {
            var iAfter = fixedManual.IndexOf(after);
            var iBefore = fixedManual.IndexOf(before);

            if(iAfter == -1 || iBefore == -1 || iBefore <= iAfter)
            {
                continue;
            }
            hasChanged = true;
            fixedManual[iAfter] = before;
            fixedManual[iBefore] = after;
        }
    }while(hasChanged);

    fixedManuals.Add(fixedManual);
}

result = fixedManuals.Sum(t=> t[ t.Count /2 + (t.Count % 2 == 0 ? 1 : 0) ] );
Console.WriteLine("Challenge 2:");
Console.WriteLine(result);