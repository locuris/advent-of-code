module day14

type Cave =
    | Stone of int
    | Empty of int
    | Ledge of int
    

let part1 (lines: string array) : string =
    let height = lines.Length
    lines
    |> Array.mapi (fun i line ->
        line.ToCharArray()
        |> Array.map (fun chr ->
            let h = height - i
            match chr with
            | '#' -> Ledge(h)
            | '.' -> Empty(h)
            | 'O' -> Stone(h)
            | _ -> failwith "BOOM")
        |> Array.rev
        )
    