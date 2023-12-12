module day11

type Point = int64 * int64

let getDistance (xA, yA) (xB, yB) =
    abs (xA - xB) + abs (yA - yB)
    
let getAnswer (lines: string array) (expansionRate: int64) =
    let starMap = lines
                  |> Array.mapi (fun y line ->
                      line.ToCharArray()
                      |>  Array.mapi (fun x chr ->
                          Point(x, y), chr))
                  |> Array.collect id
    let rows = starMap |> Array.groupBy (fun ((_, y), _) -> y) |> Map.ofArray
    let expandedRows = rows |> Map.filter (fun _ row -> not (row |> Array.exists (fun ((_, _), chr) -> chr = '#'))) |> Map.keys |> Array.ofSeq
    let columns = starMap |> Array.groupBy (fun ((x, _), _) -> x) |> Map.ofArray
    let expandedColumns = columns |> Map.filter (fun _ column -> not (column |> Array.exists (fun ((_, _), chr) -> chr = '#'))) |> Map.keys |> Array.ofSeq
    
    let stars =  starMap |> Array.filter (fun ((_, _), chr) -> chr = '#') |> Array.map (fun ((x, y), chr) ->
        let xExp = x + (int64((expandedColumns |> Array.filter (fun c -> c < x)).Length) * expansionRate)
        let yExp = y + (int64((expandedRows |> Array.filter (fun r -> r < y)).Length) * expansionRate)
        Point(xExp, yExp))
    
    let pairs = seq {
        for i in 0 .. stars.Length - 1 do
            for j in i + 1 .. stars.Length - 1 do
                yield stars.[i], stars.[j]
    }
    
    pairs |> Seq.toArray |> Array.sumBy (fun ((xA, yA), (xB, yB)) -> getDistance (xA,yA) (xB, yB)) |> string


let part1 (lines: string array) : string =
    getAnswer lines 1L
        
let part2 (lines: string array) : string =
    getAnswer lines 999999L