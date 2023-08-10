module day12


type Direction =
    | East = 1
    | South = 2
    | West = 3
    | North = 4

let intToDirection directionInt =
    match directionInt * (if directionInt < 0 then -1 else 1) with
    | 1 -> Direction.East
    | 2 -> Direction.South
    | 3 -> Direction.West
    | 4 -> Direction.North
    | _ -> failwith $"Invalid integer ({directionInt}) for Direction"

type Ship =
    { mutable direction: Direction
      mutable position: int * int }

type Ship with

    member this.turnShip (left: bool) (degrees: int) =
        let currentDirection = int this.direction
        let mutable turnUnits: int = degrees / 90
        if left then
            turnUnits <- 4 - turnUnits
        let mutable newDirection = currentDirection + turnUnits
        if newDirection > 4 then
            newDirection <- newDirection % 4
        this.direction <- intToDirection newDirection

    member this.moveForward(distance: int) = this.moveShip this.direction distance

    member this.moveShip (direction: Direction) (distance: int) =
        let mx, my =
            match direction with
            | Direction.East -> 1 * distance, 0
            | Direction.South -> 0, -1 * distance
            | Direction.West -> -1 * distance, 0
            | Direction.North -> 0, 1 * distance
            | _ -> failwith "Invalid Direction"

        let x, y = this.position
        this.position <- x + mx, y + my

    member this.manhattanDistance: int =
        let x, y = this.position
        (abs (x) + abs (y))

let day12part1 (lines: string array) : string =
    let ship: Ship =
        { direction = Direction.East
          position = 0, 0 }

    for line in lines do
        let value = line.Substring(1) |> int

        match line.Substring(0, 1) with
        | "N" -> ship.moveShip Direction.North value
        | "E" -> ship.moveShip Direction.East value
        | "S" -> ship.moveShip Direction.South value
        | "W" -> ship.moveShip Direction.West value
        | "F" -> ship.moveForward value
        | "L" -> ship.turnShip true value
        | "R" -> ship.turnShip false value
        | _ -> failwith $"Unrecognised command. {line}"

    ship.manhattanDistance |> string
