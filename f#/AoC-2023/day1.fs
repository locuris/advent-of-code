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


let getValueForLine (line: string) =
    let first = line.ToCharArray() |> Array.find Char.IsDigit |> string
    let last = line.ToCharArray() |> Array.findBack Char.IsDigit |> string
    int (first + last)

let part1 (lines: string array) =
    let mutable answer = 0
    for line in lines do        
        answer <- answer + getValueForLine line
    answer |> string

let findAllSubstringIndices (mainStr: string) (subStr: string) =
    let rec findFromIndex (startIndex: int) (acc: int list) =
        let foundIndex = mainStr.IndexOf(subStr, startIndex)
        if foundIndex <> -1 then
            findFromIndex (foundIndex + 1) (foundIndex :: acc)
        else
            List.rev acc

    findFromIndex 0 []


let createSubList myList =
    match myList with
    | [] -> [] 
    | [x] -> [x] 
    | first :: rest ->
        let last = List.last rest
        [first; last]


let part2 (lines: string array) =
    let mutable answer = 0
    for line in lines do
        let indices = digits |> Array.map (fun d -> (d, findAllSubstringIndices line d))
        let first = indices |> Seq.filter (fun (_, i) -> Seq.isEmpty i |> not) |> Seq.minBy (fun (d, i) -> i |> Seq.min) |> fst |> (fun d -> digitMap.Item d)
        let last = indices |> Seq.filter (fun (_, i) -> Seq.isEmpty i |> not) |> Seq.maxBy (fun (d, i) -> i |> Seq.max) |> fst |> (fun d -> digitMap.Item d)
        answer <- answer + (int (first + last))
    answer |> string