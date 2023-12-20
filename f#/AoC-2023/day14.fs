module day14

open System


type Point = int * int

type Cave =
    | Stone of int
    | Empty of int
    | Ledge of int
    
type Rock =
    | stone = '0'
    | empty = '.'
    | ledge = '#'
    
    
    
let caveToChar cave =
    match cave with
    | Stone _ -> "O"
    | Empty _ -> "."
    | Ledge _ -> "#"
    
    
let switchToColumns (cave: Cave array array) =
    let mutable y = -1
    cave |> Array.collect (fun caveRow ->
        y <- y + 1
        caveRow |> Array.mapi (fun x rock ->
            Point(x, y), rock))
    |> Array.groupBy (fun ((x, _), _) -> x)
    |> Array.map (fun (_, rocks) -> rocks |> Array.map snd)
    
    
let printCave (cave: Cave array array) =
    cave |> Array.iter (fun caveRow ->
        printfn ""
        caveRow |> Array.iter (fun rock -> printf $"{caveToChar rock}"))
    
let applyGravity (caveColumn: Cave array) =
    let mutable floor = -1
    let mutable index = 0
    let stonesToFall =
            caveColumn
            |> Array.map (fun cave ->
                match cave with
                | Stone i ->
                    if floor > -1 then
                        let newStone = floor
                        floor <- i
                        Stone(newStone)
                        else Stone(i)
                | Empty i ->
                    if floor = -1 then
                        floor <- i
                    cave
                | Ledge i ->
                    floor <- -1
                    cave)
            |> Array.filter (fun cave ->
                let currentIndex = index
                index <- 1 + index
                match cave with
                | Stone i -> i <> currentIndex
                | _ -> false)
            |> Array.mapi (fun old cave ->
                match cave with
                | Stone i -> (i, (old ,cave)))
            |> Map.ofArray
            
    caveColumn |> Array.mapi (fun i cave ->
        if stonesToFall.ContainsKey(i) then
            stonesToFall[i]
        else (*if stonesToFall.Values |> Seq.exists (fun (_, cve) -> match cvs with | Stone n | Empty n | Ledge n -> n = i; | _ -> false))*) 0, cave)
    

let part1 (lines: string array) : string =
    let height = lines.Length
    let cave =
        lines
        |> Array.mapi (fun i line ->
            line.ToCharArray()
            |> Array.map (fun chr ->
                let h = height - i
                match chr with
                | '#' -> Ledge(h)
                | '.' -> Empty(h)
                | 'O' -> Stone(h)
                | _ -> failwith "BOOM")) |> switchToColumns    
    printCave cave
    let d = cave |> Array.map applyGravity
    ""
    
let part2 (lines: string array) : string =
    failwithf "Not implemented yet"