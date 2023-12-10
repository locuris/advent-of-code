module day2

open Common.Input

type Game =
    struct
        val Id: int
        val KnownBlue: int
        val KnownRed: int
        val KnownGreen: int
        new(id: int, knownBlue: int, knownRed: int, knowGreen: int) =
            {Id = id; KnownBlue = knownBlue; KnownRed = knownRed; KnownGreen = knowGreen }
        
        static member GetPower(this: Game) = this.KnownRed * this.KnownBlue * this.KnownGreen
    end 
    
let createGame(line: string) : Game =    
    Game(       
       line |> GetMatchAsString @"\d+" |> int,
       line |> GetGroupsAsStringArray @"(\d+).blue" |> Array.maxBy int |> int,
       line |> GetGroupsAsStringArray @"(\d+).red" |> Array.maxBy int |> int,
       line |> GetGroupsAsStringArray @"(\d+).green" |> Array.maxBy int |> int
    )
    
let part1(lines: string array) =
    lines |> Array.map createGame |> Array.sumBy (fun game ->
        if game.KnownBlue <= 14 && game.KnownRed <= 12 && game.KnownGreen <= 13 then game.Id else 0) |> string
    
let part2(lines: string array) =
    lines |> Array.map createGame |> Array.sumBy Game.GetPower |> string