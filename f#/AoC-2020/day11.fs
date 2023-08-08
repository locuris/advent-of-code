module day11

type SeatState =
    | Floor = '.'
    | Empty = 'L'
    | Occupied = '#'

let prepareSeatLayout (lines: string array) : SeatState array2d =
    let width = lines.[0].Length
    let height = lines.Length
    let seats: SeatState array2d = Array2D.zeroCreate width height

    for i in 0 .. width - 1 do
        for j in 0 .. height - 1 do
            seats.[i, j] <-
                match lines.[j].[i] with
                | '.' -> SeatState.Floor
                | 'L' -> SeatState.Empty
                | '#' -> SeatState.Occupied
                | _ -> failwith "Invalid seat state"

    seats

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

let day11part1 (lines: string array) : string =
    let mutable currentSeats = prepareSeatLayout lines
    let mutable nextSeats = updateSeats currentSeats

    while currentSeats <> nextSeats do
        currentSeats <- nextSeats
        nextSeats <- updateSeats currentSeats

    currentSeats
    |> Array2D.map (fun seat -> if seat = SeatState.Occupied then 1 else 0)
    |> Seq.cast<int>
    |> Seq.sum
    |> string
