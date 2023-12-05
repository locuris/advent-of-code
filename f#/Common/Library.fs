namespace Common

open System
open System.Collections.Generic
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
        
    let getMatches (text: string, pattern: string) : string seq =
        Regex.Matches (text, pattern)
        |> Seq.cast<Match>
        |> Seq.map (fun m -> m.Value)
        
    let getGroups (text: string, pattern: string) : string seq =
        let firstMatch = Regex.Matches (text, pattern) |> Seq.cast<Match> |> Seq.item 0
        firstMatch.Groups.Values |> Seq.map (fun g -> g.Value)
        