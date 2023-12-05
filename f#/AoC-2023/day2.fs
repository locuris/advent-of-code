module day2

type Game =
    struct
        val Id: int
        val KnownBlue: int
        val KnownRed: int
        val KnownGreen: int
        new(id: int, knownBlue: int, knownRed: int, knowGreen: int) =
            {Id = id; KnownBlue = knownBlue; KnownRed = knownRed; KnownGreen = knowGreen }
        
        member this.GetPower() = this.KnownRed * this.KnownBlue * this.KnownGreen
    end
    
let createGame(line: string) =
    let firstSplit = line.Split(':')
    let secondSplit = firstSplit[1].Split(';')
    let turns = secondSplit |> Array.map (fun s ->
        let goes = s.Split(',')
        goes |> Array.map (fun g ->
            let cube = g.Split(' ')
            (cube[2].Trim(),int (cube[1].Trim()))))
    Game(       
       firstSplit |> Array.item 0 |> (fun g -> g.Split(' ')) |> Array.item 1 |> int,
       turns |> Array.map (fun turn -> turn |> Array.map (fun (c, n) -> if c = "blue" then (c, n) else (c, 0)) |> Array.maxBy snd |> snd) |> Array.max,
       turns |> Array.map (fun turn -> turn |> Array.map (fun (c, n) -> if c = "red" then (c, n) else (c, 0)) |> Array.maxBy snd |> snd) |> Array.max,
       turns |> Array.map (fun turn -> turn |> Array.map (fun (c, n) -> if c = "green" then (c, n) else (c, 0)) |> Array.maxBy snd |> snd) |> Array.max
    )
    
let part1(lines: string array) =
    lines |> Array.map createGame |> Array.sumBy (fun game ->
        if game.KnownBlue <= 14 && game.KnownRed <= 12 && game.KnownGreen <= 13 then game.Id else 0) |> string
    
let part2(lines: string array) =
    lines |> Array.map createGame |> Array.sumBy (fun game -> game.GetPower()) |> string