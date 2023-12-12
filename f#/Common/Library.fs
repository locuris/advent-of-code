namespace Common

open System
open System.Collections.Generic
open System.Diagnostics
open System.Drawing
open System.Text.RegularExpressions

module GridHelpers =
    let getSize(input: string array): int * int =
        input |> Array.item 0 |> String.length, input.Length
        
module Data =    
    
    (*type Point(x: int, y: int) =
        member this.X = x
        member this.Y = y
        
        static member (+) (a: Point, b: Point) =
           Point(a.X + b.X, a.Y + b.Y)
           
        interface IComparable with
            member this.CompareTo(other) =
                match other with
                | :? Point as point ->
                    if this.X = point.X && this.Y = point.Y then 0
                    elif this.X * this.Y > point.X * point.Y then 1
                    else -1
                | _ -> -1
        
        override this.Equals other =
            match other with
            | :? Point as p -> p.X = this.X && p.Y = this.Y
            | _ -> false
            
        override this.GetHashCode() =
            this.X * this.Y*)
            
    
    let countOf value collection : int =
        collection |> Array.filter (fun item -> item = value) |> Array.length
        
    let ofArray2D (collection: 'T[,]) : 'T array =
        let xSize = collection |> Array2D.length1
        let ySize = collection |> Array2D.length2
        seq {
            for y in 0..ySize do
                for x in 0..xSize do
                    yield collection[x,y]
        } |> Array.ofSeq
        
    let MapOfArray2D (collection: 'T[,]) : Map<(int * int), 'T> =
        let xSize = collection |> Array2D.length1
        let ySize = collection |> Array2D.length2
        seq {
            for y in 0..ySize-1 do
                for x in 0..xSize-1 do
                    yield (x, y), collection[x,y]
        } |> Map.ofSeq
        
    
        
module Input =
    
    let mainMenu() =
        Console.Write("Enter the day you want to run: ")
        let day = Console.ReadLine() |> int
        Console.Write("Enter the part you want to run: ")
        let part = Console.ReadLine() |> int
        Console.Write("Run test input? (y/n): ")
        let test = Console.ReadLine().ToLower()= "y"
        day, part, test
        
    let GetLinesGroupedByNewLine (lines: string array): string array list =
        
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
        
    let GetMatches (pattern: string) (text: string) : Match seq =
        Regex.Matches (text, pattern)
        |> Seq.cast<Match>
        
    let GetMatchesAsStringArray (pattern: string) (text: string) : string array =
        GetMatches pattern text
        |> Seq.map (fun m -> m.Value)
        |> Array.ofSeq
        
    let GetGroupsAsStringArray (pattern: string) (text: string) : string array =
        Regex.Matches (text, pattern)
        |> Seq.cast<Match>
        |> Seq.collect (fun m -> m.Groups)
        |> Seq.filter (fun g -> g.GetType() = typeof<Group>)
        |> Seq.map (fun g -> g.Value)
        |> Array.ofSeq
        
    let GetMatchAsString (pattern: string) (text: string) : string =
        let match_ = Regex.Match(text, pattern)
        match_.Value
        
    let InputAsCharArray2D (text: string array) : char[,] * Size =
        let size = Size(text[0].Length, text.Length)
        Array2D.init size.Width size.Height (fun x y -> text[y].ToCharArray()[x]), size