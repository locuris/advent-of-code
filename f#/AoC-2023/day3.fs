module day3

open System
open System.Text.RegularExpressions
open Common.GridHelpers

let isValid(xStart: int, xEnd: int, yPos: int, grid: char[,], xSize: int, ySize: int) : bool =
    let mutable checkXStart = xStart - 1
    if checkXStart < 0 then checkXStart <- 0
    let mutable checkYStart = yPos - 1
    if checkYStart < 0 then checkYStart <- 0
    let mutable checkXEnd = xEnd + 1
    if checkXEnd >= xSize then checkXEnd <- xEnd
    let mutable checkYEnd = yPos + 1
    if checkYEnd >= ySize then checkYEnd <- yPos
    let mutable valid = false
    for x in checkXStart..checkXEnd do
        for y in checkYStart..checkYEnd do
            if Char.IsDigit(grid[x, y]) || grid[x, y] = '.' then ()
            else valid <- true
    valid
    
let isValidGear((xPos: int, yPos: int), grid: char[,], xSize: int, ySize: int, numberPos: (string * (int * int) * (int * int)) array) = 
    let xStart = if xPos = 0 then xPos else xPos - 1
    let yStart = if yPos = 0 then yPos else yPos - 1
    let yEnd = if yPos >= ySize - 1 then yPos else yPos + 1
    let xEnd = if xPos >= xSize - 1 then xPos else xPos + 1
    let mutable nSet = Set.empty
    for x in xStart..xEnd do
        for y in yStart..yEnd do
            numberPos |> Array.iter (fun (n, (xs, yp), (xe, _)) -> if x >= xs && x <= xe && y = yp then nSet <- nSet.Add(int(n)))
    let mutable power = 1
    nSet |> Set.iter (fun n -> power <- power * n)
    nSet.Count = 2, power

let part1(lines: string array) =    
    let xSize, ySize = getSize lines
    let grid = Array2D.init xSize ySize (fun x y -> lines[y].ToCharArray()[x])
    let numberPosMap = lines |> Array.mapi (fun y line ->
        Regex.Matches (line, @"\b\d+\b")
        |> Seq.cast<Match>
        |> Seq.map (fun m -> m.Value, (m.Index, y), (m.Index + m.Length - 1, y)) |> Seq.toArray) |> Array.collect id
    numberPosMap |> Array.sumBy (fun (number, (xStart, y), (xEnd, _)) -> if isValid(xStart, xEnd, y, grid, xSize, ySize) then int(number) else 0) |> string
    
let part2(lines: string array) =    
    let xSize, ySize = getSize lines
    let grid = Array2D.init xSize ySize (fun x y -> lines[y].ToCharArray()[x])
    let numberPosMap = lines |> Array.mapi (fun y line ->
        Regex.Matches (line, @"\b\d+\b")
        |> Seq.cast<Match>
        |> Seq.map (fun m -> m.Value, (m.Index, y), (m.Index + m.Length - 1, y)) |> Seq.toArray) |> Array.collect id
    let mutable gearCoords = List.empty
    grid |> Array2D.iteri (fun x y c -> if c = '*' then gearCoords <- gearCoords @ [(x,y)])
    gearCoords |> List.sumBy (fun (x, y) ->
                              let c, ns = isValidGear((x, y), grid, xSize, ySize, numberPosMap)
                              if c then ns else 0) |> string