namespace Common

open System
open System.Collections.Generic
open System.Diagnostics
open System.Text.RegularExpressions

module GridHelpers =
    let getSize(input: string array): int * int =
        input |> Array.item 0 |> String.length, input.Length
        
module Input =
    let mainMenu() =
        Console.Write("Enter the day you want to run: ")
        let day = Console.ReadLine() |> int
        Console.Write("Enter the part you want to run: ")
        let part = Console.ReadLine() |> int
        Console.Write("Run test input? (y/n): ")
        let test = Console.ReadLine().ToLower()= "y"
        day, part, test
        
    let getLinesGroupedByNewLine (lines: string array): string array list =
        let finalList = ResizeArray<string array>()
        let currentList = ResizeArray<string>()
        for line in lines do
            if not (line = "") then
                currentList.Add(line)
            else
                finalList.Add(currentList.ToArray())
                currentList.Clear()
                
        finalList.Add(currentList.ToArray())
        finalList |> List.ofSeq
        
    let getMatches (pattern: string) (text: string) : Match seq =
        Regex.Matches (text, pattern)
        |> Seq.cast<Match>
        
    let getMatchesAsStringArray (pattern: string) (text: string) : string array =
        getMatches pattern text
        |> Seq.map (fun m -> m.Value)
        |> Array.ofSeq
        
    let getGroupsAsStringArray (pattern: string) (text: string) : string array =
        Regex.Matches (text, pattern)
        |> Seq.cast<Match>
        |> Seq.collect (fun m -> m.Groups)
        |> Seq.filter (fun g -> g.GetType() = typeof<Group>)
        |> Seq.map (fun g -> g.Value)
        |> Array.ofSeq
        
    let getMatchAsString (pattern: string) (text: string) : string =
        let match_ = Regex.Match(text, pattern)
        match_.Value
        