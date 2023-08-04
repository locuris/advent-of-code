module day6

open utilities

let day6part1 (lines: string array) =
    let groups = getLinesGroupedByNewLine lines
    groups |> List.map (Seq.concat >> Set.ofSeq >> Set.count) |> List.sum |> string
    
let day6part2 (lines: string array) =
    let groups = getLinesGroupedByNewLine lines
    groups |> List.map (Seq.map Set.ofSeq >> Seq.reduce Set.intersect >> Set.count) |> List.sum |> string