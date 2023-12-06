module day1

open System
open System.Globalization
open Microsoft.VisualBasic.CompilerServices

let digitMap = Map.ofSeq [|
    ("1", "1"); ("2", "2"); ("3", "3")
    ("4", "4"); ("5", "5"); ("6", "6")
    ("7", "7"); ("8", "8"); ("9", "9")
    ("one", "1"); ("two", "2"); ("three", "3")
    ("four", "4"); ("five", "5"); ("six", "6")
    ("seven", "7"); ("eight", "8"); ("nine", "9")
|]

let digits = [|
    "1"; "2"; "3"
    "4"; "5"; "6"
    "7"; "8"; "9"
    "one"; "two"; "three"
    "four"; "five"; "six"
    "seven"; "eight"; "nine"
|]


let getValueForLine (line: string) : int =
    let first = line.ToCharArray()
                |> Array.find Char.IsDigit |> string
    let last = line.ToCharArray()
               |> Array.findBack Char.IsDigit |> string
    int (first + last)

let part1 (lines: string array) : string =
    lines
    |> Array.map getValueForLine
    |> Array.sum
    |> string

let getValueForLineIncludeWords (line: string) : int =
    let first = digits |> Array.filter line.Contains |> Array.minBy line.IndexOf |> digitMap.TryFind |> Option.get
    let last = digits |> Array.filter line.Contains |> Array.maxBy line.LastIndexOf |> digitMap.TryFind |> Option.get
    int (first + last)


let part2 (lines: string array) : string =
    lines |> Array.map getValueForLineIncludeWords |> Array.sum |> string