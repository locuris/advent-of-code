module day11

type SeatState =
    | Floor = '.'
    | Empty = 'L'
    | Occupied = '#'

let prepareSeatLayout (lines: string array) : SeatState array2d =
    let width = lines.[0].Length
    let height = lines.Length
    let seats: SeatState array2d = Array2D.zeroCreate width height

    for w in 0 .. width - 1 do
        for h in 0 .. height - 1 do
            seats.[w, h] <-
                match lines.[h].[w] with
                | '.' -> SeatState.Floor
                | 'L' -> SeatState.Empty
                | '#' -> SeatState.Occupied
                | _ -> failwith "Invalid seat state"

    seats

let checkNeighboursBySight (seats: SeatState array2d) (x: int) (y: int) : bool =
    if x = 1 && y = 1 then
        ()
    else ()
    
    let directions =
        [ (-1, 0)
          (1, 0)
          (0, -1)
          (0, 1)
          (-1, -1)
          (-1, 1)
          (1, -1)
          (1, 1) ]

    let outOfBounds (x: int) (y: int) : bool =
        x < 0 || x >= seats.GetLength(0) || y < 0 || y >= seats.GetLength(1)

    let occupiedNeighbours =
        directions
        |> List.map (fun (dx, dy) ->
            if outOfBounds (x + dx) (y + dy) then
                0
            else
                let mutable nx = x + dx
                let mutable ny = y + dy
                let mutable neighbourSeat = seats.[nx, ny]

                while not (outOfBounds nx ny) && neighbourSeat = SeatState.Floor do
                    nx <- nx + dx
                    ny <- ny + dy

                    if not (outOfBounds nx ny) then
                        neighbourSeat <- seats.[nx, ny]

                if neighbourSeat = SeatState.Occupied then 1 else 0)
        |> List.sum

    let currentSeat = seats.[x, y]    
    
    (currentSeat = SeatState.Empty && occupiedNeighbours = 0)
    || (currentSeat = SeatState.Occupied && occupiedNeighbours >= 5)

let checkNeighbours (seats: SeatState array2d) (x: int) (y: int) : bool =
    let offsets = [ -1; 0; 1 ]

    let neighbours =
        [ for dx in offsets do
              for dy in offsets do
                  if dx <> 0 || dy <> 0 then
                      yield x + dx, y + dy ]

    let mutable occupiedNeighbours = 0

    for nx, ny in neighbours do
        if nx >= 0 && nx < seats.GetLength(0) && ny >= 0 && ny < seats.GetLength(1) then
            match seats.[nx, ny] with
            | SeatState.Occupied -> occupiedNeighbours <- occupiedNeighbours + 1
            | _ -> ()

    let currentSeat = seats.[x, y]

    (currentSeat = SeatState.Empty && occupiedNeighbours = 0)
    || (currentSeat = SeatState.Occupied && occupiedNeighbours >= 4)

let updateSeat (seat: SeatState) : SeatState =
    match seat with
    | SeatState.Empty -> SeatState.Occupied
    | SeatState.Occupied -> SeatState.Empty
    | _ -> seat

let updateSeats (seats: SeatState array2d) : SeatState array2d =
    seats
    |> Array2D.mapi (fun x y seat -> if checkNeighbours seats x y then updateSeat seat else seat)
   
let updateSeatsUsingSight (seats: SeatState array2d) : SeatState array2d =
    seats
    |> Array2D.mapi (fun x y seat -> if checkNeighboursBySight seats x y then updateSeat seat else seat)
    
let printSeats (seats: SeatState array2d) : unit =
    for y in 0 .. seats.GetLength(1) - 1 do
        for x in 0 .. seats.GetLength(0) - 1 do
            printf $"%c{match seats.[x, y] with
                        | SeatState.Floor -> '.'
                        | SeatState.Empty -> 'L'
                        | SeatState.Occupied -> '#'
                        | _ -> System.ArgumentOutOfRangeException() |> raise}"
        printfn ""
        
    printfn ""

let day11part1 (lines: string array) : string =
    let mutable currentSeats = prepareSeatLayout lines
    let mutable nextSeats = updateSeats currentSeats
    printSeats currentSeats

    while currentSeats <> nextSeats do
        currentSeats <- nextSeats
        printSeats currentSeats
        nextSeats <- updateSeats currentSeats

    currentSeats
    |> Array2D.map (fun seat -> if seat = SeatState.Occupied then 1 else 0)
    |> Seq.cast<int>
    |> Seq.sum
    |> string
    
let day11part2 (lines: string array) : string =
    let mutable currentSeats = prepareSeatLayout lines
    let mutable nextSeats = updateSeatsUsingSight currentSeats
    
    while currentSeats <> nextSeats do
        currentSeats <- nextSeats
        printSeats currentSeats
        nextSeats <- updateSeatsUsingSight currentSeats
        printSeats nextSeats

    currentSeats
    |> Array2D.map (fun seat -> if seat = SeatState.Occupied then 1 else 0)
    |> Seq.cast<int>
    |> Seq.sum
    |> string
 
