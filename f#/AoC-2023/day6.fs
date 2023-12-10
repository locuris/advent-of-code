module day6

open System
open Common.Input

let getAnswer(distances: int64 array) (times: int64 array) : string =
    let combos = (distances, times) ||> Array.map2 (fun d t -> seq { for n in 0L..t do yield (d, n * (t - n))}) |> Array.map Array.ofSeq    
    combos |> Array.map (fun pos -> pos |> Array.sumBy (fun (distance, time) -> if time > distance then 1 else 0)) |> Array.fold (fun a b -> a * b) 1 |> string

let part1(lines: string array) : string =
    let times = lines |> Array.item 0 |> GetMatchesAsStringArray @"\d+" |> Array.map Int64.Parse
    let distances = lines |> Array.item 1 |> GetMatchesAsStringArray @"\d+" |> Array.map Int64.Parse
    getAnswer distances times
                                       
    
let part2(lines: string array) : string =
    let times = lines |> Array.item 0 |> GetMatchesAsStringArray @"\d+" |> Array.fold (fun a b -> a + b) "" |> fun s ->  seq { Int64.Parse(s) } |> Array.ofSeq
    let distances = lines |> Array.item 1 |> GetMatchesAsStringArray @"\d+" |> Array.fold (fun a b -> a + b) "" |> fun s ->  seq { Int64.Parse(s) } |> Array.ofSeq
    getAnswer distances times
    

