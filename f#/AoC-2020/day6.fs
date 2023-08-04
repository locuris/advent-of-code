module day6

open utilities

let day6part1 (lines: string array) =
    let groups = getLinesGroupedByNewLine lines
    groups |> List.map (Seq.concat >> Set.ofSeq >> Set.count) |> List.sum |> string